using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;

namespace Furia.NPC.Controller
{
    public class NpcAiController : SyncScript
    {
        // Declared public member fields and properties will show in the game studio
        public TransformComponent target;

        public override void Start()
        {
            // Initialization of the script.
        }

        public override void Update()
        {
            LookTarget();
            MoveToTarget(this.target);
        }

        public void LookTarget()
        {
            Vector2 lookAngle = GetLookAtAngle(Entity.Transform.Position, target.Position);
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

        public void MoveToTarget(TransformComponent target)
        {
            Vector3 direction = target.Position - Entity.Transform.Position;
            direction.Normalize();
            Entity.Transform.Position += new Vector3(direction.X , 0f, direction.Z) * 2.5f * (float)this.Game.UpdateTime.Elapsed.TotalSeconds;
        }
    }
}
