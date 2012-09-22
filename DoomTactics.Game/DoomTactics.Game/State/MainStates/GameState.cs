﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DoomTactics.Data;
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
        public Camera Camera;
        private DoomTacticsGame _gameInstance;
        private BasicEffect _effect;
        private Tile[] _tempLevel;
        private IInputProcessor _processor;
        private SpriteBatch _spriteBatch;
        private BasicEffect _spriteEffect;
        private AlphaTestEffect _alphaTestEffect;
        private IList<ActorBase> _actors;
        private IState _nextState;
        
        public GameState(DoomTacticsGame gameInstance)
        {
            _gameInstance = gameInstance;
            _nextState = null;
        }

        public void OnEnter()
        {
            HardcodedAnimations.CreateAnimations();
            float aspectRatio = _gameInstance.Window.ClientBounds.Width / _gameInstance.Window.ClientBounds.Height;
            Camera = new Camera("camera", new Vector3(100.0f, 100.0f, 100.0f), Vector3.Zero, Vector3.Up, aspectRatio);
            MessagingSystem.Subscribe(Camera.MoveCamera, DoomEventType.CameraMoveEvent, "camera");
            _gameInstance = _gameInstance;
            _effect = new BasicEffect(_gameInstance.GraphicsDevice);
            //_tile = new Tile(_temptex, Vector3.Zero);
            //_tile2 = new Tile(_temptex, new Vector3(0.0f, 0.0f, 64.0f));
            _processor = new GameInputProcessor(Keyboard.GetState(), Mouse.GetState(), this);

            CreateLevelTemp(_gameInstance.Content);

            var imptex = _gameInstance.Content.Load<Texture2D>("sheets\\impsheet");
            _actors = new List<ActorBase>();
            /*for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    var imp = new Imp("imp", new Vector3(32.0f + 64.0f*j, 0, 32.0f + 64.0f*i), imptex);
                    _actors.Add(imp);
                }
            }*/
            var imp = new Imp("imp", new Vector3(160.0f, 0, 160.0f), imptex);
            var imp2 = new Imp("imp", new Vector3(224.0f, 0.0f, 224.0f), imptex);
            _actors.Add(imp);
            _actors.Add(imp2);

            _spriteBatch = new SpriteBatch(_gameInstance.GraphicsDevice);
            _spriteEffect = new BasicEffect(_gameInstance.GraphicsDevice);
            _alphaTestEffect = new AlphaTestEffect(_gameInstance.GraphicsDevice);
        }

        public void OnExit()
        {
            
        }

        public bool IsPaused
        {
            get { return false; }
        }

        public IState Update(GameTime gameTime)
        {
            MessagingSystem.ProcessQueued();
            _processor.ProcessInput(Keyboard.GetState(), Mouse.GetState(), gameTime);

            if (_nextState != null)
                return _nextState;

            foreach (var actor in _actors)
                actor.Update(gameTime);

            return _nextState;
        }

        public void ShowMainMenu()
        {
            _nextState = DoomTacticsGame.CreateMenuState(_gameInstance);
        }

        public void Render(GraphicsDevice device)
        {
            _effect.World = Matrix.Identity;
            _effect.View = Camera.View;
            _effect.Projection = Camera.Projection;

            _effect.TextureEnabled = true;
            _effect.EnableDefaultLighting();

            device.DepthStencilState = DepthStencilState.Default;

            foreach (var tile in _tempLevel)
            {
                foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
                {
                    _effect.Texture = tile.Texture;
                    pass.Apply();
                    tile.Render(device);
                }
            }

            _spriteEffect.TextureEnabled = true;
            _spriteEffect.VertexColorEnabled = true;

            //_spriteBatch.Begin(0, null, null, DepthStencilState.DepthRead, RasterizerState.CullNone, _alphaTestEffect);
            // Pass 1: full alpha
            _alphaTestEffect.AlphaFunction = CompareFunction.Greater;
            _alphaTestEffect.ReferenceAlpha = 128;                
            foreach (var actor in _actors)
            {
                actor.Render(device, _spriteBatch, _alphaTestEffect, Camera, 0);
            }
            // Pass 2: alpha blend
            foreach (var actor in _actors)
            {
                _alphaTestEffect.AlphaFunction = CompareFunction.Less;
                _alphaTestEffect.ReferenceAlpha = 20;
                actor.Render(device, _spriteBatch, _alphaTestEffect, Camera, 1);
            }
            //_spriteBatch.End();
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
                    _tempLevel[i * levelSize + j] = new Tile(texture, position, 32.0f);
                }
            }

            // overwrite stuff with some specific tiles
            Texture2D text = contentManager.Load<Texture2D>("textures\\GRNROCK");
            _tempLevel[35] = new Tile(text, new Vector3(5 * 64.0f, 24.0f, 3 * 64.0f), 56.0f);
            _tempLevel[36] = new Tile(text, new Vector3(6 * 64.0f, 48.0f, 3 * 64.0f), 80.0f);
            _tempLevel[45] = new Tile(text, new Vector3(5 * 64.0f, 72.0f, 4 * 64.0f), 104.0f);
            _tempLevel[46] = new Tile(text, new Vector3(6 * 64.0f, 96.0f, 4 * 64.0f), 128.0f);
        }
    }
}
