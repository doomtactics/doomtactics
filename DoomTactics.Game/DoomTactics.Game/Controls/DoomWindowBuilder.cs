using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Squid;

namespace DoomTactics.Controls
{
    public class DoomWindowBuilder : ControlBuilder<DoomWindow>
    {
        private string _title;
        private bool _canClose;

        public DoomWindowBuilder()
        {
            _title = string.Empty;
            _size = Point.Zero;
            _position = Point.Zero;
            _canClose = false;
            _parent = null;
        }

        public DoomWindowBuilder Title(string title)
        {
            _title = title;
            return this;
        }

        public DoomWindowBuilder CanClose(bool canClose)
        {
            _canClose = canClose;
            return this;
        }

        public override DoomWindow Build()
        {
            var window = new DoomWindow();
            window.Position = _position;
            window.Size = _size;
            window.TitleBar.Text = _title;
            if (_canClose)
            {
                window.TitleBar.Button.OnMouseClick += window.Button_OnMouseClick;
            }
            window.Parent = _parent;

            return window;
        }
    }
}
