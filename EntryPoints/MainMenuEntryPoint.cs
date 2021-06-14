using HideItBobby.Common;
using HideItBobby.Common.Logging;
using HideItBobby.Features;
using HideItBobby.Features.Menu;
using HideItBobby.Properties;
using HideItBobby.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using static HideItBobby.Common.Patcher;
using IFeaturesContainer = System.Collections.Generic.IDictionary<HideItBobby.Features.FeatureKey, HideItBobby.Features.IFeature>;
using ISettingsContainer = System.Collections.Generic.IDictionary<HideItBobby.Features.FeatureKey, System.Func<bool>>;

namespace HideItBobby.EntryPoints
{
    public static class MainMenuEntryPoint
    {
        #region Features
        private static readonly Lazy<IFeaturesContainer> _features = new Lazy<IFeaturesContainer>(() => new Dictionary<FeatureKey, IFeature>()
            .Register<HideMainMenuChirper>()
            .Register<HideMainMenuDLCPanel>()
            .Register<HideMainMenuLogo>()
            .Register<HideMainMenuNewsPanel>()
            .Register<HideMainMenuParadoxAccountPanel>()
            .Register<HideMainMenuVersionNumber>()
            .Register<HideMainMenuWorkshopPanel>()
            );
        internal static IFeaturesContainer Features => _features.Value;
        #endregion
        #region Settings
        private static readonly Lazy<ISettingsContainer> _settings = new Lazy<ISettingsContainer>(() => new Dictionary<FeatureKey, Func<bool>>()
            .Register(FeatureKey.HideMainMenuChirper, () => ModSettings.Data.HideMainMenuChirper)
            .Register(FeatureKey.HideMainMenuDLCPanel, () => ModSettings.Data.HideMainMenuDLCPanel)
            .Register(FeatureKey.HideMainMenuLogo, () => ModSettings.Data.HideMainMenuLogo)
            .Register(FeatureKey.HideMainMenuNewsPanel, () => ModSettings.Data.HideMainMenuNewsPanel)
            .Register(FeatureKey.HideMainMenuParadoxAccountPanel, () => ModSettings.Data.HideMainMenuParadoxAccountPanel)
            .Register(FeatureKey.HideMainMenuVersionNumber, () => ModSettings.Data.HideMainMenuVersionNumber)
            .Register(FeatureKey.HideMainMenuWorkshopPanel, () => ModSettings.Data.HideMainMenuWorkshopPanel)
            );
        internal static ISettingsContainer Settings => _settings.Value;
        #endregion

        public static bool IsInitialized { get; private set; }

        public static void Initialize()
        {
            if (IsInitialized) return;
#if DEV
            Log.Info($"initializing {nameof(InGameEntryPoint)}");
#endif
            Patch(MainMenuPatch.Data);
            Features.InitializeAll();
            IsInitialized = true;
        }
        public static void Terminate()
        {
            if (!IsInitialized) return;
            Disable();
#if DEV
            Log.Info($"terminating {nameof(MainMenuEntryPoint)}");
#endif
            Unpatch(MainMenuPatch.Data);
            Features.TerminateAll();
            IsInitialized = false;
        }

        public static void Enable()
        {
            Initialize();
#if DEV
            Log.Info($"enabling {nameof(MainMenuEntryPoint)}");
#endif
            MainMenuUpdater.Enable();
        }
        public static void Disable()
        {
#if DEV
            Log.Info($"disabling {nameof(MainMenuEntryPoint)}");
#endif
            MainMenuUpdater.Disable();
        }
    }

    #region Harmony patch
    internal static class MainMenuPatch
    {
        public static readonly PatchData Data = new PatchData(
            patchId: $"{nameof(MainMenuEntryPoint)}.{nameof(MainMenuPatch)}.{nameof(Postfix)}",
            target: () => typeof(MainMenu).GetMethod("Awake", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance),
            postfix: () => typeof(MainMenuPatch).GetMethod(nameof(Postfix), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static)
        );

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Postfix()
        {
            if (!Data.IsPatchApplied) return;
#if DEV
            Log.Info($"{nameof(MainMenuPatch)}.{nameof(Postfix)} enabling updater.");
#endif
            MainMenuEntryPoint.Features.ResetErrors();
            MainMenuUpdater.Enable();
        }
    }
    #endregion

    #region Updater
    public sealed class MainMenuUpdater : MonoBehaviour
    {
        private static GameObject ParentObject;
        private static byte Counter;

