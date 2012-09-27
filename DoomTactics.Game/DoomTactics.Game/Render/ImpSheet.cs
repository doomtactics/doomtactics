using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DoomTactics
{
    public class ImpSheet : SpriteSheet
    {
        public ImpSheet(Texture2D texture)
            : base(texture)
        {
            PopulateSheet();
        }

        private void PopulateSheet()
        {
            TextureMap.Add(new AngledSprite("impidle1", Angle.Forward), new SpriteRenderDetails(new Rectangle(0, 0, 40, 57), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("impidle1", Angle.ForwardLeft), new SpriteRenderDetails(new Rectangle(40, 0, 40, 57), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("impidle1", Angle.ForwardRight), new SpriteRenderDetails(new Rectangle(40, 0, 40, 57), SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("impidle1", Angle.Left), new SpriteRenderDetails(new Rectangle(88, 0, 40, 57), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("impidle1", Angle.Right), new SpriteRenderDetails(new Rectangle(88, 0, 40, 57), SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("impidle1", Angle.BackLeft), new SpriteRenderDetails(new Rectangle(130, 0, 40, 57), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("impidle1", Angle.BackRight), new SpriteRenderDetails(new Rectangle(130, 0, 40, 57), SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("impidle1", Angle.Back), new SpriteRenderDetails(new Rectangle(169, 0, 40, 57), SpriteEffects.None));

            TextureMap.Add(new AngledSprite("impidle2", Angle.Forward), new SpriteRenderDetails(new Rectangle(212, 0, 40, 57), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("impidle2", Angle.ForwardLeft), new SpriteRenderDetails(new Rectangle(257, 0, 40, 57), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("impidle2", Angle.ForwardRight), new SpriteRenderDetails(new Rectangle(257, 0, 40, 57), SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("impidle2", Angle.Left), new SpriteRenderDetails(new Rectangle(301, 0, 40, 57), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("impidle2", Angle.Right), new SpriteRenderDetails(new Rectangle(301, 0, 40, 57), SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("impidle2", Angle.BackLeft), new SpriteRenderDetails(new Rectangle(342, 0, 40, 57), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("impidle2", Angle.BackRight), new SpriteRenderDetails(new Rectangle(342, 0, 40, 57), SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("impidle2", Angle.Back), new SpriteRenderDetails(new Rectangle(380, 0, 40, 57), SpriteEffects.None));
        }
    }
}
