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

        // gameplay -- actor spawn etc
        SpawnActor,
        DespawnActor,
        RemoveFromCurrentTile,

        // damage
        DisplayDamage,

        // scripting
        AnimationScriptStart,
        AnimationScriptComplete,
    }    

    public interface IDoomEvent
    {
        DoomEventType EventType { get; }
        string[] ListenerNames { get; }
    }
}
