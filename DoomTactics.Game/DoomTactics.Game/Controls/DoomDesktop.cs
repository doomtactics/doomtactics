using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Squid;

namespace DoomTactics.Controls
{
    public class DoomDesktop : Desktop
    {
        
        protected override void Initialize()
        {
            Reflector.RegisterAssemblies(System.Reflection.Assembly.GetAssembly(typeof(DoomWindow)));

            #region cursors

            Point cursorSize = new Point(32, 32);
            Point halfSize = cursorSize / 2;

            AddCursor(Cursors.Default, new Cursor { Texture = "squid\\cursors\\Arrow.png", Size = cursorSize, HotSpot = Point.Zero });
            AddCursor(Cursors.Link, new Cursor { Texture = "squid\\cursors\\Link.png", Size = cursorSize, HotSpot = Point.Zero });
            AddCursor(Cursors.Move, new Cursor { Texture = "squid\\cursors\\Move.png", Size = cursorSize, HotSpot = halfSize });
            AddCursor(Cursors.Select, new Cursor { Texture = "squid\\cursors\\Select.png", Size = cursorSize, HotSpot = halfSize });
            AddCursor(Cursors.SizeNS, new Cursor { Texture = "squid\\cursors\\SizeNS.png", Size = cursorSize, HotSpot = halfSize });
            AddCursor(Cursors.SizeWE, new Cursor { Texture = "squid\\cursors\\SizeWE.png", Size = cursorSize, HotSpot = halfSize });
            AddCursor(Cursors.HSplit, new Cursor { Texture = "squid\\cursors\\SizeNS.png", Size = cursorSize, HotSpot = halfSize });
            AddCursor(Cursors.VSplit, new Cursor { Texture = "squid\\cursors\\SizeWE.png", Size = cursorSize, HotSpot = halfSize });
            AddCursor(Cursors.SizeNESW, new Cursor { Texture = "squid\\cursors\\SizeNESW.png", Size = cursorSize, HotSpot = halfSize });
            AddCursor(Cursors.SizeNWSE, new Cursor { Texture = "squid\\cursors\\SizeNWSE.png", Size = cursorSize, HotSpot = halfSize });            

            #endregion

            #region styles

            ControlStyle itemStyle = new ControlStyle();
            itemStyle.Tiling = TextureMode.Grid;
            itemStyle.Grid = new Margin(6);
            itemStyle.Texture = "squid\\button_hot.dds";
            itemStyle.Default.Texture = "squid\\button_default.dds";
            itemStyle.Pressed.Texture = "squid\\button_down.dds";
            itemStyle.SelectedPressed.Texture = "squid\\button_down.dds";
            itemStyle.Focused.Texture = "squid\\button_down.dds";
            itemStyle.SelectedFocused.Texture = "squid\\button_down.dds";
            itemStyle.Selected.Texture = "squid\\button_down.dds";
            itemStyle.SelectedHot.Texture = "squid\\button_down.dds";
            itemStyle.TextPadding = new Margin(10, 0, 10, 0);

            ControlStyle inputStyle = new ControlStyle();
            inputStyle.Texture = "squid\\input_default.dds";
            inputStyle.Hot.Texture = "squid\\input_focused.dds";
            inputStyle.Focused.Texture = "squid\\input_focused.dds";
            inputStyle.TextPadding = new Margin(8);
            inputStyle.Tiling = TextureMode.Grid;
            inputStyle.Focused.Tint = ColorInt.RGBA(1, 0, 0, 1);
            inputStyle.Grid = new Margin(6);

            ControlStyle buttonStyle = new ControlStyle();
            buttonStyle.Texture = "squid\\button_default.dds";
            buttonStyle.Hot.Texture = "squid\\button_hot.dds";
            buttonStyle.Focused.Texture = "squid\\button_hot.dds";
            buttonStyle.Pressed.Texture = "squid\\button_down.dds";
            buttonStyle.Checked.Texture = "squid\\button_down.dds";
            buttonStyle.CheckedHot.Texture = "squid\\button_down.dds";
            buttonStyle.TextAlign = Alignment.MiddleCenter;
            buttonStyle.Tiling = TextureMode.Grid;
            buttonStyle.Grid = new Margin(2);

            ControlStyle frameStyle = new ControlStyle();
            frameStyle.Tiling = TextureMode.GridRepeat;
            frameStyle.Grid = new Margin(8);
            frameStyle.Texture = "squid\\grid.dds";

            ControlStyle frameStyle2 = new ControlStyle();
            frameStyle2.CheckedHot.Tiling = TextureMode.Grid;
            frameStyle2.Tiling = TextureMode.Grid;
            frameStyle2.Grid = new Margin(8);
            frameStyle2.Texture = "squid\\grid2.dds";
            frameStyle2.TextPadding = new Margin(8);

            ControlStyle vscrollTrackStyle = new ControlStyle();
            vscrollTrackStyle.Tiling = TextureMode.Grid;
            vscrollTrackStyle.Grid = new Margin(0, 3, 0, 3);
            vscrollTrackStyle.Texture = "squid\\vscroll_track.dds";

            ControlStyle vscrollButtonStyle = new ControlStyle();
            vscrollButtonStyle.Tiling = TextureMode.Grid;
            vscrollButtonStyle.Grid = new Margin(0, 4, 0, 4);
            vscrollButtonStyle.Texture = "squid\\vscroll_button.dds";
            vscrollButtonStyle.Hot.Texture = "squid\\vscroll_button_hot.dds";
            vscrollButtonStyle.Pressed.Texture = "squid\\vscroll_button_down.dds";

            ControlStyle vscrollUp = new ControlStyle();
            vscrollUp.Default.Texture = "squid\\vscrollUp_default.dds";
            vscrollUp.Hot.Texture = "squid\\vscrollUp_hot.dds";
            vscrollUp.Pressed.Texture = "squid\\vscrollUp_down.dds";
            vscrollUp.Focused.Texture = "squid\\vscrollUp_hot.dds";

            ControlStyle hscrollTrackStyle = new ControlStyle();
            hscrollTrackStyle.Tiling = TextureMode.Grid;
            hscrollTrackStyle.Grid = new Margin(3, 0, 3, 0);
            hscrollTrackStyle.Texture = "squid\\hscroll_track.dds";

            ControlStyle hscrollButtonStyle = new ControlStyle();
            hscrollButtonStyle.Tiling = TextureMode.Grid;
            hscrollButtonStyle.Grid = new Margin(4, 0, 4, 0);
            hscrollButtonStyle.Texture = "squid\\hscroll_button.dds";
            hscrollButtonStyle.Hot.Texture = "squid\\hscroll_button_hot.dds";
            hscrollButtonStyle.Pressed.Texture = "squid\\hscroll_button_down.dds";

            ControlStyle hscrollUp = new ControlStyle();
            hscrollUp.Default.Texture = "squid\\hscrollUp_default.dds";
            hscrollUp.Hot.Texture = "squid\\hscrollUp_hot.dds";
            hscrollUp.Pressed.Texture = "squid\\hscrollUp_down.dds";
            hscrollUp.Focused.Texture = "squid\\hscrollUp_hot.dds";


            ControlStyle checkButtonStyle = new ControlStyle();
            checkButtonStyle.Default.Texture = "squid\\checkbox_default.dds";
            checkButtonStyle.Hot.Texture = "squid\\checkbox_hot.dds";
            checkButtonStyle.Pressed.Texture = "squid\\checkbox_down.dds";
            checkButtonStyle.Checked.Texture = "squid\\checkbox_checked.dds";
            checkButtonStyle.CheckedFocused.Texture = "squid\\checkbox_checked_hot.dds";
            checkButtonStyle.CheckedHot.Texture = "squid\\checkbox_checked_hot.dds";
            checkButtonStyle.CheckedPressed.Texture = "squid\\checkbox_down.dds";

            ControlStyle comboLabelStyle = new ControlStyle();
            comboLabelStyle.TextPadding = new Margin(10, 0, 0, 0);
            comboLabelStyle.Default.Texture = "squid\\combo_default.dds";
            comboLabelStyle.Hot.Texture = "squid\\combo_hot.dds";
            comboLabelStyle.Pressed.Texture = "squid\\combo_down.dds";
            comboLabelStyle.Focused.Texture = "squid\\combo_hot.dds";
            comboLabelStyle.Tiling = TextureMode.Grid;
            comboLabelStyle.Grid = new Margin(6, 0, 0, 0);

            ControlStyle comboButtonStyle = new ControlStyle();
            comboButtonStyle.Default.Texture = "squid\\combo_button_default.dds";
            comboButtonStyle.Hot.Texture = "squid\\combo_button_hot.dds";
            comboButtonStyle.Pressed.Texture = "squid\\combo_button_down.dds";
            comboButtonStyle.Focused.Texture = "squid\\combo_button_hot.dds";

            ControlStyle labelStyle = new ControlStyle();
            labelStyle.TextAlign = Alignment.TopRight;
            labelStyle.TextPadding = new Squid.Margin(8);

            Skin skin = new Skin();

            skin.Styles.Add("item", itemStyle);
            skin.Styles.Add("textbox", inputStyle);
            skin.Styles.Add("button", buttonStyle);
            skin.Styles.Add("frame", frameStyle);
            skin.Styles.Add("frame2", frameStyle2);
            skin.Styles.Add("checkBox", checkButtonStyle);
            skin.Styles.Add("comboLabel", comboLabelStyle);
            skin.Styles.Add("comboButton", comboButtonStyle);
            skin.Styles.Add("vscrollTrack", vscrollTrackStyle);
            skin.Styles.Add("vscrollButton", vscrollButtonStyle);
            skin.Styles.Add("vscrollUp", vscrollUp);
            skin.Styles.Add("hscrollTrack", hscrollTrackStyle);
            skin.Styles.Add("hscrollButton", hscrollButtonStyle);
            skin.Styles.Add("hscrollUp", hscrollUp);
            skin.Styles.Add("multiline", labelStyle);

            GuiHost.SetSkin(skin);

            #endregion
            /*
            var window1 = new DoomWindow();
            window1.Size = new Squid.Point(440, 170);
            window1.Position = new Squid.Point(40, 40);
            window1.Style = "frame";
            window1.TitleBar.Text = "These imps are [color=ffcc0000]SICK!!![/color]";
            window1.Parent = this;

            var label = new Label();
            label.Text = "fuck yea they are [color=ff00cccc]dude[/color]";
            label.Size = new Squid.Point(100, 100);
            label.Position = new Squid.Point(50, 50);
            label.Parent = window1;        */    

        }
    }
}
