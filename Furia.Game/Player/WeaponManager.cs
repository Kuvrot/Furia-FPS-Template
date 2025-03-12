using System.Collections.Generic;
using Stride.Input;
using Stride.Engine;
using Furia.Stats;

namespace Furia.Player
{
    public class WeaponManager : SyncScript
    {
        public bool enableFlashLight = true;
        public List<SpriteComponent> Weapons = [];
        public LightComponent flashLight;
        public byte currentWeaponSelected = 0;
        public WeaponScript weaponScript;
        public WeaponStats currentWeaponStats;
        public override void Start()
        {
            WeaponChange(currentWeaponSelected);
        }

        public override void Update()
        {
            FlashLightController();
            WeaponInventoryManagement();
            currentWeaponStats = Weapons[currentWeaponSelected].Entity.Get<WeaponStats>();
        }

        private void WeaponInventoryManagement()
        {
            if (Input.IsKeyPressed(Keys.Q))
            {
                if (currentWeaponSelected >= Weapons.Count - 1)
                {
                    currentWeaponSelected = 0;
                }
                else
                {
                    currentWeaponSelected++;
                }   
                WeaponChange(currentWeaponSelected);
            }
        }

        private void WeaponChange(int index)
        {
            if (Weapons.Count == 0)
            {
                return;
            }

            foreach (var weapon in Weapons)
            {
                weapon.Enabled = false;
            }

            Weapons[index].Enabled = true;
        }

        private void FlashLightController ()
        {
            if (enableFlashLight)
            {
                if (flashLight != null)
                {
                    if (Input.IsKeyPressed(Keys.F) && flashLight.Enabled)
                    {
                        flashLight.Enabled = false;
                    }
                    else if (Input.IsKeyPressed(Keys.F) && !flashLight.Enabled)
                    {
                        flashLight.Enabled = true;
                    }
                }
            }
        }
    }
}
