using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Squid;

namespace DoomTactics.Controls
{
    public class ActionMenuBuilder : ControlBuilder<ActionMenu>
    {
        private IList<ActionMenuEntry> _buttonDatas;
        private string _actorName;
        private int _currentIndex;
        private bool _isSubMenu;

        public ActionMenuBuilder()
        {
            _buttonDatas = new List<ActionMenuEntry>();
            _currentIndex = 0;
        }

        public ActionMenuBuilder Action(string buttonName, MouseClickEventHandler callback)
        {
            var ame = new ActionMenuEntry(buttonName, _currentIndex++, callback);
            _buttonDatas.Add(ame);
            return this;
        }

        public ActionMenuBuilder AsSubMenu()
        {
            _isSubMenu = true;
            return this;
        }

        public ActionMenuBuilder WithSubMenu(string buttonName, ActionMenu actionMenu)
        {
            var ame = new ActionMenuEntry(buttonName, _currentIndex++, actionMenu);
            _buttonDatas.Add(ame);
            return this;
        }

        public ActionMenuBuilder ActorName(string name)
        {
            _actorName = name;
            return this;
        }

        public override ActionMenu Build()
        {
            var window = new ActionMenu();
            window.Position = _position;
            window.Size = _size;
            window.TitleBar.Text = _actorName;
            window.TitleBar.Button.OnMouseClick += window.Button_OnMouseClick;

            if (_isSubMenu)
                window.TitleBar.Visible = false;

            var sortedButtonData = _buttonDatas.OrderBy(x => x.ActionOrder);

            foreach (var buttonData in sortedButtonData)
            {
                var button = new Button();
                button.Text = buttonData.ActionName;
                button.AutoSize = AutoSize.Vertical;
                button.Dock = DockStyle.Top;
                button.Parent = window;
                if (buttonData.Callback != null)
                {
                    button.OnMouseClick += buttonData.Callback;
                }
                else if (buttonData.SubMenu != null)
                {
                    ActionMenu subMenu = buttonData.SubMenu;
                    button.OnMouseClick += (c, e) => window.ShowSubMenu(subMenu);
                }
            }

            window.Parent = _parent;

            return window;
        }

        private struct ActionMenuEntry
        {
            public string ActionName;
            public int ActionOrder;
            public MouseClickEventHandler Callback;
            public ActionMenu SubMenu;

            public ActionMenuEntry(string actionName, int actionOrder, MouseClickEventHandler callback)
            {
                ActionName = actionName;
                ActionOrder = actionOrder;
                Callback = callback;
                SubMenu = null;
            }

            public ActionMenuEntry(string actionName, int actionOrder, ActionMenu subMenu)
            {
                ActionName = actionName;
                ActionOrder = actionOrder;
                Callback = null;
                SubMenu = subMenu;
            }
        }
    }
}
