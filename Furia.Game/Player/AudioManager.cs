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
    public class AudioManager : SyncScript
    {
        public Sound shotSound;
        private SoundInstance _shotSound;

        public override void Start()
        {
            
        }

        public override void Update()
        {
            // Do stuff every new frame
        }

        public void PlayShotSound ()
        {
            _shotSound = shotSound.CreateInstance();
            _shotSound.Play ();
        }
    }
}
