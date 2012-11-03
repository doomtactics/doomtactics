using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DoomTactics
{
    public class MatchIntroState : GameStateBase
    {
        private const string AnnouncerSoundOne = "wheeloffateisturning";        
        private const string AnnouncerSoundTwo = "action";
        private const string WordsTexture = "text/prepareforbattleaction";
        private static readonly Rectangle WordOneRectangle = new Rectangle(0, 0, 712, 37);
        private static readonly Rectangle WordTwoRectangle = new Rectangle(0, 37, 427, 60);
        private static readonly TimeSpan AnnouncerSoundOneDuration = TimeSpan.FromMilliseconds(3000);
        private static readonly TimeSpan AnnouncerSoundTwoDuration = TimeSpan.FromMilliseconds(1200);
        private static readonly Vector2 OneDisplayPosition = new Vector2(200, 200);
        private static readonly Vector2 TwoDisplayPosition = new Vector2(350, 400);
        private static readonly Color FillColor = new Color(205, 133, 53, 70);

        private readonly Stopwatch _stopwatch;
        private Texture2D _fill;
        private Texture2D _words;
        private bool _playedSecondSound;
        private bool _drawOne;
        private bool _drawTwo;


        public MatchIntroState(GameState gameState) : base(gameState)
        {
            _stopwatch = new Stopwatch();
            InputProcessor = new NoInputProcessor();
            _playedSecondSound = false;
        }

        public override void OnEnter()
        {
            _fill = new Texture2D(GameState.GameInstance.GraphicsDevice, 1, 1);
            _fill.SetData(new[] { Color.White });
            _words = GameState.GameInstance.Content.Load<Texture2D>(WordsTexture);
            _drawOne = true;
            var evt = new SoundEvent(DoomEventType.PlaySound, AnnouncerSoundOne);
            MessagingSystem.DispatchEvent(evt, "MatchIntro");
            _stopwatch.Start();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);            
            if (_stopwatch.Elapsed > (AnnouncerSoundOneDuration + AnnouncerSoundTwoDuration))
            {
                _stopwatch.Stop();
                NextState = new StateTransition(() => new FreeCamera(GameState, null));
            }
            else if (_stopwatch.Elapsed > AnnouncerSoundOneDuration && !_playedSecondSound)
            {                
                _playedSecondSound = true;
                _drawOne = false;
                _drawTwo = true;
                var evt = new SoundEvent(DoomEventType.PlaySound, AnnouncerSoundTwo);
                MessagingSystem.DispatchEvent(evt, "MatchIntro");
            }
        }

        public override void Render(GraphicsDevice device)
        {
            base.Render(device);
            GameState.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, null, null, null);
            GameState.SpriteBatch.Draw(_fill, new Rectangle(0, 0, GameState.GameInstance.GraphicsDevice.Viewport.Width, GameState.GameInstance.GraphicsDevice.Viewport.Height), FillColor);           
            GameState.SpriteBatch.End();

            if (_drawOne || _drawTwo)
            {
                GameState.SpriteBatch.Begin();
                if (_drawOne)
                    GameState.SpriteBatch.Draw(_words, OneDisplayPosition, WordOneRectangle, Color.White);
                if (_drawTwo)
                    GameState.SpriteBatch.Draw(_words, TwoDisplayPosition, WordTwoRectangle, Color.White);
                GameState.SpriteBatch.End();
            }
        }

        public override void OnExit()
        {
            _fill.Dispose();
        }

        public override bool IsPaused
        {
            get { return false; }
        }
    }
}
