using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DoomTactics
{
    public class MenuState : IState
    {
        public static string[] mainMenuItems = new[]
        {
            "NEW GAME",
            "CONTINUE",
            "MULTIPLAYER",
            "OPTIONS",
            "QUIT"
        };

        public bool IsPaused
        {
            get { return false; }
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public void Render(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public void ProcessInput(KeyboardState keyState, MouseState mouseState, GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
