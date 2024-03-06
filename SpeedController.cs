using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace YourProjectName
{
    internal sealed class ModEntry : Mod
    {
        // varialbes 
        private bool isKeyPressedK;
        private bool isKeyPressedL;
        private bool isPromptDisplayed;
        private uint promptDisplayTime = 420000; 
        private uint promptDisplayTimer; 

        // main init
        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
            helper.Events.Input.ButtonPressed += this.OnButtonPressed;
        }

        // event that runs every tick (60 times a second)
        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {

            if (Context.IsWorldReady && Game1.player.isRidingHorse())
            {
                // Check for keypresses and adjust player speed
                if (isKeyPressedK)
                {
                    SetPlayerSpeed(10); 
                    ShowPrompt("You are SUPER fast.");

                }
                else if (isKeyPressedL)
                {
                    SetPlayerSpeed(3); 
                    ShowPrompt("You are fast.");

                }
                else if (!isKeyPressedK && !isKeyPressedL)
                {
                    SetPlayerSpeed(1);
                    ShowPrompt("Running as normal.");
                }

            }

            // Check if the prompt is currently displayed and if it's time to hide it
            if (promptDisplayTimer > 0 && promptDisplayTimer >= promptDisplayTime * 60) // Convert seconds to ticks (60 ticks per second)
            {
                // Hide the prompt
                Game1.hudMessages.Clear();
                promptDisplayTimer = 0;
                isPromptDisplayed = false;
            }

            if (isPromptDisplayed)
            {
                promptDisplayTimer++;
            }
        }

        //looks for button press
        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            // ignore press if player hasn't loaded a save yet or is not on horse
            if (!Context.IsWorldReady && !Game1.player.isRidingHorse())
                return;
            
            Farmer player = Game1.player;

            if($"{e.Button}" == "K" && !isKeyPressedL && !isKeyPressedK)
            {
                SetPlayerSpeed(10);
                ShowPrompt("You are SUPER fast.");
                isKeyPressedK = !isKeyPressedK;

            }
            else if ($"{e.Button}" == "K" && !isKeyPressedL && isKeyPressedK)
            {
                SetPlayerSpeed(1);
                ShowPrompt("You are back to normal.");
                isKeyPressedK = !isKeyPressedK;

            }
            else if ($"{e.Button}" == "L" && !isKeyPressedK && !isKeyPressedL)
            {
                SetPlayerSpeed(3);
                ShowPrompt("You are fast.");
                isKeyPressedL = !isKeyPressedL;
            }
            else if ($"{e.Button}" == "L" && !isKeyPressedK && isKeyPressedL)
            {
                SetPlayerSpeed(1);
                ShowPrompt("You are back to normal.");
                isKeyPressedL = !isKeyPressedL;
            }
        }

        //sets the players speed
        private void SetPlayerSpeed(int speed)
        {
            // Get the player instance
            Farmer player = Game1.player;

            // Set the player's walking speed
            player.addedSpeed = speed;
            player.speed = speed;
        }

        // shows a prompt
        private void ShowPrompt(string message)
        {
            // Display the prompt message
            if (!isPromptDisplayed)
            {
                Game1.addHUDMessage(new HUDMessage(message, Color.White, 7000));
                isPromptDisplayed = true;
                promptDisplayTimer = 0;
            }
        }
    }
}