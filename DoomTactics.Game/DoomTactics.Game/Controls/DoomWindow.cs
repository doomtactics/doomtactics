using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Squid;

namespace DoomTactics.Controls
{
    public class DoomWindow : Window
    {
        public TitleBar TitleBar { get; private set; }
     
        protected override void Initialize()
        {
            AllowDragOut = true;
            Padding = new Margin(7);

            TitleBar = new TitleBar();
            TitleBar.Dock = DockStyle.Top;
            TitleBar.Size = new Squid.Point(122, 35);
            TitleBar.OnMouseDown += delegate(Control sender, MouseEventArgs args) { StartDrag(); };
            TitleBar.OnMouseUp += delegate(Control sender, MouseEventArgs args) { StopDrag(); };
            TitleBar.Cursor = Cursors.Move;
            TitleBar.Style = "frame2";
            TitleBar.Margin = new Margin(0, 0, 0, -1);            

            AllowDragOut = false;

            Controls.Add(TitleBar);
        }

        public void Button_OnMouseClick(Control sender, MouseEventArgs args)
        {
            Animation.Custom(FadeAndClose());
        }

        private System.Collections.IEnumerator FadeAndClose()
        {
            Enabled = false;

            Animation.Size(new Point(Size.x, 10), 500);

            yield return Animation.Opacity(0, 500);

            Close();
        }
    }

    public class TitleBar : Label 
    {
        public Button Button { get; private set; }

        protected override void Initialize()
        {
 	        Button = new Button();
            Button.Size = new Point(30, 30);
            Button.Style = "button";
            Button.Tooltip = "Close Window";
            Button.Dock = DockStyle.Right;
            Button.Margin = new Margin(0, 8, 8, 8);
            Elements.Add(Button);
        }
    }
}
