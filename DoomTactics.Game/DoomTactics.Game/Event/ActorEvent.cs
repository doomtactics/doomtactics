using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DoomTactics
{
    public struct SpawnActorEvent : IDoomEvent
    {
        private readonly DoomEventType _eventType;
        private readonly ActorBase _actor;
        private readonly string[] _listenerNames;

        public SpawnActorEvent(DoomEventType eventType, ActorBase actorToSpawn, params string[] listenerNames)
        {
            _eventType = eventType;
            _actor = actorToSpawn;
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