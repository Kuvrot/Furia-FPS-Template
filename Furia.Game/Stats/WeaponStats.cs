using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;
using Stride.Animations;
using Stride.Core;
using Stride.Graphics;
using Stride.Games;
using Stride.Rendering.Sprites;

namespace Furia.Stats
{
    public class WeaponStats : SyncScript
    {
        //Weapon properties
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

        //WeaponAnimations (optional)
        [Display("Spritesheet Animations (optional)")]
        public bool isSpriteSheetAnimation = true; // Controls if the weapon will use sprite animations
        public byte animationSpeed = 12; //Frames per second
        public byte reloadStartFrame = 0, reloadEndFrame = 0;
        public byte shootStartFrame = 0, shootEndFrame = 0;

        private byte currentStartFrame = 0, currentEndFrame = 0;
        private float clock = 0;

        private bool playingAnimation = false;

        //Components
        private SpriteComponent spriteComponent;
        private SpriteFromSheet spriteSheet;

        public override void Start()
        {
            //This is how you are supposed to set sprite frames https://doc.stride3d.net/4.0/en/manual/sprites/use-sprites.html
            spriteComponent = Entity.Get<SpriteComponent>();
            spriteSheet = spriteComponent.SpriteProvider as SpriteFromSheet;
        }

        public override void Update()
        {
            if (playingAnimation)
            {
                if (Counter())
                {
                    if (spriteSheet.CurrentFrame < currentEndFrame)
                    {
                        spriteSheet.CurrentFrame += 1;
                    }
                    else
                    {
                        spriteSheet.CurrentFrame = 0;
                        playingAnimation = false;
                    }
                }
            }
        }
        public void PlayReloadAnimation()
        {
            spriteSheet.CurrentFrame = reloadStartFrame;
            currentStartFrame = reloadStartFrame;
            currentEndFrame = reloadEndFrame;
            playingAnimation = true;
        }

        public void PlayShootAnimation()
        {
            spriteSheet.CurrentFrame = shootStartFrame;
            currentStartFrame = shootStartFrame;
            currentEndFrame = shootEndFrame;
            playingAnimation = true;
        }
        private bool Counter()
        {
            if (clock >= animationSpeed * 0.01)
            {
                clock = 0;
                return true;
            }

            clock += 1 * (float)Game.UpdateTime.Elapsed.TotalSeconds;

            return false;
        }
    }
}
