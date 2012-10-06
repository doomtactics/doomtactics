using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DoomTactics
{
    public class GameStateBaseInputProcessor : InputProcessor
    {
        protected readonly GameState GameState;

        public GameStateBaseInputProcessor(KeyboardState keyState, MouseState mouseState, GameState gameState)
            : base(keyState, mouseState)
        {
            GameState = gameState;
        }

        public override void ProcessInput(KeyboardState keyState, MouseState mouseState, GameTime gameTime)
        {
            if (KeyPressed(Keys.Escape, keyState))
            {
                GameState.ReturnToMainMenu();
            }

            base.ProcessInput(keyState, mouseState, gameTime);
        }
    }
}
