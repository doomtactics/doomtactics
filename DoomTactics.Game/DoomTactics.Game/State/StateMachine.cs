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
        private IState _currentState;

        public StateMachine(IState initialState)
        {
            _currentState = initialState;
        }

        public IState Currentstate { get { return _currentState; } }

        public void Update(GameTime gameTime)
        {
            var nextState = _currentState.Update(gameTime);
            if (nextState != null)
            {
                _currentState.OnExit();
                _currentState = nextState;
                _currentState.OnEnter();
            }
        }

        public void Render(GraphicsDevice graphicsDevice)
        {
            _currentState.Render(graphicsDevice);
        }
    }
}
