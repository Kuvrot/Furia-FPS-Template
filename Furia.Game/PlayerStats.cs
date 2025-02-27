using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;
using Stride.Audio;

namespace Furia.Player
{
    public class PlayerStats : SyncScript
    {
        public float health = 100;

        public Sound hitSound;

        //Components
        private UiManager uiManager;
        private AudioManager audioManager;

        public override void Start()
        {
            uiManager = Entity.GetChild(0).Get<UiManager>();
            audioManager = Entity.Get<AudioManager>();
        }

        public override void Update()
        {
            uiManager.UpdateHealthBar(health);
        }

        public void GetHit(float damageAmount)
        {
            health -= damageAmount;
            audioManager?.PlaySound(hitSound);
            uiManager.UpdateHitScreen();
        }
    }
}
