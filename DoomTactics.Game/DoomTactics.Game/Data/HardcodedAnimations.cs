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
                {
                    var anim1 = new AnimationEntry("impidle1", TimeSpan.FromMilliseconds(200));
                    var anim2 = new AnimationEntry("impidle2", TimeSpan.FromMilliseconds(200));
                    var animList = new List<AnimationEntry>() {anim1, anim2};
                    var impIdle = new ActorAnimation(animList, true, "Prototype");
                    ActorAnimationManager.RegisterAnimation("impidle", impIdle);
                }
                // Imp shoot
                {
                    var anim1 = new AnimationEntry("impshoot1", TimeSpan.FromMilliseconds(200));
                    var anim2 = new AnimationEntry("impshoot2", TimeSpan.FromMilliseconds(200));
                    var anim3 = new AnimationEntry("impshoot3", TimeSpan.FromMilliseconds(200));
                    var animList = new List<AnimationEntry>() {anim1, anim2, anim3};
                    var impShoot = new ActorAnimation(animList, false, "Prototype");
                    ActorAnimationManager.RegisterAnimation("impshoot", impShoot);
                }

                // Imp fireball idle
                {
                    var anim1 = new AnimationEntry("impfireball1", TimeSpan.FromMilliseconds(125));
                    var anim2 = new AnimationEntry("impfireball2", TimeSpan.FromMilliseconds(125));
                    var animList = new List<AnimationEntry>() {anim1, anim2};
                    ActorAnimationManager.RegisterAnimation("impfireballidle", new ActorAnimation(animList, true, "Prototype"));
                }
                // Imp fireball death
                {
                    var anim1 = new AnimationEntry("impfireballdeath1", TimeSpan.FromMilliseconds(225));
                    var anim2 = new AnimationEntry("impfireballdeath2", TimeSpan.FromMilliseconds(225));
                    var anim3 = new AnimationEntry("impfireballdeath3", TimeSpan.FromMilliseconds(225));
                    var ballDeath = new ActorAnimation(new List<AnimationEntry>() {anim1, anim2, anim3}, false,
                                                       "Prototype");
                    ActorAnimationManager.RegisterAnimation("impfireballdeath", ballDeath);
                }
            }
        }
    }
}
