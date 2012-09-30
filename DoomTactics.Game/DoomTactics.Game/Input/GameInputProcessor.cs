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
            if (_gameState.CurrentControlScheme == ControlScheme.FreeCamera)
            {
                ProcessFreeCameraControls(keyState, mouseState);
            }
            else
            {
                ProcessHudControls(keyState, mouseState);
            }

            if (KeyPressed(Keys.V, keyState))
            {
                if (_gameState.CurrentControlScheme == ControlScheme.FreeCamera)
                {
                    _gameState.ShowHud();
                    _gameState.CurrentControlScheme = ControlScheme.Locked;                    
                }
                else
                {
                    _gameState.HideHud();
                    _gameState.CurrentControlScheme = ControlScheme.FreeCamera;
                }                
            }

            if (KeyPressed(Keys.Escape, keyState))
            {
                _gameState.ReturnToMainMenu(); 
            }

            OldKeyState = keyState;
            OldMouseState = Mouse.GetState();
        }
       
        private void ProcessFreeCameraControls(KeyboardState keyState, MouseState mouseState)
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
        }

        private void ProcessHudControls(KeyboardState keyState, MouseState mouseState)
        {
            
        }

        private bool KeyPressed(Keys key, KeyboardState newKeyState)
        {
            return (newKeyState.IsKeyDown(key) && !OldKeyState.IsKeyDown(key));
        }

        private bool KeyReleased(Keys key, KeyboardState newKeyState)
        {
            return (newKeyState.IsKeyUp(key) && !OldKeyState.IsKeyUp(key));
        }

    }
}
