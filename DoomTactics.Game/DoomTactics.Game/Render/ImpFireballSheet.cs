using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DoomTactics
{
    public class ImpFireballSheet : SpriteSheet
    {
        public ImpFireballSheet(Texture2D texture) : base(texture)
        {
            PopulateSheet();
        }

        private void PopulateSheet()
        {
            AddUnangledSprite("impfireball1", new SpriteRenderDetails(new Rectangle(42, 49, 15, 15), SpriteEffects.None));
            AddUnangledSprite("impfireball2", new SpriteRenderDetails(new Rectangle(42, 69, 15, 15), SpriteEffects.None));
            AddUnangledSprite("impfireballdeath1", new SpriteRenderDetails(new Rectangle(0, 49, 37, 35), SpriteEffects.None));
            AddUnangledSprite("impfireballdeath2", new SpriteRenderDetails(new Rectangle(55, 0, 43, 49), SpriteEffects.None));
            AddUnangledSprite("impfireballdeath3", new SpriteRenderDetails(new Rectangle(0, 0, 50, 44), SpriteEffects.None));
        }
    }
}
