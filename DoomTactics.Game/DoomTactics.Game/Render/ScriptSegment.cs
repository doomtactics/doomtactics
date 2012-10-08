using System;

namespace DoomTactics
{
    public class ScriptSegment
    {
        public Action OnStart;
        public Func<bool> EndCondition;
        public Action OnComplete;
    }
}