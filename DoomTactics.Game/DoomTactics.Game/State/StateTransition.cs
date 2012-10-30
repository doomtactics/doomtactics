using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoomTactics
{
    public class StateTransition
    {
        public Func<IState> NextState { get; private set; }

        public StateTransition(Func<IState> nextState)
        {
            NextState = nextState;
        }
    }
}
