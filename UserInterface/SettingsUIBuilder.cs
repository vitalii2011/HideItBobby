using ColossalFramework.UI;
using HideItBobby.Common.Logging;
using HideItBobby.EntryPoints;
using HideItBobby.Features;
using HideItBobby.Features.Ruining.Compatibility;
using HideItBobby.Features.UIElements.Compatibility;
using HideItBobby.Settings;
using HideItBobby.Translation;
using ICities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static HideItBobby.Properties.ModProperties;
using static HideItBobby.Translation.Phrase;
using static HideItBobby.UserInterface.Palette;

namespace HideItBobby.UserInterface
{
    internal static class SettingsUIBuilder
    {
        public static ICollection<IDisposable> Build(UIHelperBase helper, Mod mod)
        {
            try
            {
                var disposables = new List<IDisposable>();

                #region Dev tools
#if DEV
                var devTools = helper.AddGroup(DevToolsHeader, textScale: 2, textColor: HoneyYellow);
                disposables.Add(devTools);

                devTools.AddSpace(3);
                disposables.Add(devTools.AddLabel(DevToolsDescriptionLine1));
                disposables.Add(devTools.AddLabel(DevToolsDescriptionLine2, textColor: RedRYB));
                devTools.AddSpace(3);

                disposables.Add(devTools.AddButton(new Template(DevToolsEnable, ShortName), button =>
                 {
                     try
                     {
                         mod.OnEnabled();
                     }
                     catch (Exception e)
                     {
                         Log.Error($"{nameof(SettingsUIBuilder)} eventCallback failed", e);
                     }
                 }));
                disposables.Add(devTools.AddButton(new Template(DevToolsDisable, ShortName), button =>
                 {
                     try
                     {
                         mod.OnDisabled();
                     }
                     catch (Exception e)
                     {
                         Log.Error($"{nameof(SettingsUIBuilder)} eventCallback failed", e);
                     }
                 }));
                devTools.AddSpace(6);
                disposables.Add(devTools.AddButton(new Template(DevToolsInitialize, nameof(MainMenuEntryPoint)), button =>
                 {
                     try
                     {
                         MainMenuEntryPoint.Initialize();
                     }
                     catch (Exception e)
                     {
                         Log.Error($"{nameof(SettingsUIBuilder)} eventCallback failed", e);
                     }
                 }));
                disposables.Add(devTools.AddButton(new Template(DevToolsEnable, nameof(MainMenuEntryPoint)), button =>
                 {
                     try
                     {
                         MainMenuEntryPoint.Enable();
                     }
                     catch (Exception e)
                     {
                         Log.Error($"{nameof(SettingsUIBuilder)} eventCallback failed", e);
                     }
                 }));
                disposables.Add(devTools.AddButton(new Template(DevToolsTerminate, nameof(MainMenuEntryPoint)), button =>
                 {
                     try
                     {
                         MainMenuEntryPoint.Terminate();
                     }
                     catch (Exception e)
                     {
                         Log.Error($"{nameof(SettingsUIBuilder)} eventCallback failed", e);
                     }
                 }));
                devTools.AddSpace(6);
                disposables.Add(devTools.AddButton(new Template(DevToolsInitialize, nameof(InGameEntryPoint)), button =>
                 {
                     try
                     {
                         InGameEntryPoint.Initialize();
                     }
                     catch (Exception e)
                     {
                         Log.Error($"{nameof(SettingsUIBuilder)} eventCallback failed", e);
                     }
                 }));
                disposables.Add(devTools.AddButton(new Template(DevToolsEnable, nameof(InGameEntryPoint)), button =>
                 {
                     try
                     {
                         InGameEntryPoint.Enable();
                     }
                     catch (Exception e)
                     {
                         Log.Error($"{nameof(SettingsUIBuilder)} eventCallback failed", e);
                     }
                 }));
                disposables.Add(devTools.AddButton(new Template(DevToolsTerminate, nameof(InGameEntryPoint)), button =>
                 {
                     try
                     {
                         InGameEntryPoint.Terminate();
                     }
                     catch (Exception e)
                     {
                         Log.Error($"{nameof(SettingsUIBuilder)} eventCallback failed", e);
                     }
                 }));
                devTools.AddSpace(6);
                disposables.Add(devTools.AddButton(DevToolsReloadSettings, button =>
                {
                    try
                    {
                        ModSettings.Load();
                        ModSettings.Version.Update();
                    }
                    catch (Exception e)
                    {
                        Log.Error($"{nameof(SettingsUIBuilder)} eventCallback failed", e);
                    }
                }));
                disposables.Add(devTools.AddButton(DevToolsApplySettings, button =>
                {
                    try
                    {
                        ModSettings.Version.Update();
                    }
                    catch (Exception e)
                    {
                        Log.Error($"{nameof(SettingsUIBuilder)} eventCallback failed", e);
                    }
                }));
                devTools.AddSpace(6);
                disposables.Add(devTools.AddButton(DevToolsOverwriteLanguageFiles, button =>
                {
                    try
                    {
                        var dePath = Path.Combine(Paths.TranslationsDir, "hide_it_bobby.de.xml");
                        if (File.Exists(dePath)) File.Delete(dePath);
                        File.WriteAllText(dePath, Properties.Resources.hide_it_bobby_de, System.Text.Encoding.UTF8);

                        var enPath = Path.Combine(Paths.TranslationsDir, "hide_it_bobby.en.xml");
                        if (File.Exists(enPath)) File.Delete(enPath);
                        File.WriteAllText(enPath, Properties.Resources.hide_it_bobby_en, System.Text.Encoding.UTF8);

                        var jaPath = Path.Combine(Paths.TranslationsDir, "hide_it_bobby.ja.xml");
                        if (File.Exists(jaPath)) File.Delete(jaPath);
                        File.WriteAllText(jaPath, Properties.Resources.hide_it_bobby_ja, System.Text.Encoding.UTF8);

                        var plPath = Path.Combine(Paths.TranslationsDir, "hide_it_bobby.pl.xml");
                        if (File.Exists(plPath)) File.Delete(plPath);
                        File.WriteAllText(plPath, Properties.Resources.hide_it_bobby_pl, System.Text.Encoding.UTF8);

                        var zhPath = Path.Combine(Paths.TranslationsDir, "hide_it_bobby.zh.xml");
                        if (File.Exists(zhPath)) File.Delete(zhPath);
                        File.WriteAllText(zhPath, Properties.Resources.hide_it_bobby_zh, System.Text.Encoding.UTF8);
                    }
                    catch (Exception e)
                    {
                        Log.Error($"{nameof(SettingsUIBuilder)} eventCallback failed", e);
                    }
                }));
#endif
                #endregion

                #region Language
                if (Language.Library.Count > 1)
                {
                    var langList = Language.Library.Values
                        .Select(x => x.TryGetValue(LanguageName, out var name) ? name : null)
                        .Where(x => !(x is null))
                        .ToList();
                    if (!langList.Contains("English")) langList.Add("English");
                    langList.Sort();

                    var selectedLang = Language.KeyToName(ModSettings.Data.UseGameLanguage ? Language.GameLanguage : ModSettings.Data.SelectedLanguage) ?? "English";
                    var selectedLangIx = langList.IndexOf(selectedLang);

                    var language = helper.AddGroup(LanguageHeader, textScale: 2, textColor: White);
                    disposables.Add(language);
                    var langDropdown = language.AddDropdown(
                        SelectLanguage,
                        langList.ToArray(),
                        selectedLangIx,
                        (component, selection) =>
                        {
                            try
                            {
                                if (ModSettings.Data.UseGameLanguage) return;
                                var langKey = Language.NameToKey(langList[selection]);
                                ModSettings.Data.SelectedLanguage = langKey;
                                ModSettings.Save();
                                Language.ChangeTo(langKey);
                            }
                            catch (Exception e)
                            {
                                Log.Error($"{nameof(SettingsUIBuilder)} eventCallback failed", e);
                            }
                        },
                        textColor: ModSettings.Data.UseGameLanguage ? DarkGray : White,
                        color: ModSettings.Data.UseGameLanguage ? DarkGray : White,
                        enabled: !ModSettings.Data.UseGameLanguage
                        );
                    disposables.Add(langDropdown);
                    var langDropdownLabel = langDropdown.Parent.Find<UILabel>("Label");
                    disposables.Add(AddFeatureCheckbox(language, UseGameLanguage, ModSettings.Data.UseGameLanguage, value =>
                    {
                        ModSettings.Data.UseGameLanguage = value;
                        langDropdown.Component.items = langList.ToArray();
                        langDropdown.Component.selectedIndex = langList.IndexOf(Language.KeyToName(ModSettings.Data.UseGameLanguage ? Language.GameLanguage : ModSettings.Data.SelectedLanguage) ?? "English");

                        if (value)
                        {
                            Language.ChangeToGameLanguage();
                            langDropdown.Component.textColor = DarkGray;
                            langDropdown.Component.color = DarkGray;
                            langDropdownLabel.textColor = DarkGray;
                        }
                        else
                        {
                            langDropdown.Component.textColor = White;
                            langDropdown.Component.color = White;
                            langDropdownLabel.textColor = White;
                        }
                        langDropdown.Component.isInteractive = !value;
                    }));
                }
                #endregion

                var availableFeatures = helper.AddGroup(AvailableFeaturesHeader, textScale: 2, textColor: White);
                disposables.Add(availableFeatures);
                var unAvailableFeatures = helper.AddGroup(UnavailableFeaturesHeader, textScale: 2, textColor: White);
                disposables.Add(unAvailableFeatures);
                disposables.Add(unAvailableFeatures.AddLabel(UnavailableFeaturesDescription, textColor: HoneyYellow));

                #region MainMenu
                disposables.Add(AddGroupHeader(availableFeatures, MainMenuGroup));
                disposables.Add(AddFeatureCheckbox(availableFeatures, MainMenuChirper, ModSettings.Data.HideMainMenuChirper, value => ModSettings.Data.HideMainMenuChirper = value));
                disposables.Add(AddFeatureCheckbox(availableFeatures, MainMenuDLCPanel, ModSettings.Data.HideMainMenuDLCPanel, value => ModSettings.Data.HideMainMenuDLCPanel = value));
                disposables.Add(AddFeatureCheckbox(availableFeatures, MainMenuLogoImage, ModSettings.Data.HideMainMenuLogo, value => ModSettings.Data.HideMainMenuLogo = value));
                disposables.Add(AddFeatureCheckbox(availableFeatures, MainMenuNewsPanel, ModSettings.Data.HideMainMenuNewsPanel, value => ModSettings.Data.HideMainMenuNewsPanel = value));
                disposables.Add(AddFeatureCheckbox(availableFeatures, MainMenuParadoxAccountPanel, ModSettings.Data.HideMainMenuParadoxAccountPanel, value => ModSettings.Data.HideMainMenuParadoxAccountPanel = value));
                disposables.Add(AddFeatureCheckbox(availableFeatures, MainMenuVersionNumber, ModSettings.Data.HideMainMenuVersionNumber, value => ModSettings.Data.HideMainMenuVersionNumber = value));
                disposables.Add(AddFeatureCheckbox(availableFeatures, MainMenuWorkshopPanel, ModSettings.Data.HideMainMenuWorkshopPanel, value => ModSettings.Data.HideMainMenuWorkshopPanel = value));
                #endregion

                #region UIElements
                disposables.Add(AddGroupHeader(availableFeatures, InGameUIGroup));
                disposables.Add(AddFeatureCheckbox(availableFeatures, InfoViewsButton, ModSettings.Data.HideInfoViewsButton, value => ModSettings.Data.HideInfoViewsButton = value));
                if (HideDisastersButtonCompatibility.Instance.IsCompatible)
                {
                    disposables.Add(AddFeatureCheckbox(availableFeatures, DisastersButton, ModSettings.Data.HideDisastersButton, value => ModSettings.Data.HideDisastersButton = value));
                }
                disposables.Add(AddFeatureCheckbox(availableFeatures, ChirperButton, ModSettings.Data.HideChirperButton, value => ModSettings.Data.HideChirperButton = value));
                disposables.Add(AddFeatureCheckbox(availableFeatures, RadioButton, ModSettings.Data.HideRadioButton, value => ModSettings.Data.HideRadioButton = value));
                disposables.Add(AddFeatureCheckbox(availableFeatures, GearButton, ModSettings.Data.HideGearButton, value => ModSettings.Data.HideGearButton = value));
                disposables.Add(AddFeatureCheckbox(availableFeatures, ZoomButton, ModSettings.Data.HideZoomButton, value => ModSettings.Data.HideZoomButton = value));
                disposables.Add(AddFeatureCheckbox(availableFeatures, UnlockButton, ModSettings.Data.HideUnlockButton, value => ModSettings.Data.HideUnlockButton = value));
                disposables.Add(AddFeatureCheckbox(availableFeatures, AdvisorPanel, ModSettings.Data.HideAdvisorPanel, value => ModSettings.Data.HideAdvisorPanel = value));
                disposables.Add(AddFeatureCheckbox(availableFeatures, AdvisorButton, ModSettings.Data.HideAdvisorButton, value => ModSettings.Data.HideAdvisorButton = value));
                disposables.Add(AddFeatureCheckbox(availableFeatures, BulldozerBar, ModSettings.Data.HideBulldozerBar, value => ModSettings.Data.HideBulldozerBar = value));
                disposables.Add(AddFeatureCheckbox(availableFeatures, BulldozerButton, ModSettings.Data.HideBulldozerButton, value => ModSettings.Data.HideBulldozerButton = value));
                disposables.Add(AddFeatureCheckbox(availableFeatures, CinematicCameraButton, ModSettings.Data.HideCinematicCameraButton, value => ModSettings.Data.HideCinematicCameraButton = value));
                disposables.Add(AddFeatureCheckbox(availableFeatures, FreeCameraButton, ModSettings.Data.HideFreeCameraButton, value => ModSettings.Data.HideFreeCameraButton = value));
                disposables.Add(AddFeatureCheckbox(availableFeatures, CongratulationPanel, ModSettings.Data.HideCongratulationPanel, value => ModSettings.Data.HideCongratulationPanel = value));
                disposables.Add(AddFeatureCheckbox(availableFeatures, TimePanel, ModSettings.Data.HideTimePanel, value => ModSettings.Data.HideTimePanel = value));
                disposables.Add(AddFeatureCheckbox(availableFeatures, ZoomAndUnlockBackground, ModSettings.Data.HideZoomAndUnlockBackground, value => ModSettings.Data.HideZoomAndUnlockBackground = value));
                disposables.Add(AddFeatureCheckbox(availableFeatures, Separators, ModSettings.Data.HideSeparators, value => ModSettings.Data.HideSeparators = value));
                disposables.Add(AddFeatureCheckbox(availableFeatures, CityName, ModSettings.Data.HideCityName, value => ModSettings.Data.HideCityName = value));
                disposables.Add(AddFeatureCheckbox(availableFeatures, PauseOutlineEffect, ModSettings.Data.HidePauseOutline, value => ModSettings.Data.HidePauseOutline = value));
                if (HideThermometerCompatibility.Instance.IsCompatible)
                {
                    disposables.Add(AddFeatureCheckbox(availableFeatures, Thermometer, ModSettings.Data.HideThermometer, value => ModSettings.Data.HideThermometer = value));
                }
                UISlider toolbarPositionSlider = null;
                UILabel toolbarPositionLabel = null;
                disposables.Add(AddFeatureCheckbox(availableFeatures, ModifyToolbarPosition, ModSettings.Data.ModifyToolbarPosition, value =>
                {
                    ModSettings.Data.ModifyToolbarPosition = value;

                    if (!value)
                    {
                        if (!(toolbarPositionSlider is null)) toolbarPositionSlider.color = DarkGray;
                        if (!(toolbarPositionLabel is null)) toolbarPositionLabel.textColor = DarkGray;
                    }
                    else
                    {
                        if (!(toolbarPositionSlider is null)) toolbarPositionSlider.color = White;
                        if (!(toolbarPositionLabel is null)) toolbarPositionLabel.textColor = White;
                    }
                    if (!(toolbarPositionSlider is null)) toolbarPositionSlider.isEnabled = value;
                }));
                var toolbarPosition = availableFeatures.AddSlider(
                    null,
                    0.0f,
                    1.0f,
                    0.01f,
                    ModSettings.Data.ToolbarPosition,
                    (_, newValue) =>
                    {
                        ModSettings.Data.ToolbarPosition = newValue;
                        ModSettings.Save();
                        ModSettings.Version.Update();
                    },
                    width: 700f,
                    textColor: ModSettings.Data.ModifyToolbarPosition ? White : Gray,
                    enabled: ModSettings.Data.ModifyToolbarPosition);
                toolbarPositionSlider = toolbarPosition.Component;
                toolbarPositionLabel = toolbarPosition.Parent.Find<UILabel>("Label");
                disposables.Add(toolbarPosition);

                #endregion

                #region Objects
                disposables.Add(AddGroupHeader(availableFeatures, ObjectsAndPropsGroup));
                disposables.Add(AddFeatureCheckbox(availableFeatures, Seagulls, ModSettings.Data.HideSeagulls, value => ModSettings.Data.HideSeagulls = value));
                disposables.Add(AddFeatureCheckbox(availableFeatures, Wildlife, ModSettings.Data.HideWildlife, value => ModSettings.Data.HideWildlife = value));
                #endregion

                #region Decorations
                disposables.Add(AddGroupHeader(availableFeatures, DecorationsGroup));
                disposables.Add(AddFeatureCheckbox(availableFeatures, CliffDecorations, ModSettings.Data.HideCliffDecorations, value => ModSettings.Data.HideCliffDecorations = value));
                disposables.Add(AddFeatureCheckbox(availableFeatures, GrassDecorations, ModSettings.Data.HideGrassDecorations, value => ModSettings.Data.HideGrassDecorations = value));
                disposables.Add(AddFeatureCheckbox(availableFeatures, FertileDecorations, ModSettings.Data.HideFertileDecorations, value => ModSettings.Data.HideFertileDecorations = value));
                #endregion

                #region Ruining
                var ruiningIsCompatible = HideRuiningCompatibility.Instance.IsCompatible;
                if (ruiningIsCompatible)
                {
                    disposables.Add(AddGroupHeader(availableFeatures, RuiningGroup));
                    disposables.Add(AddFeatureCheckbox(availableFeatures, TreeRuining, ModSettings.Data.HideTreeRuining, value => ModSettings.Data.HideTreeRuining = value));
                    disposables.Add(AddFeatureCheckbox(availableFeatures, PropRuining, ModSettings.Data.HidePropRuining, value => ModSettings.Data.HidePropRuining = value));
                    availableFeatures.AddSpace(3);
                    disposables.Add(availableFeatures.AddButton(UpdateRuiningButton, button =>
                    {
                        try
                        {
#if DEV
                            Log.Info($"updating ruining under trees");
#endif
                            FeatureFlags flags;
                            flags = InGameEntryPoint.Features.Resolve(FeatureKey.HideTreeRuining).Run();
#if DEV
                            Log.Info($"flags: {flags}");
                            Log.Info($"updating ruining under props");
#endif
                            flags = InGameEntryPoint.Features.Resolve(FeatureKey.HidePropRuining).Run();
#if DEV
                            Log.Info($"flags: {flags}");
#endif
                        }
                        catch (Exception e)
                        {
                            Log.Error($"{nameof(SettingsUIBuilder)} eventCallback failed", e);
                        }
                    }));
                }
                else
                {
                    disposables.Add(AddGroupHeader(unAvailableFeatures, RuiningGroup));
                    disposables.Add(unAvailableFeatures.AddLabel(RuiningUnavailableDescriptionLine1, textColor: White));
                    disposables.Add(unAvailableFeatures.AddLabel(RuiningUnavailableDescriptionLine2, textColor: White));
                    unAvailableFeatures.AddSpace(3);
                    disposables.Add(AddUnavailableFeatureCheckbox(unAvailableFeatures, TreeRuining, ModSettings.Data.HideTreeRuining, value => ModSettings.Data.HideTreeRuining = value));
                    disposables.Add(AddUnavailableFeatureCheckbox(unAvailableFeatures, PropRuining, ModSettings.Data.HidePropRuining, value => ModSettings.Data.HidePropRuining = value));
                }
                #endregion

                #region GroundAndWaterColor
                disposables.Add(AddGroupHeader(availableFeatures, GroundAndWaterColorGroup));
                disposables.Add(AddFeatureCheckbox(availableFeatures, GrassFertilityGroundColor, ModSettings.Data.DisableGrassFertilityGroundColor, value => ModSettings.Data.DisableGrassFertilityGroundColor = value));
                disposables.Add(AddFeatureCheckbox(availableFeatures, GrassFieldGroundColor, ModSettings.Data.DisableGrassFieldGroundColor, value => ModSettings.Data.DisableGrassFieldGroundColor = value));
                disposables.Add(AddFeatureCheckbox(availableFeatures, GrassForestGroundColor, ModSettings.Data.DisableGrassForestGroundColor, value => ModSettings.Data.DisableGrassForestGroundColor = value));
                disposables.Add(AddFeatureCheckbox(availableFeatures, GrassPollutionGroundColor, ModSettings.Data.DisableGrassPollutionGroundColor, value => ModSettings.Data.DisableGrassPollutionGroundColor = value));
                disposables.Add(AddFeatureCheckbox(availableFeatures, DirtyWaterColor, ModSettings.Data.DisableDirtyWaterColor, value => ModSettings.Data.DisableDirtyWaterColor = value));
                #endregion

                #region Effects
                disposables.Add(AddGroupHeader(availableFeatures, EffectsGroup));
                disposables.Add(AddFeatureCheckbox(availableFeatures, OreAreaEffect, ModSettings.Data.HideOreArea, value => ModSettings.Data.HideOreArea = value));
                disposables.Add(AddFeatureCheckbox(availableFeatures, OilAreaEffect, ModSettings.Data.HideOilArea, value => ModSettings.Data.HideOilArea = value));
                disposables.Add(AddFeatureCheckbox(availableFeatures, SandAreaEffect, ModSettings.Data.HideSandArea, value => ModSettings.Data.HideSandArea = value));
                disposables.Add(AddFeatureCheckbox(availableFeatures, FertilityAreaEffect, ModSettings.Data.HideFertilityArea, value => ModSettings.Data.HideFertilityArea = value));
                disposables.Add(AddFeatureCheckbox(availableFeatures, ForestAreaEffect, ModSettings.Data.HideForestArea, value => ModSettings.Data.HideForestArea = value));
                disposables.Add(AddFeatureCheckbox(availableFeatures, ShoreAreaEffect, ModSettings.Data.HideShoreArea, value => ModSettings.Data.HideShoreArea = value));
                disposables.Add(AddFeatureCheckbox(availableFeatures, PollutedAreaEffect, ModSettings.Data.HidePollutedArea, value => ModSettings.Data.HidePollutedArea = value));
                disposables.Add(AddFeatureCheckbox(availableFeatures, BurnedAreaEffect, ModSettings.Data.HideBurnedArea, value => ModSettings.Data.HideBurnedArea = value));
                disposables.Add(AddFeatureCheckbox(availableFeatures, DestroyedAreaEffect, ModSettings.Data.HideDestroyedArea, value => ModSettings.Data.HideDestroyedArea = value));
                disposables.Add(AddFeatureCheckbox(availableFeatures, PollutionFog, ModSettings.Data.HidePollutionFog, value => ModSettings.Data.HidePollutionFog = value));
                disposables.Add(AddFeatureCheckbox(availableFeatures, VolumeFog, ModSettings.Data.HideVolumeFog, value => ModSettings.Data.HideVolumeFog = value));
                disposables.Add(AddFeatureCheckbox(availableFeatures, DistanceFog, ModSettings.Data.HideDistanceFog, value => ModSettings.Data.HideDistanceFog = value));
                disposables.Add(AddFeatureCheckbox(availableFeatures, EdgeFog, ModSettings.Data.HideEdgeFog, value => ModSettings.Data.HideEdgeFog = value));
                #endregion

                return disposables;
            }
            catch (Exception e)
            {
                Log.Error($"{nameof(SettingsUIBuilder)}.{nameof(Build)} failed", e);
                return null;
            }
        }

