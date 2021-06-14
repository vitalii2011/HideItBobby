using HideItBobby.Common.Logging;
using HideItBobby.EntryPoints;
using HideItBobby.Properties;
using HideItBobby.Translation;
using HideItBobby.UserInterface;
using ICities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace HideItBobby
{
    public sealed class Mod : IUserMod
    {
        private ICollection<IDisposable> _disposables;

        public string Name => ModProperties.LongName;
        public string Description => ModProperties.Description;

        public void OnEnabled()
        {
#if DEV
            Log.Info($"mod enabled");
#endif            
            try
            {
                MainMenuEntryPoint.Features.ResetErrors();
                InGameEntryPoint.Features.ResetErrors();

                Language.Initialize();
                MainMenuEntryPoint.Enable();
                InGameEntryPoint.Initialize();
            }
            catch (Exception e)
            {
                Log.Error($"{nameof(Mod)}.{nameof(OnEnabled)} failed", e);
            }
        }

        public void OnDisabled()
        {
#if DEV
            Log.Info($"mod disabled");
#endif
            try
            {
                MainMenuEntryPoint.Terminate();
                InGameEntryPoint.Terminate();
                Language.Terminate();

                if (!(_disposables is null))
                {
                    foreach (var item in _disposables)
                        if (!(item is null)) item.Dispose();
                    _disposables.Clear();
                }
            }
            catch (Exception e)
            {
                Log.Error($"{nameof(Mod)}.{nameof(OnDisabled)} failed", e);
            }
        }

        public void OnSettingsUI(UIHelperBase helper)
        {
            try
            {
                if (!(_disposables is null))
                {
                    foreach (var item in _disposables)
                        if (!(item is null)) item.Dispose();
                    _disposables.Clear();
                }
                _disposables = SettingsUIBuilder.Build(helper, this);
            }
            catch (Exception e)
            {
                Log.Error($"{nameof(Mod)}.{nameof(OnSettingsUI)} failed", e);
            }
        }
    }
}