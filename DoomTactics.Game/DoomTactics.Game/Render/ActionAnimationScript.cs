using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DoomTactics
{
    public class ActionAnimationScript
    {
        private IList<ScriptSegment> _scriptSegments;
        private readonly string _name;
        private int _currentIndex;

        public string Name { get { return _name; } }

        public ActionAnimationScript(IList<ScriptSegment> scriptSegments, string name)
        {
            _scriptSegments = scriptSegments;
            _name = name;
            _currentIndex = 0;
        }

        public void Start()
        {
            _scriptSegments[_currentIndex].OnStart.Invoke();
            MessagingSystem.DispatchEvent(new AnimationScriptEvent(DoomEventType.AnimationScriptStart, _name));
        }

        public void Update()
        {
            if (_scriptSegments[_currentIndex].EndCondition.Invoke())
            {
                _scriptSegments[_currentIndex].OnComplete.Invoke();
                Next();
            }
        }

        public void Next()
        {
            _currentIndex++;
            if (_currentIndex < _scriptSegments.Count)
            {
                _scriptSegments[_currentIndex].OnStart.Invoke();
            }
            else
            {
                MessagingSystem.DispatchEvent(new AnimationScriptEvent(DoomEventType.AnimationScriptComplete, _name));
            }
        }
    }
}
