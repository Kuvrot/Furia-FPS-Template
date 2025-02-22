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
        public byte weaponID = 0;
        public float MaxShootDistance = 100f;
        public float ShootImpulse = 5f;
        public float Cooldown = 0.3f;
        public float ReloadCooldown = 2.0f;
        public SpriteComponent RemainingBullets;
        public int remainingBullets = 0;
        public int maxBullets = 30;
        public int inventoryBullets = 100;
        public bool infiniteAmmo = false;
        public bool isMelee = false;
        public float damage = 50;

        public override void Start()
        {
            // Initialization of the script.
        }

        public override void Update()
        {
        }
    }
}
