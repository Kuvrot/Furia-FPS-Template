using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;

namespace Furia.Core
{
    public class GameManager : SyncScript
    {
        #region Singleton
        public static GameManager instance { get; private set; }

        public override void Start()
        {
            instance = this;
        }
        #endregion

        public bool showEnemyHealthBars = false;
        public TransformComponent player;

        public override void Update()
        {
            // Do stuff every new frame
        }
    }
}
