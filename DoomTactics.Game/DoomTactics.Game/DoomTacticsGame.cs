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
        private StateMachine _stateMachine;
        private DoomDesktop _desktop;
        private SquidInputManager _inputManager;

        public DoomTacticsGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 960;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            GuiHost.Renderer = new RendererXna(this);            
            base.Initialize();

        }

        protected override void LoadContent()
        {
            _desktop = new DoomDesktop() { Name = "desk" };
            _inputManager = new SquidInputManager(this);
            var menuState = new MenuState(this, _inputManager, _desktop);
            _stateMachine = new StateMachine(menuState);
        }

        protected override void Update(GameTime gameTime)
        {           
            _stateMachine.Update(gameTime);            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GuiHost.TimeElapsed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            
            GraphicsDevice.Clear(Color.Coral);            
            _stateMachine.Render(GraphicsDevice);
            base.Draw(gameTime);
        }
    }
}
