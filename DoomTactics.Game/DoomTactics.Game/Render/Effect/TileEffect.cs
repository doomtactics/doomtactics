using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DoomTactics
{    
    public class TileEffect : DoomEffect
    {
        public TileEffect(ContentManager contentManager)
        {
            Effect = contentManager.Load<Effect>("shaders/tile");
            Tint = new Vector4(1.0f);
        }
        
        public Texture2D Texture
        {
            set { Effect.Parameters["Texture"].SetValue(value); }
        }

        public Vector4 Tint
        {
            set { Effect.Parameters["Tint"].SetValue(value); }
        }
    }
}
