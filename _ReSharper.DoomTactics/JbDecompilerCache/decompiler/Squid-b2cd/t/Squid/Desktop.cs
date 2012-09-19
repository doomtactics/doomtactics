// Type: Squid.Desktop
// Assembly: Squid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=5ba804b1072f6e05
// Assembly location: E:\code\DoomTactics\DoomTactics\lib\Squid.dll

using Squid.Xml;
using System.Collections.Generic;

namespace Squid
{
  public class Desktop : Control, IControlContainer
  {
    private Dictionary<string, Cursor> MyCursors = new Dictionary<string, Cursor>();
    private List<Window> ModalQueue = new List<Window>();
    private List<Control> Dropdowns = new List<Control>();
    private Control hot;
    private Window window;
    private float TooltipTimer;
    private bool IsDragDrop;
    private Control DragDropSender;
    private Control DragData;
    private Control _focused;
    internal Control MouseDownControl;

    public Control HotControl { get; internal set; }

    public Control PressedControl { get; internal set; }

    public ControlCollection Controls { get; set; }

    public Label TooltipControl { get; private set; }

    public bool ShowCursor { get; set; }

    [XmlIgnore]
    public string CurrentCursor { get; set; }

    [XmlIgnore]
    public bool DesignMode { get; set; }

    public int TooltipDelay { get; set; }

    public Point TooltipOffset { get; set; }

    public Alignment TooltipAlign { get; set; }

    [XmlIgnore]
    public Control FocusedControl
    {
      get
      {
        return this._focused;
      }
      internal set
      {
        if (this._focused == value)
          return;
        if (this._focused != null)
          this._focused.LostFocus();
        this._focused = value;
        if (this._focused == null)
          return;
        this._focused.GotFocus();
      }
    }

    public Desktop()
    {
      this.TooltipDelay = 400;
      this.TooltipControl = new Label();
      this.TooltipControl.Size = new Point(200, 100);
      this.TooltipControl.NoEvents = true;
      this.TooltipControl.AutoSize = AutoSize.HorizontalVertical;
      this.TooltipControl.Style = "tooltip";
      this.TooltipAlign = Alignment.TopRight;
    }

    public void SetHot(Control control)
    {
      this.HotControl = control;
    }

    public Window GetWindowAt(int x, int y)
    {
      for (int index = this.Controls.Count - 1; index >= 0; --index)
      {
        Window window = this.Controls[index] as Window;
        if (window != null && window.Enabled && window.Visible && window.Hit(GuiHost.MousePosition.x, GuiHost.MousePosition.y))
          return window;
      }
      return (Window) null;
    }

    internal void DoDragDrop(Control sender, Control data)
    {
      if (this.IsDragDrop)
        return;
      this.IsDragDrop = true;
      this.DragData = data;
      this.DragDropSender = sender;
      this.Controls.Add(this.DragData);
    }

    private void ProcessDragDrop()
    {
      if (this.DragData == null)
        return;
      if (this.IsDragDrop)
      {
        this.DragData.Position += GuiHost.MouseMovement;
      }
      else
      {
        this.Controls.Remove(this.DragData);
        this.DragData = (Control) null;
      }
    }

    private void EndDragDrop()
    {
      if (!this.IsDragDrop)
        return;
      this.IsDragDrop = false;
      Control dropTarget = this.GetDropTarget(this.DragDropSender);
      if (dropTarget != null && dropTarget != this.DragDropSender)
      {
        dropTarget.FireOnDragDrop(new DragDropEventArgs()
        {
          DraggedControl = this.DragData,
          Source = this.DragDropSender
        });
      }
      else
      {
        if (dropTarget != null)
          return;
        this.FireOnDragDrop(new DragDropEventArgs()
        {
          DraggedControl = this.DragData,
          Source = this.DragDropSender
        });
      }
    }

