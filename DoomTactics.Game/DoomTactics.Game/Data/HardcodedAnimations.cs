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
                // imp pain
                {
                    var anim1 = new AnimationEntry("imppain", TimeSpan.FromMilliseconds(300));
                    var animList = new List<AnimationEntry>() {anim1};
                    var impPain = new ActorAnimation(animList, false, "Prototype");
                    ActorAnimationManager.RegisterAnimation("imppain", impPain);
                }
                // imp death
                {
                    var anim1 = new AnimationEntry("impdeath1", TimeSpan.FromMilliseconds(200));
                    var anim2 = new AnimationEntry("impdeath2", TimeSpan.FromMilliseconds(200));
                    var anim3 = new AnimationEntry("impdeath3", TimeSpan.FromMilliseconds(200));
                    var anim4 = new AnimationEntry("impdeath4", TimeSpan.FromMilliseconds(200));
                    var anim5 = new AnimationEntry("impdeath5", TimeSpan.FromMilliseconds(200));
                    var animList = new List<AnimationEntry>() {anim1, anim2, anim3, anim4, anim5};
                    var impDie = new ActorAnimation(animList, false, "Prototype");
                    ActorAnimationManager.RegisterAnimation("impdie", impDie);
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
