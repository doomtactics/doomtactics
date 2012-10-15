using System;
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
            CurrentAnimation = ActorAnimationManager.Make("impfireballidle", ActorId);
            SpriteSheet = SpriteSheetFactory.CreateSpriteSheet(ActorType.ImpFireball);
            IncreaseChargeTime = false;
        }

        public override void Die()
        {
            CurrentAnimation = ActorAnimationManager.Make("impfireballdeath", ActorId);
            Velocity = Vector3.Zero;
        }        
    }
}
