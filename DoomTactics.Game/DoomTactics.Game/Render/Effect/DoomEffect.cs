using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DoomTactics
{
    public abstract class DoomEffect
    {
        protected Effect Effect;

        public Matrix WorldViewProj
        {
            set 
            { 
                Effect.Parameters["WorldViewProj"].SetValue(value); 
            }
        }

        public Effect GetEffect()
        {
            return Effect;
        }

    }
}
