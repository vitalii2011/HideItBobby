using HideItBobby.Common;
using HideItBobby.Common.Logging;
using HideItBobby.Features;
using HideItBobby.Features.Decorations;
using HideItBobby.Features.Effects;
using HideItBobby.Features.Effects.Shared;
using HideItBobby.Features.Fixes;
using HideItBobby.Features.GroundAndWaterColor;
using HideItBobby.Features.Objects;
using HideItBobby.Features.Ruining;
using HideItBobby.Features.UIElements;
using HideItBobby.Properties;
using HideItBobby.Settings;
using ICities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using IFeaturesContainer = System.Collections.Generic.IDictionary<HideItBobby.Features.FeatureKey, HideItBobby.Features.IFeature>;
using ISettingsContainer = System.Collections.Generic.IDictionary<HideItBobby.Features.FeatureKey, System.Func<bool>>;

namespace HideItBobby.EntryPoints
{
    public sealed class InGameEntryPoint : LoadingExtensionBase
    {
        #region Features
        private static readonly Lazy<IFeaturesContainer> _features = new Lazy<IFeaturesContainer>(() => new Dictionary<FeatureKey, IFeature>()
            //Decorations
            .Register<HideCliffDecorations>()
            .Register<HideFertileDecorations>()
            .Register<HideGrassDecorations>()
            //Effects
            .Register<HideOreAreaEffect>()
            .Register<HideOilAreaEffect>()
            .Register<HideSandAreaEffect>()
            .Register<HideFertilityAreaEffect>()
            .Register<HideForestAreaEffect>()
            .Register<HideShoreAreaEffect>()
            .Register<HidePollutedAreaEffect>()
            .Register<HideBurnedAreaEffect>()
            .Register<HideDestroyedAreaEffect>()
            .Register<HideDistanceFog>()
            .Register<HideEdgeFog>()
            .Register<HidePollutionFog>()
            .Register<HideVolumeFog>()
            //GroundAndWaterColor
            .Register<DisableDirtyWaterColor>()
            .Register<DisableGrassFertilityGroundColor>()
            .Register<DisableGrassFieldGroundColor>()
            .Register<DisableGrassForestGroundColor>()
            .Register<DisableGrassPollutionGroundColor>()
            //Objects
            .Register<HideSeagulls>()
            .Register<HideWildlife>()
            //Props
            //todo
            //Ruining
            .Register<HideTreeRuining>()
            .Register<HidePropRuining>()
            //UIElements
            .Register<HideAdvisorButton>()
            .Register<HideBulldozerButton>()
            .Register<HideChirperButton>()
            .Register<HideCinematicCameraButton>()
            .Register<HideCityName>()
            .Register<HideDisastersButton>()
            .Register<HideFreeCameraButton>()
            .Register<HideGearButton>()
            .Register<HideInfoViewsButton>()
            .Register<HideRadioButton>()
            .Register<HideSeparators>()
            .Register<HideTimePanel>()
            .Register<HideUnlockButton>()
            .Register<HideZoomAndUnlockBackground>()
            .Register<HideZoomButton>()
            .Register<HideCongratulationPanel>()
            .Register<HideAdvisorPanel>()
            .Register<HidePauseOutline>()
            .Register<HideBulldozerBar>()
            .Register<HideThermometer>()
            .Register<ToolbarPosition>()
            //Fixes
            .Register<LowerInfoPanelZOrder>()
            );
        internal static IFeaturesContainer Features => _features.Value;
        #endregion
        #region Settings
        private static readonly Lazy<ISettingsContainer> _settings = new Lazy<ISettingsContainer>(() => new Dictionary<FeatureKey, Func<bool>>()
            //Decorations
            .Register(FeatureKey.HideCliffDecorations, () => ModSettings.Data.HideCliffDecorations)
            .Register(FeatureKey.HideFertileDecorations, () => ModSettings.Data.HideFertileDecorations)
            .Register(FeatureKey.HideGrassDecorations, () => ModSettings.Data.HideGrassDecorations)
            //Effects
            .Register(FeatureKey.HideOreAreaEffect, () => ModSettings.Data.HideOreArea)
            .Register(FeatureKey.HideOilAreaEffect, () => ModSettings.Data.HideOilArea)
            .Register(FeatureKey.HideSandAreaEffect, () => ModSettings.Data.HideSandArea)
            .Register(FeatureKey.HideFertilityAreaEffect, () => ModSettings.Data.HideFertilityArea)
            .Register(FeatureKey.HideForestAreaEffect, () => ModSettings.Data.HideForestArea)
            .Register(FeatureKey.HideShoreAreaEffect, () => ModSettings.Data.HideShoreArea)
            .Register(FeatureKey.HidePollutedAreaEffect, () => ModSettings.Data.HidePollutedArea)
            .Register(FeatureKey.HideBurnedAreaEffect, () => ModSettings.Data.HideBurnedArea)
            .Register(FeatureKey.HideDestroyedAreaEffect, () => ModSettings.Data.HideDestroyedArea)
            .Register(FeatureKey.HideDistanceFog, () => ModSettings.Data.HideDistanceFog)
            .Register(FeatureKey.HideEdgeFog, () => ModSettings.Data.HideEdgeFog)
            .Register(FeatureKey.HidePollutionFog, () => ModSettings.Data.HidePollutionFog)
            .Register(FeatureKey.HideVolumeFog, () => ModSettings.Data.HideVolumeFog)
            //GroundAndWaterColor
            .Register(FeatureKey.DisableDirtyWaterColor, () => ModSettings.Data.DisableDirtyWaterColor)
            .Register(FeatureKey.DisableGrassFertilityGroundColor, () => ModSettings.Data.DisableGrassFertilityGroundColor)
            .Register(FeatureKey.DisableGrassFieldGroundColor, () => ModSettings.Data.DisableGrassFieldGroundColor)
            .Register(FeatureKey.DisableGrassForestGroundColor, () => ModSettings.Data.DisableGrassForestGroundColor)
            .Register(FeatureKey.DisableGrassPollutionGroundColor, () => ModSettings.Data.DisableGrassPollutionGroundColor)
            //Objects
            .Register(FeatureKey.HideSeagulls, () => ModSettings.Data.HideSeagulls)
            .Register(FeatureKey.HideWildlife, () => ModSettings.Data.HideWildlife)
            //Props
            //todo
            //Ruining
            .Register(FeatureKey.HideTreeRuining, () => ModSettings.Data.HideTreeRuining)
            .Register(FeatureKey.HidePropRuining, () => ModSettings.Data.HidePropRuining)
            //UIElements
            .Register(FeatureKey.HideAdvisorButton, () => ModSettings.Data.HideAdvisorButton)
            .Register(FeatureKey.HideBulldozerButton, () => ModSettings.Data.HideBulldozerButton)
            .Register(FeatureKey.HideChirperButton, () => ModSettings.Data.HideChirperButton)
            .Register(FeatureKey.HideCinematicCameraButton, () => ModSettings.Data.HideCinematicCameraButton)
            .Register(FeatureKey.HideCityName, () => ModSettings.Data.HideCityName)
            .Register(FeatureKey.HideDisastersButton, () => ModSettings.Data.HideDisastersButton)
            .Register(FeatureKey.HideFreeCameraButton, () => ModSettings.Data.HideFreeCameraButton)
            .Register(FeatureKey.HideGearButton, () => ModSettings.Data.HideGearButton)
            .Register(FeatureKey.HideInfoViewsButton, () => ModSettings.Data.HideInfoViewsButton)
            .Register(FeatureKey.HideRadioButton, () => ModSettings.Data.HideRadioButton)
            .Register(FeatureKey.HideSeparators, () => ModSettings.Data.HideSeparators)
            .Register(FeatureKey.HideTimePanel, () => ModSettings.Data.HideTimePanel)
            .Register(FeatureKey.HideUnlockButton, () => ModSettings.Data.HideUnlockButton)
            .Register(FeatureKey.HideZoomAndUnlockBackground, () => ModSettings.Data.HideZoomAndUnlockBackground)
            .Register(FeatureKey.HideZoomButton, () => ModSettings.Data.HideZoomButton)
            .Register(FeatureKey.HideCongratulationPanel, () => ModSettings.Data.HideCongratulationPanel)
            .Register(FeatureKey.HideAdvisorPanel, () => ModSettings.Data.HideAdvisorPanel)
            .Register(FeatureKey.HidePauseOutline, () => ModSettings.Data.HidePauseOutline)
            .Register(FeatureKey.HideBulldozerBar, () => ModSettings.Data.HideBulldozerBar)
            .Register(FeatureKey.HideThermometer, () => ModSettings.Data.HideThermometer)
            .Register(FeatureKey.ToolbarPosition, () => ModSettings.Data.ModifyToolbarPosition)
            //Fixes
            .Register(FeatureKey.LowerInfoPanelZOrder, () => true)
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
            Features.InitializeAll();
            IsInitialized = true;
        }
        public static void Terminate()
        {
            if (!IsInitialized) return;
            Disable();
#if DEV
            Log.Info($"terminating {nameof(InGameEntryPoint)}");
#endif
            Features.TerminateAll();
            IsInitialized = false;
        }

