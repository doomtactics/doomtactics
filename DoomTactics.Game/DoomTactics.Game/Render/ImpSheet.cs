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
        public ImpSheet(Texture2D texture) : base(texture)
        {
            PopulateSheet();
        }

        private void PopulateSheet()
        {
            _textureMap.Add("impidle1", new Rectangle(0, 0, 40, 57));
            _textureMap.Add("impidle2", new Rectangle(212, 0, 40, 57));
        }
    }
}
