using ColossalFramework.UI;
using HideItBobby.Common.Logging;
using HideItBobby.Translation;
using System;
using UnityEngine;

namespace HideItBobby.UserInterface
{
    internal interface ITranslatedComponent
    {
        UIComponent Component { get; }
    }

    internal class TranslatedComponent<TComponent> : ITranslatedComponent, IDisposable
        where TComponent : UIComponent
    {
        private readonly Action<TComponent, string> _onLanguageChangeAction;

        public TComponent Component { get; }
        UIComponent ITranslatedComponent.Component => Component;

        public TranslatedComponent(TComponent component, Action<TComponent, string> onLanguageChange)
        {
            Component = component;
            _onLanguageChangeAction = onLanguageChange;
            Language.LanugageChanged += OnLanguageChanged;
        }

        protected virtual void OnLanguageChanged(string key)
        {
            if (_isDisposed) return;
            try
            {
                if(!(_onLanguageChangeAction is null)) _onLanguageChangeAction(Component, key);
            }
            catch (Exception e)
            {
                Log.Error($"{nameof(TranslatedComponent<TComponent>)}.{nameof(OnLanguageChanged)} failed", e);
            }
        }

        #region IDisposable
        private bool _isDisposed;
        public bool IsDisposed => _isDisposed;

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            if (disposing)
            {
                // dispose managed state (managed objects)
                Language.LanugageChanged -= OnLanguageChanged;
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
    }

    internal sealed class TranslatedComponent<TParent, TComponent> : TranslatedComponent<TComponent>
        where TComponent : UIComponent
        where TParent : UIComponent
    {
        private readonly Action<TParent, TComponent, string> _onLanguageChangeAction;

        public TranslatedComponent(TParent parent, TComponent component, Action<TParent, TComponent, string> onLanguageChange)
            : base(component, null)
        {
            Parent = parent;
            _onLanguageChangeAction = onLanguageChange;
        }

        public TParent Parent { get; }

        protected override void OnLanguageChanged(string key)
        {
            if (IsDisposed) return;
            try
            {
                if (!(_onLanguageChangeAction is null)) _onLanguageChangeAction(Parent, Component, key);
            }
            catch (Exception e)
            {
                Log.Error($"{nameof(TranslatedComponent<TParent, TComponent>)}.{nameof(OnLanguageChanged)} failed", e);
            }
        }
    }

    internal sealed class TranslatedGroup : TranslatedComponent<UIPanel>
    {
        public TranslatedGroup(UIHelper uiHelper, UIPanel uiPanel, Action<UIPanel, string> onLanguageChange)
            : base(uiPanel, onLanguageChange)
        {
            UIHelper = uiHelper;
        }

        public UIHelper UIHelper { get; }
    }
}
