using System;

namespace DoomTactics
{
    public class ScriptSegment
    {
        public Action<ScriptVariables> OnStart;
        public Func<ScriptVariables, bool> EndCondition;
        public Action<ScriptVariables> OnComplete;
        public DoomEventType EndOnEventType;
        public string EndOnEventSender;
    }
}