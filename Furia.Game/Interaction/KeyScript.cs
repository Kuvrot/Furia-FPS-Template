using System;
using Stride.Core.Mathematics;
using Stride.Engine;
using Furia.Player;
using Stride.Core;
using Stride.Audio;
using Furia.Core;

namespace Furia.Interaction
{
    public class KeyScript : SyncScript
    {
        [Display ("Key ID, \n (must be the same as the door that unlocks)")]
        public int idKey = 0;
        [Display("Does the item look at the player?")]
        public bool itemRotation = true;

        public Sound pickUpSound;

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

            if (GetPlayerDistance() < 1.5f)
            {
                GameManager.instance.player.Entity.Get<InventoryManager>().keys.Add(idKey);
                audioManager.PlaySoundOnce(pickUpSound);
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
            return Vector3.Distance(GameManager.instance.player.Position, Entity.Transform.Position);
        }
    }
}
