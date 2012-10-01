using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Squid;

namespace DoomTactics.Controls
{
    public class DoomWindowBuilder
    {
        private string _title;
        private Squid.Point _size;
        private Squid.Point _position;
        private bool _canClose;
        private Control _parentControl;

        public DoomWindowBuilder()
        {
            _title = string.Empty;
            _size = Point.Zero;
            _position = Point.Zero;
            _canClose = false;
            _parentControl = null;
        }

        public DoomWindowBuilder Title(string title)
        {
            _title = title;
            return this;
        }

        public DoomWindowBuilder Size(int x, int y)
        {
            _size = new Point(x, y);
            return this;
        }

        public DoomWindowBuilder Position(int x, int y)
        {
            _position = new Point(x, y);
            return this;
        }

        public DoomWindowBuilder CanClose(bool canClose)
        {
            _canClose = canClose;
            return this;
        }

        public DoomWindowBuilder Parent(Control parentControl)
        {
            _parentControl = parentControl;
            return this;
        }

        public DoomWindow Build()
        {
            var window = new DoomWindow();
            window.Position = _position;
            window.Size = _size;
            window.TitleBar.Text = _title;
            if (_canClose)
            {
                window.TitleBar.Button.OnMouseClick += window.Button_OnMouseClick;
            }
            window.Parent = _parentControl;

            return window;
        }
    }
}
