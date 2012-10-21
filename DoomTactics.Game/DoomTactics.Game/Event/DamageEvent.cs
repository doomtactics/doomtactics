using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoomTactics
{
    public struct DamageEvent : IDoomEvent
    {
        private readonly DoomEventType _eventType;
        private readonly string[] _listenerNames;
        private readonly int _damage;
        private readonly ActorBase _damagedActor;

        public DamageEvent(DoomEventType eventType, int damage, ActorBase damagedActor, params string[] listenerNames)
        {
            _eventType = eventType;
            _listenerNames = listenerNames;
            _damage = damage;
            _damagedActor = damagedActor;
        }

        public ActorBase DamagedActor
        {
            get { return _damagedActor; }
        }

        public int Damage
        {
            get { return _damage; }
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
