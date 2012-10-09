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
        public StateTransition NextState { get; private set; }

        private readonly DoomTacticsGame _game;
        private readonly SquidInputManager _squidInputManager;
        private readonly DoomDesktop _desktop;
        private DoomWindow _window;
        private Button[] _buttons;
        private static readonly string[] ButtonNames = new[] {"New Game", "Multiplayer", "Options", "Quit"};

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

            InitializeControls();
        }

        private void InitializeControls()
        {
            _window = new DoomWindowBuilder()
                .Title("DOOM TACTICS")
                .CanClose(false)
                .Size(440, 170)
                .Position(40, 40)
                .Parent(_desktop)
                .Build();
            _window.Size = new Squid.Point(440, 170);
            _window.TitleBar.Text = "DOOM TACTICS";
            _window.Position = new Squid.Point(40, 40);
            _window.Style = "frame";
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

        public void Update(GameTime gameTime)
        {
            _squidInputManager.Update(gameTime);
            _desktop.Update();
        }

        public void Render(GraphicsDevice device)
        {
            _desktop.Size = new Squid.Point(device.Viewport.Width, device.Viewport.Height);
            _desktop.Draw();
        }

        private void NewGameClicked(Control sender, MouseEventArgs args)
        {
            NextState = new StateTransition(new GameState(_game, _squidInputManager));
        }

        private void ExitGameClicked(Control sender, MouseEventArgs args)
        {
            _game.Exit();
        }
    }
}