        public static bool IsEnabled => !(ParentObject is null);

        public MainMenuUpdater() : base()
        {
            name = $"{ModProperties.ShortName}{nameof(MainMenuUpdater)}";
        }

        public static void Enable()
        {
#if DEV
            Log.Info($"{nameof(MainMenuEntryPoint)}.{nameof(MainMenuUpdater)}.{nameof(Enable)} enabling {nameof(MainMenuUpdater)}.");
#endif
            if (!IsEnabled)
            {
#if DEV
                Log.Info($"{nameof(MainMenuEntryPoint)}.{nameof(MainMenuUpdater)} reapplying features");
#endif
                foreach (var feature in MainMenuEntryPoint.Features.Values)
                {
                    var flags = feature.Run();
#if DEV
                    if (flags.ErrorCount > 0 || !flags) Log.Info($"{nameof(MainMenuEntryPoint)}.{nameof(MainMenuUpdater)} {feature.Key} flags: {flags}");
#endif
                }
#if DEV
                Log.Info($"creating {nameof(MainMenuEntryPoint)}.{nameof(MainMenuUpdater)}");
#endif
                ParentObject = new GameObject($"{ModProperties.LongName} {nameof(MainMenuEntryPoint)}");
                ParentObject.AddComponent<MainMenuUpdater>();
            }
        }

        public static void Disable()
        {
#if DEV
            Log.Info($"{nameof(MainMenuEntryPoint)}.{nameof(MainMenuUpdater)}.{nameof(Disable)} disabling {nameof(MainMenuUpdater)}.");
#endif
            if (IsEnabled)
            {
#if DEV
                Log.Info($"destroying {nameof(MainMenuEntryPoint)}.{nameof(MainMenuUpdater)}");
#endif
                var toDestroy = ParentObject;
                ParentObject = null;
                Destroy(ParentObject);
            }
        }


        private Counter _appliedSettings = int.MaxValue;

        public void Start() => Update();

        public void Update()
        {
            if (!IsEnabled)
            {
#if DEV
                Log.Info($"destroying {nameof(MainMenuEntryPoint)}.{nameof(MainMenuUpdater)} on update loop");
#endif
                Destroy(this);
                return;
            }
            if (Counter++ % 2 == 0)
            {
                // if settings are updated check all features and execute if needed
                if (ModSettings.Version > _appliedSettings)
                {
#if DEV
                    Log.Info($"{nameof(MainMenuEntryPoint)}.{nameof(MainMenuUpdater)} updating");
#endif
                    try
                    {
                        var success = true;
                        foreach (var feature in MainMenuEntryPoint.Features.Values)
                        {
                            var flags = feature.Set(MainMenuEntryPoint.Settings.Get(feature.Key));
#if DEV
                            if (flags.ErrorCount > 0 || !flags) Log.Info($"{nameof(MainMenuEntryPoint)}.{nameof(MainMenuUpdater)} {feature.Key} flags: {flags}");
#endif
                            if (flags.IsAvailable
                                && !flags.IsError
                                && ((flags.Executed && !flags.EndResult)
                                || flags.ErrorCount > 0))
                            {
                                success = false;
                            }
                        }
                        if (success) _appliedSettings = ModSettings.Version.Clone();
                    }
                    catch (Exception e)
                    {
                        Log.Error($"{nameof(MainMenuEntryPoint)}.{nameof(MainMenuUpdater)}.{nameof(Update)} failed", e);
                    }
                }
                else
                {
                    // if settings are not updated check updatable features if needs running
                    foreach (var feature in MainMenuEntryPoint.Features.Values.Where(f => (f is IUpdatable)).Cast<IUpdatable>())
                    {
                        if (!feature.IsCurrent)
                        {
#if DEV
                            Log.Info($"{nameof(MainMenuEntryPoint)}.{nameof(MainMenuUpdater)} updating feature {(feature as IFeature)?.Key}");
#endif
                            feature.Update();
                        }
                    }
                }
            }
        }

        public void OnDestroy()
        {
            if (!(ParentObject is null))
            {
#if DEV
                Log.Info($"destroying {nameof(MainMenuEntryPoint)}.{nameof(MainMenuUpdater)}");
#endif
                var toDestroy = ParentObject;
                ParentObject = null;
                Destroy(ParentObject);
            }
        }
    }
    #endregion
}