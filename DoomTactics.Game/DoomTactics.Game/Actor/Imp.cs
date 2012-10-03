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
        public Imp(string id, Vector3 position)
            : base(id)
        {
            Height = 56;
            Width = 40;
            Speed = 4;
            Position = position;
            CurrentAnimation = ActorAnimationManager.Make("impidle", "testimp");
            SpriteSheet = SpriteSheetFactory.CreateSpriteSheet(ActorType.Imp);
        }

        public void ShootFireball(Tile tile)
        {
            
        }
    }
}
