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
        public override void Start()
        {
            
        }

        public override void Update()
        {
            // Do stuff every new frame
        }

        public void PlaySound (Sound sound)
        {
            if (sound != null) {
                SoundInstance soundInstance = sound.CreateInstance();
                soundInstance.Play();
            }
        }

        public async void PlaySoundOnce(Sound sound)
        {
            if (sound != null)
            {
                SoundInstance soundInstance = sound.CreateInstance();
                await soundInstance.ReadyToPlay();
                soundInstance.Play();
            }
        }
    }
}
