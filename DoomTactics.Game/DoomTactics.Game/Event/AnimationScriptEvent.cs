using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoomTactics
{
    public struct AnimationScriptEvent : IDoomEvent
    {
        private readonly DoomEventType _eventType;
        private readonly string _scriptName;
        private readonly string[] _listenerNames;

        public AnimationScriptEvent(DoomEventType eventType, string scriptName, params string[] listenerNames)
        {
            _eventType = eventType;
            _scriptName = scriptName;
            _listenerNames = listenerNames;
        }

        public string ScriptName
        {
            get { return _scriptName; }
        }

        public DoomEventType EventType
        {
            get { return _eventType; }
        }

        public string[] ListenerNames
        {
            get { return _listenerNames; }
        }
    }
}
