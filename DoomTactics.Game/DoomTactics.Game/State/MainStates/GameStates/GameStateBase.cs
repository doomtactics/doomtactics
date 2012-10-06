using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DoomTactics.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DoomTactics
{
    public abstract class GameStateBase : IState
    {
        protected readonly DoomDesktop Desktop;
        protected readonly GameState GameState;

        protected GameStateBase(DoomDesktop desktop, GameState gameState)
        {
            Desktop = desktop;
            GameState = gameState;
        }

        public virtual bool HighlightHoveredTile { get; protected set; }

        public abstract void OnEnter();

        public abstract void OnExit();

        public abstract bool IsPaused { get; }

        public virtual IState Update(GameTime gameTime)
        {
            foreach (var actor in GameState.Level.Actors)
            {
                actor.Update(gameTime);
            }

            return null;
        }

        public virtual void Render(GraphicsDevice device)
        {
            
        }
    }
}
