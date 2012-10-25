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
        
        // text
        DespawnText,

        // scripting
        AnimationScriptStart,
        AnimationScriptComplete,

        // sound
        PlaySound,
    }    

    public interface IDoomEvent
    {
        DoomEventType EventType { get; }
        string[] ListenerNames { get; }
    }
}
