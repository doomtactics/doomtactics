using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DoomTactics.Controls;
using DoomTactics.Data;
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
        public readonly DoomDesktop Desktop;
        public readonly SquidInputManager SquidInputManager;
        public ActorBase ActiveUnit;
        public StateTransition NextState { get; private set; }

        private readonly DoomTacticsGame _gameInstance;
        public BasicEffect Effect;
        private IInputProcessor _processor;
        public SpriteBatch SpriteBatch;
        public BasicEffect SpriteEffect;
        public HighlightEffectContainer HighlightingEffectContainer;
        public AlphaTestEffect AlphaTestEffect;
        private readonly StateMachine _stateMachine;


        public GameState(DoomTacticsGame gameInstance, SquidInputManager squidInputManager)
        {
            _gameInstance = gameInstance;
            Desktop = new DoomDesktop();
            SquidInputManager = squidInputManager;
            _stateMachine = new StateMachine(new FreeCamera(this, null));
        }

        public void OnEnter()
        {
            // setup
            SpriteSheetFactory.Initialize(_gameInstance.Content);
            HardcodedAnimations.CreateAnimations();

            // camera
            float aspectRatio = (float)_gameInstance.Window.ClientBounds.Width / _gameInstance.Window.ClientBounds.Height;
            Camera = new Camera("camera", new Vector3(-96f, 32f, 32f), new Vector3(32.0f, 32.0f, 32.0f), Vector3.Up,
                                aspectRatio);
            MessagingSystem.Subscribe(Camera.MoveCamera, DoomEventType.CameraMoveEvent, "camera", null);
            MessagingSystem.Subscribe(OnChargeTimeReached, DoomEventType.ChargeTimeReached, "gamestate", null);
            MessagingSystem.Subscribe(OnActorSpawn, DoomEventType.SpawnActor, "gamestate", null);
            MessagingSystem.Subscribe(OnActorDespawn, DoomEventType.DespawnActor, "gamestate", null);

            Effect = new BasicEffect(_gameInstance.GraphicsDevice);

            CreateLevelTemp(_gameInstance.Content);

            SpriteBatch = new SpriteBatch(_gameInstance.GraphicsDevice);
            SpriteEffect = new BasicEffect(_gameInstance.GraphicsDevice);
            HighlightingEffectContainer = new HighlightEffectContainer(_gameInstance.Content);
            AlphaTestEffect = new AlphaTestEffect(_gameInstance.GraphicsDevice);
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

        public void Update(GameTime gameTime)
        {
            MessagingSystem.ProcessQueued();

            _stateMachine.Update(gameTime);
        }

        public void ReturnToMainMenu()
        {
            NextState = new StateTransition(DoomTacticsGame.CreateMenuState(_gameInstance));
        }

        public void Render(GraphicsDevice device)
        {
            _stateMachine.Render(device);
        }

        public Tile FindHighlightedTile()
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
            Tile intersected = null;
            float? minDistance = float.MaxValue;
            foreach (var tile in Level.Tiles)
            {
                float? distance = ray.Intersects(tile.CreateBoundingBox());
                if (distance.HasValue && distance < minDistance)
                {
                    intersected = tile;
                    minDistance = distance;
                }
            }
            return intersected;
        }

        private void CreateLevelTemp(ContentManager contentManager)
        {
            var tempLevelData = HardcodedTestLevel.CreateLevel();
            Level = LevelFactory.CreateLevel(contentManager, tempLevelData);
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
            if (ActiveUnit == null)
            {
                var turnEvent = (TurnEvent)doomEvent;
                ActiveUnit = turnEvent.Actor;
                //Camera.MoveTo(ActiveUnit.Position + new Vector3(200, ActiveUnit.Height + 10, 200));
                //Camera.LookAt(ActiveUnit.Position + new Vector3(0, ActiveUnit.Height / 2, 0));
                _stateMachine.SetState(new ActionSelection(this, ActiveUnit));
                //ShowUnitStatus(ActiveUnit);
            }
        }

        public void OnActorSpawn(IDoomEvent evt)
        {
            var actorEvent = (SpawnActorEvent)evt;
            Level.Actors.Add(actorEvent.Actor);
        }

        public void OnActorDespawn(IDoomEvent evt)
        {
            var actorEvent = (DespawnActorEvent) evt;
            Level.Actors.Remove(actorEvent.DespawnTarget);
        }

        public ActorBase GetNextActiveUnit()
        {
            return Level.Actors.FirstOrDefault(x => x.ChargeTime == 100 && x != ActiveUnit);
        }
    }
}
