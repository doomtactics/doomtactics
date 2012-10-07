using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoomTactics
{
    public class StateTransition
    {
        public IState NextState { get; private set; }
        public bool ReturnToPreviousState { get; private set; }

        public StateTransition(IState nextState) : this(nextState, false)
        {
            
        }

        public StateTransition(IState nextState, bool returnToPreviousState)
        {
            NextState = nextState;
            ReturnToPreviousState = returnToPreviousState;
        }
    }
}
