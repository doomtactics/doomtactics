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
        private readonly TargetSelection _targetSelection;

        public TargetSelectionProcessor(KeyboardState keyState, MouseState mouseState, GameState gameState, TargetSelection targetSelection) 
            : base(keyState, mouseState, gameState)
        {
            _targetSelection = targetSelection;
        }

        public override void ProcessInput(KeyboardState keyState, MouseState mouseState, GameTime gameTime)
        {
            if (KeyPressed(Keys.V, keyState))
            {
                _targetSelection.ReturnToPrevious();
            }
            if (MouseButtonClicked(mouseState.LeftButton, OldMouseState.LeftButton))
            {
                _targetSelection.PerformAction();
            }

            base.ProcessInput(keyState, mouseState, gameTime);
        }
    }
}
