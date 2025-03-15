using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;
using Stride.Physics;
using Furia.Player;
using Stride.Audio;
using Furia.Core;

namespace Furia.Interaction
{
    public class DoorScript : SyncScript
    {
        public int idDoor = 0;
        public float openAngle = 90;
        public bool isOpen = false, isLocked = false;
        public TransformComponent door;
        public Sound openDoorSound, closeDoorSound, lockedDoorSound;

        public StaticColliderComponent doorCollider;
        private AudioManager audioManager;

        public override void Start()
        {
            audioManager = Entity.Get<AudioManager>();
        }

        public override void Update()
        {
            if (GetPlayerDistance() < 2.5f)
            {
                DebugText.Print(GetPlayerDistance().ToString(), new Int2(500, 300));

                if (Input.IsKeyPressed(Keys.E))
                {
                    DoorInteraction();
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
        private float GetPlayerDistance()
        {
            return Vector3.Distance(GameManager.instance.player.Position, Entity.Transform.Position);
        }
    }
}
