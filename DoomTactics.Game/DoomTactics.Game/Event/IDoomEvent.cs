namespace DoomTactics
{
    public enum DoomEventType
    {
        CameraMoveEvent,
    }    

    public interface IDoomEvent
    {
        DoomEventType EventType { get; }
        string[] ListenerNames { get; }
    }
}
