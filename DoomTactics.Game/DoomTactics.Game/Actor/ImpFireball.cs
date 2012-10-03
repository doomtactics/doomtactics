using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoomTactics
{
    public class ImpFireball : ActorBase
    {
        private static long ballNum = 0;

        public ImpFireball(string id) : base(id + ballNum++)
        {
            CurrentAnimation = ActorAnimationManager.Make("impidle", "testimp");
            SpriteSheet = SpriteSheetFactory.CreateSpriteSheet(ActorType.ImpFireball);
        }
    }
}
