using System;
using System.Collections.Generic;
using System.Linq;
using DoomTactics.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Squid;

namespace DoomTactics
{
    public class DoomTacticsGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        private IState state;
        private DoomDesktop _desktop;
        private SquidInputManager _inputManager;

        public DoomTacticsGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            GuiHost.Renderer = new RendererXna(this);            
            base.Initialize();

        }

        protected override void LoadContent()
        {
            state = new GameState(this);
            _desktop = new DoomDesktop() { Name = "desk" };
            _desktop.ShowCursor = true;
            _inputManager = new SquidInputManager(this);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {           
            state.Update(gameTime);
            _inputManager.Update(gameTime);
            _desktop.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GuiHost.TimeElapsed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            _desktop.Size = new Squid.Point(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            GraphicsDevice.Clear(Color.Coral);            
            state.Render(GraphicsDevice);
            _desktop.Draw();
            base.Draw(gameTime);
        }
    }
}
