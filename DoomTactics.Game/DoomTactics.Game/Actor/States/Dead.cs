using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoomTactics
{
    public class Dead : ActorState
    {
        public override bool Targetable
        {
            get { return false; }
        }

        public override bool CanMove
        {
            get { return false; }
        }

        public override bool CanAct
        {
            get { return false; }
        }

        public override bool CanTakeTurn
        {
            get { return false; }
        }
    }
}
