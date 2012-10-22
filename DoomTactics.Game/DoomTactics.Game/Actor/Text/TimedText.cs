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
        private int _outlineSize;
        private Color _textColor;
        private Color _outlineColor;

        public TimedText(string text, Vector3 position, SpriteFont font, int timeToDisplay) : this(text, position, font, timeToDisplay, 0, Color.White, Color.Black)
        {
            
        }

        public TimedText(string text, Vector3 position, SpriteFont font, int timeToDisplay, int outlineSize, Color textColor, Color outlineColor)
        {
            _text = text;
            _position = position;
            _font = font;
            _timeToDisplay = timeToDisplay;
            _outlineSize = outlineSize;
            _textColor = textColor;
            _outlineColor = outlineColor;
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

        public void Render(GraphicsDevice device, Camera camera, SpriteBatch spriteBatch, AlphaTestEffect textEffect)
        {
            Matrix bill = Matrix.CreateBillboard(_position, camera.Position, Vector3.Down, camera.Direction);
            //Matrix bill = Matrix.CreateConstrainedBillboard(_position, camera.Position, Vector3.Down, camera.Direction,
            //                                    Vector3.Forward);
            float xOffset = _font.MeasureString(_text).X/2;

            textEffect.World = bill;
            textEffect.View = camera.View;
            textEffect.Projection = camera.Projection;
            textEffect.ReferenceAlpha = 0;
            textEffect.AlphaFunction = CompareFunction.Greater;
            textEffect.ReferenceAlpha = 128;
            textEffect.VertexColorEnabled = true;            

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.PointClamp, DepthStencilState.DepthRead,
                                  RasterizerState.CullNone, textEffect);
            for (int x = -_outlineSize; x <= _outlineSize; x++)
            {
                for (int y = -_outlineSize; y <= _outlineSize; y++)
                {
                    if (x == 0 && y == 0) continue;

                    spriteBatch.DrawString(_font, _text, new Vector2(-xOffset + x, y), _outlineColor);
                }
            }
                
            spriteBatch.DrawString(_font, _text, new Vector2(-xOffset, 0), _textColor);

            spriteBatch.End();
        }
    }
}
