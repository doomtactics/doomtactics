using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DoomTactics
{
    public class CacoFireball : ActorBase
    {
        private static long _ballNum = 0;
        private const string DeathSound = "fireballdie";

        public CacoFireball(string id, Vector3 position, Vector3 velocity) : base(id + _ballNum++, position, velocity)
        {
            Height = 32;
            Width = 32;
            CurrentAnimation = ActorAnimationManager.Make("cacofireballidle", ActorId);
            SpriteSheet = SpriteSheetFactory.CreateSpriteSheet(ActorType.CacoFireball);
            IncreaseChargeTime = false;
            MovementVelocityModifier = 8.0f;
        }

        public override void SetupStats()
        {
            BaseStats = GameplayStats.Statsless();
            CurrentStats = GameplayStats.Statsless();
        }

        public override void Die()
        {
            MessagingSystem.DispatchEvent(new SoundEvent(DoomEventType.PlaySound, DeathSound), ActorId);
            CurrentAnimation = ActorAnimationManager.Make("cacofireballdeath", ActorId);
            Velocity = Vector3.Zero;
        }        
    }
}
