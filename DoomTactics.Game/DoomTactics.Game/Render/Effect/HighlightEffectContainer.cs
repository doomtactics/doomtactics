using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DoomTactics
{
    public class HighlightEffectContainer
    {
        private Effect _effect;

        public HighlightEffectContainer(ContentManager contentManager)
        {
            _effect = contentManager.Load<Effect>("shaders\\highlight");            
        }        
      
        public Effect GetEffect()
        {
            return _effect;
        }

    }
}
