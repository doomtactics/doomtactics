using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoomTactics.Data
{
    public static class HardcodedAnimations
    {
        public static void CreateAnimations()
        {
            {
                // Imp idle            
                var anim1 = new AnimationEntry("impidle1", TimeSpan.FromMilliseconds(200));
                var anim2 = new AnimationEntry("impidle2", TimeSpan.FromMilliseconds(200));
                var animList = new List<AnimationEntry>() {anim1, anim2};
                var impIdle = new ActorAnimation(animList, true, "Prototype");
                ActorAnimationManager.RegisterAnimation("impidle", impIdle);
            }
        }
    }
}
