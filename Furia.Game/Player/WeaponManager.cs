using System.Collections.Generic;
using Stride.Input;
using Stride.Engine;
using Furia.Stats;
using static Stride.Graphics.GeometricPrimitives.GeometricPrimitive;

namespace Furia.Player
{
    public class WeaponManager : SyncScript
    {
        public bool enableFlashLight = true;
        public List<EntityComponent> Weapons = [];
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

        public void WeaponChange(int index)
        {
            if (Weapons.Count == 0)
            {
                return;
            }

            foreach (var weapon in Weapons)
            {
               if (weapon.Entity.Get<ModelComponent>() != null)
                {
                    weapon.Entity.Get<ModelComponent>().Enabled = false;
                }

                if (weapon.Entity.Get<SpriteComponent>() != null)
                {
                    weapon.Entity.Get<SpriteComponent>().Enabled = false;
                }
            }

            if (Weapons[index].Entity.Get<ModelComponent>() != null)
            {
                Weapons[index].Entity.Get<ModelComponent>().Enabled = true;
            }

            if (Weapons[index].Entity.Get<SpriteComponent>() != null)
            {
                Weapons[index].Entity.Get<SpriteComponent>().Enabled = true;
            }

            currentWeaponStats = Weapons[currentWeaponSelected].Entity.Get<WeaponStats>();
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
