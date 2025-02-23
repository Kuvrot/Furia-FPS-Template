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

namespace Furia.Player
{
    public class UiManager : SyncScript
    {

        public bool crossHair = true;
        public UIComponent UI;
        private UIPage Page;

        public override void Start()
        {
            Page = UI.Page;
        }

        public override void Update()
        {
            // Do stuff every new frame
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
    }
}
