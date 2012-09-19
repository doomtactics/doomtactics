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
        SpriteBatch spriteBatch;
        private IState state;
        private BasicEffect basicEffect;
        private DoomDesktop _desktop;

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
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {           
            state.Update(gameTime);
            _desktop.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GuiHost.TimeElapsed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            _desktop.Size = new Squid.Point(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            GraphicsDevice.Clear(Color.Coral);
            _desktop.Draw();
            state.Render(GraphicsDevice);              
            base.Draw(gameTime);
        }
    }
}