        public static void Enable()
        {
            Initialize();
#if DEV
            Log.Info($"enabling {nameof(InGameEntryPoint)}");
#endif
            InGameUpdater.Enable();
        }
        public static void Disable()
        {
#if DEV
            Log.Info($"disabling {nameof(InGameEntryPoint)}");
#endif
            InGameUpdater.Disable();
        }

        #region LoadingExtension
#if DEV
        public override void OnCreated(ILoading loading)
        {
            Log.Warning($"{nameof(InGameEntryPoint)}{nameof(OnCreated)} called");
            base.OnCreated(loading);
        }

        public override void OnReleased()
        {

            Log.Info($"{nameof(InGameEntryPoint)}{nameof(OnReleased)} called");
            base.OnReleased();
        }
#endif

        public override void OnLevelLoaded(LoadMode mode)
        {
#if DEV
            Log.Info($"{nameof(InGameEntryPoint)}{nameof(OnLevelLoaded)} called with {nameof(mode)}={Enum.GetName(typeof(LoadMode), mode)}");
#endif
            if (loadingManager.currentMode != AppMode.Game) return;
            try
            {
                Enable();
            }
            catch (Exception e)
            {
                Log.Error($"{nameof(InGameEntryPoint)}.{nameof(OnLevelLoaded)} failed", e);
            }
        }