    public void Update()
    {
      if (!this.DesignMode)
      {
        this.ProcessDragDrop();
        if (GuiHost.MouseButton(0) == ButtonState.Up)
          this.EndDragDrop();
      }
      int num1 = -1;
      int num2 = -1;
      for (int index = 0; index < GuiHost.Buttons.Length; ++index)
      {
        if (GuiHost.MouseButton(index) == ButtonState.Press)
          num1 = index;
        if (GuiHost.MouseButton(index) == ButtonState.Down)
          num2 = index;
      }
      if (num1 == -1)
      {
        this.hot = this.GetControlAt(GuiHost.MousePosition.x, GuiHost.MousePosition.y);
        if (!this.DesignMode && this.hot != null && this.ModalQueue.Count > 0)
        {
          Control control = (Control) this.ModalQueue[this.ModalQueue.Count - 1];
          bool flag = control == this.hot || this.hot.IsChildOf(control);
          if (this.hot.Owner != null)
            flag = this.hot.Owner.IsChildOf(control);
          if (!flag)
            this.hot = (Control) this;
        }
        if (this.hot != this.HotControl)
        {
          if (this.hot != null)
          {
            this.CurrentCursor = this.hot.Cursor;
            this.hot.MouseEnter();
          }
          if (this.HotControl != null)
            this.HotControl.MouseLeave();
          this.HotControl = this.hot;
        }
      }
      else if (num1 > 0)
        this.hot = (Control) null;
      for (int index = 0; index < GuiHost.Buttons.Length; ++index)
      {
        if (GuiHost.MouseButton(index) == ButtonState.Up && this.MouseDownControl != null)
        {
          this.MouseDownControl.MouseUp(index);
          break;
        }
      }
      if (!this.DesignMode && num2 > -1)
      {
        if (this.ModalQueue.Count == 0)
        {
          Window windowAt = this.GetWindowAt(GuiHost.MousePosition.x, GuiHost.MousePosition.y);
          if (windowAt != null && windowAt != this.window && windowAt.Dock == DockStyle.None)
          {
            windowAt.BringToFront();
            windowAt.Focus();
            this.window = windowAt;
          }
        }
        this.FocusedControl = this.hot == null || !this.hot.AllowFocus ? (Control) null : this.hot;
        if (this.Dropdowns.Count > 0)
        {
          if (this.hot == null)
          {
            this.CloseDropdowns();
          }
          else
          {
            for (int index = this.Dropdowns.Count - 1; index >= 0 && (this.hot != this.Dropdowns[index] && !this.Dropdowns[index].Contains(this.hot)); --index)
            {
              this.Dropdowns[index].Parent = (Control) null;
              this.Dropdowns.RemoveAt(index);
            }
          }
        }
      }
      if (!this.DesignMode)
      {
        if (this.hot != null)
          this.hot.DoEvents();
        if (this.FocusedControl != null)
          this.FocusedControl.DoKeyEvents();
        this.SetTooltip(this.hot);
      }
      this.UpdateEarly();
      this.UpdateLate();
      foreach (KeyData keyData in GuiHost.KeyBuffer)
      {
        if (keyData.Pressed && keyData.Key == Keys.TAB)
        {
          if (GuiHost.ShiftPressed)
            this.TabPrevious();
          else
            this.TabNext();
        }
      }
    }

    public Control PickControl(int x, int y, Desktop.PickMode mode)
    {
      if (mode == Desktop.PickMode.Control)
        return this.PickFirst(x, y);
      if (mode == Desktop.PickMode.Container)
        return this.PickDeep(x, y);
      else
        return (Control) null;
    }

    public void ShowDropdown(Control control, bool stack)
    {
      if (this.DesignMode || control == null)
        return;
      if (!stack)
        this.CloseDropdowns();
      if (this.Dropdowns.Contains(control))
        return;
      if (this.Dropdowns.Count > 0)
      {
        int num = 0;
        bool flag = false;
        for (int index = 0; index < this.Dropdowns.Count; ++index)
        {
          if (this.Dropdowns[index].Owner == control.Owner)
          {
            num = index;
            flag = true;
            break;
          }
        }
        if (flag)
        {
          for (int index = this.Dropdowns.Count - 1; index >= num; --index)
          {
            this.Dropdowns[index].Parent = (Control) null;
            this.Dropdowns.RemoveAt(index);
          }
        }
      }
      control.Parent = (Control) this;
      this.Dropdowns.Add(control);
    }

    public void CloseDropdowns()
    {
      foreach (Control control in this.Dropdowns)
        control.Parent = (Control) null;
      this.Dropdowns.Clear();
    }

