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
        protected IInputProcessor InputProcessor;

        protected readonly GameState GameState;

        protected GameStateBase(GameState gameState)
        {
            GameState = gameState;
        }

        public virtual bool HighlightHoveredTile { get; protected set; }

        public abstract void OnEnter();

        public abstract void OnExit();

        public abstract bool IsPaused { get; }

        public virtual IState Update(GameTime gameTime)
        {
            if (GameState.ActiveUnit == null)
            {
                foreach (var actor in GameState.Level.Actors)
                {
                    actor.IncreaseCT();
                }
            }

            foreach (var actor in GameState.Level.Actors)
            {
                actor.Update(gameTime);
            }

            return null;
        }

        public virtual void Render(GraphicsDevice device)
        {
            RenderTiles();
            RenderActors();
        }

        private void RenderActors()
        {
            throw new NotImplementedException();
        }

        private void RenderTiles()
        {
            throw new NotImplementedException();
        }
    }
}
