using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;
using Silk.NET.Core;
using Furia.NPC.Controller;
using Furia.NPC.Animation;

namespace Furia.NPC.Stats
{
    public class NpcStats : SyncScript
    {
        public string npcName = "(optional)";
        public float health = 100;

        public float movementSpeed = 2.5f;
        public float damage = 10;
        public float attackRate = 1f; //In seconds

        //Components
        private Npc2dAnimationController animationController;

        public override void Start()
        {
            animationController = Entity.GetChild(0).Get<Npc2dAnimationController>();
        }

        public override void Update()
        {
            // Do stuff every new frame
        }

        public void GetHit (float damageAmount)
        {
            health -= damageAmount;
        }
    }
}
