using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoomTactics
{
    public struct TextEvent : IDoomEvent
    {
        private readonly DoomEventType _eventType;
        private readonly TimedText _text;
        private readonly string[] _listenerNames;

        public TextEvent(DoomEventType eventType, TimedText text, params string[] listenerNames)
        {
            _eventType = eventType;
            _text = text;
            _listenerNames = listenerNames;
        }

        public TimedText Text
        {
            get { return _text; }
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
