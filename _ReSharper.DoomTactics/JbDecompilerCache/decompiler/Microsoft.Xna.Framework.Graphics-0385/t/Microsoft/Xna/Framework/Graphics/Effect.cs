// Type: Microsoft.Xna.Framework.Graphics.Effect
// Assembly: Microsoft.Xna.Framework.Graphics, Version=4.0.0.0, Culture=neutral, PublicKeyToken=842cf8be1de50553
// Assembly location: C:\Program Files (x86)\Microsoft XNA\XNA Game Studio\v4.0\References\Windows\x86\Microsoft.Xna.Framework.Graphics.dll

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace Microsoft.Xna.Framework.Graphics
{
  /// <summary>
  /// Used to set and query effects, and to choose techniques. Reference page contains links to related conceptual articles.
  /// </summary>
  public class Effect : GraphicsResource, IGraphicsResource
  {
    internal static object pSyncObject = new object();
    internal unsafe ID3DXEffect* pComPtr = pInterface;
    private EffectTechniqueCollection pTechniqueCollection;
    private EffectParameterCollection pParamCollection;
    internal byte[] pCachedEffectData;
    internal WeakReference pParentEffect;
    internal List<WeakReference> pClonedEffects;
    internal EffectTechnique _currentTechnique;

    /// <summary>
    /// Gets a collection of parameters used for this effect.
    /// </summary>
    public EffectParameterCollection Parameters
    {
      get
      {
        return this.pParamCollection;
      }
    }

    /// <summary>
    /// Gets a collection of techniques that are defined for this effect.  Reference page contains code sample.
    /// </summary>
    public EffectTechniqueCollection Techniques
    {
      get
      {
        return this.pTechniqueCollection;
      }
    }

    /// <summary>
    /// Gets or sets the active technique.  Reference page contains code sample.
    /// </summary>
    public EffectTechnique CurrentTechnique
    {
      get
      {
        return this._currentTechnique;
      }
      set
      {
        Helpers.CheckDisposed((object) this, (IntPtr) ((void*) this.pComPtr));
        if (value == null)
          throw new ArgumentNullException("value", FrameworkResources.NullNotAllowed);
        if (value == this._currentTechnique)
          return;
        if (value._parent != this)
          throw new InvalidOperationException();
        EffectPass effectPass = this._parent.activePass;
        if (effectPass != null)
        {
          effectPass.EndPass();
          this._parent.activePass = (EffectPass) null;
        }
        ID3DXEffect* id3DxEffectPtr1 = this.pComPtr;
        ID3DXEffect* id3DxEffectPtr2 = id3DxEffectPtr1;
        sbyte* numPtr = value._handle;
        int num = __calli((__FnPtr<int (IntPtr, sbyte*)>) *(int*) (*(int*) id3DxEffectPtr1 + 232))((sbyte*) id3DxEffectPtr2, (IntPtr) numPtr);
        if (num < 0)
          throw GraphicsHelpers.GetExceptionFromResult((uint) num);
        this._currentTechnique = value;
      }
    }

    static Effect()
    {
    }

    private Effect(ID3DXEffect* pInterface, GraphicsDevice pDevice)
    {
      // ISSUE: fault handler
      try
      {
        this._parent = pDevice;
        this.InitializeHelpers();
      }
      __fault
      {
        base.Dispose(true);
      }
    }

    /// <summary>
    /// Creates an instance of this object.
    /// </summary>
    /// <param name="graphicsDevice">The device.</param><param name="effectCode">The effect code.</param>
    public Effect(GraphicsDevice graphicsDevice, byte[] effectCode)
    {
      // ISSUE: fault handler
      try
      {
        this.CreateEffectFromCode(graphicsDevice, effectCode);
      }
      __fault
      {
        base.Dispose(true);
      }
    }

    /// <summary>
    /// Creates an instance of this object.
    /// </summary>
    /// <param name="cloneSource">An object to copy.</param>
    protected Effect(Effect cloneSource)
    {
      // ISSUE: fault handler
      try
      {
        if (cloneSource == null)
          throw new ArgumentNullException("cloneSource", FrameworkResources.NullNotAllowed);
        IntPtr pComPtr = (IntPtr) ((void*) cloneSource.pComPtr);
        Helpers.CheckDisposed((object) cloneSource, pComPtr);
        GraphicsDevice graphicsDevice = cloneSource._parent;
        if (graphicsDevice == null)
          throw new ArgumentNullException("graphicsDevice", FrameworkResources.DeviceCannotBeNullOnResourceCreate);
        ID3DXEffect* id3DxEffectPtr1 = (ID3DXEffect*) 0;
        ID3DXEffect* id3DxEffectPtr2 = cloneSource.pComPtr;
        ID3DXEffect* id3DxEffectPtr3 = id3DxEffectPtr2;
        StateTrackerDevice* stateTrackerDevicePtr = graphicsDevice.pStateTracker;
        // ISSUE: explicit reference operation
        // ISSUE: variable of a reference type
        ID3DXEffect*& local = @id3DxEffectPtr1;
        // ISSUE: cast to a function pointer type
        // ISSUE: function pointer call
        int num1 = __calli((__FnPtr<int (IntPtr, IDirect3DDevice9*, ID3DXEffect**)>) *(int*) (*(int*) id3DxEffectPtr2 + 308))((ID3DXEffect**) id3DxEffectPtr3, (IDirect3DDevice9*) stateTrackerDevicePtr, (IntPtr) local);
        if (num1 < 0)
          throw GraphicsHelpers.GetExceptionFromResult((uint) num1);
        this._parent = graphicsDevice;
        this.pComPtr = id3DxEffectPtr1;
        this.pParentEffect = new WeakReference((object) cloneSource);
        cloneSource.AddClonedEffect(this);
        graphicsDevice.Resources.AddTrackedObject((object) this, (void*) id3DxEffectPtr1, 0U, this._internalHandle, ref this._internalHandle);
        this.InitializeHelpers();
        Effect effect = this;
        EffectTechnique effectTechnique = effect.pTechniqueCollection[0];
        effect.CurrentTechnique = effectTechnique;
        int index = 0;
        if (0 >= this.pParamCollection.Count)
          return;
        do
        {
          EffectParameter effectParameter = this.pParamCollection[index];
          if (effectParameter.ElementCount > 1)
          {
            switch (effectParameter._paramType)
            {
              case EffectParameterType.Bool:
              case EffectParameterType.Int32:
              case EffectParameterType.Single:
                int num2 = effectParameter._rows;
                int num3 = effectParameter._columns;
                int count = effectParameter.ElementCount * num3 * num2;
                if (count > 0)
                {
                  float[] valueSingleArray = cloneSource.pParamCollection[index].GetValueSingleArray(count);
                  effectParameter.SetValue(valueSingleArray);
                  break;
                }
                else
                  break;
            }
          }
          ++index;
        }
        while (index < this.pParamCollection.Count);
      }
      __fault
      {
        base.Dispose(true);
      }
    }

    private unsafe void InitializeHelpers()
    {
      Helpers.CheckDisposed((object) this, (IntPtr) ((void*) this.pComPtr));
      ID3DXEffect* id3DxEffectPtr1 = this.pComPtr;
      ID3DXEffect* id3DxEffectPtr2 = id3DxEffectPtr1;
      _D3DXEFFECT_DESC d3DxeffectDesc;
      // ISSUE: explicit reference operation
      // ISSUE: variable of a reference type
      _D3DXEFFECT_DESC& local = @d3DxeffectDesc;
      // ISSUE: cast to a function pointer type
      // ISSUE: function pointer call
      int num = __calli((__FnPtr<int (IntPtr, _D3DXEFFECT_DESC*)>) *(int*) (*(int*) id3DxEffectPtr1 + 12))((_D3DXEFFECT_DESC*) id3DxEffectPtr2, (IntPtr) local);
      if (num < 0)
        throw GraphicsHelpers.GetExceptionFromResult((uint) num);
      EffectTechniqueCollection techniqueCollection1 = this.pTechniqueCollection;
      if (techniqueCollection1 != null && this.pParamCollection != null)
      {
        techniqueCollection1.UpdateParent((ID3DXBaseEffect*) this.pComPtr);
        this.pParamCollection.UpdateParent((ID3DXBaseEffect*) this.pComPtr, (sbyte*) 0, false);
      }
      else
      {
        Effect effect1 = this;
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        EffectTechniqueCollection techniqueCollection2 = new EffectTechniqueCollection((ID3DXBaseEffect*) effect1.pComPtr, this, ^(int&) ((IntPtr) &d3DxeffectDesc + 8));
        effect1.pTechniqueCollection = techniqueCollection2;
        Effect effect2 = this;
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        EffectParameterCollection parameterCollection = new EffectParameterCollection((ID3DXBaseEffect*) effect2.pComPtr, this, (sbyte*) 0, ^(int&) ((IntPtr) &d3DxeffectDesc + 4), false);
        effect2.pParamCollection = parameterCollection;
      }
    }

    private unsafe void CreateEffectFromCode(GraphicsDevice graphicsDevice, byte[] effectCode)
    {
      if (effectCode != null)
      {
        int length = effectCode.Length;
        if (length != 0)
        {
          if (length % 4 != 0)
          {
            string paramName = "effectCode";
            throw new ArgumentException(string.Format(FrameworkResources.ArrayMultipleFour, (object) paramName), paramName);
          }
          else
          {
            if (graphicsDevice == null)
              throw new ArgumentNullException("graphicsDevice", FrameworkResources.DeviceCannotBeNullOnResourceCreate);
            if ((uint) length >= 8U)
            {
              ID3DXBuffer* id3DxBufferPtr1 = (ID3DXBuffer*) 0;
              fixed (byte* numPtr1 = &effectCode[0])
              {
                if ((int) *(uint*) numPtr1 == -1125119025)
                {
                  uint* numPtr2 = (uint*) ((IntPtr) numPtr1 + 4);
                  uint num1 = *numPtr2;
                  if (effectCode.Length >= (int) num1 + 4)
                  {
                    void* voidPtr = (void*) ((IntPtr) numPtr1 + (int) num1);
                    if (*(int*) voidPtr == -16840447)
                    {
                      StateTrackerDevice* stateTrackerDevicePtr = graphicsDevice.pStateTracker;
                      *(int*) ((IntPtr) stateTrackerDevicePtr + 92) = 0;
                      *(int*) ((IntPtr) stateTrackerDevicePtr + 96) = 0;
                      *(int*) ((IntPtr) stateTrackerDevicePtr + 100) = 0;
                      fixed (ID3DXEffect** id3DxEffectPtr1 = &this.pComPtr)
                      {
                        int effectEx = \u003CModule\u003E.D3DXCreateEffectEx((IDirect3DDevice9*) stateTrackerDevicePtr, voidPtr, (uint) effectCode.Length - num1, (_D3DXMACRO*) 0, (ID3DXInclude*) 0, (sbyte*) 0, 131072U, (ID3DXEffectPool*) 0, id3DxEffectPtr1, &id3DxBufferPtr1);
                        if ((IntPtr) id3DxBufferPtr1 != IntPtr.Zero)
                        {
                          ID3DXBuffer* id3DxBufferPtr2 = id3DxBufferPtr1;
                          // ISSUE: cast to a function pointer type
                          // ISSUE: function pointer call
                          int num2 = (int) __calli((__FnPtr<uint (IntPtr)>) *(int*) (*(int*) id3DxBufferPtr2 + 8))((IntPtr) id3DxBufferPtr2);
                          id3DxBufferPtr1 = (ID3DXBuffer*) 0;
                        }
                        uint shaderModel1 = (uint) *(int*) ((IntPtr) stateTrackerDevicePtr + 92);
                        if ((int) shaderModel1 != 0)
                          graphicsDevice._profileCapabilities.ThrowNotSupportedException(FrameworkResources.ProfileVertexShaderModel, (object) Effect.FormatShaderModel(shaderModel1));
                        uint shaderModel2 = (uint) *(int*) ((IntPtr) stateTrackerDevicePtr + 96);
                        if ((int) shaderModel2 != 0)
                          graphicsDevice._profileCapabilities.ThrowNotSupportedException(FrameworkResources.ProfilePixelShaderModel, (object) Effect.FormatShaderModel(shaderModel2));
                        if (effectEx < 0)
                          throw GraphicsHelpers.GetExceptionFromResult((uint) effectEx);
                        int num3 = *(int*) ((IntPtr) stateTrackerDevicePtr + 100);
                        if (num3 < 0)
                          throw GraphicsHelpers.GetExceptionFromResult((uint) num3);
                        this._parent = graphicsDevice;
                        this.pCachedEffectData = effectCode;
                        DeviceResourceManager resources = graphicsDevice.Resources;
                        Effect effect1 = this;
                        ID3DXEffect* id3DxEffectPtr2 = effect1.pComPtr;
                        int num4 = 0;
                        long num5 = (long) this._internalHandle;
                        // ISSUE: explicit reference operation
                        // ISSUE: variable of a reference type
                        ulong& updatedHandle = @this._internalHandle;
                        resources.AddTrackedObject((object) effect1, (void*) id3DxEffectPtr2, (uint) num4, (ulong) num5, updatedHandle);
                        this.InitializeHelpers();
                        Effect effect2 = this;
                        EffectTechnique effectTechnique = effect2.pTechniqueCollection[0];
                        effect2.CurrentTechnique = effectTechnique;
                        List<EffectTechnique>.Enumerator enumerator1 = this.pTechniqueCollection.GetEnumerator();
                        if (!enumerator1.MoveNext())
                          return;
                        do
                        {
                          List<EffectPass>.Enumerator enumerator2 = enumerator1.Current.pPasses.GetEnumerator();
                          if (enumerator2.MoveNext())
                          {
                            uint* numPtr3 = (uint*) ((IntPtr) numPtr2 + 4);
                            do
                            {
                              EffectPass current = enumerator2.Current;
                              if (numPtr3 < voidPtr)
                              {
                                uint* numPtr4 = (uint*) ((IntPtr) numPtr2 + 4);
                                uint* numPtr5 = (uint*) ((IntPtr) numPtr3 + 4);
                                current._stateFlags = (EffectStateFlags) *numPtr4;
                                numPtr2 = (uint*) ((IntPtr) numPtr4 + 4);
                                numPtr3 = (uint*) ((IntPtr) numPtr5 + 4);
                                current._textureFlags = *numPtr2;
                              }
                              else
                                goto label_27;
                            }
                            while (enumerator2.MoveNext());
                          }
                        }
                        while (enumerator1.MoveNext());
                        return;
                      }
                    }
                  }
                }
              }
            }
label_27:
            throw new InvalidOperationException(FrameworkResources.MustUserShaderCode);
          }
        }
      }
      throw new ArgumentNullException("effectCode", FrameworkResources.NullNotAllowed);
    }

    private static string FormatShaderModel(uint shaderModel)
    {
      if ((int) shaderModel == 513)
        return "2.x";
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "{0}.{1}", new object[2]
      {
        (object) (shaderModel >> 8),
        (object) (uint) ((int) shaderModel & (int) byte.MaxValue)
      });
    }

    /// <summary>
    /// Copies data from an existing object to this object.
    /// </summary>
    public virtual Effect Clone()
    {
      return new Effect(this);
    }

    /// <summary>
    /// Applies the effect state just prior to rendering the effect.
    /// </summary>
    protected internal virtual void OnApply()
    {
    }

    internal unsafe void OnLostDevice()
    {
      Helpers.CheckDisposed((object) this, (IntPtr) ((void*) this.pComPtr));
      ID3DXEffect* id3DxEffectPtr = this.pComPtr;
      // ISSUE: cast to a function pointer type
      // ISSUE: function pointer call
      int num = __calli((__FnPtr<int (IntPtr)>) *(int*) (*(int*) id3DxEffectPtr + 276))((IntPtr) id3DxEffectPtr);
      if (num < 0)
        throw GraphicsHelpers.GetExceptionFromResult((uint) num);
    }

    internal unsafe void OnResetDevice()
    {
      Helpers.CheckDisposed((object) this, (IntPtr) ((void*) this.pComPtr));
      ID3DXEffect* id3DxEffectPtr = this.pComPtr;
      // ISSUE: cast to a function pointer type
      // ISSUE: function pointer call
      int num = __calli((__FnPtr<int (IntPtr)>) *(int*) (*(int*) id3DxEffectPtr + 280))((IntPtr) id3DxEffectPtr);
      if (num < 0)
        throw GraphicsHelpers.GetExceptionFromResult((uint) num);
    }

    internal void AddClonedEffect(Effect effect)
    {
      if (this.pClonedEffects == null)
        this.pClonedEffects = new List<WeakReference>();
      this.pClonedEffects.Add(new WeakReference((object) effect));
    }

    int IGraphicsResource.SaveDataForRecreation()
    {
      this.pParamCollection.SaveDataForRecreation();
      this.ReleaseNativeObject(false);
      return 0;
    }

    unsafe int IGraphicsResource.RecreateAndPopulateObject()
    {
      if ((IntPtr) this.pComPtr != IntPtr.Zero)
        return 0;
      GraphicsDevice graphicsDevice = this._parent;
      if (graphicsDevice == null)
        return -2147467259;
      int num = 0;
      WeakReference weakReference = this.pParentEffect;
      if (weakReference == null)
      {
        this.CreateEffectFromCode(graphicsDevice, this.pCachedEffectData);
      }
      else
      {
        Effect effect = weakReference.Target as Effect;
        if (effect != null)
        {
          ID3DXEffect* id3DxEffectPtr1 = effect.pComPtr;
          if ((IntPtr) id3DxEffectPtr1 != IntPtr.Zero)
          {
            ID3DXEffect* id3DxEffectPtr2 = (ID3DXEffect*) 0;
            ID3DXEffect* id3DxEffectPtr3 = id3DxEffectPtr1;
            StateTrackerDevice* stateTrackerDevicePtr = this._parent.pStateTracker;
            // ISSUE: explicit reference operation
            // ISSUE: variable of a reference type
            ID3DXEffect*& local = @id3DxEffectPtr2;
            // ISSUE: cast to a function pointer type
            // ISSUE: function pointer call
            num = __calli((__FnPtr<int (IntPtr, IDirect3DDevice9*, ID3DXEffect**)>) *(int*) (*(int*) id3DxEffectPtr1 + 308))((ID3DXEffect**) id3DxEffectPtr3, (IDirect3DDevice9*) stateTrackerDevicePtr, (IntPtr) local);
            this.pComPtr = id3DxEffectPtr2;
            this._parent.Resources.AddTrackedObject((object) this, (void*) id3DxEffectPtr2, 0U, this._internalHandle, ref this._internalHandle);
            this.InitializeHelpers();
          }
        }
      }
      List<WeakReference> list = this.pClonedEffects;
      if (list != null)
      {
        int index = 0;
        if (0 < list.Count)
        {
          do
          {
            if (num >= 0)
            {
              Effect effect = this.pClonedEffects[index].Target as Effect;
              if (effect != null && (IntPtr) effect.pComPtr == IntPtr.Zero)
                num = effect.RecreateAndPopulateObject();
            }
            ++index;
          }
          while (index < this.pClonedEffects.Count);
        }
      }
      EffectTechnique effectTechnique1 = this._currentTechnique;
      if (effectTechnique1 != null && (IntPtr) this.pComPtr != IntPtr.Zero)
      {
        EffectTechnique effectTechnique2 = effectTechnique1;
        this._currentTechnique = (EffectTechnique) null;
        this.CurrentTechnique = effectTechnique2;
      }
      return num;
    }

    unsafe void IGraphicsResource.ReleaseNativeObject([MarshalAs(UnmanagedType.U1)] bool disposeManagedResource)
    {
      bool lockTaken = false;
      try
      {
        Monitor.Enter(Effect.pSyncObject, ref lockTaken);
        GraphicsDevice graphicsDevice = this._parent;
        if (graphicsDevice != null && (IntPtr) this.pComPtr != IntPtr.Zero)
          graphicsDevice.Resources.ReleaseAllReferences(this._internalHandle, disposeManagedResource);
        this.pComPtr = (ID3DXEffect*) 0;
      }
      finally
      {
        if (lockTaken)
          Monitor.Exit(Effect.pSyncObject);
      }
    }

    [return: MarshalAs(UnmanagedType.U1)]
    internal virtual bool WantParameter(EffectParameter _param1)
    {
      return true;
    }

    internal static unsafe Effect GetManagedObject(ID3DXEffect* pInterface, GraphicsDevice pDevice, uint pool)
    {
      Effect effect = pDevice.Resources.GetCachedObject((void*) pInterface) as Effect;
      if (effect != null)
      {
        ID3DXEffect* id3DxEffectPtr = pInterface;
        // ISSUE: cast to a function pointer type
        // ISSUE: function pointer call
        int num = (int) __calli((__FnPtr<uint (IntPtr)>) *(int*) (*(int*) id3DxEffectPtr + 8))((IntPtr) id3DxEffectPtr);
        effect.isDisposed = false;
        GC.ReRegisterForFinalize((object) effect);
      }
      else
      {
        effect = new Effect(pInterface, pDevice);
        pDevice.Resources.AddTrackedObject((object) effect, (void*) pInterface, pool, 0UL, ref effect._internalHandle);
      }
      return effect;
    }

    private void OnObjectCreation()
    {
      this.InitializeHelpers();
    }

    private void \u0021Effect()
    {
      if (this.isDisposed)
        return;
      this.isDisposed = true;
      this.ReleaseNativeObject(true);
    }

    private void \u007EEffect()
    {
      this.\u0021Effect();
    }

    /// <summary>
    /// Releases the unmanaged resources used by the Effect and optionally releases the managed resources.
    /// </summary>
    /// <param name="">[MarshalAsAttribute(U1)] true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    [HandleProcessCorruptedStateExceptions]
    protected override void Dispose([MarshalAs(UnmanagedType.U1)] bool _param1)
    {
      if (param0)
      {
        try
        {
          this.\u007EEffect();
        }
        finally
        {
          base.Dispose(true);
        }
      }
      else
      {
        try
        {
          this.\u0021Effect();
        }
        finally
        {
          base.Dispose(false);
        }
      }
    }
  }
}
