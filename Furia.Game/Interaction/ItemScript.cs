using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;
using Stride.Core;
using Stride.Audio;
using Stride.Physics;
using Furia.Player;
using Furia.Core;
using System;

namespace Furia.Interaction
{
    public class ItemScript : SyncScript
    {
        [Display("Does the item rotates?")]
        public bool itemRotation = true;

        [Display("How much amount of this will be given to the player")]
        public int giveAmount = 25;

        private bool isPickedUp = false;

        // Leave this null if the item adds health or ammo
        [Display("Pickup weapon (only if the item is a gun)")]
        public EntityComponent weapon;

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
                RotateItem();
            }

            if (GetPlayerDistance() < 1.5f && !isPickedUp)
            {
               
                if (weapon == null)
                {
                    switch (amountType)
                    {
                        case AmountType.Health:
                            GameManager.instance.player.Entity.Get<PlayerStats>().health += giveAmount;
                            break;

                        case AmountType.Ammo:
                            if (!GameManager.instance.player.Entity.Get<WeaponManager>().currentWeaponStats.isMelee)
                            {
                                GameManager.instance.player.Entity.Get<WeaponManager>().currentWeaponStats.inventoryAmmo += giveAmount;
                            }
                            break;
                    }
                }
                else
                {
                    GameManager.instance.player.Entity.Get<WeaponManager>().Weapons.Add(weapon);
                    GameManager.instance.player.Entity.Get<WeaponManager>().currentWeaponSelected = (byte)(GameManager.instance.player.Entity.Get<WeaponManager>().Weapons.Count - 1);
                    GameManager.instance.player.Entity.Get<WeaponManager>().WeaponChange(GameManager.instance.player.Entity.Get<WeaponManager>().currentWeaponSelected); 
                }

                audioManager.PlaySoundOnce(pickUpSound);
                isPickedUp = true;
                Entity.Scene.Entities.Remove(Entity);
            }
        }

        private void RotateItem()
        {
            Vector2 lookAngle = GetLookAtAngle(Entity.Transform.Position, GameManager.instance.player.Position);
            Quaternion result = Quaternion.RotationYawPitchRoll(lookAngle.Y, 0, 0);
            Entity.Transform.Rotation = result;
        }

        private Vector2 GetLookAtAngle(Vector3 source, Vector3 destination)
        {
            Vector3 dist = source - destination;
            float altitude = (float)Math.Atan2(dist.Y, Math.Sqrt(dist.X * dist.X + dist.Z * dist.Z));
            float azimuth = (float)Math.Atan2(dist.X, dist.Z);
            return new Vector2(altitude, azimuth);
        }

        private float GetPlayerDistance()
        {
            return Vector3.Distance(GameManager.instance.player.Position , Entity.Transform.Position);
        }
    }
}
