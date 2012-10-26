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
        private static DoomDesktop _desktop;
        private static SquidInputManager _inputManager;
        private bool _enableMusic;
        private bool _enableSound;
       
        public DoomTacticsGame(bool enableMusic, bool enableSound)
        {
            _enableMusic = enableMusic;
            _enableSound = enableSound;
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
            MusicManager.Initialize(Content);
            MusicManager.SetMusic(_enableMusic);
            SoundManager.Initialize(Content);
            SoundManager.SetPlaySound(_enableSound);
            MessagingSystem.Subscribe(SoundManager.OnPlaySound, DoomEventType.PlaySound, "soundsystem", null);

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

        public static IState CreateMenuState(DoomTacticsGame gameInstance)
        {
            return new MenuState(gameInstance, _inputManager, _desktop);
        }
    }
}
