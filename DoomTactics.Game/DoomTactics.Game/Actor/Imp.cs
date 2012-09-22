using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DoomTactics
{
    public class Imp : ActorBase
    {
        public Imp(string id, Vector3 position, Texture2D impTex)
            : base(id)
        {
            Height = 56;
            Width = 40;
            Position = position;
            CurrentAnimation = ActorAnimationManager.Make("impidle", "testimp");
            SpriteSheet = new ImpSheet(impTex);
        }
        
    }
}
