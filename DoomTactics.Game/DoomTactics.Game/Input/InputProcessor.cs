using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DoomTactics
{
    public abstract class InputProcessor : IInputProcessor
    {
        public KeyboardState OldKeyState { get; protected set; }

        public MouseState OldMouseState { get; protected set; }

        public InputProcessor(KeyboardState keyState, MouseState mouseState)
        {
            OldKeyState = keyState;
            OldMouseState = mouseState;
        }        

        public virtual void ProcessInput(KeyboardState keyState, MouseState mouseState, GameTime gameTime)
        {
            OldKeyState = keyState;
            OldMouseState = Mouse.GetState();
        }

        protected bool MouseButtonClicked(ButtonState newButtonState, ButtonState oldButtonState)
        {
            return (oldButtonState != ButtonState.Pressed && newButtonState == ButtonState.Pressed);
        }

        protected bool MouseButtonReleased(ButtonState newButtonState, ButtonState oldButtonState)
        {
            return (oldButtonState != ButtonState.Released && newButtonState == ButtonState.Released);
        }

        protected bool KeyPressed(Keys key, KeyboardState newKeyState)
        {
            return (newKeyState.IsKeyDown(key) && !OldKeyState.IsKeyDown(key));
        }

        protected bool KeyReleased(Keys key, KeyboardState newKeyState)
        {
            return (newKeyState.IsKeyUp(key) && !OldKeyState.IsKeyUp(key));
        }
    }
}
