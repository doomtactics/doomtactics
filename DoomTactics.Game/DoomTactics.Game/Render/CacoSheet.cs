using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DoomTactics
{
    public class CacoSheet : SpriteSheet
    {
        public CacoSheet(Texture2D texture) : base(texture)
        {
            PopulateSheet();
        }

        private void PopulateSheet()
        {
            TextureMap.Add(new AngledSprite("cacoidle", Angle.Forward), new SpriteRenderDetails(new Rectangle(0, 189, 63, 66), new Vector2(31, 67), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("cacoidle", Angle.ForwardLeft), new SpriteRenderDetails(new Rectangle(190, 285, 61, 66), new Vector2(26, 67), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("cacoidle", Angle.ForwardRight), new SpriteRenderDetails(new Rectangle(190, 285, 61, 66), new Vector2(35, 67), SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("cacoidle", Angle.Left), new SpriteRenderDetails(new Rectangle(0, 392, 62, 67), new Vector2(27, 68), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("cacoidle", Angle.Right), new SpriteRenderDetails(new Rectangle(0, 392, 62, 67), new Vector2(35, 68), SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("cacoidle", Angle.BackLeft), new SpriteRenderDetails(new Rectangle(128, 149, 63, 67), new Vector2(32, 68), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("cacoidle", Angle.BackRight), new SpriteRenderDetails(new Rectangle(128, 149, 63, 67), new Vector2(31, 68), SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("cacoidle", Angle.Back), new SpriteRenderDetails(new Rectangle(400, 68, 63, 65), new Vector2(28, 66), SpriteEffects.None));

            TextureMap.Add(new AngledSprite("cacoshoot1", Angle.Forward), new SpriteRenderDetails(new Rectangle(384, 136, 63, 65), new Vector2(31, 70), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("cacoshoot1", Angle.ForwardLeft), new SpriteRenderDetails(new Rectangle(127, 353, 61, 66), new Vector2(29, 69), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("cacoshoot1", Angle.ForwardRight), new SpriteRenderDetails(new Rectangle(127, 353, 61, 66), new Vector2(32, 69), SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("cacoshoot1", Angle.Left), new SpriteRenderDetails(new Rectangle(190, 217, 62, 67), new Vector2(30, 68), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("cacoshoot1", Angle.Right), new SpriteRenderDetails(new Rectangle(190, 217, 62, 67), new Vector2(32, 68), SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("cacoshoot1", Angle.BackLeft), new SpriteRenderDetails(new Rectangle(448, 134, 63, 65), new Vector2(32, 67), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("cacoshoot1", Angle.BackRight), new SpriteRenderDetails(new Rectangle(448, 134, 63, 65), new Vector2(31, 67), SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("cacoshoot1", Angle.Back), new SpriteRenderDetails(new Rectangle(0, 256, 63, 65), new Vector2(32, 68), SpriteEffects.None));

            TextureMap.Add(new AngledSprite("cacoshoot2", Angle.Forward), new SpriteRenderDetails(new Rectangle(134, 78, 63, 69), new Vector2(31, 71), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("cacoshoot2", Angle.ForwardLeft), new SpriteRenderDetails(new Rectangle(318, 203, 60, 69), new Vector2(29, 72), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("cacoshoot2", Angle.ForwardRight), new SpriteRenderDetails(new Rectangle(318, 203, 60, 69), new Vector2(31, 72), SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("cacoshoot2", Angle.Left), new SpriteRenderDetails(new Rectangle(0, 322, 62, 69), new Vector2(30, 68), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("cacoshoot2", Angle.Right), new SpriteRenderDetails(new Rectangle(0, 322, 62, 69), new Vector2(32, 68), SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("cacoshoot2", Angle.BackLeft), new SpriteRenderDetails(new Rectangle(64, 149, 63, 68), new Vector2(32, 67), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("cacoshoot2", Angle.BackRight), new SpriteRenderDetails(new Rectangle(64, 149, 63, 68), new Vector2(31, 67), SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("cacoshoot2", Angle.Back), new SpriteRenderDetails(new Rectangle(192, 148, 63, 67), new Vector2(32, 68), SpriteEffects.None));

            TextureMap.Add(new AngledSprite("cacoshoot3", Angle.Forward), new SpriteRenderDetails(new Rectangle(144, 0, 63, 71), new Vector2(31, 72), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("cacoshoot3", Angle.ForwardLeft), new SpriteRenderDetails(new Rectangle(256, 203, 61, 71), new Vector2(29, 72), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("cacoshoot3", Angle.ForwardRight), new SpriteRenderDetails(new Rectangle(256, 203, 61, 71), new Vector2(32, 72), SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("cacoshoot3", Angle.Left), new SpriteRenderDetails(new Rectangle(64, 218, 62, 75), new Vector2(30, 72), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("cacoshoot3", Angle.Right), new SpriteRenderDetails(new Rectangle(64, 218, 62, 75), new Vector2(32, 72), SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("cacoshoot3", Angle.BackLeft), new SpriteRenderDetails(new Rectangle(0, 117, 63, 71), new Vector2(32, 70), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("cacoshoot3", Angle.BackRight), new SpriteRenderDetails(new Rectangle(0, 117, 63, 71), new Vector2(31, 70), SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("cacoshoot3", Angle.Back), new SpriteRenderDetails(new Rectangle(70, 78, 63, 70), new Vector2(32, 68), SpriteEffects.None));

            TextureMap.Add(new AngledSprite("cacopain1", Angle.Forward), new SpriteRenderDetails(new Rectangle(208, 0, 63, 71), new Vector2(31, 68), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("cacopain1", Angle.ForwardLeft), new SpriteRenderDetails(new Rectangle(127, 287, 62, 65), new Vector2(30, 67), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("cacopain1", Angle.ForwardRight), new SpriteRenderDetails(new Rectangle(127, 287, 62, 65), new Vector2(32, 67), SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("cacopain1", Angle.Left), new SpriteRenderDetails(new Rectangle(127, 218, 62, 68), new Vector2(29, 68), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("cacopain1", Angle.Right), new SpriteRenderDetails(new Rectangle(127, 218, 62, 68), new Vector2(33, 68), SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("cacopain1", Angle.BackLeft), new SpriteRenderDetails(new Rectangle(272, 0, 63, 67), new Vector2(31, 69), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("cacopain1", Angle.BackRight), new SpriteRenderDetails(new Rectangle(272, 0, 63, 67), new Vector2(32, 69), SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("cacopain1", Angle.Back), new SpriteRenderDetails(new Rectangle(256, 136, 63, 66), new Vector2(30, 68), SpriteEffects.None));

            TextureMap.Add(new AngledSprite("cacopain2", Angle.Forward), new SpriteRenderDetails(new Rectangle(208, 68, 63, 67), new Vector2(31, 68), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("cacopain2", Angle.ForwardLeft), new SpriteRenderDetails(new Rectangle(63, 363, 62, 65), new Vector2(31, 66), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("cacopain2", Angle.ForwardRight), new SpriteRenderDetails(new Rectangle(63, 363, 62, 65), new Vector2(31, 66), SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("cacopain2", Angle.Left), new SpriteRenderDetails(new Rectangle(64, 294, 62, 68), new Vector2(31, 67), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("cacopain2", Angle.Right), new SpriteRenderDetails(new Rectangle(64, 294, 62, 68), new Vector2(31, 67), SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("cacopain2", Angle.BackLeft), new SpriteRenderDetails(new Rectangle(336, 0, 63, 67), new Vector2(31, 69), SpriteEffects.None));
            TextureMap.Add(new AngledSprite("cacopain2", Angle.BackRight), new SpriteRenderDetails(new Rectangle(336, 0, 63, 67), new Vector2(32, 69), SpriteEffects.FlipHorizontally));
            TextureMap.Add(new AngledSprite("cacopain2", Angle.Back), new SpriteRenderDetails(new Rectangle(320, 136, 63, 66), new Vector2(30, 68), SpriteEffects.None));


            AddUnangledSprite("cacodeath1", new SpriteRenderDetails(new Rectangle(272, 68, 63, 67), new Vector2(31, 68), SpriteEffects.None));
            AddUnangledSprite("cacodeath2", new SpriteRenderDetails(new Rectangle(400, 0, 63, 67), new Vector2(31, 68), SpriteEffects.None));
            AddUnangledSprite("cacodeath3", new SpriteRenderDetails(new Rectangle(336, 68, 63, 67), new Vector2(31, 68), SpriteEffects.None));
            AddUnangledSprite("cacodeath4", new SpriteRenderDetails(new Rectangle(76, 0, 67, 77), new Vector2(31, 72), SpriteEffects.None));
            AddUnangledSprite("cacodeath5", new SpriteRenderDetails(new Rectangle(0, 50, 69, 66), new Vector2(35, 63), SpriteEffects.None));
            AddUnangledSprite("cacodeath6", new SpriteRenderDetails(new Rectangle(0, 0, 75, 49), new Vector2(37, 47), SpriteEffects.None));
        }
    }
}
