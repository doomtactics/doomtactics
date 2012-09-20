using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DoomTactics
{
    public class BootstrapState : IState
    {       
        public BootstrapState()
        {
            
        }

        public void OnEnter()
        {
            throw new NotImplementedException();
        }

        public void OnExit()
        {
            throw new NotImplementedException();
        }

        public bool IsPaused
        {
            get { return false; }
        }

        public IState Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public void Render(GraphicsDevice device)
        {
            
        }

        public void ProcessInput(KeyboardState keyState, MouseState mouseState, GameTime gameTime)
        {
            
        }
    }
}
