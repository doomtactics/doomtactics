using System;
using System.Collections.Generic;
using System.Globalization;
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
        public readonly DoomTacticsGame GameInstance;
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
        public AlphaTestEffect TextEffect;
        public AlphaTestEffect AlphaTestEffect;
        public SpriteFont DamageFont;

        // ui
        public readonly DoomDesktop Desktop;
        public readonly SquidInputManager SquidInputManager;


        public GameState(DoomTacticsGame gameInstance, SquidInputManager squidInputManager)
        {
            GameInstance = gameInstance;
            Desktop = new DoomDesktop();
            SquidInputManager = squidInputManager;
            _stateMachine = new StateMachine(new MatchIntroState(this));            
        }

        public void OnEnter()
        {
            // setup
            SpriteSheetFactory.Initialize(GameInstance.Content);
            HardcodedAnimations.CreateAnimations();

            // music
            MusicManager.PlayMusic("music/e1m1", true);

            // fonts
            FloatingTexts = new List<TimedText>();
            DamageFont = GameInstance.Content.Load<SpriteFont>("fonts/Doom12");

            // camera
            float aspectRatio = (float)GameInstance.Window.ClientBounds.Width / GameInstance.Window.ClientBounds.Height;
            Camera = new Camera("camera", new Vector3(0, 210f, 0), new Vector3(180f, 130f, 216f), Vector3.Up,
                                aspectRatio);
            MessagingSystem.Subscribe(Camera.MoveCamera, DoomEventType.CameraMoveEvent, "camera", null);
            MessagingSystem.Subscribe(OnChargeTimeReached, DoomEventType.ChargeTimeReached, "gamestate", null);
            MessagingSystem.Subscribe(OnActorSpawn, DoomEventType.SpawnActor, "gamestate", null);
            MessagingSystem.Subscribe(OnActorDespawn, DoomEventType.DespawnActor, "gamestate", null);
            MessagingSystem.Subscribe(OnRemoveActorFromTile, DoomEventType.RemoveFromCurrentTile, "gamestate", null);
            MessagingSystem.Subscribe(OnDisplayDamage, DoomEventType.DisplayDamage, "gamestate", null);
            MessagingSystem.Subscribe(OnDespawnText, DoomEventType.DespawnText, "gamestate", null);

            Effect = new TileEffect(GameInstance.Content);

            CreateLevelTemp(GameInstance.Content);

            SpriteBatch = new SpriteBatch(GameInstance.GraphicsDevice);
            SpriteEffect = new BasicEffect(GameInstance.GraphicsDevice);
            AlphaTestEffect = new AlphaTestEffect(GameInstance.GraphicsDevice);
            TextEffect = new AlphaTestEffect(GameInstance.GraphicsDevice);
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
            NextState = new StateTransition(() => DoomTacticsGame.CreateMenuState(GameInstance));
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

            nearpoint = GameInstance.GraphicsDevice.Viewport.Unproject(nearpoint, Camera.Projection, Camera.View,
                                                                        Matrix.Identity);
            farpoint = GameInstance.GraphicsDevice.Viewport.Unproject(farpoint, Camera.Projection, Camera.View,
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
            //var tempLevelData = HardcodedTestLevel.CreateLevel();
            var tempLevelData = HeightTestLevel.CreateLevel();
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
            float yOffset = evt.DamagedActor.Height + DamageFont.MeasureString(evt.Damage.ToString()).Y;            
            var floatingText = new TimedText(evt.Damage.ToString(), evt.DamagedActor.Position + new Vector3(0, yOffset, 0), DamageFont, 1500, 1, Color.Red, Color.Black);
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

            nearpoint = GameInstance.GraphicsDevice.Viewport.Unproject(nearpoint, Camera.Projection, Camera.View,
                                                                        Matrix.Identity);
            farpoint = GameInstance.GraphicsDevice.Viewport.Unproject(farpoint, Camera.Projection, Camera.View,
                                                                       Matrix.Identity);

            Vector3 direction = Vector3.Normalize(farpoint - nearpoint);

            return new Ray(nearpoint, direction);
        }

        public bool IsAIControlled(ActorBase actor)
        {
            return (actor.Team != 1);
        }
    }
}
