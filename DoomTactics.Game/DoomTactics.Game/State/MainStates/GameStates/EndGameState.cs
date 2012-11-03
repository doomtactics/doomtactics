using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DoomTactics
{
    public class EndGameState : GameStateBase
    {
        private const string AnnouncerYou = "you";
        private const string AnnouncerWin = "win";
        private const string AnnouncerLose = "lose";
        private const string WordsTexture = "text/prepareforbattleaction";
        private static readonly Rectangle WinRectangle = new Rectangle(0, 97, 464, 62);
        private static readonly Rectangle LoseRectangle = new Rectangle(0, 160, 548, 62);
        private static readonly TimeSpan AnnouncerSoundOneDuration = TimeSpan.FromMilliseconds(500);
        private static readonly TimeSpan AnnouncerSoundTwoDuration = TimeSpan.FromMilliseconds(3000);
        private static readonly Vector2 DisplayPosition = new Vector2(350, 400);
        private static readonly Color FillColor = new Color(205, 133, 53, 70);

        private readonly Stopwatch _stopwatch;
        private readonly bool _playerWon;
        private Texture2D _fill;
        private Texture2D _words;
        private bool _playedSoundOne;
        private bool _playedSoundTwo;
        private Rectangle WordRectangle;


        public EndGameState(GameState gameState, bool playerWon)
            : base(gameState)
        {
            _stopwatch = new Stopwatch();
            InputProcessor = new NoInputProcessor();
            _playerWon = playerWon;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _fill = new Texture2D(GameState.GameInstance.GraphicsDevice, 1, 1);
            _fill.SetData(new[] { Color.White });
            _words = GameState.GameInstance.Content.Load<Texture2D>(WordsTexture);

            WordRectangle = _playerWon ? WinRectangle : LoseRectangle;
        }

        public override void OnExit()
        {
            _fill.Dispose();
            _words.Dispose();
        }

        public override void Render(GraphicsDevice device)
        {
            base.Render(device);
            base.Render(device);
            GameState.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, null, null, null);
            GameState.SpriteBatch.Draw(_fill, new Rectangle(0, 0, GameState.GameInstance.GraphicsDevice.Viewport.Width, GameState.GameInstance.GraphicsDevice.Viewport.Height), FillColor);
            GameState.SpriteBatch.End();

            GameState.SpriteBatch.Begin();
            GameState.SpriteBatch.Draw(_words, DisplayPosition, WordRectangle, Color.White);
            GameState.SpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (_stopwatch.Elapsed > AnnouncerSoundOneDuration + AnnouncerSoundTwoDuration)
            {
                NextState = new StateTransition(() => new FreeCamera(GameState, null));
            }
            else if (!_playedSoundTwo && _stopwatch.Elapsed > AnnouncerSoundOneDuration)
            {
                _playedSoundTwo = true;
                if (_playerWon)
                    MessagingSystem.DispatchEvent(new SoundEvent(DoomEventType.PlaySound, AnnouncerWin), "EndGameState");
                else
                    MessagingSystem.DispatchEvent(new SoundEvent(DoomEventType.PlaySound, AnnouncerLose), "EndGameState");
            }
            else if (!_playedSoundOne)
            {
                MessagingSystem.DispatchEvent(new SoundEvent(DoomEventType.PlaySound, AnnouncerYou), "EndGameState");
                _playedSoundOne = true;
            }
        }

        public override bool IsPaused
        {
            get { return false; }
        }
    }
}
