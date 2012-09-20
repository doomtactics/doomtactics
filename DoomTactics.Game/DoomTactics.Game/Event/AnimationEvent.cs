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
        private readonly AnimationEventType _animationEventType;
        private readonly string[] _listenerNames;

        public AnimationEvent(DoomEventType eventType, AnimationEventType animationEventType, params string[] listenerNames)
        {
            _animationEventType = animationEventType;
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

        public AnimationEventType AnimationEventType
        {
            get { return _animationEventType; }
        }
    }
}
