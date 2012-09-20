using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DoomTactics
{
    public abstract class SpriteSheet
    {
        private readonly Texture2D _texture;
        protected IDictionary<string, Rectangle> _textureMap;

        public Texture2D Texture
        {
            get { return _texture; }
        }

        protected SpriteSheet(Texture2D texture)
        {
            _texture = texture;
            _textureMap = new Dictionary<string, Rectangle>();
        }
        
        public Rectangle GetRectangle(string name)
        {
            return _textureMap[name];
        }
        
    }
}
