﻿using System;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Audio;
using Stride.Core;
using Furia.Player;
using Furia.Core;
using Stride.Input;
using System.Collections.Generic;
using Stride.UI.Panels;
using Stride.UI.Controls;

namespace Furia.Interaction
{
    public class DialogueSystem : SyncScript
    {
        [Display("Does the character looks at the player?")]
        public bool rotate = true;

        [Display("isManualInteraction (leave true if you have to press a key to start the dialogue)")]
        public bool isManualInteraction = true;

        [Display("dialogue interval (in seconds)")]
        public float dialogueInterval = 1;

        public List<String> dialogues = [];

        [Display("dialogueSound, \n plays a sound for each dialogue, this list and the dialogues list have the same index, \n leave null if not nedeed")]
        public List<Sound> dialogueSound = [];

        private AudioManager audioManager;
        private float clock = 0;
        private bool isDialogueStarted = false;
        private bool isDialogueEnded = false;
        private int dialogueIndex = 0;
        private UIPage page;
        private Canvas dialogueCanvas;
        private TextBlock dialogueText;

        public override void Start()
        {
            audioManager = Entity.Get<AudioManager>();
            page = GameManager.instance.ui.Page;
            dialogueCanvas = page.RootElement.FindName("dialoguePanel") as Canvas;
            dialogueText = page.RootElement.FindName("dialogueText") as TextBlock;
        }

        public override void Update()
        {
            if (rotate)
            {
                RotateCharacter();
            }

            if (GetPlayerDistance() < 3f && !isDialogueStarted)
            {
                if (isManualInteraction)
                {
                    if (Input.IsKeyPressed(Keys.E))
                    {
                        dialogueCanvas.Opacity = 1;
                        isDialogueStarted = true;
                    }
                }
                else
                {
                    dialogueCanvas.Opacity = 1;
                    isDialogueStarted = true;
                }

                clock = dialogueInterval;
            }

            if (isDialogueStarted)
            {
                StartDialogue();
            }
        }

        private void StartDialogue ()
        {
            if (Counter())
            {
                if (dialogueIndex < dialogues.Count)
                {
                    displayDialogue(dialogueIndex);
                    dialogueIndex++;
                }
                else
                {
                    dialogueCanvas.Opacity = 0;
                    
                    isDialogueEnded = true;

                    if (isDialogueStarted)
                    {
                        if (GetPlayerDistance() > 3f)
                        {
                            isDialogueStarted = false;
                        }
                    }

                }
            }
        }

        private void RotateCharacter()
        {
            Vector2 lookAngle = GetLookAtAngle(Entity.Transform.Position, GameManager.instance.player.Position);
            Quaternion result = Quaternion.RotationYawPitchRoll(lookAngle.Y, 0, 0);
            Entity.Transform.Rotation = result;
        }

        private Vector2 GetLookAtAngle(Vector3 source, Vector3 destination)
        {
            Vector3 dist = source - destination;
            float altitude = (float)Math.Atan2(dist.Y, Math.Sqrt(dist.X * dist.X + dist.Z * dist.Z));
            float azimuth = (float)Math.Atan2(dist.X, dist.Z);
            return new Vector2(altitude, azimuth);
        }

        private float GetPlayerDistance()
        {
            return Vector3.Distance(GameManager.instance.player.Position, Entity.Transform.Position);
        }

        private bool Counter()
        {
            if (clock >= dialogueInterval)
            {
                clock = 0;
                return true;
            }

            clock += 1 * (float)Game.UpdateTime.Elapsed.TotalSeconds;

            return false;
        }

        private void displayDialogue(int index)
        {
            dialogueCanvas.Opacity = 1;
            
            if (dialogues.Count > 0)
            {
                dialogueText.Text = dialogues[index];
            }
            
            if (dialogueSound.Count > 0)
            {
                audioManager.PlaySoundOnce(dialogueSound[index]);
            }
        }
    }
}