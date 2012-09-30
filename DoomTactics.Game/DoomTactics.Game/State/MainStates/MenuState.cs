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
    public class MenuState : IState
    {
        private readonly DoomTacticsGame _game;
        private readonly SquidInputManager _squidInputManager;
        private readonly DoomDesktop _desktop;
        private DoomWindow _window;
        private Button[] _buttons;
        private static readonly string[] ButtonNames = new[] {"New Game", "Multiplayer", "Options", "Quit"};
        private IState _nextState;

        private enum MainMenuOptions
        {
            NewGame = 0,
            Multiplayer,
            Options,
            Quit
        }

        public MenuState(DoomTacticsGame game, SquidInputManager squidInputManager, DoomDesktop desktop)
        {
            _game = game;
            _squidInputManager = squidInputManager;
            _desktop = desktop;
            _desktop.ShowCursor = true;
            _nextState = null;

            InitializeControls();
        }

        private void InitializeControls()
        {
            _window = new DoomWindow(canClose: false);
            _window.Size = new Squid.Point(440, 170);
            _window.Position = new Squid.Point(40, 40);
            _window.Style = "frame";
            _window.TitleBar.Text = "DOOM TACTICS";            
            _window.Parent = _desktop;

            _buttons = new Button[ButtonNames.Length];
            for (int i = 0; i < ButtonNames.Length; i++)
            {
                var button = new Button();
                button.Text = ButtonNames[i];
                button.AutoSize = AutoSize.Vertical;
                button.Dock = DockStyle.Top;
                button.Parent = _window;
                _buttons[i] = button;
            }
           
            _buttons[(int)MainMenuOptions.NewGame].OnMouseClick += NewGameClicked;
            _buttons[(int) MainMenuOptions.Quit].OnMouseClick += ExitGameClicked;
        }

        public void OnEnter()
        {
            
        }

        public void OnExit()
        {
            
        }

        public bool IsPaused
        {
            get { return false; }
        }

        public IState Update(GameTime gameTime)
        {
            _squidInputManager.Update(gameTime);
            _desktop.Update();

            return _nextState;
        }

        public void Render(GraphicsDevice device)
        {
            _desktop.Size = new Squid.Point(device.Viewport.Width, device.Viewport.Height);
            _desktop.Draw();
        }

        private void NewGameClicked(Control sender, MouseEventArgs args)
        {
            _nextState = new GameState(_game, _squidInputManager);
        }

        private void ExitGameClicked(Control sender, MouseEventArgs args)
        {
            _game.Exit();
        }
    }
}
