using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Squid;

namespace DoomTactics.Controls
{
    public abstract class ControlBuilder<T> 
        where T : Control
    {

// ReSharper disable InconsistentNaming
        protected Point _size;
        protected Point _position;
        protected Control _parent;
// ReSharper restore InconsistentNaming

        public virtual ControlBuilder<T> Size(int length, int width)
        {
            _size = new Point(length, width);
            return this;
        }

        public virtual ControlBuilder<T> Position(int x, int y)
        {
            _position = new Point(x, y);
            return this;
        }

        public virtual ControlBuilder<T> Parent(Control parent)
        {
            _parent = parent;
            return this;
        }

        public abstract T Build();
    }
}
