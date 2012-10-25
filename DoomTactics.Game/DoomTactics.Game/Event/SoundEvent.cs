using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoomTactics
{
    public struct SoundEvent : IDoomEvent
    {
        private readonly DoomEventType _eventType;
        private readonly string _soundName;
        private readonly string[] _listenerNames;

        public SoundEvent(DoomEventType eventType, string soundName, params string[] listenerNames)
        {
            _eventType = eventType;
            _soundName = soundName;
            _listenerNames = listenerNames;
        }

        public string SoundName
        {
            get { return _soundName; }
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
