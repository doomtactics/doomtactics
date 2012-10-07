using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DoomTactics
{
    public class TargetSelectionProcessor : GameStateBaseInputProcessor
    {
        public TargetSelectionProcessor(KeyboardState keyState, MouseState mouseState, GameState gameState, TargetSelection targetSelection) 
            : base(keyState, mouseState, gameState)
        {
        }

        public override void ProcessInput(KeyboardState keyState, MouseState mouseState, GameTime gameTime)
        {
            if (MouseButtonClicked(mouseState.LeftButton, OldMouseState.LeftButton))
            {
                // GameState.PerformActionOnHoveredTile(new Vector2(mouseState.X, mouseState.Y));
            }

            base.ProcessInput(keyState, mouseState, gameTime);
        }
    }
}
