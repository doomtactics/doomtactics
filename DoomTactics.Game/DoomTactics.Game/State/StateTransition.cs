using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoomTactics
{
    public class StateTransition
    {
        public IState NextState { get; private set; }

        public StateTransition(IState nextState)
        {
            NextState = nextState;
        }
    }
}
