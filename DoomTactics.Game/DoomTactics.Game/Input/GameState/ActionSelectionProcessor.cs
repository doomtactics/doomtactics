using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DoomTactics
{
    public class ActionSelectionProcessor : GameStateBaseInputProcessor
    {
        private readonly ActionSelection _actionSelection;        

        public ActionSelectionProcessor(GameState gameState, ActionSelection actionSelection) 
            : base(gameState)
        {
            _actionSelection = actionSelection;
        }

        public override void ProcessInput(KeyboardState keyState, MouseState mouseState, GameTime gameTime)
        {
            if (MouseButtonClicked(mouseState.LeftButton, OldMouseState.LeftButton))
            {
                // GameState.ShowCurrentlyHoveredUnitStatus(new Vector2(mouseState.X, mouseState.Y));
            }
            if (KeyPressed(Keys.V, keyState))
            {
                _actionSelection.SwitchToFreeCamera();   
            }

            base.ProcessInput(keyState, mouseState, gameTime);
        }
    }
}
