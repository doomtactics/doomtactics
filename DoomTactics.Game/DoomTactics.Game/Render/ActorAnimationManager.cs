using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoomTactics
{
    public static class ActorAnimationManager
    {
        private static IDictionary<string, ActorAnimation> _animations = new Dictionary<string, ActorAnimation>(); 

        public static void RegisterAnimation(string name, ActorAnimation anim)
        {
            _animations.Add(name, anim);
        }
        
        public static ActorAnimation Make(string animationName, string entityName)
        {
            // entity name is needed for the events dispatched by the animation
            return _animations[animationName].MakeCopy(entityName);
        }
    }
}
