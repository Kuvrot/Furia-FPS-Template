using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;
using Stride.Physics;
using Furia.NPC.Animation;
using Furia.NPC.Stats;
using Furia.Core;
using Furia.Player;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Furia.NPC.Controller
{
    public class NpcAiController : SyncScript
    {
        // Declared public member fields and properties will show in the game studio
        private TransformComponent target;
        
        //Components
        private NpcStats stats;
        private Npc2dAnimationController animationController;
        private CharacterComponent characterComponent;
        private AudioManager audioManager;

        //Enemy properties
        private bool aggresiveMode = false;

        private double clock = 0;
        private float distance = 0;

        public override void Start()
        {
            animationController = Entity.GetChild(0).Get<Npc2dAnimationController>();
            stats = Entity.Get<NpcStats>();
            target = GameManager.instance.player;
            characterComponent = Entity.Get<CharacterComponent>();
            clock = new Random().NextDouble() * (stats.attackRate - 0.0f) + 0.0f;
            audioManager = Entity.Get<AudioManager>();
        }

        public override void Update()
        {
            LookTarget();

            if (stats.health > 0)
            {
                EnemyAiSystem();
            }
            else
            {
                KillNpc();
            }
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
            characterComponent.SetVelocity(new Vector3(direction.X, 0f, direction.Z) * 2.5f);
        }

        public void StopMoving()
        {
            characterComponent.SetVelocity(Vector3.Zero);
        }

        public void KillNpc()
        {
            StopMoving();
            animationController.PlayDeathAnimation();
        }

        public void EnemyAiSystem ()
        {
            if (aggresiveMode)
            {
                if (GetDistance() >= stats.stoppingDistance)
                {
                    MoveToTarget(this.target);
                    animationController.PlayWalkAnimation();
                }
                else
                {
                    StopMoving();
                    animationController.PlayAttackAnimation();
                    
                    if (Counter())
                    {
                        audioManager?.PlaySound(stats.attackSound);

                        if (!stats.isRangeNPC)
                        {
                            GameManager.instance.player.Entity.Get<PlayerStats>().GetHit(stats.damage);
                        }
                        else
                        {
                            if (new Random().Next() * (100 - 0) + 0 <= 25) // If it is a range npc, then his shots will have a 25% chance of impact
                            {
                                GameManager.instance.player.Entity.Get<PlayerStats>().GetHit(stats.damage);
                            }
                        }
                    }
                }
            }
            else
            {
                aggresiveMode = DetectTarget();
            }
        }

        private bool DetectTarget()
        {
            if (target != null)
            {
                if (GetDistance() <= stats.detectRange)
                {
                    return true;
                }
            }

            return false;
        }

        private float GetDistance()
        {
            return distance = Vector3.Distance(Entity.Transform.Position, target.Position);
        }

        private bool Counter()
        {
            if (clock >= stats.attackRate)
            {
                clock = 0;
                return true;
            }

            clock += 1 * (float)Game.UpdateTime.Elapsed.TotalSeconds;

            return false;
        }
    }
}
