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
        private static readonly TimeSpan AnnouncerSoundOneDuration = TimeSpan.FromMilliseconds(3000);
        private static readonly TimeSpan AnnouncerSoundTwoDuration = TimeSpan.FromMilliseconds(500);

        private readonly Stopwatch _stopwatch;
        private Texture2D _fill;
        private bool _playedSecondSound;
        private static readonly Color FillColor = new Color(205, 133, 53, 70);

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
