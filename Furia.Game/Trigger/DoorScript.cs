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

namespace Furia.Trigger
{
    public class DoorScript : SyncScript
    {
        public float openAngle = 90;
        public bool isOpen = false;
        public TransformComponent door;

        public StaticColliderComponent doorCollider;
        private static StaticColliderComponent collider;
        private float currentAngle = 0;

        public override void Start()
        {
            collider = Entity.Get<StaticColliderComponent>();
        }

        public override void Update() 
        {
            foreach (var collision in collider.Collisions)
            {
                if (collision.ColliderA.Entity.Name == "Player")
                {
                    if (Input.IsKeyPressed(Keys.E) && !isOpen)
                    {
                        DebugText.Print("Open", new Int2(500, 300));
                        Quaternion result = door.Entity.Transform.Rotation + Quaternion.RotationYawPitchRoll(openAngle, 0, 0);
                        door.Entity.Transform.Rotation = result;
                        doorCollider.Enabled = false;
                        isOpen = true;
                    }
                    else if (Input.IsKeyPressed(Keys.E) && isOpen)
                    {
                        DebugText.Print("Closed", new Int2(500, 300));
                        Quaternion result = door.Entity.Transform.Rotation - Quaternion.RotationYawPitchRoll(openAngle, 0, 0);
                        door.Entity.Transform.Rotation = result;
                        doorCollider.Enabled = true;
                        isOpen = false;
                    }
                }
            }
        }
    }
}
