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
    public class WeaponManager : SyncScript
    {
        public bool enableFlashLight = true;

        public LightComponent flashLight;

        public override void Start()
        {
            // Initialization of the script.
        }

        public override void Update()
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
