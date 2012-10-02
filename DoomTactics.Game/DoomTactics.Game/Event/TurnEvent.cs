using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoomTactics
{
    public struct TurnEvent : IDoomEvent
    {
        private readonly DoomEventType _eventType;
        private readonly string[] _listenerNames;
        private readonly ActorBase _actor;

        public TurnEvent(DoomEventType eventType, ActorBase actor, params string[] listenerNames)
        {
            _eventType = eventType;
            _actor = actor;
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

        public ActorBase Actor
        {
            get { return _actor; }
        }
    }
}
