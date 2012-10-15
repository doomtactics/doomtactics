using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DoomTactics
{
    public struct SpriteRenderDetails
    {
        public Rectangle SourceRectangle;
        public Rectangle TargetRectangle;
        public SpriteEffects SpriteEffects;

        public SpriteRenderDetails(Rectangle sourceRectangle, Vector2 offsets, SpriteEffects spriteEffects)
        {
            // For now offsets.Y is not used.  Not sure if it's needed for this 3D rendering.
            SourceRectangle = sourceRectangle;
            SpriteEffects = spriteEffects;
            TargetRectangle = new Rectangle(-(int)offsets.X, -sourceRectangle.Height, sourceRectangle.Width, sourceRectangle.Height);
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

        public SpriteRenderDetails GetRenderDetails(string name, Angle angle)
        {
            return GetRenderDetails(new AngledSprite(name, angle));
        }

        public SpriteRenderDetails GetRenderDetails(AngledSprite angledSprite)
        {
            return TextureMap[angledSprite];
        }
        
    }
}
