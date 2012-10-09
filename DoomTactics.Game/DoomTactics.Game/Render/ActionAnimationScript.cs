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
        private bool _receivedEvent;

        public string Name { get { return _name; } }

        public ActionAnimationScript(IList<ScriptSegment> scriptSegments, string name)
        {
            _scriptSegments = scriptSegments;
            _name = name;
            _currentIndex = 0;
        }

        public void Start()
        {
            if (_scriptSegments[_currentIndex].OnStart != null)
            {
                _scriptSegments[_currentIndex].OnStart.Invoke();
            }
            MessagingSystem.DispatchEvent(new AnimationScriptEvent(DoomEventType.AnimationScriptStart, _name), _name);
        }

        public void Update()
        {
            if (_currentIndex < _scriptSegments.Count)
            {
                if (_scriptSegments[_currentIndex].EndCondition.Invoke())
                {
                    if (_scriptSegments[_currentIndex].OnComplete != null)
                    {
                        _scriptSegments[_currentIndex].OnComplete.Invoke();
                    }
                    Next();
                }
            }
        }

        private void ReceivedEvent()
        {
            _receivedEvent = true;
        }

        public void Next()
        {
            _receivedEvent = false;
            _currentIndex++;
            MessagingSystem.Unsubscribe(_name);
            if (_currentIndex < _scriptSegments.Count)
            {
                if (_scriptSegments[_currentIndex].EndOnEventSender != null)
                {
                    MessagingSystem.Subscribe((evt) => ReceivedEvent(), _scriptSegments[_currentIndex].EndOnEventType, _name, _scriptSegments[_currentIndex].EndOnEventSender);
                    _scriptSegments[_currentIndex].EndCondition = () => _receivedEvent;
                }
                if (_scriptSegments[_currentIndex].OnStart != null)
                {
                    _scriptSegments[_currentIndex].OnStart.Invoke();
                }
            }
            else
            {
                MessagingSystem.DispatchEvent(new AnimationScriptEvent(DoomEventType.AnimationScriptComplete, _name), _name);
            }
        }
    }
}
