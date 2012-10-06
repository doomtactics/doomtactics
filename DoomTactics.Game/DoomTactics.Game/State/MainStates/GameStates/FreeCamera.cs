using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DoomTactics.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DoomTactics
{
    public class FreeCamera : GameStateBase
    {
        public FreeCamera(DoomDesktop desktop, GameState gameState)
            : base(desktop, gameState)
        {
            HighlightHoveredTile = false;
        }

        public override void OnEnter()
        {
            Desktop.Visible = false;
        }

        public override void OnExit()
        {
            
        }

        public override bool IsPaused
        {
            get { return false; }
        }

        public override IState Update(GameTime gameTime)
        {
            IState nextState = base.Update(gameTime);
            if (nextState != null)
                return nextState;


            return nextState;
        }

        public override void Render(GraphicsDevice device)
        {
            base.Render(device);
        }
    }
}
