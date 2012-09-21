using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoomTactics
{
    public enum AnimationEventType
    {
        AnimationStarted,
        AnimationFinished
    }

    public class AnimationEvent : IDoomEvent
    {
        private readonly DoomEventType _eventType;
        private readonly string _entityName;
        private readonly string[] _listenerNames;

        public AnimationEvent(DoomEventType eventType, string entityName, params string[] listenerNames)
        {
            _entityName = entityName;
            _eventType = eventType;
            _listenerNames = listenerNames;
        }

        public DoomEventType EventType
        {
            get { return _eventType; }
        }

        public string[] ListenerNames
        {
            get { return _listenerNames; }
        }

        public string EntityName
        {
            get { return _entityName; }
        }
    }
}
