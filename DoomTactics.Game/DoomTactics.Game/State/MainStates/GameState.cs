using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DoomTactics.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DoomTactics
{
    public class GameState : IState
    {
        private readonly Camera _camera;
        private readonly DoomTacticsGame _gameInstance;
        private readonly BasicEffect _effect;
        //private readonly Cube _cube; 
        private readonly Tile _tile;
        private readonly Tile _tile2;
        private readonly Texture2D _temptex;

        public GameState(DoomTacticsGame gameInstance)
        {
            _camera = new Camera();
            _gameInstance = gameInstance;
            _effect = new BasicEffect(_gameInstance.GraphicsDevice);
            // _cube = new Cube(Vector3.Zero, new Vector3(1, 1, 1));
            _temptex = _gameInstance.Content.Load<Texture2D>("bubble");
            _tile = new Tile(_temptex, Vector3.Zero);
            _tile2 = new Tile(_temptex, new Vector3(0.0f, 0.0f, 1.0f));
        }

        public bool IsPaused
        {
            get { return false; }
        }

        public void Update(GameTime gameTime)
        {
            _camera.Position += new Vector3(0.01f, 0.02f, 0.03f);
        }

        public void Render(GraphicsDevice device)
        {
            _effect.World = Matrix.Identity;
            _effect.View = Matrix.CreateLookAt(_camera.Position, Vector3.Zero, Vector3.Up);
            _effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                                                                         device.Viewport.AspectRatio, 1.0f,
                                                                         1000.0f);

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
