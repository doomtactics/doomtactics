using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DoomTactics
{
    public interface IInputProcessor
    {
        KeyboardState OldKeyState { get; }
        MouseState OldMouseState { get; }
        void ProcessInput(KeyboardState keyState, MouseState mouseState, GameTime gameTime);
    }
}
