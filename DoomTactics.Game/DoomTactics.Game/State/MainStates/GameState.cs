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
using Squid;

namespace DoomTactics
{
    public class GameState : IState
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public Camera Camera;
        public Level Level;

        private readonly DoomTacticsGame _gameInstance;
        private readonly DoomDesktop _desktop;
        private readonly SquidInputManager _squidInputManager;
        private BasicEffect _effect;
        private IInputProcessor _processor;
        private SpriteBatch _spriteBatch;
        private BasicEffect _spriteEffect;
        private HighlightEffectContainer _highlightingEffectContainer;
        private AlphaTestEffect _alphaTestEffect;
        private IState _nextState;
        private ActorBase _activeUnit;
        private IDictionary<ActorType, Func<Vector3, Vector3, ActorBase>> _spawnMethods;
        private readonly StateMachine _stateMachine;


        public GameState(DoomTacticsGame gameInstance, SquidInputManager squidInputManager)
        {
            _gameInstance = gameInstance;
            _desktop = new DoomDesktop();
            _squidInputManager = squidInputManager;
            _nextState = null;
            _stateMachine = new StateMachine(new FreeCamera(_desktop, this));
            _spawnMethods = new Dictionary<ActorType, Func<Vector3, Vector3, ActorBase>>();
            CreateSpawnMethodsTemp();
        }

        private void CreateSpawnMethodsTemp()
        {
            _spawnMethods.Add(ActorType.ImpFireball, (p, v) =>
                                                        {
                                                            var fireball = new ImpFireball("fireball");
                                                            fireball.Position = p;
                                                            fireball.Velocity = v;
                                                            return fireball;
                                                        });
        }

        public void OnEnter()
        {
            // setup
            SpriteSheetFactory.Initialize(_gameInstance.Content);
            HardcodedAnimations.CreateAnimations();            

            // camera
            float aspectRatio = (float) _gameInstance.Window.ClientBounds.Width/_gameInstance.Window.ClientBounds.Height;
            Camera = new Camera("camera", new Vector3(-96f, 32f, 32f), new Vector3(32.0f, 32.0f, 32.0f), Vector3.Up,
                                aspectRatio);
            MessagingSystem.Subscribe(Camera.MoveCamera, DoomEventType.CameraMoveEvent, "camera");
            MessagingSystem.Subscribe(OnChargeTimeReached, DoomEventType.ChargeTimeReached, "gamestate");
            MessagingSystem.Subscribe(OnActorSpawn, DoomEventType.SpawnActor, "gamestate");

            _effect = new BasicEffect(_gameInstance.GraphicsDevice);
            _processor = new GameInputProcessor(Keyboard.GetState(), Mouse.GetState(), this);

            CreateLevelTemp(_gameInstance.Content);

            _spriteBatch = new SpriteBatch(_gameInstance.GraphicsDevice);
            _spriteEffect = new BasicEffect(_gameInstance.GraphicsDevice);
            _highlightingEffectContainer = new HighlightEffectContainer(_gameInstance.Content);
            _alphaTestEffect = new AlphaTestEffect(_gameInstance.GraphicsDevice);
        }

        public void OnExit()
        {
            MessagingSystem.Unsubscribe("camera");
            MessagingSystem.Unsubscribe("gamestate");
        }

        public bool IsPaused
        {
            get { return false; }
        }

        public IState Update(GameTime gameTime)
        {
            MessagingSystem.ProcessQueued();

            _stateMachine.Update(gameTime);

            if (_nextState != null)
                return _nextState;

            return _nextState;
        }

        public void ReturnToMainMenu()
        {
            _nextState = DoomTacticsGame.CreateMenuState(_gameInstance);
        }

        public void Render(GraphicsDevice device)
        {
            Level.DrawBackground(device, _spriteBatch);

            _effect.World = Matrix.Identity;
            _effect.View = Camera.View;
            _effect.Projection = Camera.Projection;

            _effect.TextureEnabled = true;
            _effect.EnableDefaultLighting();

            device.RasterizerState = RasterizerState.CullNone;
            device.DepthStencilState = DepthStencilState.Default;

            Tile highlightedTile = FindHighlightedTile();
            foreach (var tile in Level.Tiles)
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
            foreach (var actor in Level.Actors)
            {
                actor.Render(device, _spriteBatch, _alphaTestEffect, Camera, 0);
            }
            // Pass 2: alpha blend
            foreach (var actor in Level.Actors)
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
            if ((_stateMachine.CurrentState as GameStateBase).HighlightHoveredTile)
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

                foreach (var tile in Level.Tiles)
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
            Level = LevelFactory.CreateLevel(contentManager, tempLevelData);
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
                if (actor != _activeUnit)
                {
                    new DoomWindowBuilder()
                        .CanClose(true)
                        .Title(actor.ActorID)
                        .Size(200, 200)
                        .Position(50, 100)                        
                        .Parent(_desktop)
                        .Build();
                }
                else
                {
                    new ActionMenuBuilder()                        
                        .ActorName(actor.ActorID)
                        .Action("Action", OpenActionSubmenu)
                        .Action("Wait", null)
                        .Action("Turn", null)
                        .Position(50, 100)
                        .Size(200, 200)
                        .Parent(_desktop)
                        .Build();                 
                }
            }
        }

        private void OpenActionSubmenu(Control control, MouseEventArgs args)
        {
            new ActionMenuBuilder()
                .Action("Fireball", SwitchToTileSelectionMode)
                .Action("Eviscerate", null)
                .Position(250, 120)
                .Size(200, 150)
                .Parent(_desktop)
                .Build();
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

            foreach (var actor in Level.Actors)
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

        public void OnChargeTimeReached(IDoomEvent doomEvent)
        {
            if (_activeUnit == null)
            {
                var turnEvent = (TurnEvent) doomEvent;
                _activeUnit = turnEvent.Actor;
                Camera.MoveTo(_activeUnit.Position + new Vector3(200, _activeUnit.Height + 10, 200));
                Camera.LookAt(_activeUnit.Position + new Vector3(0, _activeUnit.Height / 2, 0));
                ShowUnitStatus(_activeUnit);
            }
        }

        public void PerformActionOnHoveredTile(Vector2 vector2)
        {
            var tile = FindHighlightedTile();
            (_activeUnit as Imp).ShootFireball(tile);
        }

        public void OnActorSpawn(IDoomEvent evt)
        {
            var actorEvent = (SpawnActorEvent) evt;
            var spawnMethod = _spawnMethods[actorEvent.ActorType];
            var newActor = spawnMethod.Invoke(actorEvent.SpawnPosition, actorEvent.InitialVelocity);
            Level.Actors.Add(newActor);
        }

        public void OnActorDespawn(IDoomEvent evt)
        {
            
        }

    }
}
