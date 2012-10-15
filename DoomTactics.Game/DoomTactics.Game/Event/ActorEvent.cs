using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DoomTactics
{
    public struct ActorEvent : IDoomEvent
    {
        private readonly DoomEventType _eventType;
        private readonly ActorBase _actor;
        private readonly string[] _listenerNames;

        public ActorEvent(DoomEventType eventType, ActorBase actor, params string[] listenerNames)
        {
            _eventType = eventType;
            _actor = actor;
            _listenerNames = listenerNames;
        }

        public ActorBase Actor
        {
            get { return _actor; }
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