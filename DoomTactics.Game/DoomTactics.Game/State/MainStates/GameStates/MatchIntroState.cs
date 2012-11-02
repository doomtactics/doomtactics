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
        private const string AnnouncerSoundOne = "sound/announcer/wheel";        
        private const string AnnouncerSoundTwo = "sound/announcer/action";
        private static readonly TimeSpan AnnouncerSoundOneDuration = TimeSpan.FromMilliseconds(5000);
        private static readonly TimeSpan AnnouncerSoundTwoDuration = TimeSpan.FromMilliseconds(3000);

        private readonly Stopwatch _stopwatch;
        private Texture2D _fill;
        private bool _playedSecondSound;
        private static readonly Color FillColor = new Color(128, 128, 128, 128);        

        public MatchIntroState(GameState gameState) : base(gameState)
        {
            _stopwatch = new Stopwatch();
            InputProcessor = new NoInputProcessor();
            _playedSecondSound = false;
        }

        public override void OnEnter()
        {
            _fill = new Texture2D(GameState.GameInstance.GraphicsDevice, 1, 1);
            _fill.SetData(new[] { FillColor });
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
