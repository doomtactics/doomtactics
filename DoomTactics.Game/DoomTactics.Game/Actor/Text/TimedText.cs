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
        private readonly Vector3 _position;
        private readonly SpriteFont _font;
        private readonly int _timeToDisplay;

        public TimedText(Vector3 position, SpriteFont font, int timeToDisplay)
        {
            _position = position;
            _font = font;
            _timeToDisplay = timeToDisplay;
        }

        public void Update(GameTime gameTime)
        {
            
        }

        public void Render(GraphicsDevice device, SpriteBatch batch, Effect spriteEffect)
        {
            
        }
    }
}
