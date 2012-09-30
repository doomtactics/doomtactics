﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DoomTactics.Data;
using DoomTactics.Input;
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
        private Level _tempLevel;
        private IInputProcessor _processor;
        private SpriteBatch _spriteBatch;
        private BasicEffect _spriteEffect;
        private AlphaTestEffect _alphaTestEffect;
        //private IList<ActorBase> _actors;
        private IState _nextState;
        public ControlScheme CurrentControlScheme;

        public GameState(DoomTacticsGame gameInstance)
        {
            _gameInstance = gameInstance;
            _nextState = null;
        }

        public void OnEnter()
        {
            // setup
            SpriteSheetFactory.Initialize(_gameInstance.Content);
            HardcodedAnimations.CreateAnimations();

            // control scheme
            CurrentControlScheme = ControlScheme.FreeCamera;

            // camera
            float aspectRatio = (float)_gameInstance.Window.ClientBounds.Width / _gameInstance.Window.ClientBounds.Height;
            Camera = new Camera("camera", new Vector3(-96f, 32f, 32f), new Vector3(32.0f, 32.0f, 32.0f), Vector3.Up, aspectRatio);
            MessagingSystem.Subscribe(Camera.MoveCamera, DoomEventType.CameraMoveEvent, "camera");
            
            _effect = new BasicEffect(_gameInstance.GraphicsDevice);
            //_tile = new Tile(_temptex, Vector3.Zero);
            //_tile2 = new Tile(_temptex, new Vector3(0.0f, 0.0f, 64.0f));
            _processor = new GameInputProcessor(Keyboard.GetState(), Mouse.GetState(), this);

            CreateLevelTemp(_gameInstance.Content);

            var imptex = _gameInstance.Content.Load<Texture2D>("sheets\\impsheet");

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

            foreach (var actor in _tempLevel.Actors)
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

            foreach (var tile in _tempLevel.Tiles)
            {
                tile.Render(device, _effect);             
            }

            _spriteEffect.TextureEnabled = true;
            _spriteEffect.VertexColorEnabled = true;

            //_spriteBatch.Begin(0, null, null, DepthStencilState.DepthRead, RasterizerState.CullNone, _alphaTestEffect);
            // Pass 1: full alpha
            _alphaTestEffect.AlphaFunction = CompareFunction.Greater;
            _alphaTestEffect.ReferenceAlpha = 128;                
            foreach (var actor in _tempLevel.Actors)
            {
                actor.Render(device, _spriteBatch, _alphaTestEffect, Camera, 0);
            }
            // Pass 2: alpha blend
            foreach (var actor in _tempLevel.Actors)
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
            var tempLevelData = HardcodedTestLevel.CreateLevel();
            _tempLevel = LevelFactory.CreateLevel(contentManager, tempLevelData);
        }
    }
}
