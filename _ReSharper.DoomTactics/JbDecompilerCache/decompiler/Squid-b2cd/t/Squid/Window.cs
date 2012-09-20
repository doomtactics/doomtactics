// Type: Squid.Window
// Assembly: Squid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=5ba804b1072f6e05
// Assembly location: E:\code\DoomTactics\DoomTactics\lib\Squid.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Squid
{
  [Toolbox]
  public class Window : Control, IControlContainer
  {
    private Resizer Sizer;
    private Point DragOffset;
    private bool IsDragging;

    [Hidden]
    public ControlCollection Controls { get; set; }

    [Hidden]
    public bool Modal { get; set; }

    [Category("Feel")]
    public int SnapDistance { get; set; }

    [Category("Feel")]
    public bool AllowDragOut { get; set; }

    [Category("Feel")]
    public bool Resizable
    {
      get
      {
        return this.Sizer.ParentControl == this;
      }
      set
      {
        if (value)
        {
          if (this.Sizer.ParentControl == this)
            return;
          this.Elements.Add((Control) this.Sizer);
        }
        else
          this.Elements.Remove((Control) this.Sizer);
      }
    }

    [Category("Feel")]
    public Margin GripSize
    {
      get
      {
        return this.Sizer.GripSize;
      }
      set
      {
        this.Sizer.GripSize = value;
      }
    }

    protected override void OnUpdate()
    {
      if (!this.IsDragging)
        return;
      this.Drag();
    }

    protected override void Initialize()
    {
      this.Style = "window";
      this.Scissor = true;
      this.Sizer = new Resizer();
      this.Sizer.Dock = DockStyle.Fill;
      this.SnapDistance = 12;
      this.MinSize = new Point(200, 100);
      this.MaxSize = new Point(800, 600);
    }

    public void StartDrag()
    {
      this.DragOffset = GuiHost.MousePosition - this.Position;
      this.IsDragging = true;
    }

    public void StopDrag()
    {
      this.IsDragging = false;
    }

    private void Drag()
    {
      Point point = GuiHost.MousePosition - this.DragOffset;
      if (!this.Modal)
      {
        foreach (Control control in (List<Control>) this.Container.Controls)
        {
          if (control is Window && control != this)
          {
            int y = control.Position.y;
            int num1 = control.Position.y + control.Size.y;
            int x = control.Position.x;
            int num2 = control.Position.x + control.Size.x;
            if (Math.Abs(point.x - num2) <= this.SnapDistance && (point.y + this.Size.y >= y && point.y <= num1))
              point.x = num2;
            if (Math.Abs(point.x + this.Size.x - x) <= this.SnapDistance && (point.y + this.Size.y >= y && point.y <= num1))
              point.x = x - this.Size.x;
            if (Math.Abs(point.y - num1) <= this.SnapDistance && (point.x + this.Size.x >= x && point.x <= num2))
              point.y = num1;
            if (Math.Abs(point.y + this.Size.y - y) <= this.SnapDistance && (point.x + this.Size.x >= x && point.x <= num2))
              point.y = y - this.Size.y;
          }
        }
      }
      if (!this.AllowDragOut)
      {
        if (point.x < 0)
          point.x = 0;
        if (point.y < 0)
          point.y = 0;
        if (point.x + this.Size.x > this.Parent.Size.x)
          point.x = this.Parent.Size.x - this.Size.x;
        if (point.y + this.Size.y > this.Parent.Size.y)
          point.y = this.Parent.Size.y - this.Size.y;
        if (point.x < this.SnapDistance)
          point.x = 0;
        if (point.y < this.SnapDistance)
          point.y = 0;
        if (point.x + this.Size.x > this.Parent.Size.x - this.SnapDistance)
          point.x = this.Parent.Size.x - this.Size.x;
        if (point.y + this.Size.y > this.Parent.Size.y - this.SnapDistance)
          point.y = this.Parent.Size.y - this.Size.y;
      }
      this.Position = point;
    }

    public virtual void Show(Desktop target)
    {
      target.Controls.Add((Control) this);
      if (this.Modal)
        target.RegisterModal(this);
      this.Visible = true;
    }

    public virtual void Close()
    {
      if (this.Desktop == null)
        return;
      if (this.Modal)
        this.Desktop.UnregisterModal(this);
      this.Desktop.Controls.Remove((Control) this);
      this.Visible = false;
    }
  }
}
