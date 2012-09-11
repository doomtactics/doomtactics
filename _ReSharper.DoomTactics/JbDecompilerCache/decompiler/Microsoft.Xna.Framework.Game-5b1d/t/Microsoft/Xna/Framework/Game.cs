// Type: Microsoft.Xna.Framework.Game
// Assembly: Microsoft.Xna.Framework.Game, Version=4.0.0.0, Culture=neutral, PublicKeyToken=842cf8be1de50553
// Assembly location: C:\Program Files (x86)\Microsoft XNA\XNA Game Studio\v4.0\References\Windows\x86\Microsoft.Xna.Framework.Game.dll

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Microsoft.Xna.Framework
{
  /// <summary>
  /// Provides basic graphics device initialization, game logic, and rendering code.
  /// </summary>
  public class Game : IDisposable
  {
    private readonly TimeSpan maximumElapsedTime = TimeSpan.FromMilliseconds(500.0);
    private GameTime gameTime = new GameTime();
    private bool isFixedTimeStep = true;
    private int updatesSinceRunningSlowly1 = int.MaxValue;
    private int updatesSinceRunningSlowly2 = int.MaxValue;
    private List<IUpdateable> updateableComponents = new List<IUpdateable>();
    private List<IUpdateable> currentlyUpdatingComponents = new List<IUpdateable>();
    private List<IDrawable> drawableComponents = new List<IDrawable>();
    private List<IDrawable> currentlyDrawingComponents = new List<IDrawable>();
    private List<IGameComponent> notYetInitialized = new List<IGameComponent>();
    private GameServiceContainer gameServices = new GameServiceContainer();
    private IGraphicsDeviceManager graphicsDeviceManager;
    private IGraphicsDeviceService graphicsDeviceService;
    private GameHost host;
    private bool isActive;
    private bool exitRequested;
    private TimeSpan inactiveSleepTime;
    private bool isMouseVisible;
    private bool inRun;
    private bool endRunRequired;
    private GameClock clock;
    private TimeSpan totalGameTime;
    private TimeSpan targetElapsedTime;
    private TimeSpan accumulatedElapsedGameTime;
    private TimeSpan lastFrameElapsedGameTime;
    private bool drawRunningSlowly;
    private bool doneFirstUpdate;
    private bool doneFirstDraw;
    private bool forceElapsedTimeToZero;
    private bool suppressDraw;
    private GameComponentCollection gameComponents;
    private ContentManager content;
    private LaunchParameters launchParameters;
    private EventHandler<EventArgs> Activated;
    private EventHandler<EventArgs> Deactivated;
    private EventHandler<EventArgs> Exiting;
    private EventHandler<EventArgs> Disposed;

    /// <summary>
    /// Gets the start up parameters in LaunchParameters.
    /// </summary>
    public LaunchParameters LaunchParameters
    {
      get
      {
        return this.launchParameters;
      }
    }

    /// <summary>
    /// Gets the collection of GameComponents owned by the game.
    /// </summary>
    public GameComponentCollection Components
    {
      get
      {
        return this.gameComponents;
      }
    }

    /// <summary>
    /// Gets the GameServiceContainer holding all the service providers attached to the Game.
    /// </summary>
    public GameServiceContainer Services
    {
      get
      {
        return this.gameServices;
      }
    }

    /// <summary>
    /// Gets or sets the time to sleep when the game is inactive.
    /// </summary>
    public TimeSpan InactiveSleepTime
    {
      get
      {
        return this.inactiveSleepTime;
      }
      set
      {
        if (value < TimeSpan.Zero)
          throw new ArgumentOutOfRangeException("value", Resources.InactiveSleepTimeCannotBeZero);
        this.inactiveSleepTime = value;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the mouse cursor should be visible.
    /// </summary>
    public bool IsMouseVisible
    {
      get
      {
        return this.isMouseVisible;
      }
      set
      {
        this.isMouseVisible = value;
        if (this.Window == null)
          return;
        this.Window.IsMouseVisible = value;
      }
    }

    /// <summary>
    /// Gets or sets the target time between calls to Update when IsFixedTimeStep is true. Reference page contains links to related code samples.
    /// </summary>
    public TimeSpan TargetElapsedTime
    {
      get
      {
        return this.targetElapsedTime;
      }
      set
      {
        if (value <= TimeSpan.Zero)
          throw new ArgumentOutOfRangeException("value", Resources.TargetElaspedCannotBeZero);
        this.targetElapsedTime = value;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether to use fixed time steps.
    /// </summary>
    public bool IsFixedTimeStep
    {
      get
      {
        return this.isFixedTimeStep;
      }
      set
      {
        this.isFixedTimeStep = value;
      }
    }

    /// <summary>
    /// Gets the underlying operating system window.
    /// </summary>
    public GameWindow Window
    {
      get
      {
        if (this.host != null)
          return this.host.Window;
        else
          return (GameWindow) null;
      }
    }

    /// <summary>
    /// Indicates whether the game is currently the active application.
    /// </summary>
    public bool IsActive
    {
      get
      {
        bool flag = false;
        if (GamerServicesDispatcher.IsInitialized)
          flag = Guide.IsVisible;
        if (this.isActive)
          return !flag;
        else
          return false;
      }
    }

    /// <summary>
    /// Gets the current GraphicsDevice.
    /// </summary>
    public GraphicsDevice GraphicsDevice
    {
      get
      {
        IGraphicsDeviceService graphicsDeviceService = this.graphicsDeviceService;
        if (graphicsDeviceService == null)
        {
          graphicsDeviceService = this.Services.GetService(typeof (IGraphicsDeviceService)) as IGraphicsDeviceService;
          if (graphicsDeviceService == null)
            throw new InvalidOperationException(Resources.NoGraphicsDeviceService);
        }
        return graphicsDeviceService.GraphicsDevice;
      }
    }

    /// <summary>
    /// Gets or sets the current ContentManager.
    /// </summary>
    public ContentManager Content
    {
      get
      {
        return this.content;
      }
      set
      {
        if (value == null)
          throw new ArgumentNullException();
        this.content = value;
      }
    }

    internal bool IsActiveIgnoringGuide
    {
      get
      {
        return this.isActive;
      }
    }

    bool ShouldExit
    {
      private get
      {
        return this.exitRequested;
      }
    }

    /// <summary>
    /// Raised when the game gains focus.
    /// </summary>
    /// <param name=""/>
    public event EventHandler<EventArgs> Activated
    {
      add
      {
        EventHandler<EventArgs> eventHandler = this.Activated;
        EventHandler<EventArgs> comparand;
        do
        {
          comparand = eventHandler;
          eventHandler = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref this.Activated, comparand + value, comparand);
        }
        while (eventHandler != comparand);
      }
      remove
      {
        EventHandler<EventArgs> eventHandler = this.Activated;
        EventHandler<EventArgs> comparand;
        do
        {
          comparand = eventHandler;
          eventHandler = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref this.Activated, comparand - value, comparand);
        }
        while (eventHandler != comparand);
      }
    }

    /// <summary>
    /// Raised when the game loses focus.
    /// </summary>
    /// <param name=""/>
    public event EventHandler<EventArgs> Deactivated
    {
      add
      {
        EventHandler<EventArgs> eventHandler = this.Deactivated;
        EventHandler<EventArgs> comparand;
        do
        {
          comparand = eventHandler;
          eventHandler = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref this.Deactivated, comparand + value, comparand);
        }
        while (eventHandler != comparand);
      }
      remove
      {
        EventHandler<EventArgs> eventHandler = this.Deactivated;
        EventHandler<EventArgs> comparand;
        do
        {
          comparand = eventHandler;
          eventHandler = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref this.Deactivated, comparand - value, comparand);
        }
        while (eventHandler != comparand);
      }
    }

    /// <summary>
    /// Raised when the game is exiting.
    /// </summary>
    /// <param name=""/>
    public event EventHandler<EventArgs> Exiting
    {
      add
      {
        EventHandler<EventArgs> eventHandler = this.Exiting;
        EventHandler<EventArgs> comparand;
        do
        {
          comparand = eventHandler;
          eventHandler = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref this.Exiting, comparand + value, comparand);
        }
        while (eventHandler != comparand);
      }
      remove
      {
        EventHandler<EventArgs> eventHandler = this.Exiting;
        EventHandler<EventArgs> comparand;
        do
        {
          comparand = eventHandler;
          eventHandler = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref this.Exiting, comparand - value, comparand);
        }
        while (eventHandler != comparand);
      }
    }

    /// <summary>
    /// Raised when the game is being disposed.
    /// </summary>
    /// <param name=""/>
    public event EventHandler<EventArgs> Disposed
    {
      add
      {
        EventHandler<EventArgs> eventHandler = this.Disposed;
        EventHandler<EventArgs> comparand;
        do
        {
          comparand = eventHandler;
          eventHandler = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref this.Disposed, comparand + value, comparand);
        }
        while (eventHandler != comparand);
      }
      remove
      {
        EventHandler<EventArgs> eventHandler = this.Disposed;
        EventHandler<EventArgs> comparand;
        do
        {
          comparand = eventHandler;
          eventHandler = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref this.Disposed, comparand - value, comparand);
        }
        while (eventHandler != comparand);
      }
    }

    /// <summary>
    /// Initializes a new instance of this class, which provides basic graphics device initialization, game logic, rendering code, and a game loop.  Reference page contains code sample.
    /// </summary>
    public Game()
    {
      FrameworkDispatcher.Update();
      this.EnsureHost();
      this.launchParameters = new LaunchParameters();
      this.gameComponents = new GameComponentCollection();
      this.gameComponents.ComponentAdded += new EventHandler<GameComponentCollectionEventArgs>(this.GameComponentAdded);
      this.gameComponents.ComponentRemoved += new EventHandler<GameComponentCollectionEventArgs>(this.GameComponentRemoved);
      this.content = new ContentManager((IServiceProvider) this.gameServices);
      this.host.Window.Paint += new EventHandler<EventArgs>(this.Paint);
      this.clock = new GameClock();
      this.totalGameTime = TimeSpan.Zero;
      this.accumulatedElapsedGameTime = TimeSpan.Zero;
      this.lastFrameElapsedGameTime = TimeSpan.Zero;
      this.targetElapsedTime = TimeSpan.FromTicks(166667L);
      this.inactiveSleepTime = TimeSpan.FromMilliseconds(20.0);
    }

    /// <summary>
    /// Allows a Game to attempt to free resources and perform other cleanup operations before garbage collection reclaims the Game.
    /// </summary>
    ~Game()
    {
      this.Dispose(false);
    }

    /// <summary>
    /// Call this method to initialize the game, begin running the game loop, and start processing events for the game.
    /// </summary>
    public void Run()
    {
      this.RunGame(true);
    }

    internal void StartGameLoop()
    {
      this.RunGame(false);
    }

    private void RunGame(bool useBlockingRun)
    {
      try
      {
        this.graphicsDeviceManager = this.Services.GetService(typeof (IGraphicsDeviceManager)) as IGraphicsDeviceManager;
        if (this.graphicsDeviceManager != null)
          this.graphicsDeviceManager.CreateDevice();
        this.Initialize();
        this.inRun = true;
        this.BeginRun();
        this.gameTime.ElapsedGameTime = TimeSpan.Zero;
        this.gameTime.TotalGameTime = this.totalGameTime;
        this.gameTime.IsRunningSlowly = false;
        this.Update(this.gameTime);
        this.doneFirstUpdate = true;
        if (useBlockingRun)
        {
          if (this.host != null)
            this.host.Run();
          this.EndRun();
        }
        else
        {
          if (this.host != null)
            this.host.StartGameLoop();
          this.endRunRequired = true;
        }
      }
      catch (NoSuitableGraphicsDeviceException ex)
      {
        if (this.ShowMissingRequirementMessage((Exception) ex))
          return;
        throw;
      }
      catch (NoAudioHardwareException ex)
      {
        if (this.ShowMissingRequirementMessage((Exception) ex))
          return;
        throw;
      }
      finally
      {
        if (!this.endRunRequired)
          this.inRun = false;
      }
    }

    /// <summary>
    /// Run the game through what would happen in a single tick of the game clock; this method is designed for debugging only.
    /// </summary>
    public void RunOneFrame()
    {
      if (this.host == null)
        return;
      this.host.RunOneFrame();
    }

    /// <summary>
    /// Updates the game's clock and calls Update and Draw.
    /// </summary>
    public void Tick()
    {
      if (this.ShouldExit)
        return;
      if (!this.isActive)
        Thread.Sleep((int) this.inactiveSleepTime.TotalMilliseconds);
      this.clock.Step();
      bool flag = true;
      TimeSpan timeSpan1 = this.clock.ElapsedAdjustedTime;
      if (timeSpan1 < TimeSpan.Zero)
        timeSpan1 = TimeSpan.Zero;
      if (this.forceElapsedTimeToZero)
      {
        timeSpan1 = TimeSpan.Zero;
        this.forceElapsedTimeToZero = false;
      }
      if (timeSpan1 > this.maximumElapsedTime)
        timeSpan1 = this.maximumElapsedTime;
      if (this.isFixedTimeStep)
      {
        if (Math.Abs(timeSpan1.Ticks - this.targetElapsedTime.Ticks) < this.targetElapsedTime.Ticks >> 6)
          timeSpan1 = this.targetElapsedTime;
        this.accumulatedElapsedGameTime += timeSpan1;
        long num = this.accumulatedElapsedGameTime.Ticks / this.targetElapsedTime.Ticks;
        this.accumulatedElapsedGameTime = TimeSpan.FromTicks(this.accumulatedElapsedGameTime.Ticks % this.targetElapsedTime.Ticks);
        this.lastFrameElapsedGameTime = TimeSpan.Zero;
        if (num == 0L)
          return;
        TimeSpan timeSpan2 = this.targetElapsedTime;
        if (num > 1L)
        {
          this.updatesSinceRunningSlowly2 = this.updatesSinceRunningSlowly1;
          this.updatesSinceRunningSlowly1 = 0;
        }
        else
        {
          if (this.updatesSinceRunningSlowly1 < int.MaxValue)
            ++this.updatesSinceRunningSlowly1;
          if (this.updatesSinceRunningSlowly2 < int.MaxValue)
            ++this.updatesSinceRunningSlowly2;
        }
        this.drawRunningSlowly = this.updatesSinceRunningSlowly2 < 20;
        while (num > 0L && !this.ShouldExit)
        {
          --num;
          try
          {
            this.gameTime.ElapsedGameTime = timeSpan2;
            this.gameTime.TotalGameTime = this.totalGameTime;
            this.gameTime.IsRunningSlowly = this.drawRunningSlowly;
            this.Update(this.gameTime);
            flag = flag & this.suppressDraw;
            this.suppressDraw = false;
          }
          finally
          {
            this.lastFrameElapsedGameTime += timeSpan2;
            this.totalGameTime += timeSpan2;
          }
        }
      }
      else
      {
        TimeSpan timeSpan2 = timeSpan1;
        this.drawRunningSlowly = false;
        this.updatesSinceRunningSlowly1 = int.MaxValue;
        this.updatesSinceRunningSlowly2 = int.MaxValue;
        if (!this.ShouldExit)
        {
          try
          {
            this.gameTime.ElapsedGameTime = this.lastFrameElapsedGameTime = timeSpan2;
            this.gameTime.TotalGameTime = this.totalGameTime;
            this.gameTime.IsRunningSlowly = false;
            this.Update(this.gameTime);
            flag = flag & this.suppressDraw;
            this.suppressDraw = false;
          }
          finally
          {
            this.totalGameTime += timeSpan2;
          }
        }
      }
      if (flag)
        return;
      this.DrawFrame();
    }

    /// <summary>
    /// Prevents calls to Draw until the next Update.
    /// </summary>
    public void SuppressDraw()
    {
      this.suppressDraw = true;
    }

    /// <summary>
    /// Exits the game.
    /// </summary>
    public void Exit()
    {
      this.exitRequested = true;
      this.host.Exit();
      if (!this.inRun || !this.endRunRequired)
        return;
      this.EndRun();
      this.inRun = false;
    }

    /// <summary>
    /// Called after all components are initialized but before the first update in the game loop.
    /// </summary>
    protected virtual void BeginRun()
    {
    }

    /// <summary>
    /// Called after the game loop has stopped running before exiting.
    /// </summary>
    protected virtual void EndRun()
    {
    }

    /// <summary>
    /// Reference page contains links to related conceptual articles.
    /// </summary>
    /// <param name="gameTime">Time passed since the last call to Update.</param>
    protected virtual void Update(GameTime gameTime)
    {
      for (int index = 0; index < this.updateableComponents.Count; ++index)
        this.currentlyUpdatingComponents.Add(this.updateableComponents[index]);
      for (int index = 0; index < this.currentlyUpdatingComponents.Count; ++index)
      {
        IUpdateable updateable = this.currentlyUpdatingComponents[index];
        if (updateable.Enabled)
          updateable.Update(gameTime);
      }
      this.currentlyUpdatingComponents.Clear();
      FrameworkDispatcher.Update();
      this.doneFirstUpdate = true;
    }

    /// <summary>
    /// Starts the drawing of a frame. This method is followed by calls to Draw and EndDraw.
    /// </summary>
    protected virtual bool BeginDraw()
    {
      if (this.graphicsDeviceManager != null && !this.graphicsDeviceManager.BeginDraw())
        return false;
      else
        return true;
    }

    /// <summary>
    /// Reference page contains code sample.
    /// </summary>
    /// <param name="gameTime">Time passed since the last call to Draw.</param>
    protected virtual void Draw(GameTime gameTime)
    {
      for (int index = 0; index < this.drawableComponents.Count; ++index)
        this.currentlyDrawingComponents.Add(this.drawableComponents[index]);
      for (int index = 0; index < this.currentlyDrawingComponents.Count; ++index)
      {
        IDrawable drawable = this.currentlyDrawingComponents[index];
        if (drawable.Visible)
          drawable.Draw(gameTime);
      }
      this.currentlyDrawingComponents.Clear();
    }

    /// <summary>
    /// Ends the drawing of a frame. This method is preceeded by calls to Draw and BeginDraw.
    /// </summary>
    protected virtual void EndDraw()
    {
      if (this.graphicsDeviceManager == null)
        return;
      this.graphicsDeviceManager.EndDraw();
    }

    private void Paint(object sender, EventArgs e)
    {
      if (!this.doneFirstDraw)
        return;
      this.DrawFrame();
    }

    /// <summary>
    /// Called after the Game and GraphicsDevice are created, but before LoadContent.  Reference page contains code sample.
    /// </summary>
    protected virtual void Initialize()
    {
      this.HookDeviceEvents();
      while (this.notYetInitialized.Count != 0)
      {
        this.notYetInitialized[0].Initialize();
        this.notYetInitialized.RemoveAt(0);
      }
      if (this.graphicsDeviceService == null || this.graphicsDeviceService.GraphicsDevice == null)
        return;
      this.LoadContent();
    }

    /// <summary>
    /// Resets the elapsed time counter.
    /// </summary>
    public void ResetElapsedTime()
    {
      this.forceElapsedTimeToZero = true;
      this.drawRunningSlowly = false;
      this.updatesSinceRunningSlowly1 = int.MaxValue;
      this.updatesSinceRunningSlowly2 = int.MaxValue;
    }

    private void DrawFrame()
    {
      try
      {
        if (this.ShouldExit || !this.doneFirstUpdate || (this.Window.IsMinimized || !this.BeginDraw()))
          return;
        this.gameTime.TotalGameTime = this.totalGameTime;
        this.gameTime.ElapsedGameTime = this.lastFrameElapsedGameTime;
        this.gameTime.IsRunningSlowly = this.drawRunningSlowly;
        this.Draw(this.gameTime);
        this.EndDraw();
        this.doneFirstDraw = true;
      }
      finally
      {
        this.lastFrameElapsedGameTime = TimeSpan.Zero;
      }
    }

    private void GameComponentRemoved(object sender, GameComponentCollectionEventArgs e)
    {
      if (!this.inRun)
        this.notYetInitialized.Remove(e.GameComponent);
      IUpdateable updateable = e.GameComponent as IUpdateable;
      if (updateable != null)
      {
        this.updateableComponents.Remove(updateable);
        updateable.UpdateOrderChanged -= new EventHandler<EventArgs>(this.UpdateableUpdateOrderChanged);
      }
      IDrawable drawable = e.GameComponent as IDrawable;
      if (drawable == null)
        return;
      this.drawableComponents.Remove(drawable);
      drawable.DrawOrderChanged -= new EventHandler<EventArgs>(this.DrawableDrawOrderChanged);
    }

    private void GameComponentAdded(object sender, GameComponentCollectionEventArgs e)
    {
      if (this.inRun)
        e.GameComponent.Initialize();
      else
        this.notYetInitialized.Add(e.GameComponent);
      IUpdateable updateable = e.GameComponent as IUpdateable;
      if (updateable != null)
      {
        int num = this.updateableComponents.BinarySearch(updateable, (IComparer<IUpdateable>) UpdateOrderComparer.Default);
        if (num < 0)
        {
          int index = ~num;
          while (index < this.updateableComponents.Count && this.updateableComponents[index].UpdateOrder == updateable.UpdateOrder)
            ++index;
          this.updateableComponents.Insert(index, updateable);
          updateable.UpdateOrderChanged += new EventHandler<EventArgs>(this.UpdateableUpdateOrderChanged);
        }
      }
      IDrawable drawable = e.GameComponent as IDrawable;
      if (drawable == null)
        return;
      int num1 = this.drawableComponents.BinarySearch(drawable, (IComparer<IDrawable>) DrawOrderComparer.Default);
      if (num1 >= 0)
        return;
      int index1 = ~num1;
      while (index1 < this.drawableComponents.Count && this.drawableComponents[index1].DrawOrder == drawable.DrawOrder)
        ++index1;
      this.drawableComponents.Insert(index1, drawable);
      drawable.DrawOrderChanged += new EventHandler<EventArgs>(this.DrawableDrawOrderChanged);
    }

    private void DrawableDrawOrderChanged(object sender, EventArgs e)
    {
      IDrawable drawable = sender as IDrawable;
      this.drawableComponents.Remove(drawable);
      int num = this.drawableComponents.BinarySearch(drawable, (IComparer<IDrawable>) DrawOrderComparer.Default);
      if (num >= 0)
        return;
      int index = ~num;
      while (index < this.drawableComponents.Count && this.drawableComponents[index].DrawOrder == drawable.DrawOrder)
        ++index;
      this.drawableComponents.Insert(index, drawable);
    }

    private void UpdateableUpdateOrderChanged(object sender, EventArgs e)
    {
      IUpdateable updateable = sender as IUpdateable;
      this.updateableComponents.Remove(updateable);
      int num = this.updateableComponents.BinarySearch(updateable, (IComparer<IUpdateable>) UpdateOrderComparer.Default);
      if (num >= 0)
        return;
      int index = ~num;
      while (index < this.updateableComponents.Count && this.updateableComponents[index].UpdateOrder == updateable.UpdateOrder)
        ++index;
      this.updateableComponents.Insert(index, updateable);
    }

    /// <summary>
    /// Raises the Activated event. Override this method to add code to handle when the game gains focus.
    /// </summary>
    /// <param name="sender">The Game.</param><param name="args">Arguments for the Activated event.</param>
    protected virtual void OnActivated(object sender, EventArgs args)
    {
      if (this.Activated == null)
        return;
      this.Activated((object) this, args);
    }

    /// <summary>
    /// Raises the Deactivated event. Override this method to add code to handle when the game loses focus.
    /// </summary>
    /// <param name="sender">The Game.</param><param name="args">Arguments for the Deactivated event.</param>
    protected virtual void OnDeactivated(object sender, EventArgs args)
    {
      if (this.Deactivated == null)
        return;
      this.Deactivated((object) this, args);
    }

    /// <summary>
    /// Raises an Exiting event. Override this method to add code to handle when the game is exiting.
    /// </summary>
    /// <param name="sender">The Game.</param><param name="args">Arguments for the Exiting event.</param>
    protected virtual void OnExiting(object sender, EventArgs args)
    {
      if (this.Exiting == null)
        return;
      this.Exiting((object) null, args);
    }

    private void EnsureHost()
    {
      if (this.host != null)
        return;
      this.host = (GameHost) new WindowsGameHost(this);
      this.host.Activated += new EventHandler<EventArgs>(this.HostActivated);
      this.host.Deactivated += new EventHandler<EventArgs>(this.HostDeactivated);
      this.host.Suspend += new EventHandler<EventArgs>(this.HostSuspend);
      this.host.Resume += new EventHandler<EventArgs>(this.HostResume);
      this.host.Idle += new EventHandler<EventArgs>(this.HostIdle);
      this.host.Exiting += new EventHandler<EventArgs>(this.HostExiting);
    }

    private void HostSuspend(object sender, EventArgs e)
    {
      this.clock.Suspend();
    }

    private void HostResume(object sender, EventArgs e)
    {
      this.clock.Resume();
    }

    private void HostExiting(object sender, EventArgs e)
    {
      this.OnExiting((object) this, EventArgs.Empty);
    }

    private void HostIdle(object sender, EventArgs e)
    {
      this.Tick();
    }

    private void HostDeactivated(object sender, EventArgs e)
    {
      if (!this.isActive)
        return;
      this.isActive = false;
      this.OnDeactivated((object) this, EventArgs.Empty);
    }

    private void HostActivated(object sender, EventArgs e)
    {
      if (this.isActive)
        return;
      this.isActive = true;
      this.OnActivated((object) this, EventArgs.Empty);
    }

    private void HookDeviceEvents()
    {
      this.graphicsDeviceService = this.Services.GetService(typeof (IGraphicsDeviceService)) as IGraphicsDeviceService;
      if (this.graphicsDeviceService == null)
        return;
      this.graphicsDeviceService.DeviceCreated += new EventHandler<EventArgs>(this.DeviceCreated);
      this.graphicsDeviceService.DeviceResetting += new EventHandler<EventArgs>(this.DeviceResetting);
      this.graphicsDeviceService.DeviceReset += new EventHandler<EventArgs>(this.DeviceReset);
      this.graphicsDeviceService.DeviceDisposing += new EventHandler<EventArgs>(this.DeviceDisposing);
    }

    private void UnhookDeviceEvents()
    {
      if (this.graphicsDeviceService == null)
        return;
      this.graphicsDeviceService.DeviceCreated -= new EventHandler<EventArgs>(this.DeviceCreated);
      this.graphicsDeviceService.DeviceResetting -= new EventHandler<EventArgs>(this.DeviceResetting);
      this.graphicsDeviceService.DeviceReset -= new EventHandler<EventArgs>(this.DeviceReset);
      this.graphicsDeviceService.DeviceDisposing -= new EventHandler<EventArgs>(this.DeviceDisposing);
    }

    private void DeviceResetting(object sender, EventArgs e)
    {
    }

    private void DeviceReset(object sender, EventArgs e)
    {
    }

    private void DeviceCreated(object sender, EventArgs e)
    {
      this.LoadContent();
    }

    private void DeviceDisposing(object sender, EventArgs e)
    {
      this.content.Unload();
      this.UnloadContent();
    }

    /// <summary/>
    protected virtual void LoadContent()
    {
    }

    /// <summary>
    /// Called when graphics resources need to be unloaded. Override this method to unload any game-specific graphics resources.
    /// </summary>
    protected virtual void UnloadContent()
    {
    }

    /// <summary>
    /// Immediately releases the unmanaged resources used by this object.
    /// </summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    /// <summary>
    /// Releases all resources used by the Game class.
    /// </summary>
    /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      if (!disposing)
        return;
      lock (this)
      {
        IGameComponent[] local_0 = new IGameComponent[this.gameComponents.Count];
        this.gameComponents.CopyTo(local_0, 0);
        for (int local_1 = 0; local_1 < local_0.Length; ++local_1)
        {
          IDisposable local_2 = local_0[local_1] as IDisposable;
          if (local_2 != null)
            local_2.Dispose();
        }
        IDisposable local_3 = this.graphicsDeviceManager as IDisposable;
        if (local_3 != null)
          local_3.Dispose();
        this.UnhookDeviceEvents();
        if (this.Disposed == null)
          return;
        this.Disposed((object) this, EventArgs.Empty);
      }
    }

    /// <summary>
    /// This is used to display an error message if there is no suitable graphics device or sound card.
    /// </summary>
    /// <param name="exception">The exception to display.</param>
    protected virtual bool ShowMissingRequirementMessage(Exception exception)
    {
      if (this.host != null)
        return this.host.ShowMissingRequirementMessage(exception);
      else
        return false;
    }
  }
}
