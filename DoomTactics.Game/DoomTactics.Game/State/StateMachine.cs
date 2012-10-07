using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DoomTactics
{
    public class StateMachine
    {
        private readonly Stack<IState> _stateStack;         

        public StateMachine(IState initialState)
        {
            _stateStack = new Stack<IState>();
            _stateStack.Push(initialState);
            CurrentState.OnEnter();
        }

        public IState CurrentState { get { return _stateStack.Peek(); } }

        public void Update(GameTime gameTime)
        {
            var stateTransition = CurrentState.Update(gameTime);
            if (stateTransition != null)
            {
                if (stateTransition.ReturnToPreviousState)
                {
                    CurrentState.OnExit();
                    _stateStack.Pop();
                    CurrentState.OnEnter();
                }
                else
                {
                    TransitionTo(stateTransition.NextState);
                }
            }
        }

        public void Render(GraphicsDevice graphicsDevice)
        {
            CurrentState.Render(graphicsDevice);
        }

        public void SetState(IState newState)
        {
            TransitionTo(newState);
        }    
   
        private void TransitionTo(IState nextState)
        {
            var oldState = _stateStack.Pop();
            oldState.OnExit();
            nextState.OnEnter();
            _stateStack.Push(nextState);
        }
    }
}
