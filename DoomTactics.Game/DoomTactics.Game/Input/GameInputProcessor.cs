using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DoomTactics.Input
{
    public class GameInputProcessor : IInputProcessor
    {
        public KeyboardState OldKeyState { get; set; }
        public MouseState OldMouseState { get; set; }
        private GameState _gameState;

        public GameInputProcessor(KeyboardState initialKey, MouseState initialMouse, GameState gameState)
        {
            OldKeyState = initialKey;
            OldMouseState = initialMouse;
            _gameState = gameState;
        }

        public void ProcessInput(KeyboardState keyState, MouseState mouseState, GameTime gameTime)
        {
            Vector2 mouseMove = new Vector2(mouseState.X - OldMouseState.X, mouseState.Y - OldMouseState.Y);
            if (mouseState.RightButton == ButtonState.Pressed)
            {                
                var cameraEvent = new CameraEvent(DoomEventType.CameraMoveEvent, mouseMove);
                MessagingSystem.DispatchEvent(cameraEvent);                 
            }
            else
            {
                bool forward = keyState.IsKeyDown(Keys.W);
                bool backward = keyState.IsKeyDown(Keys.S);
                bool right = keyState.IsKeyDown(Keys.D);
                bool left = keyState.IsKeyDown(Keys.A);
                var cameraEvent = new CameraEvent(DoomEventType.CameraMoveEvent, mouseMove, forward, backward, right,
                                                  left);
                MessagingSystem.DispatchEvent(cameraEvent);                
            }

            // lock mouse
            Mouse.SetPosition(100, 100);
            OldKeyState = keyState;
            OldMouseState = Mouse.GetState();
        }
       
    }
}
