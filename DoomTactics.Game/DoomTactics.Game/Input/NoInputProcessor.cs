using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DoomTactics
{
    public class NoInputProcessor : InputProcessor
    {
        public NoInputProcessor(KeyboardState keyState, MouseState mouseState) : base(keyState, mouseState)
        {
        }

        public override void ProcessInput(KeyboardState keyState, MouseState mouseState, GameTime gameTime)
        {
            
        }
    }
}
