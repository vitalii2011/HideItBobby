using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace HideItBobby
{
    public class ModManager : MonoBehaviour
    {
        private bool _initialized;

        private TerrainProperties _terrainProperties;
        private Vector3 _noColorOffset;
        private Vector3 _defaultGrassFertilityColorOffset;
        private Vector3 _defaultGrassFieldColorOffset;
        private Vector3 _defaultGrassForestColorOffset;
        private Vector3 _defaultGrassPollutionColorOffset;
        private Color _defaultWaterColorClean;
        private Color _defaultWaterColorDirty;
        private FogProperties _fogProperties;
        private float _defaultPollutionAmount;
        private float _defaultFogDensity;
        private float _defaultColorDecay;

        public void Start()
        {
            try
            {
                _terrainProperties = FindObjectOfType<TerrainProperties>();

                _noColorOffset = new Vector3(0f, 0f, 0f);
                _defaultGrassFertilityColorOffset = Shader.GetGlobalVector("_GrassFertilityColorOffset");
                _defaultGrassFieldColorOffset = Shader.GetGlobalVector("_GrassFieldColorOffset");
                _defaultGrassForestColorOffset = Shader.GetGlobalVector("_GrassForestColorOffset");
                _defaultGrassPollutionColorOffset = Shader.GetGlobalVector("_GrassPollutionColorOffset");
                _defaultWaterColorClean = Shader.GetGlobalColor("_WaterColorClean");
                _defaultWaterColorDirty = Shader.GetGlobalColor("_WaterColorDirty");

                _fogProperties = FindObjectOfType<FogProperties>();

                if (_fogProperties != null)
                {
                    _defaultPollutionAmount = _fogProperties.m_PollutionAmount;
                    _defaultFogDensity = _fogProperties.m_FogDensity;
                    _defaultColorDecay = _fogProperties.m_ColorDecay;
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Hide it, BOBby!] ModManager:Start -> Exception: " + e.Message);
            }
        }

        public void Update()
        {
            try
            {
                if (!_initialized || ModConfig.Instance.ConfigUpdated)
                {
                    ToggleSingleUIComponent("InfoMenu", ModConfig.Instance.InfoViewsButton);
                    if (SteamHelper.IsDLCOwned(SteamHelper.DLC.NaturalDisastersDLC))
                    {
                        ToggleSingleUIComponent("WarningPhasePanel", ModConfig.Instance.DisastersButton);
                    }
                    ToggleSingleUIComponent("ChirperPanel", ModConfig.Instance.ChirperButton);
                    ToggleSingleUIComponent("RadioPanel", ModConfig.Instance.RadioButton);
                    ToggleSingleUIComponent("Esc", ModConfig.Instance.GearButton);
                    ToggleSingleUIComponent("ZoomComposite", ModConfig.Instance.ZoomButton);
                    ToggleSingleUIComponent("UnlockButton", ModConfig.Instance.UnlockButton);
                    ToggleSingleUIComponent("AdvisorButton", ModConfig.Instance.AdvisorButton);
                    ToggleSingleUIComponent("BulldozerButton", ModConfig.Instance.BulldozerButton);
                    ToggleSingleUIComponent("CinematicCameraPanel", ModConfig.Instance.CinematicCameraButton);
                    ToggleSingleUIComponent("Freecamera", ModConfig.Instance.FreeCameraButton);
                    ToggleSingleUIComponent("PanelTime", ModConfig.Instance.TimePanel);
                    ToggleSingleUIComponent("Sprite", "TSBar", ModConfig.Instance.ZoomAndUnlockBackground);
                    ToggleMultipleUIComponents("Separator", "MainToolstrip", ModConfig.Instance.Separators);
                    ToggleMultipleUIComponents("SmallSeparator", "MainToolstrip", ModConfig.Instance.Separators);
                    ToggleDecorations(
                        ModConfig.Instance.CliffDecorations,
                        ModConfig.Instance.FertileDecorations,
                        ModConfig.Instance.GrassDecorations);
                    ToggleGroundColor(
                        ModConfig.Instance.GrassFertilityGroundColor,
                        ModConfig.Instance.GrassFieldGroundColor,
                        ModConfig.Instance.GrassForestGroundColor,
                        ModConfig.Instance.GrassPollutionGroundColor);
                    ToggleWaterColor(ModConfig.Instance.DirtyWaterColor);
                    ToggleFogEffects(
                        ModConfig.Instance.PollutionFog,
                        ModConfig.Instance.VolumeFog,
                        ModConfig.Instance.DistanceFog,
                        ModConfig.Instance.EdgeFog);

                    if (ModConfig.Instance.Seagulls)
                    {
                        ModUtils.RefreshSeagulls();
                    }

                    if (ModConfig.Instance.Wildlife)
                    {
                        ModUtils.RefreshWildlife();
                    }

                    ModUtils.RefreshTexture();

                    _initialized = true;
                    ModConfig.Instance.ConfigUpdated = false;
                }

                InfoViewManager.Instance.UpdateInfoView();

            }
            catch (Exception e)
            {
                Debug.Log("[Hide it, BOBby!] ModManager:Update -> Exception: " + e.Message);
            }
        }

        private void ToggleSingleUIComponent(string name, bool disable)
        {
            try
            {
                UIComponent component = GameObject.Find(name).GetComponent<UIComponent>();

                if (component != null)
                {
                    component.isVisible = !disable;
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Hide it, BOBby!] ModManager:ToggleSingleUIComponent -> Exception: " + e.Message);
            }
        }

        private void ToggleSingleUIComponent(string name, string parentName, bool disable)
        {
            try
            {
                UIComponent parent = GameObject.Find(parentName).GetComponent<UIComponent>();

                if (parent != null)
                {
                    UIComponent component = parent.Find(name).GetComponent<UIComponent>();

                    if (component != null)
                    {
                        component.isVisible = !disable;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Hide it, BOBby!] ModManager:ToggleSingleUIComponent -> Exception: " + e.Message);
            }
        }

        private void ToggleMultipleUIComponents(string name, string parentName, bool disable)
        {
            try
            {
                UIComponent parent = GameObject.Find(parentName).GetComponent<UIComponent>();

                if (parent != null)
                {
                    foreach (UIComponent component in parent.components)
                    {
                        if (component.name.Equals(name))
                        {
                            component.isVisible = !disable;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Hide it, BOBby!] ModManager:ToggleMultipleUIComponents -> Exception: " + e.Message);
            }
        }

        private void ToggleDecorations(bool disableCliff, bool disableFertile, bool disableGrass)
        {
            try
            {
                if (_terrainProperties != null)
                {
                    _terrainProperties.m_useCliffDecorations = !disableCliff;
                    _terrainProperties.m_useFertileDecorations = !disableFertile;
                    _terrainProperties.m_useGrassDecorations = !disableGrass;
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Hide it, BOBby!] ModManager:ToggleDecorations -> Exception: " + e.Message);
            }
        }

        private void ToggleGroundColor(bool disableGrassFertility, bool disableGrassField, bool disableGrassForest, bool disableGrassPollution)
        {
            try
            {
                if (_terrainProperties != null)
                {
                    Shader.SetGlobalVector("_GrassFertilityColorOffset", disableGrassFertility ? _noColorOffset : _defaultGrassFertilityColorOffset);
                    Shader.SetGlobalVector("_GrassFieldColorOffset", disableGrassField ? _noColorOffset : _defaultGrassFieldColorOffset);
                    Shader.SetGlobalVector("_GrassForestColorOffset", disableGrassForest ? _noColorOffset : _defaultGrassForestColorOffset);
                    Shader.SetGlobalVector("_GrassPollutionColorOffset", disableGrassPollution ? new Vector4(_noColorOffset.x, _noColorOffset.y, _noColorOffset.z, _terrainProperties.m_cliffSandNormalTiling) : new Vector4(_defaultGrassPollutionColorOffset.x, _defaultGrassPollutionColorOffset.y, _defaultGrassPollutionColorOffset.z, _terrainProperties.m_cliffSandNormalTiling));
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Hide it, BOBby!] ModManager:ToggleGroundColor -> Exception: " + e.Message);
            }
        }

        private void ToggleWaterColor(bool disableDirtyWater)
        {
            try
            {
                Shader.SetGlobalColor("_WaterColorDirty", disableDirtyWater ? _defaultWaterColorClean : _defaultWaterColorDirty);
            }
            catch (Exception e)
            {
                Debug.Log("[Hide it, BOBby!] ModManager:ToggleWaterColor -> Exception: " + e.Message);
            }
        }

        private void ToggleFogEffects(bool disablePollution, bool disableVolume, bool disableDistance, bool disableEdge)
        {
            try
            {
                if (_fogProperties != null)
                {
                    _fogProperties.m_PollutionAmount = disablePollution ? 0f : _defaultPollutionAmount;
                    _fogProperties.m_FogDensity = disableVolume ? 0f : _defaultFogDensity;
                    _fogProperties.m_ColorDecay = disableDistance ? 1f : _defaultColorDecay;
                    _fogProperties.m_edgeFog = !disableEdge;
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Hide it, BOBby!] ModManager:ToggleFogEffects -> Exception: " + e.Message);
            }
        }
    }
}
