using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoomTactics
{
    public abstract class ActorState
    {
        public virtual bool Targetable { get { return true; } }
        public virtual bool CanMove { get { return true; } }
        public virtual bool CanAct { get { return true; } }
        public virtual bool CanTakeTurn { get { return true; } }
    }
}
