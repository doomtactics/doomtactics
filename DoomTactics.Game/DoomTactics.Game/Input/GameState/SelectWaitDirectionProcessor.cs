using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DoomTactics
{
    public class SelectWaitDirectionProcessor : GameStateBaseInputProcessor
    {
        private readonly SelectWaitDirection _selectWaitDirection;

        public SelectWaitDirectionProcessor(KeyboardState keyState, MouseState mouseState, GameState gameState, SelectWaitDirection selectWaitDirection) : base(keyState, mouseState, gameState)
        {
            _selectWaitDirection = selectWaitDirection;
        }

        public override void ProcessInput(KeyboardState keyState, MouseState mouseState, GameTime gameTime)
        {
            if (MouseButtonClicked(mouseState.LeftButton, OldMouseState.LeftButton))
            {
                _selectWaitDirection.FinalizeWaitDirection(new Vector2(mouseState.X, mouseState.Y));
            }
            base.ProcessInput(keyState, mouseState, gameTime);
        }
    }
}
