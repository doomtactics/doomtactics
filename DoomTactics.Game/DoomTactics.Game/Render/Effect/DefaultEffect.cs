using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DoomTactics
{    
    public class DefaultEffect : DoomEffect
    {
        public DefaultEffect(ContentManager contentManager)
        {
            Effect = contentManager.Load<Effect>("shaders/default");
        }
        
        public Texture2D Texture
        {
            set { Effect.Parameters["Texture"].SetValue(value); }
        }
    }
}
