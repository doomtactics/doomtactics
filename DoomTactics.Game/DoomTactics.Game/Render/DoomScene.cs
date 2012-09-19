using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DoomTactics.Controls;
using Microsoft.Xna.Framework;

namespace DoomTactics
{
    public class DoomScene : DrawableGameComponent
    {
        private DoomDesktop _desktop;

        public DoomScene(Game game) : base(game)
        {
            
        }

        protected override void LoadContent()
        {
            _desktop = new DoomDesktop() {Name = "desk"};
            _desktop.ShowCursor = true;

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            _desktop.Size = new Squid.Point(Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height);
            _desktop.Update();
            _desktop.Draw();
        }
    }
}
