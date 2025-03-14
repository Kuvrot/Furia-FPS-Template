using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;
using Stride.Core;
using Stride.Audio;
using System;
using System.Security.Cryptography.X509Certificates;
using static BulletSharp.Dbvt;
using Stride.Physics;
using Furia.Player;
using Furia.Core;

namespace Furia.Trigger
{
    public class ItemScript : SyncScript
    {
        [Display("Does the item rotates?")]
        public bool itemRotation = true;
        public float rotationSpeed = 15f;

        [Display("How much amount of this will be given to the player")]
        public int giveAmount = 25;

        public enum AmountType
        { 
            Health, 
            Ammo
        }

        public AmountType amountType;

        public Sound pickUpSound;

        private StaticColliderComponent collider;
        private AudioManager audioManager;

        public override void Start()
        {
            audioManager = Entity.Get<AudioManager>();
        }

        public override void Update()
        {
            if (itemRotation)
            {
                RotateItem(rotationSpeed);
            }

            collider ??= Entity.Get<StaticColliderComponent>();
            foreach (var collision in collider.Collisions)
            {
                if (collision.ColliderA.Entity.Get<PlayerController>() != null)
                {
                    switch (amountType)
                    {
                        case AmountType.Health:
                            GameManager.instance.player.Entity.Get<PlayerStats>().health+=giveAmount;
                            break;
                        case AmountType.Ammo:
                            GameManager.instance.player.Entity.Get<WeaponManager>().currentWeaponStats.inventoryAmmo += giveAmount;
                            break;

                    }
                    
                    audioManager.PlaySoundOnce(pickUpSound);
                }
            }
        }

        private void RotateItem(float speed)
        {
            Entity.Transform.Rotation += Quaternion.RotationYawPitchRoll(speed * (float)Game.UpdateTime.Elapsed.TotalSeconds, 0 , 0);
        }
    }
}
