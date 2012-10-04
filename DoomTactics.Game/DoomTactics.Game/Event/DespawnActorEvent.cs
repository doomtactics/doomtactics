using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoomTactics
{
    public struct DespawnActorEvent : IDoomEvent
    {
        private readonly DoomEventType _eventType;
        private readonly ActorBase _despawnTarget;
        private readonly string[] _listenerNames;

        public DespawnActorEvent(DoomEventType eventType, ActorBase despawnTarget, params string[] listenerNames)
        {
            _eventType = eventType;
            _despawnTarget = despawnTarget;
            _listenerNames = listenerNames;
        }

        public ActorBase DespawnTarget
        {
            get { return _despawnTarget; }
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
