using System.Collections.Generic;
using Stride.Engine;
using Furia.NPC.Animation;
using Stride.Audio;
using Furia.Player;
using Stride.Core;
using Furia.NPC.Controller;

namespace Furia.NPC.Stats
{
    public class NpcStats : SyncScript
    {
        public string npcName = "(optional)";
        public float health = 100;

        public float movementSpeed = 2.5f;
        public float damage = 10;
        public float attackRate = 1f; //In seconds
        [Display("Attack range")]
        public float stoppingDistance = 3; //Attack range
        public float detectRange = 10;

        public bool isRangeNPC = false; // Is a range unit/attacks from distance

        [Display("Projectiles (if the projectile is visible and avoidable)")]
        public List<Prefab> Projectiles = [];

        public int probabilityOfLoot = 100;

        [Display("accuaracy (% of hitting the player)")] //only works for range enemies
        public byte accuracy = 25;

        //Audio
        public Sound attackSound;
        public Sound hitSound;

        //Components
        private Npc2dAnimationController animationController;
        private AudioManager audioManager;

        public override void Start()
        {
            animationController = Entity.GetChild(0).Get<Npc2dAnimationController>();
            audioManager = Entity.Get<AudioManager>();
        }

        public override void Update()
        {
            // Do stuff every new frame
        }

        public void GetHit (float damageAmount)
        {
            health -= damageAmount;
            audioManager?.PlaySound(hitSound);
            Entity.Get<NpcAiController>().SetHit(true);
        }
    }
}
