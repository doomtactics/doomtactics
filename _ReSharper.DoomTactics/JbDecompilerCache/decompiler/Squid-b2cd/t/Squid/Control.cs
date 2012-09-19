// Type: Squid.Control
// Assembly: Squid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=5ba804b1072f6e05
// Assembly location: E:\code\DoomTactics\DoomTactics\lib\Squid.dll

using Squid.Xml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Squid
{
  public class Control
  {
    private static Dictionary<int, KeyValuePair<Control, Rectangle>> ScissorStack = new Dictionary<int, KeyValuePair<Control, Rectangle>>();
    internal DockRegion Bounds = new DockRegion();
    internal DockRegion Dock0 = new DockRegion();
    internal DockRegion Dock1 = new DockRegion();
    private float FadeOut = 1f;
    private bool _visible = true;
    private bool _enabled = true;
    private bool _parentEnabled = true;
    private AnchorStyles _anchor = AnchorStyles.Top | AnchorStyles.Left;
    private SizeChangedEventHandler OnSizeChanged;
    private MouseEnterEventHandler OnMouseEnter;
    private MouseLeaveEventHandler OnMouseLeave;
    private MouseUpEventHandler OnMouseUp;
    private MouseDownEventHandler OnMouseDown;
    private MouseDownEventHandler OnMousePress;
    private MouseClickEventHandler OnMouseClick;
    private MouseDoubleClickEventHandler OnMouseDoubleClick;
    private KeyEventHandler OnKeyDown;
    private KeyEventHandler OnKeyUp;
    private FocusEventHandler OnGotFocus;
    private FocusEventHandler OnLostFocus;
    private DragDropEventHandler OnDragDrop;
    protected Rectangle Area;
    protected ElementCollection Elements;
    internal bool _isElement;
    internal bool Removed;
    internal Control Owner;
    private float FadeIn;
    private DateTime Clicked;
    private bool Double;
    private Margin _padding;
    private Margin _margin;
    private Point _size;
    private Point _position;
    private Control _parent;
    private ControlState _state;
    private ControlState _oldState;
    private Point _floatingPosition;
    private Point _floatingSize;
    private DockStyle _dock;

    internal Control ParentControl
    {
      get
      {
        return this._parent;
      }
      set
      {
        if (this._parent == value)
          return;
        this._parent = value;
        this.SetBounds();
      }
    }

    /// <summary>
    /// Returns true if the control is a child element
    /// 
    /// </summary>
    public bool IsElement
    {
      get
      {
        return this._isElement;
      }
    }

    /// <summary>
    /// Returns the depth of the control
    /// 
    /// </summary>
    public int Depth { get; internal set; }

    /// <summary>
    /// Name of the control
    /// 
    /// </summary>
    [DefaultValue("")]
    [Category("Base")]
    public string Name { get; set; }

    /// <summary>
    /// Opacity of the control
    ///             This is muliplied with any style opacity and hierarchical opacity
    /// 
    /// </summary>
    [ValueRange(0.0f, 1f)]
    [Category("Design")]
    [DefaultValue(1f)]
    public float Opacity { get; set; }

    /// <summary>
    /// Gets/Sets the autosize behaviour
    /// 
    /// </summary>
    [Category("Layout")]
    [DefaultValue(AutoSize.None)]
    public AutoSize AutoSize { get; set; }

    /// <summary>
    /// Gets/Sets wether or not the control processes OnDragDrop events
    /// 
    /// </summary>
    [DefaultValue(false)]
    [Category("Feel")]
    public bool AllowDrop { get; set; }

    /// <summary>
    /// Gets/Sets wether or not the control is able to acquire focus
    /// 
    /// </summary>
    [Category("Feel")]
    [DefaultValue(false)]
    public bool AllowFocus { get; set; }

    /// <summary>
    /// Gets/Sets wether or not hardware scissor test is used
    /// 
    /// </summary>
    [Category("Design")]
    [DefaultValue(false)]
    public bool Scissor { get; set; }

    /// <summary>
    /// Gets/Sets the docking behaviour
    /// 
    /// </summary>
    [DefaultValue(DockStyle.None)]
    [Category("Layout")]
    public DockStyle Dock
    {
      get
      {
        return this._dock;
      }
      set
      {
        if (value == DockStyle.None)
        {
          this._size = this._floatingSize;
          this._position = this._floatingPosition;
        }
        else if (this._dock == DockStyle.None)
        {
          this._floatingSize = this._size;
          this._floatingPosition = this._position;
        }
        this._dock = value;
      }
    }

    /// <summary>
    /// Name of the ControlStyle
    /// 
    /// </summary>
    [Style]
    [DefaultValue("")]
    [Category("Skin")]
    public string Style { get; set; }

    /// <summary>
    /// Gets/Sets wether or not the control is interactive and processes any events
    /// 
    /// </summary>
    [Category("Base")]
    [DefaultValue(false)]
    public bool NoEvents { get; set; }

    /// <summary>
    /// user defined data object
    /// 
    /// </summary>
    [XmlIgnore]
    [Hidden]
    public object Tag { get; set; }

    /// <summary>
    /// user defined data object
    /// 
    /// </summary>
    [XmlIgnore]
    [Hidden]
    public object UserData { get; set; }

    /// <summary>
    /// tab index
    /// 
    /// </summary>
    [DefaultValue(0)]
    [Category("Feel")]
    public int TabIndex { get; set; }

    /// <summary>
    /// Gets/Sets wether or not the control is visible
    /// 
    /// </summary>
    [Category("Base")]
    [DefaultValue(true)]
    public bool Visible
    {
      get
      {
        return this._visible;
      }
      set
      {
        this._visible = value;
      }
    }

    /// <summary>
    /// Returns the parent of the control as IControlContainer
    /// 
    /// </summary>
    public IControlContainer Container
    {
      get
      {
        return this._parent as IControlContainer;
      }
    }

    /// <summary>
    /// Name of the cursor to use
    /// 
    /// </summary>
    [Category("Design")]
    [DefaultValue("")]
    public string Cursor { get; set; }

    /// <summary>
    /// Tooltip text
    /// 
    /// </summary>
    [DefaultValue("")]
    [Category("Design")]
    public string Tooltip { get; set; }

    /// <summary>
    /// Returns the z-index
    /// 
    /// </summary>
    public int ZIndex
    {
      get
      {
        if (this.Container != null)
          return this.Container.Controls.Count - (this.Container.Controls.IndexOf(this) + 1);
        else
          return 0;
      }
    }

    /// <summary>
    /// Gets/Sets wether or not the control, and all its children, is enabled
    /// 
    /// </summary>
    [DefaultValue(true)]
    [Category("Base")]
    public bool Enabled
    {
      get
      {
        if (!this._parentEnabled)
          return false;
        else
          return this._enabled;
      }
      set
      {
        if (this._enabled == value)
          return;
        this._enabled = value;
        if (this is IControlContainer)
        {
          foreach (Control control in (List<Control>) ((IControlContainer) this).Controls)
            control.SetEnabled(value);
        }
        foreach (Control control in (List<Control>) this.Elements)
          control.SetEnabled(value);
      }
    }

    /// <summary>
    /// Gets/Sets the parent
    /// 
    /// </summary>
    [XmlIgnore]
    [Hidden]
    public Control Parent
    {
      get
      {
        return this._parent;
      }
      set
      {
        if (value == this || value == this._parent || value != null && value.IsChildOf(this))
          return;
        if (this.Container != null)
          this.Container.Controls.BaseRemove(this);
        this.ParentControl = value;
        if (this.Container == null)
          return;
        this.Container.Controls.BaseAdd(this);
      }
    }

    /// <summary>
    /// Gets/Setss the minimum size.
    ///             This is only used during Control.Resize
    /// 
    /// </summary>
    [Category("Layout")]
    [DefaultValue(typeof (Point), "0; 0")]
    public Point MinSize { get; set; }

    /// <summary>
    /// Gets/Setss the maximum size.
    ///             This is only used during Control.Resize
    /// 
    /// </summary>
    [Category("Layout")]
    [DefaultValue(typeof (Point), "0; 0")]
    public Point MaxSize { get; set; }

    /// <summary>
    /// Gets/Sets the size
    /// 
    /// </summary>
    [DefaultValue(typeof (Point), "0; 0")]
    [Category("Layout")]
    public Point Size
    {
      get
      {
        return this._size;
      }
      set
      {
        if (this._size.x == value.x && this._size.y == value.y)
          return;
        this._size = value;
        if (this.OnSizeChanged != null)
          this.OnSizeChanged(this);
        this.SetBounds();
      }
    }

    /// <summary>
    /// Gets/Sets the position (relative to parent)
    /// 
    /// </summary>
    [DefaultValue(typeof (Point), "0; 0")]
    [Category("Layout")]
    public Point Position
    {
      get
      {
        return this._position;
      }
      set
      {
        if (this._position.x == value.x && this._position.y == value.y)
          return;
        this._position = value;
        this.SetBounds();
      }
    }

    /// <summary>
    /// Gets/Sets the anchoring behaviour
    /// 
    /// </summary>
    [Category("Layout")]
    [DefaultValue(AnchorStyles.Top | AnchorStyles.Left)]
    public AnchorStyles Anchor
    {
      get
      {
        return this._anchor;
      }
      set
      {
        if (this._anchor == value)
          return;
        this._anchor = value;
        this.SetBounds();
      }
    }

    /// <summary>
    /// Defines the space around a control that keeps other controls at a specified distance from the control's borders.
    /// 
    /// </summary>
    [DefaultValue(typeof (Margin), "0; 0; 0; 0")]
    [Category("Layout")]
    public Margin Margin
    {
      get
      {
        return this._margin;
      }
      set
      {
        this._margin = value;
      }
    }

    /// <summary>
    /// Defines the space inside of a control that keeps child controls at a specified distance from the control's borders.
    /// 
    /// </summary>
    [Category("Layout")]
    [DefaultValue(typeof (Margin), "0; 0; 0; 0")]
    public Margin Padding
    {
      get
      {
        return this._padding;
      }
      set
      {
        this._padding = value;
      }
    }

    /// <summary>
    /// Gets/Sets the state
    /// 
    /// </summary>
    [XmlIgnore]
    [Hidden]
    public ControlState State
    {
      get
      {
        return this._state;
      }
      set
      {
        if (this._state == value)
          return;
        this._oldState = this._state;
        this._state = value;
        this.OnStateChanged();
        float num = this.FadeIn;
        this.FadeIn = this.FadeOut;
        this.FadeOut = num;
      }
    }

    /// <summary>
    /// Gets/Sets the local fading speed
    /// 
    /// </summary>
    [Category("Design")]
    [DefaultValue(0.0f)]
    public float FadeSpeed { get; set; }

    /// <summary>
    /// Returns the screen position
    /// 
    /// </summary>
    public Point Location
    {
      get
      {
        if (this._parent != null)
          return this._parent.Location + this._position;
        else
          return this._position;
      }
    }

    /// <summary>
    /// Returns the desktop
    /// 
    /// </summary>
    public Desktop Desktop
    {
      get
      {
        if (this is Desktop)
          return (Desktop) this;
        if (this._parent != null)
          return this._parent.Desktop;
        else
          return (Desktop) null;
      }
    }

    /// <summary>
    /// Use this to run animations as coroutines
    /// 
    /// </summary>
    public ControlAnimation Animation { get; private set; }

    public event SizeChangedEventHandler OnSizeChanged
    {
      [MethodImpl(MethodImplOptions.Synchronized)] add
      {
        this.OnSizeChanged = this.OnSizeChanged + value;
      }
      [MethodImpl(MethodImplOptions.Synchronized)] remove
      {
        this.OnSizeChanged = this.OnSizeChanged - value;
      }
    }

    public event MouseEnterEventHandler OnMouseEnter
    {
      [MethodImpl(MethodImplOptions.Synchronized)] add
      {
        this.OnMouseEnter = this.OnMouseEnter + value;
      }
      [MethodImpl(MethodImplOptions.Synchronized)] remove
      {
        this.OnMouseEnter = this.OnMouseEnter - value;
      }
    }

    public event MouseLeaveEventHandler OnMouseLeave
    {
      [MethodImpl(MethodImplOptions.Synchronized)] add
      {
        this.OnMouseLeave = this.OnMouseLeave + value;
      }
      [MethodImpl(MethodImplOptions.Synchronized)] remove
      {
        this.OnMouseLeave = this.OnMouseLeave - value;
      }
    }

    public event MouseUpEventHandler OnMouseUp
    {
      [MethodImpl(MethodImplOptions.Synchronized)] add
      {
        this.OnMouseUp = this.OnMouseUp + value;
      }
      [MethodImpl(MethodImplOptions.Synchronized)] remove
      {
        this.OnMouseUp = this.OnMouseUp - value;
      }
    }

    public event MouseDownEventHandler OnMouseDown
    {
      [MethodImpl(MethodImplOptions.Synchronized)] add
      {
        this.OnMouseDown = this.OnMouseDown + value;
      }
      [MethodImpl(MethodImplOptions.Synchronized)] remove
      {
        this.OnMouseDown = this.OnMouseDown - value;
      }
    }

    public event MouseDownEventHandler OnMousePress
    {
      [MethodImpl(MethodImplOptions.Synchronized)] add
      {
        this.OnMousePress = this.OnMousePress + value;
      }
      [MethodImpl(MethodImplOptions.Synchronized)] remove
      {
        this.OnMousePress = this.OnMousePress - value;
      }
    }

    public event MouseClickEventHandler OnMouseClick
    {
      [MethodImpl(MethodImplOptions.Synchronized)] add
      {
        this.OnMouseClick = this.OnMouseClick + value;
      }
      [MethodImpl(MethodImplOptions.Synchronized)] remove
      {
        this.OnMouseClick = this.OnMouseClick - value;
      }
    }

    public event MouseDoubleClickEventHandler OnMouseDoubleClick
    {
      [MethodImpl(MethodImplOptions.Synchronized)] add
      {
        this.OnMouseDoubleClick = this.OnMouseDoubleClick + value;
      }
      [MethodImpl(MethodImplOptions.Synchronized)] remove
      {
        this.OnMouseDoubleClick = this.OnMouseDoubleClick - value;
      }
    }

    public event KeyEventHandler OnKeyDown
    {
      [MethodImpl(MethodImplOptions.Synchronized)] add
      {
        this.OnKeyDown = this.OnKeyDown + value;
      }
      [MethodImpl(MethodImplOptions.Synchronized)] remove
      {
        this.OnKeyDown = this.OnKeyDown - value;
      }
    }

    public event KeyEventHandler OnKeyUp
    {
      [MethodImpl(MethodImplOptions.Synchronized)] add
      {
        this.OnKeyUp = this.OnKeyUp + value;
      }
      [MethodImpl(MethodImplOptions.Synchronized)] remove
      {
        this.OnKeyUp = this.OnKeyUp - value;
      }
    }

    public event FocusEventHandler OnGotFocus
    {
      [MethodImpl(MethodImplOptions.Synchronized)] add
      {
        this.OnGotFocus = this.OnGotFocus + value;
      }
      [MethodImpl(MethodImplOptions.Synchronized)] remove
      {
        this.OnGotFocus = this.OnGotFocus - value;
      }
    }

    public event FocusEventHandler OnLostFocus
    {
      [MethodImpl(MethodImplOptions.Synchronized)] add
      {
        this.OnLostFocus = this.OnLostFocus + value;
      }
      [MethodImpl(MethodImplOptions.Synchronized)] remove
      {
        this.OnLostFocus = this.OnLostFocus - value;
      }
    }

    public event DragDropEventHandler OnDragDrop
    {
      [MethodImpl(MethodImplOptions.Synchronized)] add
      {
        this.OnDragDrop = this.OnDragDrop + value;
      }
      [MethodImpl(MethodImplOptions.Synchronized)] remove
      {
        this.OnDragDrop = this.OnDragDrop - value;
      }
    }

    static Control()
    {
    }

    public Control()
    {
      this.Opacity = 1f;
      this.Anchor = AnchorStyles.Top | AnchorStyles.Left;
      this.Elements = new ElementCollection(this);
      this.Animation = new ControlAnimation(this);
      if (this is IControlContainer)
        ((IControlContainer) this).Controls = new ControlCollection(this);
      this.Initialize();
    }

    /// <summary>
    /// Returns all child elements
    /// 
    /// </summary>
    /// 
    /// <returns/>
    public ElementCollection GetElements()
    {
      return this.Elements;
    }

    private void SetEnabled(bool value)
    {
      if (this._parentEnabled == value)
        return;
      this._parentEnabled = value;
      if (this is IControlContainer)
      {
        foreach (Control control in (List<Control>) ((IControlContainer) this).Controls)
          control.SetEnabled(value);
      }
      foreach (Control control in (List<Control>) this.Elements)
        control.SetEnabled(value);
    }

    /// <summary>
    /// Makes the control the focused control
    /// 
    /// </summary>
    public void Focus()
    {
      if (!this.AllowFocus || this.Desktop == null)
        return;
      this.Desktop.FocusedControl = this;
    }

    protected float GetOpacity()
    {
      ControlStyle style = GuiHost.GetStyle(this.Style);
      if (style == null)
        return 1f;
      else
        return this.GetOpacity(style.Styles[this._state].Opacity);
    }

    protected float GetOpacity(float opacity)
    {
      if (this._parent != null)
        return this._parent.GetOpacity() * opacity * this.Opacity;
      else
        return opacity * this.Opacity;
    }

    private void SetBounds()
    {
      if (this._parent == null)
        return;
      this.Bounds.Left = this._position.x;
      this.Bounds.Top = this._position.y;
      this.Bounds.Right = this._parent.Size.x - (this._position.x + this._size.x);
      this.Bounds.Bottom = this._parent.Size.y - (this._position.y + this._size.y);
    }

    private void Layout()
    {
      Point point = this._size;
      if (this.Dock != DockStyle.None)
        this.LayoutDock();
      else
        this.LayoutAnchor();
      this.SetDockRegions();
      if (this._size.x == point.x && this._size.y == point.y || this.OnSizeChanged == null)
        return;
      this.OnSizeChanged(this);
    }

    private void SetDockRegions()
    {
      Margin padding = this.Padding;
      this.Dock0.Top = padding.Top;
      this.Dock0.Left = padding.Left;
      this.Dock0.Right = this._size.x - padding.Right;
      this.Dock0.Bottom = this._size.y - padding.Bottom;
      this.Dock1.Top = 0;
      this.Dock1.Left = 0;
      this.Dock1.Right = this._size.x;
      this.Dock1.Bottom = this._size.y;
    }

    private void LayoutDock()
    {
      DockRegion dockRegion = this._isElement ? this._parent.Dock1 : this._parent.Dock0;
      if (this.Dock == DockStyle.Bottom)
      {
        int num1 = dockRegion.Bottom - this._margin.Bottom;
        int num2 = dockRegion.Left + this._margin.Left;
        int num3 = dockRegion.Right - this._margin.Right;
        this._position.x = num2;
        this._position.y = num1 - this._size.y;
        this._size.x = num3 - num2;
        dockRegion.Bottom = this._position.y - this._margin.Top;
      }
      else if (this.Dock == DockStyle.Fill)
      {
        int num1 = dockRegion.Bottom - this._margin.Bottom;
        int num2 = dockRegion.Left + this._margin.Left;
        int num3 = dockRegion.Right - this._margin.Right;
        int num4 = dockRegion.Top + this._margin.Top;
        this._position.x = num2;
        this._position.y = num4;
        this._size.x = num3 - num2;
        this._size.y = num1 - num4;
      }
      else if (this.Dock == DockStyle.Left)
      {
        int num1 = dockRegion.Left + this._margin.Left;
        int num2 = dockRegion.Top + this._margin.Top;
        int num3 = dockRegion.Bottom - this._margin.Bottom;
        this._position.x = num1;
        this._position.y = num2;
        this._size.y = num3 - num2;
        dockRegion.Left = num1 + this._size.x + this._margin.Right;
      }
      else if (this.Dock == DockStyle.Right)
      {
        int num1 = dockRegion.Right - this._margin.Right;
        int num2 = dockRegion.Top + this._margin.Top;
        int num3 = dockRegion.Bottom - this._margin.Bottom;
        this._position.x = num1 - this._size.x;
        this._position.y = num2;
        this._size.y = num3 - num2;
        dockRegion.Right = this._position.x - this._margin.Left;
      }
      else if (this.Dock == DockStyle.Top)
      {
        int num1 = dockRegion.Top + this._margin.Top;
        int num2 = dockRegion.Left + this._margin.Left;
        int num3 = dockRegion.Right - this._margin.Right;
        this._position.x = num2;
        this._position.y = num1;
        this._size.x = num3 - num2;
        dockRegion.Top = num1 + this._size.y + this._margin.Bottom;
      }
      else if (this.Dock == DockStyle.Middle)
      {
        int num1 = dockRegion.Left + this._margin.Left;
        int num2 = dockRegion.Right - this._margin.Right;
        this._position.x = num1;
        this._size.x = num2 - num1;
        this._position.y = (this._parent.Size.y - this._size.y) / 2;
      }
      else if (this.Dock == DockStyle.Center)
      {
        int num1 = dockRegion.Top + this._margin.Top;
        int num2 = dockRegion.Bottom - this._margin.Bottom;
        this._position.y = num1;
        this._size.y = num2 - num1;
        this._position.x = (this._parent.Size.x - this._size.x) / 2;
      }
      else if (this.Dock == DockStyle.MiddleCenter)
        this._position = (this._parent.Size - this._size) / 2;
      this.SetBounds();
    }

    private void LayoutAnchor()
    {
      switch (this._anchor)
      {
        case AnchorStyles.Top | AnchorStyles.Bottom:
          this._size.y = this._parent.Size.y - this.Bounds.Bottom - this._position.y;
          break;
        case AnchorStyles.Bottom | AnchorStyles.Left:
          this._position.y = this._parent.Size.y - this.Bounds.Bottom - this._size.y;
          break;
        case AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left:
          this._size.y = this._parent.Size.y - this.Bounds.Bottom - this._position.y;
          break;
        case AnchorStyles.Top | AnchorStyles.Right:
          this._position.x = this._parent.Size.x - this.Bounds.Right - this._size.x;
          break;
        case AnchorStyles.Bottom | AnchorStyles.Right:
          this._position.x = this._parent.Size.x - this.Bounds.Right - this._size.x;
          this._position.y = this._parent.Size.y - this.Bounds.Bottom - this._size.y;
          break;
        case AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right:
          this._size.y = this._parent.Size.y - this.Bounds.Bottom - this._position.y;
          this._position.x = this._parent.Size.x - this.Bounds.Right - this._size.x;
          break;
        case AnchorStyles.Left | AnchorStyles.Right:
          this._size.x = this._parent.Size.x - this.Bounds.Right - this._position.x;
          break;
        case AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right:
          this._size.x = this._parent.Size.x - this.Bounds.Right - this._position.x;
          break;
        case AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right:
          this._size.x = this._parent.Size.x - this.Bounds.Right - this._position.x;
          this._position.y = this._parent.Size.y - this.Bounds.Bottom - this._size.y;
          break;
        case AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right:
          this._size.x = this._parent.Size.x - this.Bounds.Right - this._position.x;
          this._size.y = this._parent.Size.y - this.Bounds.Bottom - this._position.y;
          break;
      }
    }

    private void Clip()
    {
      this.Area = new Rectangle(this.Location, this._size);
      if (this._parent == null)
        return;
      this.Area = this._parent.Area.Clip(this.Area);
    }

    protected Rectangle Clip(Rectangle rect)
    {
      if (this._parent != null)
        return this._parent.Area.Clip(rect);
      else
        return rect;
    }

    private void DetermineState()
    {
      if (this.NoEvents)
        return;
      Desktop desktop = this.Desktop;
      if (desktop == null)
        return;
      if (this is ICheckable && (this as ICheckable).Checked)
      {
        if (!this.Enabled)
          this.State = ControlState.CheckedDisabled;
        else if (desktop.FocusedControl == this)
          this.State = ControlState.CheckedFocused;
        else if (desktop.PressedControl == this)
          this.State = ControlState.CheckedPressed;
        else if (desktop.HotControl == this)
          this.State = ControlState.CheckedHot;
        else
          this.State = ControlState.Checked;
      }
      else if (this is ISelectable && (this as ISelectable).Selected)
      {
        if (!this.Enabled)
          this.State = ControlState.SelectedDisabled;
        else if (desktop.FocusedControl == this)
          this.State = ControlState.SelectedFocused;
        else if (desktop.PressedControl == this)
          this.State = ControlState.SelectedPressed;
        else if (desktop.HotControl == this)
          this.State = ControlState.SelectedHot;
        else
          this.State = ControlState.Selected;
      }
      else if (!this.Enabled)
        this.State = ControlState.Disabled;
      else if (desktop.FocusedControl == this)
        this.State = ControlState.Focused;
      else if (desktop.PressedControl == this)
        this.State = ControlState.Pressed;
      else if (desktop.HotControl == this)
        this.State = ControlState.Hot;
      else
        this.State = ControlState.Default;
    }

    private void DrawChildren()
    {
      if (!(this is IControlContainer))
        return;
      foreach (Control control in (List<Control>) ((IControlContainer) this).Controls)
        control.Draw();
    }

    private void DrawElements()
    {
      foreach (Control control in (List<Control>) this.Elements)
        control.Draw();
    }

    protected Point AlignText(string text, Alignment align, Margin padding, int font)
    {
      Point textSize = GuiHost.Renderer.GetTextSize(text, font);
      switch (align)
      {
        case Alignment.TopLeft:
          return this.Location + new Point(padding.Left, padding.Top);
        case Alignment.TopCenter:
          return this.Location + new Point((this._size.x - textSize.x) / 2, padding.Top);
        case Alignment.TopRight:
          return this.Location + new Point(this._size.x - textSize.x - padding.Right, padding.Top);
        case Alignment.MiddleLeft:
          return this.Location + new Point(padding.Left, (this._size.y - textSize.y) / 2);
        case Alignment.MiddleCenter:
          return this.Location + new Point((this._size.x - textSize.x) / 2, (this._size.y - textSize.y) / 2);
        case Alignment.MiddleRight:
          return this.Location + new Point(this._size.x - textSize.x - padding.Right, (this._size.y - textSize.y) / 2);
        case Alignment.BottomLeft:
          return this.Location + new Point(padding.Left, this._size.y - textSize.y - padding.Bottom);
        case Alignment.BottomCenter:
          return this.Location + new Point((this._size.x - textSize.x) / 2, this._size.y - textSize.y - padding.Bottom);
        case Alignment.BottomRight:
          return this.Location + new Point(this._size.x - textSize.x - padding.Right, this._size.y - textSize.y - padding.Bottom);
        default:
          return this.Location;
      }
    }

    protected Point AlignText(string text, Alignment align, Margin padding, int font, out Point tsize)
    {
      tsize = GuiHost.Renderer.GetTextSize(text, font);
      switch (align)
      {
        case Alignment.TopLeft:
          return this.Location + new Point(padding.Left, padding.Top);
        case Alignment.TopCenter:
          return this.Location + new Point((this._size.x - tsize.x) / 2, padding.Top);
        case Alignment.TopRight:
          return this.Location + new Point(this._size.x - tsize.x - padding.Right, padding.Top);
        case Alignment.MiddleLeft:
          return this.Location + new Point(padding.Left, (this._size.y - tsize.y) / 2);
        case Alignment.MiddleCenter:
          return this.Location + new Point((this._size.x - tsize.x) / 2, (this._size.y - tsize.y) / 2);
        case Alignment.MiddleRight:
          return this.Location + new Point(this._size.x - tsize.x - padding.Right, (this._size.y - tsize.y) / 2);
        case Alignment.BottomLeft:
          return this.Location + new Point(padding.Left, this._size.y - tsize.y - padding.Bottom);
        case Alignment.BottomCenter:
          return this.Location + new Point((this._size.x - tsize.x) / 2, this._size.y - tsize.y - padding.Bottom);
        case Alignment.BottomRight:
          return this.Location + new Point(this._size.x - tsize.x - padding.Right, this._size.y - tsize.y - padding.Bottom);
        default:
          return this.Location;
      }
    }

    /// <summary>
    /// Called by the constructor.
    ///             Override this to initialize your custom control.
    ///             Make sure you call base.Initialize
    /// 
    /// </summary>
    protected virtual void Initialize()
    {
    }

    /// <summary>
    /// Called every frame.
    ///             Override this to do per-frame operations
    /// 
    /// </summary>
    protected virtual void OnUpdate()
    {
    }

    /// <summary>
    /// Called late every frame.
    ///             Override this in special circumstances to do per-frame operations
    /// 
    /// </summary>
    protected virtual void OnLateUpdate()
    {
    }

    /// <summary>
    /// Override this method to handle any keyevents passed to the control
    /// 
    /// </summary>
    protected virtual void HandleKeyDown(KeyEventArgs args)
    {
    }

    /// <summary>
    /// Override this method to handle any keyevents passed to the control
    /// 
    /// </summary>
    protected virtual void HandleKeyUp(KeyEventArgs args)
    {
    }

    /// <summary>
    /// Override this to implement your own AutoSize behvaviour
    /// 
    /// </summary>
    protected virtual void HandleAutoSize()
    {
    }

    /// <summary>
    /// Override this to draw text.
    /// 
    /// </summary>
    /// <param name="style">The style to be used</param><param name="opacity">The opacity to be used</param>
    protected virtual void DrawText(Style style, float opacity)
    {
    }

    /// <summary>
    /// Override this to do additional drawing after the control has been drawn
    /// 
    /// </summary>
    protected virtual void DrawCustom()
    {
    }

    protected virtual void OnStateChanged()
    {
    }

    private void RepeatTexture(int tex, Point loc, Style style, float opacity, int color)
    {
      Point textureSize = GuiHost.Renderer.GetTextureSize(tex);
      int num1 = (int) ((double) textureSize.y * ((double) style.TextureUV.V2 - (double) style.TextureUV.V1));
      int num2 = (int) ((double) textureSize.x * ((double) style.TextureUV.U2 - (double) style.TextureUV.U1));
      int num3 = (int) Math.Ceiling((double) this._size.x / (double) num2);
      int num4 = (int) Math.Ceiling((double) this._size.y / (double) num1);
      if (style.Tiling == TextureMode.RepeatX)
        num4 = 1;
      else if (style.Tiling == TextureMode.RepeatY)
        num3 = 1;
      for (int index1 = 0; index1 < num4; ++index1)
      {
        for (int index2 = 0; index2 < num3; ++index2)
        {
          UVCoords uv = new UVCoords(style.TextureUV.U1, style.TextureUV.V1, style.TextureUV.U2, style.TextureUV.V2);
          int num5 = num2 + num2 * index2 - this._size.x;
          int num6 = num1 + num1 * index1 - this._size.y;
          int w = num2;
          int h = num1;
          if (num5 > 0)
          {
            uv.U2 = style.TextureUV.U2 - (float) num5 / (float) textureSize.x;
            w = num2 - num5;
          }
          if (num6 > 0)
          {
            uv.V2 = style.TextureUV.V2 - (float) num6 / (float) textureSize.y;
            h = num1 - num6;
          }
          GuiHost.Renderer.DrawTexture(tex, loc.x + num2 * index2, loc.y + num1 * index1, w, h, uv, color);
        }
      }
    }

    private void GridTexture(int tex, Style style, float opacity, int color)
    {
      bool flag = style.Tiling == TextureMode.GridRepeat;
      Point location = this.Location;
      Point textureSize = GuiHost.Renderer.GetTextureSize(tex);
      Rectangle rectangle1 = new Rectangle(location, this._size);
      Rectangle rectangle2 = new Rectangle(location + new Point(style.Grid.Left, style.Grid.Top), this._size - new Point(style.Grid.Left + style.Grid.Right, style.Grid.Top + style.Grid.Bottom));
      UVCoords uv1 = new UVCoords();
      float num1 = (style.TextureUV.U1 * (float) textureSize.x + (float) style.Grid.Left) / (float) textureSize.x;
      float num2 = (style.TextureUV.V1 * (float) textureSize.y + (float) style.Grid.Top) / (float) textureSize.y;
      float num3 = (style.TextureUV.U2 * (float) textureSize.x - (float) style.Grid.Right) / (float) textureSize.x;
      float num4 = (style.TextureUV.V2 * (float) textureSize.y - (float) style.Grid.Bottom) / (float) textureSize.y;
      if (style.Grid.Top > 0 && style.Grid.Left > 0)
      {
        uv1.U1 = style.TextureUV.U1;
        uv1.V1 = style.TextureUV.V1;
        uv1.U2 = num1;
        uv1.V2 = num2;
        GuiHost.Renderer.DrawTexture(tex, rectangle1.x1, rectangle1.y1, style.Grid.Left, style.Grid.Top, uv1, color);
      }
      if (style.Grid.Top > 0 && style.Grid.Right > 0)
      {
        uv1.U1 = num3;
        uv1.V1 = style.TextureUV.V1;
        uv1.U2 = style.TextureUV.U2;
        uv1.V2 = num2;
        GuiHost.Renderer.DrawTexture(tex, rectangle2.x2, rectangle1.y1, style.Grid.Right, style.Grid.Top, uv1, color);
      }
      if (style.Grid.Bottom > 0 && style.Grid.Left > 0)
      {
        uv1.U1 = style.TextureUV.U1;
        uv1.V1 = num4;
        uv1.U2 = num1;
        uv1.V2 = style.TextureUV.V2;
        GuiHost.Renderer.DrawTexture(tex, rectangle1.x1, rectangle2.y2, style.Grid.Left, style.Grid.Bottom, uv1, color);
      }
      if (style.Grid.Bottom > 0 && style.Grid.Right > 0)
      {
        uv1.U1 = num3;
        uv1.V1 = num4;
        uv1.U2 = style.TextureUV.U2;
        uv1.V2 = style.TextureUV.V2;
        GuiHost.Renderer.DrawTexture(tex, rectangle2.x2, rectangle2.y2, style.Grid.Right, style.Grid.Bottom, uv1, color);
      }
      Margin grid;
      if (style.Grid.Left > 0)
      {
        uv1.U1 = style.TextureUV.U1;
        uv1.V1 = num2;
        uv1.U2 = num1;
        uv1.V2 = num4;
        if (flag)
        {
          int num5 = (int) ((double) textureSize.y * ((double) style.TextureUV.V2 - (double) style.TextureUV.V1) - (double) (style.Grid.Top + style.Grid.Bottom));
          int num6 = (int) Math.Ceiling((double) rectangle2.Height / (double) num5);
          if (rectangle2.Height < num5)
          {
            uv1.V2 = num4 - (float) (num5 - rectangle2.Height) / (float) textureSize.y;
            num5 = rectangle2.Height;
          }
          int num7 = 0;
          for (int index = 0; index < num6; ++index)
          {
            num7 += num5;
            if (num7 > rectangle2.Height)
            {
              int num8 = num7 - rectangle2.Height;
              uv1.V2 = num4 - (float) num8 / (float) textureSize.y;
              int num9 = num5 - num8;
              ISquidRenderer renderer = GuiHost.Renderer;
              int texture = tex;
              int x = rectangle1.x1;
              int y = rectangle2.y1 + num5 * index;
              grid = style.Grid;
              int left = grid.Left;
              int h = num9;
              UVCoords uv2 = uv1;
              int color1 = color;
              renderer.DrawTexture(texture, x, y, left, h, uv2, color1);
            }
            else
            {
              ISquidRenderer renderer = GuiHost.Renderer;
              int texture = tex;
              int x = rectangle1.x1;
              int y = rectangle2.y1 + num5 * index;
              grid = style.Grid;
              int left = grid.Left;
              int h = num5;
              UVCoords uv2 = uv1;
              int color1 = color;
              renderer.DrawTexture(texture, x, y, left, h, uv2, color1);
            }
          }
        }
        else
          GuiHost.Renderer.DrawTexture(tex, rectangle1.x1, rectangle2.y1, style.Grid.Left, rectangle2.Height, uv1, color);
      }
      grid = style.Grid;
      if (grid.Top > 0)
      {
        uv1.U1 = num1;
        uv1.V1 = style.TextureUV.V1;
        uv1.U2 = num3;
        uv1.V2 = num2;
        if (flag)
        {
          double num5 = (double) textureSize.x * ((double) style.TextureUV.U2 - (double) style.TextureUV.U1);
          grid = style.Grid;
          int left = grid.Left;
          grid = style.Grid;
          int right = grid.Right;
          double num6 = (double) (left + right);
          int num7 = (int) (num5 - num6);
          int num8 = (int) Math.Ceiling((double) rectangle2.Width / (double) num7);
          if (rectangle2.Width < num7)
          {
            uv1.U2 = num3 - (float) (num7 - rectangle2.Width) / (float) textureSize.x;
            num7 = rectangle2.Width;
          }
          int num9 = 0;
          for (int index = 0; index < num8; ++index)
          {
            num9 += num7;
            if (num9 > rectangle2.Width)
            {
              int num10 = num9 - rectangle2.Width;
              uv1.U2 = num3 - (float) num10 / (float) textureSize.x;
              int num11 = num7 - num10;
              ISquidRenderer renderer = GuiHost.Renderer;
              int texture = tex;
              int x = rectangle2.x1 + num7 * index;
              int y = rectangle1.y1;
              int w = num11;
              grid = style.Grid;
              int top = grid.Top;
              UVCoords uv2 = uv1;
              int color1 = color;
              renderer.DrawTexture(texture, x, y, w, top, uv2, color1);
            }
            else
            {
              ISquidRenderer renderer = GuiHost.Renderer;
              int texture = tex;
              int x = rectangle2.x1 + num7 * index;
              int y = rectangle1.y1;
              int w = num7;
              grid = style.Grid;
              int top = grid.Top;
              UVCoords uv2 = uv1;
              int color1 = color;
              renderer.DrawTexture(texture, x, y, w, top, uv2, color1);
            }
          }
        }
        else
        {
          ISquidRenderer renderer = GuiHost.Renderer;
          int texture = tex;
          int x = rectangle2.x1;
          int y = rectangle1.y1;
          int width = rectangle2.Width;
          grid = style.Grid;
          int top = grid.Top;
          UVCoords uv2 = uv1;
          int color1 = color;
          renderer.DrawTexture(texture, x, y, width, top, uv2, color1);
        }
      }
      grid = style.Grid;
      if (grid.Right > 0)
      {
        uv1.U1 = num3;
        uv1.V1 = num2;
        uv1.U2 = style.TextureUV.U2;
        uv1.V2 = num4;
        if (flag)
        {
          double num5 = (double) textureSize.y * ((double) style.TextureUV.V2 - (double) style.TextureUV.V1);
          grid = style.Grid;
          int top = grid.Top;
          grid = style.Grid;
          int bottom = grid.Bottom;
          double num6 = (double) (top + bottom);
          int num7 = (int) (num5 - num6);
          int num8 = (int) Math.Ceiling((double) rectangle2.Height / (double) num7);
          if (rectangle2.Height < num7)
          {
            uv1.V2 = num4 - (float) (num7 - rectangle2.Height) / (float) textureSize.y;
            num7 = rectangle2.Height;
          }
          int num9 = 0;
          for (int index = 0; index < num8; ++index)
          {
            num9 += num7;
            if (num9 > rectangle2.Height)
            {
              int num10 = num9 - rectangle2.Height;
              uv1.V2 = num4 - (float) num10 / (float) textureSize.y;
              int num11 = num7 - num10;
              ISquidRenderer renderer = GuiHost.Renderer;
              int texture = tex;
              int x = rectangle2.x2;
              int y = rectangle2.y1 + num7 * index;
              grid = style.Grid;
              int right = grid.Right;
              int h = num11;
              UVCoords uv2 = uv1;
              int color1 = color;
              renderer.DrawTexture(texture, x, y, right, h, uv2, color1);
            }
            else
            {
              ISquidRenderer renderer = GuiHost.Renderer;
              int texture = tex;
              int x = rectangle2.x2;
              int y = rectangle2.y1 + num7 * index;
              grid = style.Grid;
              int right = grid.Right;
              int h = num7;
              UVCoords uv2 = uv1;
              int color1 = color;
              renderer.DrawTexture(texture, x, y, right, h, uv2, color1);
            }
          }
        }
        else
        {
          ISquidRenderer renderer = GuiHost.Renderer;
          int texture = tex;
          int x = rectangle2.x2;
          int y = rectangle2.y1;
          grid = style.Grid;
          int right = grid.Right;
          int height = rectangle2.Height;
          UVCoords uv2 = uv1;
          int color1 = color;
          renderer.DrawTexture(texture, x, y, right, height, uv2, color1);
        }
      }
      grid = style.Grid;
      if (grid.Bottom > 0)
      {
        uv1.U1 = num1;
        uv1.V1 = num4;
        uv1.U2 = num3;
        uv1.V2 = style.TextureUV.V2;
        if (flag)
        {
          double num5 = (double) textureSize.x * ((double) style.TextureUV.U2 - (double) style.TextureUV.U1);
          grid = style.Grid;
          int left = grid.Left;
          grid = style.Grid;
          int right = grid.Right;
          double num6 = (double) (left + right);
          int num7 = (int) (num5 - num6);
          int num8 = (int) Math.Ceiling((double) rectangle2.Width / (double) num7);
          if (rectangle2.Width < num7)
          {
            uv1.U2 = num3 - (float) (num7 - rectangle2.Width) / (float) textureSize.x;
            num7 = rectangle2.Width;
          }
          int num9 = 0;
          for (int index = 0; index < num8; ++index)
          {
            num9 += num7;
            if (num9 > rectangle2.Width)
            {
              int num10 = num9 - rectangle2.Width;
              uv1.U2 = num3 - (float) num10 / (float) textureSize.x;
              int num11 = num7 - num10;
              ISquidRenderer renderer = GuiHost.Renderer;
              int texture = tex;
              int x = rectangle2.x1 + num7 * index;
              int y = rectangle2.y2;
              int w = num11;
              grid = style.Grid;
              int bottom = grid.Bottom;
              UVCoords uv2 = uv1;
              int color1 = color;
              renderer.DrawTexture(texture, x, y, w, bottom, uv2, color1);
            }
            else
            {
              ISquidRenderer renderer = GuiHost.Renderer;
              int texture = tex;
              int x = rectangle2.x1 + num7 * index;
              int y = rectangle2.y2;
              int w = num7;
              grid = style.Grid;
              int bottom = grid.Bottom;
              UVCoords uv2 = uv1;
              int color1 = color;
              renderer.DrawTexture(texture, x, y, w, bottom, uv2, color1);
            }
          }
        }
        else
        {
          ISquidRenderer renderer = GuiHost.Renderer;
          int texture = tex;
          int x = rectangle2.x1;
          int y = rectangle2.y2;
          int width = rectangle2.Width;
          grid = style.Grid;
          int bottom = grid.Bottom;
          UVCoords uv2 = uv1;
          int color1 = color;
          renderer.DrawTexture(texture, x, y, width, bottom, uv2, color1);
        }
      }
      uv1.U1 = num1;
      uv1.V1 = num2;
      uv1.U2 = num3;
      uv1.V2 = num4;
      GuiHost.Renderer.DrawTexture(tex, rectangle2.x1, rectangle2.y1, rectangle2.Width, rectangle2.Height, uv1, color);
    }

    /// <summary>
    /// Override this to do additional drawing
    ///             Call the base method to draw the control as usual
    /// 
    /// </summary>
    /// <param name="style"/><param name="opacity"/>
    protected virtual void DrawStyle(Style style, float opacity)
    {
      Point location = this.Location;
      if (style.BackColor != 0)
        GuiHost.Renderer.DrawBox(location.x, location.y, this._size.x, this._size.y, ColorInt.FromArgb(opacity, style.BackColor));
      if (string.IsNullOrEmpty(style.Texture))
        return;
      int texture = GuiHost.Renderer.GetTexture(style.Texture);
      if (texture <= -1)
        return;
      int color = ColorInt.FromArgb(opacity, style.Tint);
      if (style.Tiling == TextureMode.Grid || style.Tiling == TextureMode.GridRepeat)
        this.GridTexture(texture, style, opacity, color);
      else if (style.Tiling == TextureMode.Stretch)
        GuiHost.Renderer.DrawTexture(texture, location.x, location.y, this._size.x, this._size.y, style.TextureUV, color);
      else
        this.RepeatTexture(texture, this.Location, style, opacity, color);
    }

    internal void UpdateLate()
    {
      this.OnLateUpdate();
      foreach (Control control in (List<Control>) this.Elements)
        control.UpdateLate();
      if (!(this is IControlContainer))
        return;
      foreach (Control control in (List<Control>) ((IControlContainer) this).Controls)
        control.UpdateLate();
    }

    internal void UpdateEarly()
    {
      if (this._parent != null)
        this.Depth = this._parent.Depth + 1;
      if ((double) this.FadeSpeed > 0.0 || (double) GuiHost.GlobalFadeSpeed > 0.0)
      {
        float num = GuiHost.TimeElapsed / ((double) this.FadeSpeed > 0.0 ? this.FadeSpeed : GuiHost.GlobalFadeSpeed);
        this.FadeOut -= num;
        this.FadeIn += num;
        this.FadeIn = (double) this.FadeIn < 0.0 ? 0.0f : ((double) this.FadeIn > 1.0 ? 1f : this.FadeIn);
        this.FadeOut = (double) this.FadeOut < 0.0 ? 0.0f : ((double) this.FadeOut > 1.0 ? 1f : this.FadeOut);
      }
      this.Animation.Update();
      if (!this.Visible && !this.Desktop.DesignMode)
        return;
      this.OnUpdate();
      this.Layout();
      this.Clip();
      Point point = Point.Zero;
      foreach (Control control in (List<Control>) this.Elements)
        control.UpdateEarly();
      if (this is IControlContainer)
      {
        ControlCollection controls = ((IControlContainer) this).Controls;
        controls.SafeRemoval = true;
        foreach (Control control in (List<Control>) controls)
        {
          control.UpdateEarly();
          if (!control.Removed)
          {
            point.x = Math.Max(point.x, control.Position.x + control.Size.x);
            point.y = Math.Max(point.y, control.Position.y + control.Size.y);
          }
        }
        controls.SafeRemoval = false;
        for (int index = controls.Count - 1; index > 0; --index)
        {
          Control control = controls[index];
          if (control.Removed)
          {
            controls.RemoveAt(index);
            control.Parent = (Control) null;
          }
        }
      }
      if (this.AutoSize != AutoSize.None)
      {
        this._size = this._parent == null ? (this.AutoSize != AutoSize.Vertical ? (this.AutoSize != AutoSize.Horizontal ? new Point(point.x, point.y) : new Point(point.x, this._size.y)) : new Point(this._size.x, point.y)) : (this.AutoSize != AutoSize.Vertical ? (this.AutoSize != AutoSize.Horizontal ? new Point(point.x, point.y) : new Point(point.x, this._size.y)) : new Point(this._size.x, point.y));
        this.HandleAutoSize();
      }
      this.DetermineState();
    }

    /// <summary>
    /// Draws the control
    /// 
    /// </summary>
    public void Draw()
    {
      if (this.Visible)
      {
        Point location = this.Location;
        if (this._size.x <= 0 || this._size.y <= 0 || (this.Area.Width <= 0 || this.Area.Height <= 0))
          return;
        if (this.Scissor || GuiHost.AlwaysScissor)
          this.SetScissor(Math.Max(0, this.Area.x1), Math.Max(0, this.Area.y1), this.Area.x2, this.Area.y2);
        if ((double) this.FadeSpeed > 0.0 || (double) GuiHost.GlobalFadeSpeed > 0.0)
        {
          Style style1 = GuiHost.GetStyle(this.Style).Styles[this._state];
          float opacity1 = this.GetOpacity(style1.Opacity);
          if (this._oldState != this._state)
          {
            Style style2 = GuiHost.GetStyle(this.Style).Styles[this._oldState];
            float opacity2 = this.GetOpacity(style2.Opacity) * this.FadeOut;
            float opacity3 = this.GetOpacity(style1.Opacity) * this.FadeIn;
            if ((double) this.FadeOut > 0.0)
            {
              this.DrawStyle(style2, opacity2);
              this.DrawText(style2, opacity2);
            }
            this.DrawStyle(style1, opacity3);
            this.DrawText(style1, opacity3);
          }
          else
          {
            this.DrawStyle(style1, opacity1);
            this.DrawText(style1, opacity1);
          }
        }
        else
        {
          Style style = GuiHost.GetStyle(this.Style).Styles[this._state];
          float opacity = this.GetOpacity(style.Opacity);
          this.DrawStyle(style, opacity);
          this.DrawText(style, opacity);
        }
        this.DrawChildren();
        this.DrawElements();
        if (this.Scissor || GuiHost.AlwaysScissor)
          this.ResetScissor();
        this.DrawCustom();
      }
      if (this.Depth != 1 || Control.ScissorStack.Count <= 0)
        return;
      Control.ScissorStack.Clear();
    }

    /// <summary>
    /// Sets the scissor test rectangle
    /// 
    /// </summary>
    /// <param name="x1">left screen coord</param><param name="y1">top screen coord</param><param name="x2">right screen coord</param><param name="y2">bottom screen coord</param>
    protected void SetScissor(int x1, int y1, int x2, int y2)
    {
      Rectangle rectangle = new Rectangle(x1, y1, x2, y2);
      if (Control.ScissorStack.ContainsKey(this.Depth))
        Control.ScissorStack[this.Depth] = new KeyValuePair<Control, Rectangle>(this, rectangle);
      else
        Control.ScissorStack.Add(this.Depth, new KeyValuePair<Control, Rectangle>(this, rectangle));
      GuiHost.Renderer.EndBatch(false);
      GuiHost.Renderer.Scissor(x1, y1, x2, y2);
      GuiHost.Renderer.StartBatch();
    }

    /// <summary>
    /// Resets the scissor test to whatever is was before the last SetScissor call
    /// 
    /// </summary>
    protected void ResetScissor()
    {
      Rectangle rectangle = this.Desktop.Area;
      for (int key = this.Depth - 1; key >= 0; --key)
      {
        if (Control.ScissorStack.ContainsKey(key))
        {
          KeyValuePair<Control, Rectangle> keyValuePair = Control.ScissorStack[key];
          if (this.IsChildOf(keyValuePair.Key))
          {
            rectangle = keyValuePair.Value;
            break;
          }
        }
      }
      GuiHost.Renderer.EndBatch(false);
      GuiHost.Renderer.Scissor(rectangle.x1, rectangle.y1, rectangle.x2, rectangle.y2);
      GuiHost.Renderer.StartBatch();
    }

    /// <summary>
    /// Return true if the point collides with the control
    /// 
    /// </summary>
    /// <param name="x">x screen coord</param><param name="y">y screen coord</param>
    /// <returns/>
    public bool Hit(int x, int y)
    {
      if (x >= this.Area.x1 && x <= this.Area.x2 && y >= this.Area.y1)
        return y <= this.Area.y2;
      else
        return false;
    }

    /// <summary>
    /// Returns the first control found below the given screen point
    ///             Returns elements and controls
    /// 
    /// </summary>
    /// <param name="x">x screen coord</param><param name="y">y screen coord</param>
    /// <returns/>
    public Control GetControlAt(int x, int y)
    {
      return this.GetControlAt(x, y, true);
    }

    /// <summary>
    /// Returns the first control matching the given name.
    ///             This method searches all siblings.
    /// 
    /// </summary>
    /// <param name="name">Name of the control to find</param>
    /// <returns>
    /// Matching control
    /// </returns>
    public Control GetControl(string name)
    {
      if (this.Name == name)
        return this;
      Control control1 = (Control) null;
      if (this is IControlContainer)
      {
        foreach (Control control2 in (List<Control>) (this as IControlContainer).Controls)
        {
          control1 = control2.GetControl(name);
          if (control1 != null)
            return control1;
        }
      }
      foreach (Control control2 in (List<Control>) this.Elements)
      {
        control1 = control2.GetControl(name);
        if (control1 != null)
          return control1;
      }
      return control1;
    }

    /// <summary>
    /// Returns the first control found below the given screen point
    /// 
    /// </summary>
    /// <param name="x"/><param name="y"/><param name="elements"/>
    /// <returns/>
    public Control GetControlAt(int x, int y, bool elements)
    {
      if (!this.Enabled)
        return (Control) null;
      if (!this.Visible)
        return (Control) null;
      if (!this.Hit(x, y))
        return (Control) null;
      Control control = this.NoEvents ? (Control) null : this;
      if (!elements && this._isElement)
        control = (Control) null;
      if (this is IControlContainer)
      {
        IControlContainer controlContainer = this as IControlContainer;
        for (int index = controlContainer.Controls.Count - 1; index >= 0; --index)
        {
          Control controlAt = controlContainer.Controls[index].GetControlAt(x, y, elements);
          if (controlAt != null && controlAt.Enabled && (controlAt.Visible && !controlAt.NoEvents))
          {
            control = controlAt;
            break;
          }
        }
      }
      for (int index = this.Elements.Count - 1; index >= 0; --index)
      {
        Control controlAt = this.Elements[index].GetControlAt(x, y, elements);
        if (controlAt != null && controlAt.Enabled && (controlAt.Visible && !controlAt.NoEvents))
        {
          control = controlAt;
          break;
        }
      }
      return control;
    }

    protected Control PickDeep(int x, int y)
    {
      if (!this.Visible)
        return (Control) null;
      if (!this.Hit(x, y))
        return (Control) null;
      Control control1 = this;
      if (this is IControlContainer)
      {
        IControlContainer controlContainer = this as IControlContainer;
        for (int index = controlContainer.Controls.Count - 1; index >= 0; --index)
        {
          Control control2 = controlContainer.Controls[index].PickDeep(x, y);
          if (control2 != null && control2.Visible)
          {
            control1 = control2;
            break;
          }
        }
      }
      for (int index = this.Elements.Count - 1; index >= 0; --index)
      {
        Control control2 = this.Elements[index].PickDeep(x, y);
        if (control2 != null && control2.Visible)
        {
          control1 = control2;
          break;
        }
      }
      return control1;
    }

    protected Control PickFirst(int x, int y)
    {
      if (!this.Visible)
        return (Control) null;
      if (!this.Hit(x, y))
        return (Control) null;
      Control control1 = this._isElement ? (Control) null : this;
      if (this is IControlContainer)
      {
        IControlContainer controlContainer = this as IControlContainer;
        for (int index = controlContainer.Controls.Count - 1; index >= 0; --index)
        {
          Control control2 = controlContainer.Controls[index].PickFirst(x, y);
          if (control2 != null && control2.Visible && !control2._isElement)
          {
            control1 = control2;
            break;
          }
        }
      }
      for (int index = this.Elements.Count - 1; index >= 0; --index)
      {
        Control control2 = this.Elements[index].PickFirst(x, y);
        if (control2 != null && control2.Visible && !(control2 is IControlContainer))
        {
          control1 = control2;
          break;
        }
      }
      return control1;
    }

    internal Control GetDropTarget(Control sender)
    {
      if (!this.Visible)
        return (Control) null;
      if (!this.Hit(GuiHost.MousePosition.x, GuiHost.MousePosition.y))
        return (Control) null;
      Control control = this.Enabled ? this : (Control) null;
      if (this is IControlContainer)
      {
        IControlContainer controlContainer = this as IControlContainer;
        for (int index = controlContainer.Controls.Count - 1; index >= 0; --index)
        {
          Control dropTarget = controlContainer.Controls[index].GetDropTarget(sender);
          if (dropTarget != null && sender != dropTarget && (dropTarget.Enabled && dropTarget.Visible) && (!dropTarget.NoEvents && dropTarget.AllowDrop))
          {
            control = dropTarget;
            break;
          }
        }
      }
      for (int index = this.Elements.Count - 1; index >= 0; --index)
      {
        Control dropTarget = this.Elements[index].GetDropTarget(sender);
        if (dropTarget != null && sender != dropTarget && (dropTarget.Enabled && dropTarget.Visible) && (!dropTarget.NoEvents && dropTarget.AllowDrop))
        {
          control = dropTarget;
          break;
        }
      }
      return control;
    }

    /// <summary>
    /// Performs a drag drop operation
    /// 
    /// </summary>
    /// <param name="data">The control to be displayed as dragged</param>
    public void DoDragDrop(Control data)
    {
      this.Desktop.DoDragDrop(this, data);
    }

    internal void FireOnDragDrop(DragDropEventArgs e)
    {
      if (this.OnDragDrop == null)
        return;
      this.OnDragDrop(this, e);
    }

    /// <summary>
    /// Returns true if the given control is a sibling of the control
    /// 
    /// </summary>
    /// <param name="control"/>
    /// <returns/>
    public bool IsChildOf(Control control)
    {
      if (control.Elements.Contains(this))
        return true;
      foreach (Control control1 in (List<Control>) control.Elements)
      {
        if (this.IsChildOf(control1))
          return true;
      }
      IControlContainer controlContainer = control as IControlContainer;
      if (controlContainer == null)
        return false;
      if (controlContainer.Controls.Contains(this))
        return true;
      foreach (Control control1 in (List<Control>) controlContainer.Controls)
      {
        if (this.IsChildOf(control1))
          return true;
      }
      return false;
    }

    public bool InRect(Point start, Point end)
    {
      Rectangle rectangle = new Rectangle();
      rectangle.x1 = Math.Min(start.x, end.x);
      rectangle.y1 = Math.Min(start.y, end.y);
      rectangle.x2 = Math.Max(start.x, end.x);
      rectangle.y2 = Math.Max(start.y, end.y);
      if (this.Area.x1 <= rectangle.x2 && this.Area.x2 >= rectangle.x1 && this.Area.y1 <= rectangle.y2)
        return this.Area.y2 >= rectangle.y1;
      else
        return false;
    }

    /// <summary>
    /// Override this in custom controls to handle special cases
    /// 
    /// </summary>
    /// <param name="control"/>
    /// <returns/>
    public virtual bool Contains(Control control)
    {
      if (control == null)
        return false;
      else
        return control.IsChildOf(this);
    }

    /// <summary>
    /// Fires the MouseClick event
    /// 
    /// </summary>
    public virtual void Click(int button)
    {
      if (this.Desktop.CheckModalLock(this))
        return;
      this.MouseDown(button);
      this.MouseRelease(button);
    }

    /// <summary>
    /// Makes the control the topmost control in its container
    /// 
    /// </summary>
    public void BringToFront()
    {
      if (this.Container == null || this.Container.Controls.IndexOf(this) == this.Container.Controls.Count - 1)
        return;
      Control control = this._parent;
      this.Parent = (Control) null;
      this.Parent = control;
    }

    /// <summary>
    /// Resize the control
    /// 
    /// </summary>
    /// <param name="p">x and y values to resize the control by</param><param name="style">defines on what edge the control should be resized</param>
    /// <returns/>
    public Point Resize(Point p, AnchorStyles style)
    {
      Point size = this.Size;
      switch (style)
      {
        case AnchorStyles.Top:
          this.ResizeTop(p.y);
          break;
        case AnchorStyles.Bottom:
          this.ResizeBottom(p.y);
          break;
        case AnchorStyles.Left:
          this.ResizeLeft(p.x);
          break;
        case AnchorStyles.Top | AnchorStyles.Left:
          this.ResizeLeft(p.x);
          this.ResizeTop(p.y);
          break;
        case AnchorStyles.Bottom | AnchorStyles.Left:
          this.ResizeLeft(p.x);
          this.ResizeBottom(p.y);
          break;
        case AnchorStyles.Right:
          this.ResizeRight(p.x);
          break;
        case AnchorStyles.Top | AnchorStyles.Right:
          this.ResizeRight(p.x);
          this.ResizeTop(p.y);
          break;
        case AnchorStyles.Bottom | AnchorStyles.Right:
          this.ResizeRight(p.x);
          this.ResizeBottom(p.y);
          break;
      }
      this.SetBounds();
      if (this.Dock != DockStyle.None)
      {
        if (this._parent != null)
          this._parent.UpdateEarly();
      }
      else
        this.UpdateEarly();
      return this._size - size;
    }

    private void ResizeLeft(int value)
    {
      Point point1 = this._position;
      Point point2 = this._size;
      point2.x -= value;
      if (this.MinSize.x >= 0 && this.MaxSize.x > 0)
        point2.x = Math.Min(point2.x, this.MaxSize.x);
      point2.x = Math.Max(point2.x, this.MinSize.x);
      point1.x += this._size.x - point2.x;
      this._position = point1;
      this._size = point2;
    }

    private void ResizeRight(int value)
    {
      Point point = this._size;
      point.x += value;
      if (this.MinSize.x >= 0 && this.MaxSize.x > 0)
        point.x = Math.Min(point.x, this.MaxSize.x);
      point.x = Math.Max(point.x, this.MinSize.x);
      this._size = point;
    }

    private void ResizeBottom(int value)
    {
      Point point = this._size;
      point.y += value;
      if (this.MinSize.y >= 0 && this.MaxSize.y > 0)
        point.y = Math.Min(point.y, this.MaxSize.y);
      point.y = Math.Max(point.y, this.MinSize.y);
      this._size = point;
    }

    private void ResizeTop(int value)
    {
      Point point1 = this._position;
      Point point2 = this._size;
      point2.y -= value;
      if (this.MinSize.y >= 0 && this.MaxSize.y > 0)
        point2.y = Math.Min(point2.y, this.MaxSize.y);
      point2.y = Math.Max(point2.y, this.MinSize.y);
      point1.y += this._size.y - point2.y;
      this._position = point1;
      this._size = point2;
    }

    /// <summary>
    /// Process key events
    /// 
    /// </summary>
    public void DoKeyEvents()
    {
      if (this.NoEvents || GuiHost.NumKeyEvents <= 0)
        return;
      foreach (KeyData key in GuiHost.KeyBuffer)
      {
        KeyEventArgs args = new KeyEventArgs(key);
        if (key.Pressed)
        {
          if (this.OnKeyDown != null)
          {
            this.OnKeyDown(this, args);
            if (!args.Cancel)
              this.HandleKeyDown(args);
          }
          else
            this.HandleKeyDown(args);
        }
        if (key.Released)
        {
          if (this.OnKeyUp != null)
          {
            this.OnKeyUp(this, args);
            if (!args.Cancel)
              this.HandleKeyUp(args);
          }
          else
            this.HandleKeyUp(args);
        }
      }
    }

    /// <summary>
    /// Process mouse events
    /// 
    /// </summary>
    public void DoEvents()
    {
      if (this.NoEvents)
        return;
      for (int index = 0; index < GuiHost.Buttons.Length; ++index)
      {
        if (GuiHost.MouseButton(index) == ButtonState.Down)
        {
          this.MouseDown(index);
          return;
        }
        else if (GuiHost.MouseButton(index) == ButtonState.Press)
        {
          this.MousePress(index);
          return;
        }
        else if (GuiHost.MouseButton(index) == ButtonState.Up)
        {
          this.MouseRelease(index);
          return;
        }
        else if (index == 0 && this.Desktop != null)
        {
          this.Desktop.PressedControl = (Control) null;
          this.Desktop.MouseDownControl = (Control) null;
        }
      }
      if (this.Desktop == null || this.Desktop.MouseDownControl == null)
        return;
      this.Desktop.PressedControl = (Control) null;
      this.Desktop.MouseDownControl = (Control) null;
    }

    internal void MouseDoubleClick(int button)
    {
      if (this.OnMouseDoubleClick == null)
        return;
      this.OnMouseDoubleClick(this, new MouseEventArgs()
      {
        Button = button
      });
    }

    internal void MousePress(int button)
    {
      if (this.Desktop == null)
        return;
      if (button == 0)
        this.Desktop.PressedControl = this;
      this.Desktop.MouseDownControl = this;
      if (this.OnMousePress == null)
        return;
      this.OnMousePress(this, new MouseEventArgs()
      {
        Button = button
      });
    }

    internal void MouseClick(int button)
    {
      if (this.OnMouseClick == null)
        return;
      this.OnMouseClick(this, new MouseEventArgs()
      {
        Button = button
      });
    }

    internal void MouseRelease(int button)
    {
      if (this.Desktop == null || this.Desktop.MouseDownControl != this)
        return;
      this.MouseClick(button);
      if (!this.Double)
        return;
      this.Double = false;
      this.MouseDoubleClick(button);
    }

    internal void MouseDown(int button)
    {
      if (this.Desktop == null)
        return;
      if (button == 0)
        this.Desktop.PressedControl = this;
      this.Desktop.MouseDownControl = this;
      DateTime now = DateTime.Now;
      TimeSpan timeSpan = now.Subtract(this.Clicked);
      this.Clicked = now;
      this.Double = timeSpan.TotalMilliseconds < (double) GuiHost.DoubleClickSpeed;
      if (this.OnMouseDown == null)
        return;
      this.OnMouseDown(this, new MouseEventArgs()
      {
        Button = button
      });
    }

    internal void MouseEnter()
    {
      if (this.OnMouseEnter == null)
        return;
      this.OnMouseEnter(this);
    }

    internal void MouseLeave()
    {
      if (this.OnMouseLeave == null)
        return;
      this.OnMouseLeave(this);
    }

    internal void MouseUp(int button)
    {
      if (this.OnMouseUp == null)
        return;
      this.OnMouseUp(this, new MouseEventArgs()
      {
        Button = button
      });
    }

    internal void GotFocus()
    {
      if (this.OnGotFocus == null)
        return;
      this.OnGotFocus(this);
    }

    internal void LostFocus()
    {
      if (this.OnLostFocus == null)
        return;
      this.OnLostFocus(this);
    }

    internal Control FindTabIndex(int index)
    {
      Control control = (Control) null;
      List<Control> list1 = (List<Control>) this.Elements;
      if (!this.Visible || this.Desktop.CheckModalLock(this))
        return (Control) null;
      if (!this.NoEvents && this.Enabled && this.TabIndex == index)
        control = this;
      for (int index1 = 0; index1 < list1.Count; ++index1)
      {
        Control tabIndex = list1[index1].FindTabIndex(index);
        if (tabIndex != null && !tabIndex.NoEvents && (tabIndex.Visible && tabIndex.Enabled) && tabIndex.TabIndex == index)
        {
          control = tabIndex;
          break;
        }
      }
      if (this is IControlContainer)
      {
        List<Control> list2 = (List<Control>) ((IControlContainer) this).Controls;
        for (int index1 = 0; index1 < list2.Count; ++index1)
        {
          Control tabIndex = list2[index1].FindTabIndex(index);
          if (tabIndex != null && !tabIndex.NoEvents && (tabIndex.Visible && tabIndex.Enabled) && tabIndex.TabIndex == index)
          {
            control = tabIndex;
            break;
          }
        }
      }
      return control;
    }

    internal int FindHighestTabIndex(int max)
    {
      int max1 = max;
      List<Control> list1 = (List<Control>) this.Elements;
      if (!this.Visible)
        return max1;
      if (!this.NoEvents && this.Enabled && this.TabIndex == max1)
        max1 = this.TabIndex;
      for (int index = 0; index < list1.Count; ++index)
      {
        int highestTabIndex = list1[index].FindHighestTabIndex(max1);
        if (highestTabIndex > max1)
          max1 = highestTabIndex;
      }
      if (this is IControlContainer)
      {
        List<Control> list2 = (List<Control>) ((IControlContainer) this).Controls;
        for (int index = 0; index < list2.Count; ++index)
        {
          int highestTabIndex = list2[index].FindHighestTabIndex(max1);
          if (highestTabIndex > max1)
            max1 = highestTabIndex;
        }
      }
      return max1;
    }
  }
}
