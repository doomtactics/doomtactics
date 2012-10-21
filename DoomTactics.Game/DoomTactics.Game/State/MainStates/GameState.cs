﻿using System;
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
        private readonly DoomTacticsGame _gameInstance;
        private readonly StateMachine _stateMachine;

        // gameplay
        public Camera Camera;
        public Level Level;
        public IList<TimedText> FloatingTexts;         
        public ActorBase ActiveUnit;
        public StateTransition NextState { get; private set; }

        // graphics
        public TileEffect Effect;
        public SpriteBatch SpriteBatch;
        public BasicEffect SpriteEffect;
        public AlphaTestEffect AlphaTestEffect;
        public SpriteFont DamageFont;

        // ui
        public readonly DoomDesktop Desktop;
        public readonly SquidInputManager SquidInputManager;


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

            // fonts
            FloatingTexts = new List<TimedText>();
            DamageFont = _gameInstance.Content.Load<SpriteFont>("fonts/Doom16");


            // camera
            float aspectRatio = (float)_gameInstance.Window.ClientBounds.Width / _gameInstance.Window.ClientBounds.Height;
            Camera = new Camera("camera", new Vector3(-96f, 32f, 32f), new Vector3(32.0f, 32.0f, 32.0f), Vector3.Up,
                                aspectRatio);
            MessagingSystem.Subscribe(Camera.MoveCamera, DoomEventType.CameraMoveEvent, "camera", null);
            MessagingSystem.Subscribe(OnChargeTimeReached, DoomEventType.ChargeTimeReached, "gamestate", null);
            MessagingSystem.Subscribe(OnActorSpawn, DoomEventType.SpawnActor, "gamestate", null);
            MessagingSystem.Subscribe(OnActorDespawn, DoomEventType.DespawnActor, "gamestate", null);
            MessagingSystem.Subscribe(OnRemoveActorFromTile, DoomEventType.RemoveFromCurrentTile, "gamestate", null);
            MessagingSystem.Subscribe(OnDisplayDamage, DoomEventType.DisplayDamage, "gamestate", null);
            MessagingSystem.Subscribe(OnDespawnText, DoomEventType.DespawnText, "gamestate", null);

            Effect = new TileEffect(_gameInstance.Content);

            CreateLevelTemp(_gameInstance.Content);

            SpriteBatch = new SpriteBatch(_gameInstance.GraphicsDevice);
            SpriteEffect = new BasicEffect(_gameInstance.GraphicsDevice);
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

        public void OnChargeTimeReached(IDoomEvent doomEvent)
        {
            if (ActiveUnit == null)
            {
                var turnEvent = (TurnEvent)doomEvent;
                ActiveUnit = turnEvent.Actor;
                _stateMachine.SetState(new ActionSelection(this, ActiveUnit));
            }
        }

        public void OnActorSpawn(IDoomEvent evt)
        {
            var actorEvent = (ActorEvent)evt;
            Level.Actors.Add(actorEvent.Actor);
        }

        public void OnActorDespawn(IDoomEvent evt)
        {
            var actorEvent = (DespawnActorEvent) evt;
            Level.Actors.Remove(actorEvent.DespawnTarget);
        }

        public void OnRemoveActorFromTile(IDoomEvent evt)
        {
            var actorEvent = (ActorEvent) evt;
            var tile = Level.GetTileOfActor(actorEvent.Actor);
            if (tile != null)
            {
                tile.SetActor(null);
            }
        }

        public void OnDisplayDamage(IDoomEvent displayDamageEvent)
        {
            var evt = (DamageEvent) displayDamageEvent;
            var floatingText = new TimedText(evt.Damage.ToString(), Vector3.Zero, DamageFont, 2000);
            FloatingTexts.Add(floatingText);

            Log.Debug("Actor " + evt.DamagedActor.ActorId + " took " + evt.Damage + " damage.");
        }

        public void OnDespawnText(IDoomEvent despawnTextEvent)
        {
            var evt = (TextEvent) despawnTextEvent;
            FloatingTexts.Remove(evt.Text);
        }

        public ActorBase GetNextActiveUnit()
        {
            return Level.Actors.FirstOrDefault(x => x.CurrentStats.ChargeTime == 100 && x != ActiveUnit);
        }

        public Ray CreateRayFromMouseCursorPosition(Vector2 mousePosition)
        {
            Vector3 nearpoint = new Vector3(mousePosition, 0);
            Vector3 farpoint = new Vector3(mousePosition, 1.0f);

            nearpoint = _gameInstance.GraphicsDevice.Viewport.Unproject(nearpoint, Camera.Projection, Camera.View,
                                                                        Matrix.Identity);
            farpoint = _gameInstance.GraphicsDevice.Viewport.Unproject(farpoint, Camera.Projection, Camera.View,
                                                                       Matrix.Identity);

            Vector3 direction = Vector3.Normalize(farpoint - nearpoint);

            return new Ray(nearpoint, direction);
        }
    }
}
