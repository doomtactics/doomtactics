﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoomTactics
{
    public class ImpFireball : ActorBase
    {
        private static long _ballNum = 0;
        private const float Speed = 2.0f;

        public ImpFireball(string id) : base(id + _ballNum++)
        {
            Height = 32;
            Width = 32;
            CurrentAnimation = ActorAnimationManager.Make("impfireballidle", ActorID);
            SpriteSheet = SpriteSheetFactory.CreateSpriteSheet(ActorType.ImpFireball);
        }
    }
}