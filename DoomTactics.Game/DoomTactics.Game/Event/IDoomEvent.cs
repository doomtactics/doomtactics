namespace DoomTactics
{
    public enum DoomEventType
    {
        CameraMoveEvent,

        AnimationStart,
        AnimationEnd
    }    

    public interface IDoomEvent
    {
        DoomEventType EventType { get; }
        string[] ListenerNames { get; }
    }
}
