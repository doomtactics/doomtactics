using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DoomTactics
{
    public interface IState
    {
        void OnEnter();
        void OnExit();
        bool IsPaused { get; }
        IState Update(GameTime gameTime);
        void Render(GraphicsDevice device);        
    }
}
