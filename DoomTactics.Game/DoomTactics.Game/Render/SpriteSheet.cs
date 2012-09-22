using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DoomTactics
{
    public static class SpriteSheetFactory
    {
        private static IDictionary<ActorType, Func<SpriteSheet>> _sheets;

        public static SpriteSheet CreateSpriteSheet(ActorType actorType)
        {
            if (_sheets == null)
                throw new InvalidOperationException("_sheets was not initialized -- need to call SpriteSheetFactory.Initialize");

            return _sheets[actorType].Invoke();
        }

        public static void Initialize(ContentManager contentManager)
        {
            if (_sheets == null)
            {
                _sheets = new Dictionary<ActorType, Func<SpriteSheet>>();
                _sheets[ActorType.Imp] = () => new ImpSheet(contentManager.Load<Texture2D>("sheets\\impsheet"));
            }
        }
    }

    public abstract class SpriteSheet
    {
        private readonly Texture2D _texture;
        protected IDictionary<string, Rectangle> TextureMap;

        public Texture2D Texture
        {
            get { return _texture; }
        }

        protected SpriteSheet(Texture2D texture)
        {
            _texture = texture;
            TextureMap = new Dictionary<string, Rectangle>();
        }
        
        public Rectangle GetRectangle(string name)
        {
            return TextureMap[name];
        }
        
    }
}
