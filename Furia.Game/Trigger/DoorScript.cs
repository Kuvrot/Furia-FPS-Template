using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;
using Microsoft.VisualBasic;
using Stride.Physics;
using Furia.Player;
using System.Diagnostics;
using Stride.Audio;

namespace Furia.Trigger
{
    public class DoorScript : SyncScript
    {
        public int idDoor = 0;
        public float openAngle = 90;
        public bool isOpen = false, isLocked = false;
        public TransformComponent door;
        public Sound openDoorSound , closeDoorSound, lockedDoorSound;

        public StaticColliderComponent doorCollider;
        private StaticColliderComponent collider;
        private AudioManager audioManager;

        public override void Start()
        {
            audioManager = Entity.Get<AudioManager>();
        }

        public override void Update() 
        {
            collider ??= Entity.Get<StaticColliderComponent>();

            foreach (var collision in collider.Collisions)
            {
                if (collision.ColliderA.Entity.Get<PlayerController>() != null)
                {
                    DebugText.Print("Press E to open", new Int2(500, 300));

                    if (Input.IsKeyPressed(Keys.E))
                    {
                        DoorInteraction();
                    }
                }
            }
        }

        private void DoorInteraction()
        {
            if (isLocked)
            {
                audioManager.PlaySound(lockedDoorSound);
                return;
            }
                
            if (!isOpen)
            {
                DebugText.Print("Open", new Int2(500, 300));
                Quaternion result = door.Entity.Transform.Rotation + Quaternion.RotationYawPitchRoll(openAngle, 0, 0);
                door.Entity.Transform.Rotation = result;
                doorCollider.Enabled = false;
                audioManager.PlaySound(openDoorSound);
                isOpen = true;
            }
            else
            {
                DebugText.Print("Closed", new Int2(500, 300));
                Quaternion result = door.Entity.Transform.Rotation - Quaternion.RotationYawPitchRoll(openAngle, 0, 0);
                door.Entity.Transform.Rotation = result;
                doorCollider.Enabled = true;
                audioManager.PlaySound(closeDoorSound);
                isOpen = false;
            }
        }
    }
}
