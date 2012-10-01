using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DoomTactics.Controls;
using DoomTactics.Data;
using DoomTactics.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NLog;

namespace DoomTactics
{
    public class GameState : IState
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public Camera Camera;
        private DoomTacticsGame _gameInstance;
        private readonly DoomDesktop _desktop;
        private readonly SquidInputManager _squidInputManager;
        private BasicEffect _effect;
        private Level _level;
        private IInputProcessor _processor;
        private SpriteBatch _spriteBatch;
        private BasicEffect _spriteEffect;
        private HighlightEffectContainer _highlightingEffectContainer;
        private AlphaTestEffect _alphaTestEffect;
        //private IList<ActorBase> _actors;
        private IState _nextState;
        public ControlScheme CurrentControlScheme;

        public GameState(DoomTacticsGame gameInstance, SquidInputManager squidInputManager)
        {
            _gameInstance = gameInstance;
            _desktop = new DoomDesktop();            
            _squidInputManager = squidInputManager;
            _nextState = null;
        }

        public void OnEnter()
        {
            // setup
            SpriteSheetFactory.Initialize(_gameInstance.Content);
            HardcodedAnimations.CreateAnimations();

            // control scheme
            CurrentControlScheme = ControlScheme.FreeCamera;
            _desktop.Visible = false;

            // camera
            float aspectRatio = (float)_gameInstance.Window.ClientBounds.Width / _gameInstance.Window.ClientBounds.Height;
            Camera = new Camera("camera", new Vector3(-96f, 32f, 32f), new Vector3(32.0f, 32.0f, 32.0f), Vector3.Up, aspectRatio);
            MessagingSystem.Subscribe(Camera.MoveCamera, DoomEventType.CameraMoveEvent, "camera");
            
            _effect = new BasicEffect(_gameInstance.GraphicsDevice);
            //_tile = new Tile(_temptex, Vector3.Zero);
            //_tile2 = new Tile(_temptex, new Vector3(0.0f, 0.0f, 64.0f));
            _processor = new GameInputProcessor(Keyboard.GetState(), Mouse.GetState(), this);

            CreateLevelTemp(_gameInstance.Content);

            _spriteBatch = new SpriteBatch(_gameInstance.GraphicsDevice);
            _spriteEffect = new BasicEffect(_gameInstance.GraphicsDevice);
            _highlightingEffectContainer = new HighlightEffectContainer(_gameInstance.Content);            
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

            if (CurrentControlScheme == ControlScheme.Locked)
            {
                _squidInputManager.Update(gameTime);
                _desktop.Update();
            }

            _processor.ProcessInput(Keyboard.GetState(), Mouse.GetState(), gameTime);

            if (_nextState != null)
                return _nextState;

            foreach (var actor in _level.Actors)
                actor.Update(gameTime);

            return _nextState;
        }

        public void ShowHud()
        {
            _desktop.Visible = true;
            _desktop.ShowCursor = true;
        }

        public void HideHud()
        {
            _desktop.Visible = false;
            _desktop.ShowCursor = false;
        }

        public void ReturnToMainMenu()
        {
            _nextState = DoomTacticsGame.CreateMenuState(_gameInstance);
        }

