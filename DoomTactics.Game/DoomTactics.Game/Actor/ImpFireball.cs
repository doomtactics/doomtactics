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

        public ImpFireball(string id, Vector3 position, Vector3 velocity) : base(id + _ballNum++, position, velocity)
        {
            Height = 32;
            Width = 32;
            CurrentAnimation = ActorAnimationManager.Make("impfireballidle", ActorId);
            SpriteSheet = SpriteSheetFactory.CreateSpriteSheet(ActorType.ImpFireball);
            IncreaseChargeTime = false;
            MovementVelocityModifier = 10.0f;
        }

        public override void SetupStats()
        {
            BaseStats = GameplayStats.Statsless();
            CurrentStats = GameplayStats.Statsless();
        }

        public override void Die()
        {
            CurrentAnimation = ActorAnimationManager.Make("impfireballdeath", ActorId);
            Velocity = Vector3.Zero;
        }        
    }
}
