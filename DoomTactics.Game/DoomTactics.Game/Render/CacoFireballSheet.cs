using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DoomTactics
{
    public class CacoFireballSheet : SpriteSheet
    {
        public CacoFireballSheet(Texture2D texture) : base(texture)
        {
            PopulateSheet();
        }

        private void PopulateSheet()
        {
            AddUnangledSprite("cacofireball1", new SpriteRenderDetails(new Rectangle(46, 48, 16, 16), new Vector2(7, 8), SpriteEffects.None));
            AddUnangledSprite("cacofireball2", new SpriteRenderDetails(new Rectangle(0, 97, 15, 15), new Vector2(7, 7), SpriteEffects.None));
            AddUnangledSprite("cacofireballdeath1", new SpriteRenderDetails(new Rectangle(0, 48, 45, 48), new Vector2(23, 24), SpriteEffects.None));
            AddUnangledSprite("cacofireballdeath2", new SpriteRenderDetails(new Rectangle(54, 0, 50, 42), new Vector2(25, 21), SpriteEffects.None));
            AddUnangledSprite("cacofireballdeath3", new SpriteRenderDetails(new Rectangle(0, 0, 53, 47), new Vector2(26, 23), SpriteEffects.None));
        }
    }
}
