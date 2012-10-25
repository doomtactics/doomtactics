using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoomTactics
{
    public class ScriptVariables
    {
        
    }

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
            _scriptSegments.Add(new ScriptSegment());
            return this;
        }

        public ActionAnimationScriptBuilder OnStart(Action onStart)
        {
            return OnStart((p) => onStart());

        }

        public ActionAnimationScriptBuilder OnStart(Action<ScriptVariables> onStart)
        {
            _scriptSegments[_currentIndex].OnStart = onStart;
            return this;
        }
       
        public ActionAnimationScriptBuilder EndCondition(Func<bool> checkCondition)
        {
            _scriptSegments[_currentIndex].EndCondition = checkCondition;
            return this;
        }

        public ActionAnimationScriptBuilder EndOnEvent(DoomEventType eventType, string sender)
        {
            _scriptSegments[_currentIndex].EndOnEventType = eventType;
            _scriptSegments[_currentIndex].EndOnEventSender = sender;
            return this;
        }

        public ActionAnimationScriptBuilder OnComplete(Action onComplete)
        {
            return OnComplete((p) => onComplete());
        }        

        public ActionAnimationScriptBuilder OnComplete(Action<ScriptVariables> onComplete)
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
            MessagingSystem.Subscribe((evt) => OnFinish(evt, _onScriptFinish), DoomEventType.AnimationScriptComplete, _name, null);
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
}
