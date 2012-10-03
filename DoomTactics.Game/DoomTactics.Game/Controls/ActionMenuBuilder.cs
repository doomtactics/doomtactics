using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Squid;

namespace DoomTactics.Controls
{
    public class ActionMenuBuilder : ControlBuilder<DoomWindow>
    {
        private IList<KeyValuePair<string, MouseClickEventHandler>> _buttonDatas;
        private string _actorName;

        public ActionMenuBuilder()
        {
            _buttonDatas = new List<KeyValuePair<string, MouseClickEventHandler>>();
        }

        public ActionMenuBuilder Action(string buttonName, MouseClickEventHandler callback)
        {
            _buttonDatas.Add(new KeyValuePair<string, MouseClickEventHandler>(buttonName, callback));
            return this;
        }

        public ActionMenuBuilder ActorName(string name)
        {
            _actorName = name;
            return this;
        }

        public override DoomWindow Build()
        {
            var window = new DoomWindow();
            window.Position = _position;
            window.Size = _size;
            window.TitleBar.Text = _actorName;
            window.TitleBar.Button.OnMouseClick += window.Button_OnMouseClick;

            foreach (var buttonData in _buttonDatas)
            {
                var button = new Button();
                button.Text = buttonData.Key;
                button.AutoSize = AutoSize.Vertical;
                button.Dock = DockStyle.Top;
                button.Parent = window;
                if (buttonData.Value != null)
                {
                    button.OnMouseClick += buttonData.Value;
                }
            }

            window.Parent = _parent;
            
            return window;
        }
    }
}
