using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoomTactics
{
    public class ActionAnimationScriptBuilder
    {
        private readonly IList<ScriptSegment> _scriptSegments;
        private int _currentIndex;
        private Action _onScriptFinish;
        private string _name;

        public ActionAnimationScriptBuilder()
        {
            _scriptSegments = new List<ScriptSegment>();
            _currentIndex = -1;
        }

        public ActionAnimationScriptBuilder Name(string name)
        {
            _name = name;
            return this;
        }

        public ActionAnimationScriptBuilder Segment()
        {
            _currentIndex++;
            return this;
        }

        public ActionAnimationScriptBuilder OnStart(Action onStart)
        {
            _scriptSegments[_currentIndex].OnStart = onStart;
            return this;
        }

        //public ActionAnimationScriptBuilder TriggeredByEvent(DoomEventType eventType, string actorName)
        //{
        //    _scriptSegments[_currentIndex].TriggeringEvent = eventType;
        //    _scriptSegments[_currentIndex].TriggeringActor = actorName;
        //    return this;
        //}

        public ActionAnimationScriptBuilder EndCondition(Func<bool> checkCondition)
        {
            _scriptSegments[_currentIndex].EndCondition = checkCondition;
            return this;
        }

        public ActionAnimationScriptBuilder OnComplete(Action onComplete)
        {
            _scriptSegments[_currentIndex].OnComplete = onComplete;
            return this;
        }        

        public ActionAnimationScriptBuilder Finish(Action onScriptFinish)
        {
            _onScriptFinish = onScriptFinish;
            return this;
        }
        
        public ActionAnimationScript Build()
        {
            var aas = new ActionAnimationScript(_scriptSegments, _name);
            MessagingSystem.Subscribe((evt) => OnFinish(evt, _onScriptFinish), DoomEventType.AnimationScriptComplete, "AnimationScript");
            return aas;
        }

        private void OnFinish(IDoomEvent doomEvent, Action onFinish)
        {
            var evt = (AnimationScriptEvent) doomEvent;
            if (evt.ScriptName == _name)
            {
                onFinish.Invoke();   
            }
        }
    }

    /*
     * var animationScript = new AnimationScript()
     *     .Segment().Start(spawnActor).EndCondition(targetIsHit).OnComplete(fireball.Die())
     *     .Segemnt().TriggedByEvent(despawnActor).StartOnEvent(eventType).EndCondition(animationComplete).Finish();
     * 
     *
     */
}
