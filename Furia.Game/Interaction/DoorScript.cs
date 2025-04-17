using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;
using Stride.Physics;
using Furia.Player;
using Stride.Audio;
using Furia.Core;
using System.Security.Cryptography.X509Certificates;

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
            CheckComponents();
        }

        public override void Update()
        {
            if (GetPlayerDistance() < 2.5f)
            {
                DebugText.Print("Press E to open", new Int2(500, 300));

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
               if (!IsKeyInPlayerInventory())
                {
                    audioManager.PlaySound(lockedDoorSound);
                    return;
                }

               isLocked = false;
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

        private bool IsKeyInPlayerInventory ()
        {
            for (int i = 0; i < GameManager.instance.player.Entity.Get<InventoryManager>().keys.Count; i++)
            {
                if (GameManager.instance.player.Entity.Get<InventoryManager>().keys[i] == idDoor)
                {
                    return true;
                }
            }

            return false;
        }

        private void CheckComponents()
        {
            if (audioManager == null || doorCollider == null)
            {
                DebugText.Print(Entity.Name + " has null components!!", new Int2(500, 300));
            }
        }
    }
}
