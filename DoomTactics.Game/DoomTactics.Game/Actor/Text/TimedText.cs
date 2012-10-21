using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DoomTactics
{
    public class TimedText
    {
        private readonly string _text;
        private readonly Vector3 _position;
        private readonly SpriteFont _font;
        private readonly int _timeToDisplay;
        private int _elapsedTime;

        public TimedText(string text, Vector3 position, SpriteFont font, int timeToDisplay)
        {
            _text = text;
            _position = position;
            _font = font;
            _timeToDisplay = timeToDisplay;
            _elapsedTime = 0;
        }

        public void Update(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            if (_elapsedTime > _timeToDisplay)
            {
                var evt = new TextEvent(DoomEventType.DespawnText, this);
                MessagingSystem.DispatchEvent(evt, "damagetext");
            }
        }

        public void Render(GraphicsDevice device, SpriteBatch batch, Effect spriteEffect)
        {
            batch.DrawString(_font, _text, Vector2.Zero, Color.White);
        }
    }
}