        public override void OnLevelUnloading()
        {
#if DEV
            Log.Info($"{nameof(InGameEntryPoint)}{nameof(OnLevelUnloading)} called");
#endif
            try
            {
                Disable();
            }
            catch (Exception e)
            {
                Log.Error($"{nameof(InGameEntryPoint)}.{nameof(OnLevelUnloading)} failed", e);
            }
        }
        #endregion
    }

    #region Updater
    public sealed class InGameUpdater : MonoBehaviour
    {
        private static GameObject ParentObject;
        private static byte Counter;

        public static bool IsEnabled => !(ParentObject is null);

        public InGameUpdater() : base()
        {
            name = $"{ModProperties.ShortName}{nameof(InGameUpdater)}";
        }

        public static void Enable()
        {
#if DEV
            Log.Info($"{nameof(InGameEntryPoint)}.{nameof(InGameUpdater)}.{nameof(Enable)} enabling {nameof(InGameUpdater)}.");
#endif
            if (!IsEnabled)
            {
#if DEV
                Log.Info($"creating {nameof(InGameEntryPoint)}.{nameof(InGameUpdater)}");
#endif
                ParentObject = new GameObject($"{ModProperties.LongName} {nameof(InGameEntryPoint)}");
                ParentObject.AddComponent<InGameUpdater>();
            }
        }

        public static void Disable()
        {
#if DEV
            Log.Info($"{nameof(InGameEntryPoint)}.{nameof(InGameUpdater)}.{nameof(Disable)} disabling {nameof(InGameUpdater)}.");
#endif
            if (IsEnabled)
            {
#if DEV
                Log.Info($"destroying {nameof(InGameEntryPoint)}.{nameof(InGameUpdater)}");
#endif
                var toDestroy = ParentObject;
                ParentObject = null;
                Destroy(ParentObject);
            }
        }


        private Counter _appliedSettings = int.MaxValue;

        public void Start()
        {
            TexturesUpdater.ResetCounter();
            Update();
        }

        public void Update()
        {
            if (!IsEnabled)
            {
#if DEV
                Log.Info($"destroying {nameof(InGameEntryPoint)}.{nameof(InGameUpdater)} on update loop");
#endif
                Destroy(this);
                return;
            }
            if (Counter++ % 10 == 0)
            {
                // if settings are updated check all features and execute if needed
                if (ModSettings.Version > _appliedSettings)
                {
#if DEV
                    Log.Info($"{nameof(InGameEntryPoint)}.{nameof(InGameUpdater)} updating");
#endif
                    try
                    {
                        var success = true;
                        foreach (var feature in InGameEntryPoint.Features.Values)
                        {
                            var flags = feature.Set(InGameEntryPoint.Settings.Get(feature.Key));
#if DEV
                            if (flags.ErrorCount > 0 || !flags) Log.Info($"{nameof(InGameEntryPoint)}.{nameof(InGameUpdater)} {feature.Key} flags: {flags}");
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

                        TexturesUpdater.Update(ModSettings.Version);
                    }
                    catch (Exception e)
                    {
                        Log.Error($"{nameof(InGameEntryPoint)}.{nameof(InGameUpdater)}.{nameof(Update)} failed", e);
                    }
                }
                else
                {
                    // if settings are not updated check updatable features if needs running
                    foreach (var feature in InGameEntryPoint.Features.Values.Where(f => (f is IUpdatable)).Cast<IUpdatable>())
                    {
                        if (!feature.IsCurrent)
                        {
#if DEV
                            Log.Info($"{nameof(InGameEntryPoint)}.{nameof(InGameUpdater)} updating feature {(feature as IFeature)?.Key}");
#endif
                            feature.Update();
                        }
                    }
                }
            }
            InfoViewUpdater.Update();
        }

        public void OnDestroy()
        {

            if (!(ParentObject is null))
            {
#if DEV
                Log.Info($"destroying {nameof(InGameEntryPoint)}.{nameof(InGameUpdater)}");
#endif
                var toDestroy = ParentObject;
                ParentObject = null;
                Destroy(ParentObject);
            }
        }
    }
    #endregion
}