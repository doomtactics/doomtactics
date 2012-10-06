using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DoomTactics
{
    public class FreeCameraInputProcessor : GameStateBaseInputProcessor
    {
        private readonly FreeCamera _freeCamera;

        public FreeCameraInputProcessor(KeyboardState keyState, MouseState mouseState, GameState gameState, FreeCamera freeCamera) 
            : base(keyState, mouseState, gameState)
        {
            _freeCamera = freeCamera;
        }

        public override void ProcessInput(KeyboardState keyState, MouseState mouseState, GameTime gameTime)
        {
            if (KeyPressed(Keys.V, keyState))
            {
                _freeCamera.SwitchToHudMode();
            }
            else
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

            base.ProcessInput(keyState, mouseState, gameTime);
        }
    }
}
