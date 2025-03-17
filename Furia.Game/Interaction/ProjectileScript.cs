using Stride.Core.Mathematics;
using Stride.Engine;
using Furia.Core;
using Stride.Physics;
using Furia.Player;

namespace Furia.Interaction
{
    public class ProjectileScript : SyncScript
    {

        public float speed = 50;
        public float lifeTime = 7;
        public float damage = 10;

        private Vector3 shootDirection;
        private float distance;
        private float clock = 0;
        private RigidbodyComponent rigidbody;

        public override void Start()
        {
            shootDirection = (GameManager.instance.player.Entity.Transform.Position - Entity.Transform.Position);
            shootDirection.Normalize();
            rigidbody = Entity.Get<RigidbodyComponent>();
            rigidbody.ApplyForce(shootDirection);
            rigidbody.LinearVelocity = shootDirection * speed * (float)Game.UpdateTime.Elapsed.TotalSeconds;
        }

        public override void Update()
        {
            //  Entity.Transform.Position += shootDirection * speed * (float)Game.UpdateTime.Elapsed.TotalSeconds;
            distance = Vector3.Distance(GameManager.instance.player.Entity.Transform.Position , Entity.Transform.Position);

            if (distance <= 1.5f)
            {
                GameManager.instance.player.Entity.Get<PlayerStats>().GetHit(damage);
                Entity.Scene.Entities.Remove(Entity);
            }

            if (Counter())
            {
                Entity.Scene.Entities.Remove(Entity);
            }
        }

        private bool Counter()
        {
            if (clock >= lifeTime)
            {
                clock = 0;
                return true;
            }

            clock += 1 * (float)Game.UpdateTime.Elapsed.TotalSeconds;

            return false;
        }
    }
}
