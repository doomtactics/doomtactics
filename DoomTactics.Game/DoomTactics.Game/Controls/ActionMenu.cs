using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoomTactics.Controls
{
    public class ActionMenu : DoomWindow
    {
        public void ShowSubMenu(ActionMenu subMenu)
        {
            subMenu.Position = new Squid.Point(this.Position.x + this.Size.x, this.Position.y + 20);
            subMenu.Parent = this.Parent;
            subMenu.TitleBar.Button.OnMouseClick += (c, e) =>
                                                        {
                                                            subMenu.Visible = false;
                                                            subMenu.Parent = null;
                                                        };
        }

        protected override void Initialize()
        {
            base.Initialize();
            
        }
        
    }
}
