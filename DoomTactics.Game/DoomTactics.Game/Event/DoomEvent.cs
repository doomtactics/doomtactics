namespace DoomTactics
{
    public struct DoomEvent : IDoomEvent
    {
        private readonly DoomEventType _eventType;
        private readonly string[] _listenerNames;

        public DoomEvent(DoomEventType eventType, params string[] listenerNames)
        {
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
    }
}
