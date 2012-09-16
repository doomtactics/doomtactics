using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DoomTactics.Input;
using DoomTactics.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DoomTactics
{
    public class GameState : IState
    {
        public readonly Camera Camera;
        private readonly DoomTacticsGame _gameInstance;
        private readonly BasicEffect _effect;
        private readonly Tile _tile;
        private readonly Tile _tile2;
        private readonly Texture2D _temptex;
        private readonly IInputProcessor _processor;
        
        public GameState(DoomTacticsGame gameInstance)
        {
            float aspectRatio = gameInstance.Window.ClientBounds.Width/gameInstance.Window.ClientBounds.Height;
            Camera = new Camera("camera", new Vector3(100.0f, 100.0f, 100.0f), Vector3.Zero, Vector3.Up, aspectRatio);
            MessagingSystem.Subscribe(Camera.MoveCamera, DoomEventType.CameraMoveEvent, "camera");
            _gameInstance = gameInstance;
            _effect = new BasicEffect(_gameInstance.GraphicsDevice);
            _temptex = _gameInstance.Content.Load<Texture2D>("bubble");
            _tile = new Tile(_temptex, Vector3.Zero);
            _tile2 = new Tile(_temptex, new Vector3(0.0f, 0.0f, 64.0f));
            _processor = new GameInputProcessor(Keyboard.GetState(), Mouse.GetState(), this);
        }

        public bool IsPaused
        {
            get { return false; }
        }

        public void Update(GameTime gameTime)
        {
            MessagingSystem.ProcessQueued();
            _processor.ProcessInput(Keyboard.GetState(), Mouse.GetState(), gameTime);
        }

        public void Render(GraphicsDevice device)
        {
            _effect.World = Matrix.Identity;
            _effect.View = Camera.View;
            _effect.Projection = Camera.Projection;

            _effect.TextureEnabled = true;
            _effect.Texture = _temptex;
            _effect.EnableDefaultLighting();

            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _tile.Render(device);
                _tile2.Render(device);
            }
            
        }

        public void ProcessInput(KeyboardState keyState, MouseState mouseState, GameTime gameTime)
        {
            
        }
    }
}
