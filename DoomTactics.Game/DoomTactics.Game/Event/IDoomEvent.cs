namespace DoomTactics
{
    public enum DoomEventType
    {
        
    }

    interface IDoomEvent
    {
        DoomEventType EventType { get; }
        string[] ListenerNames { get; }
    }
}
