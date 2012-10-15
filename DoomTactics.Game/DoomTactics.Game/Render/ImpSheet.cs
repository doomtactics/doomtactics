﻿using System;
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
        }
    }
}
