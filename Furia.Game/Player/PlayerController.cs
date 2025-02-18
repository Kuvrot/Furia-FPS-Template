// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org/ & https://stride3d.net) and Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.
using System;
using Stride.Core;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Engine.Events;
using Stride.Physics;
using System.Collections.Generic;
using Stride.Input;
using Furia.Stats;

namespace Furia.Player
{
    public class PlayerController : SyncScript
    {
        [Display("Run Speed")]
        public float MaxRunSpeed { get; set; } = 5;

        public static readonly EventKey<float> RunSpeedEventKey = new EventKey<float>();

        // This component is the physics representation of a controllable character
        private CharacterComponent character;

        private readonly EventReceiver<Vector3> moveDirectionEvent = new EventReceiver<Vector3>(PlayerInput.MoveDirectionEventKey);

        [Display("WeaponInventory")]
        public List<EntityComponent> Weapons = [];
        public int currentWeaponSelected = 0;
        public WeaponScript weaponScript;
        public AnimationController animationController;
        public WeaponScript weaponScriptAux;
        public AnimationController animationControllerAux;
        public override void Start()
        {
            base.Start();

            // Will search for an CharacterComponent within the same entity as this script
            character = Entity.Get<CharacterComponent>();
            if (character == null) throw new ArgumentException("Please add a CharacterComponent to the entity containing PlayerController!");
            
            WeaponChange(1);
        }
        
        /// <summary>
        /// Called on every frame update
        /// </summary>
        public override void Update()
        {
            Move();
            WeaponInventoryManagement();
        }

        private void Move()
        {
            // Character speed
            Vector3 moveDirection = Vector3.Zero;
            moveDirectionEvent.TryReceive(out moveDirection);

            character.SetVelocity(moveDirection * MaxRunSpeed);

            // Broadcast normalized speed
            RunSpeedEventKey.Broadcast(moveDirection.Length());
        }

        private void WeaponInventoryManagement()
        {
            if (Input.IsKeyPressed(Keys.D1))
            {
                WeaponChange(0);
            }
            else if (Input.IsKeyPressed(Keys.D2))
            {
                WeaponChange(1);
            }
        }

        private void WeaponChange (int index)
        {
            foreach (var weapon in Weapons)
            {
                weapon.Entity.Get<SpriteComponent>().Enabled = false;
            }

            

            WeaponStats currentWeaponStats = Weapons[index].Entity.Get<WeaponStats>();

            //Homologate stats
            weaponScript.weaponID = currentWeaponStats.weaponID;
            weaponScript.MaxShootDistance = currentWeaponStats.MaxShootDistance;
            weaponScript.ShootImpulse = currentWeaponStats.ShootImpulse;
            weaponScript.Cooldown = currentWeaponStats.Cooldown;
            weaponScript.ReloadCooldown = currentWeaponStats.ReloadCooldown;
            weaponScript.remainingBullets = currentWeaponStats.remainingBullets;
            weaponScript.maxBullets = currentWeaponStats.maxBullets;
            weaponScript.infiniteBullets = currentWeaponStats.infiniteBullets;
            weaponScript.damage = currentWeaponStats.damage;

            //Homologate animations
            animationController.AnimationIdle = null;
            animationController.AnimationWalk = null;
            animationController.AnimationShoot = null;
            animationController.AnimationReload = null;
            
            animationController.AnimationIdle = currentWeaponStats.AnimationIdle;
            animationController.AnimationWalk = currentWeaponStats.AnimationWalk;
            animationController.AnimationShoot = currentWeaponStats.AnimationShoot;
            animationController.AnimationReload = currentWeaponStats.AnimationReload;
            
            Weapons[index].Entity.Get<SpriteComponent>().Enabled = true;
        }
    }
}
