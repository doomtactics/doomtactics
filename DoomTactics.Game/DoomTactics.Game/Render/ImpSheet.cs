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
            TextureMap.Add(new AngledSprite("impidle1", Angle.Forward), new SpriteRenderDetails(new Rectangle(232, 0, 41, 57), new Vector2(19, 52), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("impidle1", Angle.ForwardLeft), new SpriteRenderDetails(new Rectangle(224, 58, 40, 55), new Vector2(17, 50), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("impidle1", Angle.ForwardRight), new SpriteRenderDetails(new Rectangle(224, 58, 40, 55), new Vector2(23, 50), SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("impidle1", Angle.Left), new SpriteRenderDetails(new Rectangle(437, 0, 36, 49), new Vector2(15, 44), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("impidle1", Angle.Right), new SpriteRenderDetails(new Rectangle(437, 0, 36, 49), new Vector2(21, 44), SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("impidle1", Angle.BackLeft), new SpriteRenderDetails(new Rectangle(216, 227, 30, 47), new Vector2(20, 42), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("impidle1", Angle.BackRight), new SpriteRenderDetails(new Rectangle(216, 227, 30, 47), new Vector2(10, 42), SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("impidle1", Angle.Back), new SpriteRenderDetails(new Rectangle(277, 177, 35, 49), new Vector2(21, 44), SpriteEffects.None));

            TextureMap.Add(new AngledSprite("impidle2", Angle.Forward), new SpriteRenderDetails(new Rectangle(305, 56, 39, 56), new Vector2(17, 51), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("impidle2", Angle.ForwardLeft), new SpriteRenderDetails(new Rectangle(279, 119, 38, 57), new Vector2(13, 52), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("impidle2", Angle.ForwardRight), new SpriteRenderDetails(new Rectangle(279, 119, 38, 57), new Vector2(25, 52),  SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("impidle2", Angle.Left), new SpriteRenderDetails(new Rectangle(238, 169, 38, 51), new Vector2(16, 46), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("impidle2", Angle.Right), new SpriteRenderDetails(new Rectangle(238, 169, 38, 51), new Vector2(22, 46), SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("impidle2", Angle.BackLeft), new SpriteRenderDetails(new Rectangle(392, 108, 33, 47), new Vector2(19, 42), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("impidle2", Angle.BackRight), new SpriteRenderDetails(new Rectangle(392, 108, 33, 47), new Vector2(14, 42), SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("impidle2", Angle.Back), new SpriteRenderDetails(new Rectangle(426, 108, 33, 46), new Vector2(20, 41), SpriteEffects.None));

            TextureMap.Add(new AngledSprite("impshoot1", Angle.Forward), new SpriteRenderDetails(new Rectangle(0, 114, 49, 60), new Vector2(30, 55), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("impshoot1", Angle.ForwardLeft), new SpriteRenderDetails(new Rectangle(426, 155, 32, 56), new Vector2(11, 51), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("impshoot1", Angle.ForwardRight), new SpriteRenderDetails(new Rectangle(426, 155, 32, 56), new Vector2(21, 51), SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("impshoot1", Angle.Left), new SpriteRenderDetails(new Rectangle(239, 119, 39, 49), new Vector2(23, 44), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("impshoot1", Angle.Right), new SpriteRenderDetails(new Rectangle(239, 119, 39, 49), new Vector2(16, 44), SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("impshoot1", Angle.BackLeft), new SpriteRenderDetails(new Rectangle(356, 157, 34, 47), new Vector2(20, 42), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("impshoot1", Angle.BackRight), new SpriteRenderDetails(new Rectangle(356, 157, 34, 47), new Vector2(14, 42), SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("impshoot1", Angle.Back), new SpriteRenderDetails(new Rectangle(345, 58, 38, 48), new Vector2(17, 43), SpriteEffects.None));

            TextureMap.Add(new AngledSprite("impshoot2", Angle.Forward), new SpriteRenderDetails(new Rectangle(97, 169, 44, 55), new Vector2(18, 50), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("impshoot2", Angle.ForwardLeft), new SpriteRenderDetails(new Rectangle(47, 198, 45, 54), new Vector2(25, 49), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("impshoot2", Angle.ForwardRight), new SpriteRenderDetails(new Rectangle(47, 198, 45, 54), new Vector2(20, 49), SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("impshoot2", Angle.Left), new SpriteRenderDetails(new Rectangle(196, 118, 42, 49), new Vector2(18, 44), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("impshoot2", Angle.Right), new SpriteRenderDetails(new Rectangle(196, 118, 42, 49), new Vector2(24, 44), SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("impshoot2", Angle.BackLeft), new SpriteRenderDetails(new Rectangle(384, 58, 36, 47), new Vector2(16, 42), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("impshoot2", Angle.BackRight), new SpriteRenderDetails(new Rectangle(384, 58, 36, 47), new Vector2(20, 42), SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("impshoot2", Angle.Back), new SpriteRenderDetails(new Rectangle(353, 205, 33, 46), new Vector2(12, 41), SpriteEffects.None));

            TextureMap.Add(new AngledSprite("impshoot3", Angle.Forward), new SpriteRenderDetails(new Rectangle(0, 235, 32, 55), new Vector2(5, 50), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("impshoot3", Angle.ForwardLeft), new SpriteRenderDetails(new Rectangle(176, 0, 55, 55), new Vector2(25, 50), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("impshoot3", Angle.ForwardRight), new SpriteRenderDetails(new Rectangle(176, 0, 55, 55), new Vector2(30, 50), SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("impshoot3", Angle.Left), new SpriteRenderDetails(new Rectangle(0, 0, 59, 51), new Vector2(27, 46), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("impshoot3", Angle.Right), new SpriteRenderDetails(new Rectangle(0, 0, 59, 51), new Vector2(32, 46), SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("impshoot3", Angle.BackLeft), new SpriteRenderDetails(new Rectangle(50, 150, 46, 47), new Vector2(23, 42), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("impshoot3", Angle.BackRight), new SpriteRenderDetails(new Rectangle(50, 150, 46, 47), new Vector2(23, 42), SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("impshoot3", Angle.Back), new SpriteRenderDetails(new Rectangle(125, 241, 31, 49), new Vector2(16, 44), SpriteEffects.None));

            AddUnangledSprite("impdeath1", new SpriteRenderDetails(new Rectangle(153, 118, 42, 62), new Vector2(22, 57), SpriteEffects.None));
            AddUnangledSprite("impdeath2", new SpriteRenderDetails(new Rectangle(142, 181, 41, 59), new Vector2(21, 54), SpriteEffects.None));
            AddUnangledSprite("impdeath3", new SpriteRenderDetails(new Rectangle(316, 0,   40, 54), new Vector2(18, 54), SpriteEffects.None));
            AddUnangledSprite("impdeath4", new SpriteRenderDetails(new Rectangle(58,  103, 48, 46), new Vector2(23, 45), SpriteEffects.None));
            AddUnangledSprite("impdeath5", new SpriteRenderDetails(new Rectangle(60,  0,   58, 22), new Vector2(29, 19), SpriteEffects.None));


        }
    }
}
