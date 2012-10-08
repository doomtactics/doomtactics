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
            TextureMap.Add(new AngledSprite("impidle1", Angle.Forward), new SpriteRenderDetails(new Rectangle(232, 0, 41, 57), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("impidle1", Angle.ForwardLeft), new SpriteRenderDetails(new Rectangle(224, 58, 40, 55), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("impidle1", Angle.ForwardRight), new SpriteRenderDetails(new Rectangle(224, 58, 40, 55), SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("impidle1", Angle.Left), new SpriteRenderDetails(new Rectangle(437, 0, 36, 49), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("impidle1", Angle.Right), new SpriteRenderDetails(new Rectangle(437, 0, 36, 49), SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("impidle1", Angle.BackLeft), new SpriteRenderDetails(new Rectangle(216, 227, 30, 47), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("impidle1", Angle.BackRight), new SpriteRenderDetails(new Rectangle(216, 227, 30, 47), SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("impidle1", Angle.Back), new SpriteRenderDetails(new Rectangle(277, 177, 35, 49), SpriteEffects.None));

            TextureMap.Add(new AngledSprite("impidle2", Angle.Forward), new SpriteRenderDetails(new Rectangle(305, 56, 39, 56), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("impidle2", Angle.ForwardLeft), new SpriteRenderDetails(new Rectangle(279, 119, 38, 57), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("impidle2", Angle.ForwardRight), new SpriteRenderDetails(new Rectangle(279, 119, 38, 57), SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("impidle2", Angle.Left), new SpriteRenderDetails(new Rectangle(238, 169, 38, 51), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("impidle2", Angle.Right), new SpriteRenderDetails(new Rectangle(238, 169, 38, 51), SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("impidle2", Angle.BackLeft), new SpriteRenderDetails(new Rectangle(392, 108, 33, 47), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("impidle2", Angle.BackRight), new SpriteRenderDetails(new Rectangle(392, 108, 33, 47), SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("impidle2", Angle.Back), new SpriteRenderDetails(new Rectangle(426, 108, 33, 46), SpriteEffects.None));
        }
    }
}
