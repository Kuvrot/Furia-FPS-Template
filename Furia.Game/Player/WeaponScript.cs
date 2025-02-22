// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org/ & https://stride3d.net) and Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.
using System;
using System.Threading.Tasks;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Engine.Events;
using Stride.Physics;
using Stride.Rendering.Sprites;
using Vortice.Vulkan;

namespace Furia.Player
{
    public struct WeaponFiredResult
    {
        public bool         DidFire;
        public bool         DidHit;
        public HitResult    HitResult;
    }

    public class WeaponScript : SyncScript
    {
        public byte weaponID = 0;

        public static readonly EventKey<WeaponFiredResult> WeaponFired = new EventKey<WeaponFiredResult>();

        public static readonly EventKey<bool> IsReloading = new EventKey<bool>();

        private readonly EventReceiver<bool> shootEvent = new EventReceiver<bool>(PlayerInput.ShootEventKey);

        private readonly EventReceiver<bool> reloadEvent = new EventReceiver<bool>(PlayerInput.ReloadEventKey);

        public float ShootImpulse { get; set; } = 5f;
        private float cooldownRemaining = 0f;

        private UiManager m_UI;
        private WeaponManager weaponManager;

        public override void Start()
        {
            m_UI = Entity.Get<UiManager>();
            weaponManager = Entity.GetParent().Get<WeaponManager>();
        }

        private void ReloadWeapon()
        {
            IsReloading.Broadcast(true);
            Func<Task> reloadTask = async () =>
            {
                // Countdown
                var secondsCountdown = cooldownRemaining = weaponManager.currentWeaponStats.ReloadCooldown;
                while (secondsCountdown > 0f)
                {
                    await Script.NextFrame();
                    secondsCountdown -= (float) Game.UpdateTime.Elapsed.TotalSeconds;
                }

                if (weaponManager.currentWeaponStats.inventoryBullets >= weaponManager.currentWeaponStats.maxBullets)
                {
                    weaponManager.currentWeaponStats.remainingBullets = weaponManager.currentWeaponStats.maxBullets;
                    weaponManager.currentWeaponStats.inventoryBullets -= weaponManager.currentWeaponStats.maxBullets;
                }
                else
                {
                    weaponManager.currentWeaponStats.remainingBullets = weaponManager.currentWeaponStats.inventoryBullets;
                    weaponManager.currentWeaponStats.inventoryBullets = 0;
                }
            };

            Script.AddTask(reloadTask);
        }

        /// <summary>
        /// Called on every frame update
        /// </summary>
        public override void Update()
        {
            //Update UI
            m_UI.UpdateBulletCount(weaponManager.currentWeaponStats.remainingBullets, weaponManager.currentWeaponStats.inventoryBullets);

            bool didShoot;
            shootEvent.TryReceive(out didShoot);

            bool didReload;
            reloadEvent.TryReceive(out didReload);

            cooldownRemaining = (cooldownRemaining > 0) ? (cooldownRemaining - (float)this.Game.UpdateTime.Elapsed.TotalSeconds) : 0f;
            if (cooldownRemaining > 0)
                return; // Can't shoot yet

            if ((weaponManager.currentWeaponStats.remainingBullets <= 0 && didShoot) || (weaponManager.currentWeaponStats.remainingBullets <= weaponManager.currentWeaponStats.maxBullets && didReload))
            {
                ReloadWeapon();
                return;
            }

            if (!didShoot)
                return;

            
            if (!weaponManager.currentWeaponStats.infiniteBullets)
            {
                weaponManager.currentWeaponStats.remainingBullets--;
            }

           cooldownRemaining = weaponManager.currentWeaponStats.Cooldown;

            var raycastStart = Entity.Transform.WorldMatrix.TranslationVector;
            var forward = Entity.Transform.WorldMatrix.Forward;
            var raycastEnd = raycastStart + forward * weaponManager.currentWeaponStats.MaxShootDistance;

            var result = this.GetSimulation().Raycast(raycastStart, raycastEnd);

            var weaponFired = new WeaponFiredResult {HitResult = result, DidFire = true, DidHit = false };

            if (result.Succeeded && result.Collider != null)
            {
                weaponFired.DidHit = true;

                var rigidBody = result.Collider as RigidbodyComponent;
                if (rigidBody != null)
                {
                    rigidBody.Activate();
                    rigidBody.ApplyImpulse(forward * ShootImpulse);
                    rigidBody.ApplyTorqueImpulse(forward * ShootImpulse + new Vector3(0, 1, 0));
                }
            }

            // Broadcast the fire event
            WeaponFired.Broadcast( weaponFired );
        }
    }
}
