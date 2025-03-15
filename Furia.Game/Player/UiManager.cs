using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;
using Stride.UI.Controls;
using BulletSharp;
using Stride.Physics;
using Stride.UI.Panels;

namespace Furia.Player
{
    public class UiManager : SyncScript
    {

        public bool crossHair = true;
        public UIComponent UI;
        private UIPage Page;

        private bool getHit = false;
        private float counter = 0;
        private readonly float damageTime = 0.3f;

        public override void Start()
        {
            Page = UI.Page;

            Canvas dialogueCanvas  = Page.RootElement.FindName("dialoguePanel") as Canvas;
            dialogueCanvas.Opacity = 0;
        }

        public override void Update()
        {
            if (getHit)
            {
                counter += 1 * (float)Game.UpdateTime.Elapsed.TotalSeconds;

                if (counter >= damageTime)
                {
                    counter = 0;
                    Canvas hitCanvas = Page.RootElement.FindName("hitScreen") as Canvas;
                    hitCanvas.Opacity = 0;
                    getHit = false;
                }
            }
        }

        public void UpdateBulletCount(int currentBullets , int inventoryBullets)
        {
            TextBlock BulletCount = Page.RootElement.FindName("bulletCount") as TextBlock;
            if (BulletCount != null) {
                BulletCount.Text = currentBullets.ToString() + "/" + inventoryBullets.ToString();
            }
        }

        public void UpdateHealthBar(float health)
        {
            Slider healthBar = Page.RootElement.FindName("healthBar") as Slider;
            healthBar.Value = health;
        }

        public void UpdateHitScreen ()
        {
            Canvas hitCanvas = Page.RootElement.FindName("hitScreen") as Canvas;
            hitCanvas.Opacity = 0.57f;
            getHit = true;
        }
    }
}
