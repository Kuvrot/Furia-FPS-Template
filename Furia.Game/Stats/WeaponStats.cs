using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;
using Stride.Animations;

namespace Furia.Stats
{
    public class WeaponStats : SyncScript
    {
        public int weaponID = 0;
        public float MaxShootDistance { get; set; } = 100f;
        public float ShootImpulse { get; set; } = 5f;
        public float Cooldown { get; set; } = 0.3f;
        private float cooldownRemaining = 0;
        public float ReloadCooldown { get; set; } = 2.0f;
        public SpriteComponent RemainingBullets { get; set; }
        public int remainingBullets = 0;
        public int maxBullets = 30;
        public bool infiniteBullets = false;
        public float damage = 50;


        public AnimationClip AnimationIdle { get; set; }
        public AnimationClip AnimationWalk { get; set; }
        public AnimationClip AnimationShoot { get; set; }
        public AnimationClip AnimationReload { get; set; }

        public override void Start()
        {
            // Initialization of the script.
        }

        public override void Update()
        {
            // Do stuff every new frame
        }
    }
}