        #region UI elements helpers
        private static TranslatedComponent<UILabel> AddGroupHeader(TranslatedGroup group, Template textTemplate)
        {
            group.AddSpace(12);
            var label = group.AddLabel(textTemplate, textColor: White);
            group.AddSpace(3);
            return label;
        }
        private static TranslatedComponent<UICheckBox> AddFeatureCheckbox(TranslatedGroup group, Template textTemplate, bool value, Action<bool> valueSetter)
        {
            return group.AddCheckbox(textTemplate, value, (checkBox, newValue) =>
            {
                try
                {
                    valueSetter(newValue);
                    ModSettings.Save();
                    ModSettings.Version.Update();
                    checkBox.label.textColor = newValue ? White : Gray;
                }
                catch (Exception e)
                {
                    Log.Error($"{nameof(SettingsUIBuilder)} eventCallback failed", e);
                }
            }, textColor: value ? White : Gray);
        }
        private static TranslatedComponent<UICheckBox> AddUnavailableFeatureCheckbox(TranslatedGroup group, Template textTemplate, bool value, Action<bool> valueSetter)
        {
            return group.AddCheckbox(textTemplate, value, (checkBox, newValue) =>
            {
                try
                {
                    valueSetter(newValue);
                    ModSettings.Save();
                }
                catch (Exception e)
                {
                    Log.Error($"{nameof(SettingsUIBuilder)} eventCallback failed", e);
                }
            }, textColor: Gray);
        }
        #endregion
    }
}