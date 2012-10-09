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
            CurrentState.Update(gameTime);
            if (CurrentState.NextState != null)
            {
                TransitionTo(CurrentState.NextState.NextState);
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
