namespace DoomTactics
{
    public enum DoomEventType
    {
        // camera
        CameraMoveEvent,

        // animation
        AnimationStart,
        AnimationEnd,

        // gameplay -- turn
        ChargeTimeReached,
    }    

    public interface IDoomEvent
    {
        DoomEventType EventType { get; }
        string[] ListenerNames { get; }
    }
}
