using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DoomTactics.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Squid;

namespace DoomTactics
{
    public class ActionSelection : GameStateBase
    {
        private ActionMenu _actionMenu;

        public ActionSelection(GameState gameState)
            : base(gameState)
        {
            HighlightHoveredTile = true;
            InputProcessor = new ActionSelectionProcessor(Keyboard.GetState(), Mouse.GetState(), gameState, this);
            _actionMenu = new ActionMenuBuilder()
                    .ActorName("someone")//actor.ActorID)
                    .Action("Action", null)
                    .Action("Wait", null)
                    .Action("Turn", null)
                    .Position(50, 100)
                    .Size(200, 200)
                    .Parent(GameState.Desktop)
                    .Build();           
        }

        public override void OnEnter()
        {
            GameState.Desktop.Visible = true;
            GameState.Desktop.ShowCursor = true;
        }

        public override void OnExit()
        {
            GameState.Desktop.Controls.Remove(_actionMenu);
            _actionMenu = null;
        }

        public override IState Update(GameTime gameTime)
        {
            base.Update(gameTime);
            GameState.SquidInputManager.Update(gameTime);
            GameState.Desktop.Update();

            return NextState;
        }

        public override void Render(GraphicsDevice device)
        {
            base.Render(device);
            GameState.Desktop.Size = new Squid.Point(device.Viewport.Width, device.Viewport.Height);
            GameState.Desktop.Draw();
        }

        public void SwitchToFreeCamera()
        {
            NextState = new FreeCamera(GameState);
        }

        public override bool IsPaused
        {
            get { return false; }
        }
    }
}
