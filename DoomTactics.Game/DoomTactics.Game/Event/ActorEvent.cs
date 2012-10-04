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
        private readonly ActorType _actorType;
        private readonly Vector3 _spawnPosition;
        private readonly Vector3 _initialVelocity;
        private readonly string[] _listenerNames;

        public SpawnActorEvent(DoomEventType eventType, ActorType actorType, Vector3 spawnPosition, Vector3 initialVelocity, params string[] listenerNames)
        {
            _eventType = eventType;
            _actorType = actorType;
            _spawnPosition = spawnPosition;
            _initialVelocity = initialVelocity;
            _listenerNames = listenerNames;
        }

        public Vector3 InitialVelocity
        {
            get { return _initialVelocity; }
        }

        public Vector3 SpawnPosition
        {
            get { return _spawnPosition; }
        }

        public ActorType ActorType
        {
            get { return _actorType; }
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