        public void Render(GraphicsDevice device)
        {          
            _level.DrawBackground(device, _spriteBatch);

            _effect.World = Matrix.Identity;
            _effect.View = Camera.View;
            _effect.Projection = Camera.Projection;
            
            _effect.TextureEnabled = true;
            _effect.EnableDefaultLighting();

            device.RasterizerState = RasterizerState.CullNone;
            device.DepthStencilState = DepthStencilState.Default;

            Tile highlightedTile = FindHighlightedTile();
            foreach (var tile in _level.Tiles)
            {
                bool isHighlighted = (tile == highlightedTile);
                tile.Render(device, _effect, _highlightingEffectContainer.GetEffect(), isHighlighted);
            }

            _spriteEffect.TextureEnabled = true;
            _spriteEffect.VertexColorEnabled = true;

            //_spriteBatch.Begin(0, null, null, DepthStencilState.DepthRead, RasterizerState.CullNone, _alphaTestEffect);
            // Pass 1: full alpha
            _alphaTestEffect.AlphaFunction = CompareFunction.Greater;
            _alphaTestEffect.ReferenceAlpha = 128;                
            foreach (var actor in _level.Actors)
            {                
                actor.Render(device, _spriteBatch, _alphaTestEffect, Camera, 0);
            }
            // Pass 2: alpha blend
            foreach (var actor in _level.Actors)
            {
                _alphaTestEffect.AlphaFunction = CompareFunction.Less;
                _alphaTestEffect.ReferenceAlpha = 20;
                actor.Render(device, _spriteBatch, _alphaTestEffect, Camera, 1);
            }
            //_spriteBatch.End();

            if (_desktop.Visible)
            {
                _desktop.Size = new Squid.Point(device.Viewport.Width, device.Viewport.Height);
                _desktop.Draw();
            }

        }

        public Tile FindHighlightedTile()
        {
            if (CurrentControlScheme == ControlScheme.Locked)
            {
                Vector2 mousePosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                Vector3 nearpoint = new Vector3(mousePosition, 0);
                Vector3 farpoint = new Vector3(mousePosition, 1.0f);

                nearpoint = _gameInstance.GraphicsDevice.Viewport.Unproject(nearpoint, Camera.Projection, Camera.View,
                                                                       Matrix.Identity);
                farpoint = _gameInstance.GraphicsDevice.Viewport.Unproject(farpoint, Camera.Projection, Camera.View,
                                                                            Matrix.Identity);

                Vector3 direction = Vector3.Normalize(farpoint - nearpoint);
                Ray ray = new Ray(nearpoint, direction);

                foreach (var tile in _level.Tiles)
                {
                    if (ray.Intersects(tile.CreateBoundingBox()).HasValue)
                    {
                        return tile;
                    }
                }
            }
            return null;
        }

        private void CreateLevelTemp(ContentManager contentManager)
        {
            var tempLevelData = HardcodedTestLevel.CreateLevel();
            _level = LevelFactory.CreateLevel(contentManager, tempLevelData);
        }

        public void ShowCurrentlyHoveredUnitStatus(Vector2 mousePosition)
        {
            var actor = SelectCurrentlyHoveredUnit(mousePosition);            
            ShowUnitStatus(actor);
        }

        private void ShowUnitStatus(ActorBase actor)
        {
            if (actor != null)
            {
                // create status window
                var actorInfoWindow = new DoomWindow();                
                actorInfoWindow.TitleBar.Text = actor.ActorID;
                actorInfoWindow.Size = new Squid.Point(200, 200);
                actorInfoWindow.Position = new Squid.Point(50, 100);
                actorInfoWindow.Parent = _desktop;
            }
        }

        private ActorBase SelectCurrentlyHoveredUnit(Vector2 mousePosition)
        {            
            Vector3 nearpoint = new Vector3(mousePosition, 0);
            Vector3 farpoint = new Vector3(mousePosition, 1.0f);

            nearpoint = _gameInstance.GraphicsDevice.Viewport.Unproject(nearpoint, Camera.Projection, Camera.View,
                                                                        Matrix.Identity);
            farpoint = _gameInstance.GraphicsDevice.Viewport.Unproject(farpoint, Camera.Projection, Camera.View,
                                                                        Matrix.Identity);

            Vector3 direction = Vector3.Normalize(farpoint - nearpoint);
            Ray ray = new Ray(nearpoint, direction);
            ActorBase intersected = null;
            float selectedDistance = float.MaxValue;

            foreach (var actor in _level.Actors)
            {
                float? intersectionResult = ray.Intersects(actor.CreateBoundingBox());

                if (intersectionResult.HasValue && selectedDistance > intersectionResult.Value)
                {
                    selectedDistance = intersectionResult.Value;
                    intersected = actor;
                }
            }

            if (intersected != null)            
                Log.Debug("Intersected with " + intersected.ActorID);

            return intersected;
        }
    }
}
