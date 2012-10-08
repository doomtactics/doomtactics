﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DoomTactics
{
    public class ImpFireball : ActorBase
    {
        private static long _ballNum = 0;

        public ImpFireball(string id) : base(id + _ballNum++)
        {
            Height = 32;
            Width = 32;
            CurrentAnimation = ActorAnimationManager.Make("impfireballidle", ActorID);
            SpriteSheet = SpriteSheetFactory.CreateSpriteSheet(ActorType.ImpFireball);
        }

        public override void Die()
        {
            CurrentAnimation = ActorAnimationManager.Make("impfireballdeath", ActorID);
            Velocity = Vector3.Zero;
        }        
    }
}
