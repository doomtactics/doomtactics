using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DoomTactics.Input;
using DoomTactics.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
        private Tile[] _tempLevel;
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

            CreateLevelTemp(gameInstance.Content);
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

            foreach (var tile in _tempLevel)
            {
                foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
                {
                    _effect.Texture = tile.Texture;
                    pass.Apply();
                    tile.Render(device);
                }
            }
        }

        private void CreateLevelTemp(ContentManager contentManager)
        {
            const int levelSize = 10;
            _tempLevel = new Tile[levelSize * levelSize];
            string[] textureNames = new[] {"textures\\FLAT1_1", "textures\\FLAT5_4", "textures\\FLAT5_5"};
            var random = new Random();
            for (int i = 0; i < levelSize; i++)
            {
                for (int j = 0; j < levelSize; j++)
                {
                    int num = random.Next(0, 3);
                    Texture2D texture = contentManager.Load<Texture2D>(textureNames[num]);
                    Vector3 position = new Vector3(j * 64.0f, 0.0f, i * 64.0f);
                    _tempLevel[i * levelSize + j] = new Tile(texture, position);
                }
            }

            // overwrite stuff with some specific tiles
            Texture2D text = contentManager.Load<Texture2D>("textures\\GRNROCK");
            _tempLevel[35] = new Tile(text, new Vector3(5 * 64.0f, 24.0f, 3 * 64.0f));
            _tempLevel[36] = new Tile(text, new Vector3(6 * 64.0f, 48.0f, 3 * 64.0f));
            _tempLevel[45] = new Tile(text, new Vector3(5 * 64.0f, 72.0f, 4 * 64.0f));
            _tempLevel[46] = new Tile(text, new Vector3(6 * 64.0f, 96.0f, 4 * 64.0f));
        }

        public void ProcessInput(KeyboardState keyState, MouseState mouseState, GameTime gameTime)
        {
            
        }
    }
}