    public void SetTooltip(Control control)
    {
      if (this.DesignMode)
        return;
      if (control == null || string.IsNullOrEmpty(control.Tooltip))
      {
        this.TooltipControl.Parent = (Control) null;
        this.TooltipControl.Text = string.Empty;
      }
      else
      {
        if (this.TooltipControl.Parent == null)
        {
          this.TooltipTimer += GuiHost.TimeElapsed;
          if ((double) this.TooltipTimer > (double) this.TooltipDelay)
          {
            this.TooltipTimer = 0.0f;
            this.TooltipControl.Opacity = 0.0f;
            this.TooltipControl.Parent = (Control) this;
          }
        }
        if (this.TooltipControl.Parent == null)
          return;
        this.TooltipControl.Text = control.Tooltip;
        this.TooltipControl.UpdateEarly();
        Point point1 = GuiHost.MousePosition;
        switch (this.TooltipAlign)
        {
          case Alignment.TopLeft:
            point1 = GuiHost.MousePosition;
            break;
          case Alignment.TopCenter:
            point1 = GuiHost.MousePosition - new Point(this.TooltipControl.Size.x / 2, 0);
            break;
          case Alignment.TopRight:
            point1 = GuiHost.MousePosition - new Point(this.TooltipControl.Size.x, 0);
            break;
          case Alignment.MiddleLeft:
            point1 = GuiHost.MousePosition - new Point(0, this.TooltipControl.Size.y / 2);
            break;
          case Alignment.MiddleCenter:
            point1 = GuiHost.MousePosition - new Point(this.TooltipControl.Size.x / 2, this.TooltipControl.Size.y / 2);
            break;
          case Alignment.MiddleRight:
            point1 = GuiHost.MousePosition - new Point(this.TooltipControl.Size.x, this.TooltipControl.Size.y / 2);
            break;
          case Alignment.BottomLeft:
            point1 = GuiHost.MousePosition - new Point(0, this.TooltipControl.Size.y);
            break;
          case Alignment.BottomCenter:
            point1 = GuiHost.MousePosition - new Point(this.TooltipControl.Size.x / 2, this.TooltipControl.Size.y);
            break;
          case Alignment.BottomRight:
            point1 = GuiHost.MousePosition - new Point(this.TooltipControl.Size.x, this.TooltipControl.Size.y);
            break;
        }
        Point point2 = point1 + this.TooltipOffset;
        Point point3 = point2 + this.TooltipControl.Size;
        if (point3.x > this.Size.x)
          point2.x = this.Size.x - this.TooltipControl.Size.x;
        if (point3.y > this.Size.y)
          point2.y = this.Size.y - this.TooltipControl.Size.y;
        this.TooltipControl.Position = point2;
        this.TooltipControl.UpdateEarly();
        this.TooltipControl.BringToFront();
        this.TooltipControl.Opacity = 1f;
      }
    }

    public void AddCursor(string key, Cursor cursor)
    {
      if (this.MyCursors.ContainsKey(key))
        this.MyCursors[key] = cursor;
      else
        this.MyCursors.Add(key, cursor);
    }

    public new void Draw()
    {
      GuiHost.Renderer.Scissor(this.Area.x1, this.Area.y1, this.Area.x2, this.Area.y2);
      GuiHost.Renderer.StartBatch();
      base.Draw();
      if (this.ShowCursor)
        this.DrawCursor(GuiHost.MousePosition.x, GuiHost.MousePosition.y);
      GuiHost.Renderer.EndBatch(true);
    }

    public void DrawCursor(int x, int y)
    {
      if (this.CurrentCursor != null && this.MyCursors.ContainsKey(this.CurrentCursor))
      {
        this.MyCursors[this.CurrentCursor].Draw(x, y);
      }
      else
      {
        if (!this.MyCursors.ContainsKey(Cursors.Default))
          return;
        this.MyCursors[Cursors.Default].Draw(x, y);
      }
    }

    protected override void DrawStyle(Style style, float opacity)
    {
    }

    public void TabNext()
    {
      this.Tab(1);
    }

    public void TabPrevious()
    {
      this.Tab(-1);
    }

    internal void Tab(int dir)
    {
      int num = 0;
      if (this.FocusedControl != null)
        num = this.FocusedControl.TabIndex;
      int index = num + dir;
      if (index <= 0)
        index = this.FindHighestTabIndex(0);
      if (index <= 0)
        return;
      Control control = this.FindTabIndex(index) ?? this.FindTabIndex(1);
      if (control == null)
        return;
      control.Focus();
    }

    internal void RegisterModal(Window control)
    {
      this.ModalQueue.Add(control);
    }

    internal void UnregisterModal(Window control)
    {
      this.ModalQueue.Remove(control);
    }

    internal bool CheckModalLock(Control control)
    {
      if (this.ModalQueue.Count <= 0)
        return false;
      Control control1 = (Control) this.ModalQueue[this.ModalQueue.Count - 1];
      return control1 != control && !control.IsChildOf(control1);
    }

    public enum PickMode
    {
      Control,
      Container,
    }
  }
}
