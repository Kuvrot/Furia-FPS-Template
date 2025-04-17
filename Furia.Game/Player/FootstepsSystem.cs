using System.Collections.Generic;
using Stride.Engine;
using Stride.Audio;
using Stride.Core;
using System;

namespace Furia.Player
{
    public class FootstepsSystem : SyncScript
    {
        [Display("footsteps intervals (in seconds)")]
        public float soundSpeed = 0.25f;
        public List<Sound> sounds = [];

        private bool isWalking = false;
        private float clock = 0;
        private AudioManager audioManager;

        public override void Start()
        {
            audioManager = Entity.Get<AudioManager>();
        }

        public override void Update()
        {
            if (isWalking && sounds.Count > 0)
            {
                if (Counter())
                {
                    audioManager?.PlaySoundOnce(sounds[new Random().Next(0 , sounds.Count)]);
                }
            }
        }

        public void setWalking( bool boolean)
        {
            isWalking = boolean;
        }

        private bool Counter()
        {
            if (clock >= soundSpeed)
            {
                clock = 0;
                return true;
            }

            clock += 1 * (float)Game.UpdateTime.Elapsed.TotalSeconds;

            return false;
        }
    }
}
