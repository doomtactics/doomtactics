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
                _sheets[ActorType.ImpFireball] = () => new ImpFireballSheet(contentManager.Load<Texture2D>("sheets\\impfireballsheet"));
            }
        }
    }

    public struct SpriteRenderDetails
    {
        public Rectangle Rectangle;
        public SpriteEffects SpriteEffects;

        public SpriteRenderDetails(Rectangle rectangle, SpriteEffects spriteEffects)
        {
            Rectangle = rectangle;
            SpriteEffects = spriteEffects;
        }
    }

    public enum Angle
    {
        Forward,
        ForwardRight,
        Right,
        BackRight,
        Back,
        BackLeft,
        Left,
        ForwardLeft
    }

    public struct AngledSprite
    {
        public readonly string Name;
        public readonly Angle Angle;

        public AngledSprite(string name, Angle angle)
        {
            Name = name;
            Angle = angle;
        }

        public override bool Equals(object obj)
        {
            var o = (AngledSprite) obj;
            return o.Name == this.Name && o.Angle == this.Angle;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode()*Angle.GetHashCode();
        }

        public static bool operator ==(AngledSprite a, AngledSprite b)
        {            
            return a.Equals(b);
        }

        public static bool operator !=(AngledSprite a, AngledSprite b)
        {
            return !a.Equals(b);
        }
    }

    public abstract class SpriteSheet
    {
        private readonly Texture2D _texture;
        protected IDictionary<AngledSprite, SpriteRenderDetails> TextureMap;

        public Texture2D Texture
        {
            get { return _texture; }
        }

        protected SpriteSheet(Texture2D texture)
        {
            _texture = texture;
            TextureMap = new Dictionary<AngledSprite, SpriteRenderDetails>();
        }
        
        protected void AddUnangledSprite(string spriteName, SpriteRenderDetails details)
        {
            foreach (Angle angle in Enum.GetValues(typeof(Angle)))
            {
                TextureMap.Add(new AngledSprite(spriteName, angle), details);
            }
        }

        public SpriteRenderDetails GetRectangle(string name, Angle angle)
        {
            return GetRectangle(new AngledSprite(name, angle));
        }

        public SpriteRenderDetails GetRectangle(AngledSprite angledSprite)
        {
            return TextureMap[angledSprite];
        }
        
    }
}
