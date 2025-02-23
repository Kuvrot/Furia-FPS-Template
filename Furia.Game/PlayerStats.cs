using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;

namespace Furia.Player
{
    public class PlayerStats : SyncScript
    {
        public float health = 100;

        //Components
        UiManager uiManager;

        public override void Start()
        {
            uiManager = Entity.GetChild(0).Get<UiManager>();
        }

        public override void Update()
        {
            uiManager.UpdateHealthBar(health);
        }

        public void GetHit(float damageAmount)
        {
            health -= damageAmount;
        }
    }
}
