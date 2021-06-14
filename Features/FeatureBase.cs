using HideItBobby.Common;
using HideItBobby.Common.Logging;
using System;
using System.Threading;
using UnityEngine;

namespace HideItBobby.Features
{
    internal abstract class FeatureBase : IFeature, IInitializable<FeatureFlags>, IForceToggleable<FeatureFlags>, IErrorInfo, IAvailabilityInfo, IDisposableEx
    {
        public abstract FeatureKey Key { get; }
        public virtual bool IsAvailable => true;

        protected virtual bool InitializeImpl() => true;
        protected virtual bool TerminateImpl() => true;
        protected virtual bool EnableImpl() => true;
        protected virtual bool DisableImpl() => true;

        #region Error
#if DEV || PREVIEW
        private const int ErrorTreshold = 10;
#else
        private const int ErrorTreshold = 3;
#endif
        private int _errorCount;
        public int ErrorCount => _errorCount;
        public bool IsError
        {
            get
            {
                return _errorCount >= ErrorTreshold;
            }
            set
            {
                if (value) Interlocked.Add(ref _errorCount, ErrorTreshold);
                else _errorCount = 0;
            }
        }
        protected void IncreaseErrorCount() => Interlocked.Increment(ref _errorCount);
        #endregion

        #region Disposable
        private bool _isDisposed;
        public bool IsDisposed => _isDisposed;

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            if (disposing)
            {
                // dispose managed state (managed objects)
                if (IsEnabled) try { _isEnabled = !DisableImpl(); } catch { IsError = true; }
                if (IsInitialized) try { _isInitialized = !TerminateImpl(); } catch { IsError = true; }
            }
            // free unmanaged resources (unmanaged objects) and override finalizer
            // set large fields to null
            _isDisposed = true;
        }

        // // override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~FeatureBase()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Initializable
        private bool _isInitialized;
        public virtual bool IsInitialized => _isInitialized;

        public FeatureFlags Initialize()
        {
            if (IsDisposed || IsError) return Result(false);
            if (!IsAvailable)
            {
#if DEV
                Log.Info($"{GetType().Name} is not available");
#endif
                return Result(false);
            }
            if (IsInitialized) return Result(true);
            try
            {
#if DEV
                Log.Info($"{GetType().Name} initializing");
#endif
                _isInitialized = InitializeImpl();
                return Result(IsInitialized, true);
            }
            catch (Exception e)
            {
                _isInitialized = false;
                IsError = true;
                Log.Error($"{GetType().Name}.{nameof(InitializeImpl)} failed", e);
                return Result(false);
            }
        }
        public FeatureFlags Terminate()
        {
            if (IsDisposed || IsError) return Result(false);
            if (!IsAvailable)
            {
#if DEV
                Log.Info($"{GetType().Name} is not available");
#endif
                return Result(false);
            }
            if (!IsInitialized) return Result(true);
            if (IsEnabled)
            {
                try
                {
#if DEV
                    Log.Info($"{GetType().Name} disabling");
#endif
                    _isEnabled = !DisableImpl();
                }
                catch (Exception e)
                {
                    IsError = true;
                    Log.Error($"{GetType().Name}.{nameof(DisableImpl)} failed", e);
                    return Result(false);
                }
            }
            try
            {
#if DEV
                Log.Info($"{GetType().Name} terminating");
#endif
                _isInitialized = !TerminateImpl();
                return Result(!IsInitialized, true);
            }
            catch (Exception e)
            {
                IsError = true;
                Log.Error($"{GetType().Name}.{nameof(TerminateImpl)} failed", e);
                return Result(false);
            }
        }

        void IInitializable.Initialize() => Initialize();
        void IInitializable.Terminate() => Terminate();
        #endregion

        #region Toggleable
        private bool _isEnabled;

        public virtual bool IsEnabled => _isEnabled;

        public FeatureFlags Enable() => Enable(false);
        public FeatureFlags Enable(bool force)
        {
            if (IsDisposed || IsError || !IsAvailable) return Result(false);
            if (IsEnabled && !force) return Result(true);
            if (!IsInitialized)
            {
                try
                {
#if DEV
                    Log.Info($"{GetType().Name} initializing");
#endif
                    _isInitialized = InitializeImpl();
                    if (!IsInitialized) return Result(false, true);
                }
                catch (Exception e)
                {
                    _isInitialized = false;
                    IsError = true;
                    Log.Error($"{GetType().Name}.{nameof(InitializeImpl)} failed", e);
                    return Result(false);
                }
            }
            try
            {
#if DEV
                Log.Info($"{GetType().Name} enabling");
#endif
                _isEnabled = EnableImpl();
                return Result(IsEnabled, true);
            }
            catch (Exception e)
            {
                _isEnabled = false;
                IsError = true;
                Log.Error($"{GetType().Name}.{nameof(EnableImpl)} failed", e);
                return Result(false);
            }
        }
        public FeatureFlags Disable() => Disable(false);
        public FeatureFlags Disable(bool force)
        {
            if (IsDisposed || IsError || !IsAvailable || !IsInitialized) return Result(true);
            if (!IsEnabled && !force) return Result(true);
            try
            {
#if DEV
                Log.Info($"{GetType().Name} disabling");
#endif
                _isEnabled = !DisableImpl();
                return Result(!IsEnabled, true);
            }
            catch (Exception e)
            {
                _isEnabled = true;
                IsError = true;
                Log.Error($"{GetType().Name}.{nameof(DisableImpl)} failed", e);
                return Result(false);
            }
        }

        void IToggleable.Enable() => Enable();
        void IToggleable.Disable() => Disable();
        void IForceToggleable.Enable(bool force) => Enable(force);
        void IForceToggleable.Disable(bool force) => Disable(force);
        #endregion

        #region Result helpers
        protected FeatureFlags Result(bool result) => new FeatureFlags(this, false, result);
        protected FeatureFlags Result(bool result, bool executed) => new FeatureFlags(this, executed, result);
        #endregion
    }

    internal abstract class UpdatableFeatureBase : FeatureBase, IForceUpdatable<FeatureFlags>
    {
        protected virtual bool UpdateImpl() => true;

        #region Updatable
        private bool _isCurrent;

        public virtual bool IsCurrent => _isCurrent;

        public FeatureFlags Update() => Update(false);
        public FeatureFlags Update(bool force)
        {
            if (IsDisposed || IsError || !IsAvailable || !IsInitialized || !IsEnabled) return Result(false);
            if (IsCurrent && !force) return Result(true);
            try
            {
#if DEV
                Log.Info($"{GetType().Name} updating");
#endif
                _isCurrent = UpdateImpl();
                return Result(IsCurrent, true);
            }
            catch (Exception e)
            {
                _isCurrent = false;
                IsError = true;
                Log.Error($"{GetType().Name}.{nameof(UpdateImpl)} failed", e);
                return Result(false);
            }
        }

        void IUpdatable.Update() => Update();
        void IForceUpdatable.Update(bool force) => Update(force);
        #endregion
    }
}