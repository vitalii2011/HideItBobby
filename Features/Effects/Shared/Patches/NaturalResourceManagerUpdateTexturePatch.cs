using ColossalFramework;
using HideItBobby.Common;
using HideItBobby.Common.Logging;
using HideItBobby.EntryPoints;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace HideItBobby.Features.Effects.Shared.Patches
{
    internal static class NaturalResourceManagerUpdateTexturePatch
    {
        public static readonly PatchData Data = new PatchData(
           patchId: $"SharedPatch.{nameof(NaturalResourceManagerUpdateTexturePatch)}.{nameof(Prefix)}",
           target: () => typeof(NaturalResourceManager).GetMethod("UpdateTexture", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance),
           prefix: () => typeof(NaturalResourceManagerUpdateTexturePatch).GetMethod(nameof(Prefix), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static),
           onUnpatch: () =>
           {
               _hideOreAreaEffect.Invalidate();
               _hideOilAreaEffect.Invalidate();
               _hideSandAreaEffect.Invalidate();
               _hideFertilityAreaEffect.Invalidate();
               _hideForestAreaEffect.Invalidate();
               _hideShoreAreaEffect.Invalidate();
           }
        );

        #region HideOreAreaEffect { get; }
        private static readonly Cached<HideOreAreaEffect> _hideOreAreaEffect = new Cached<HideOreAreaEffect>(
            () => InGameEntryPoint.Features.Resolve(FeatureKey.HideOreAreaEffect) as HideOreAreaEffect
            );
        private static readonly HideOreAreaEffect HideOreAreaEffect = _hideOreAreaEffect.Value;
        #endregion
        #region HideOilAreaEffect { get; }
        private static readonly Cached<HideOilAreaEffect> _hideOilAreaEffect = new Cached<HideOilAreaEffect>(
    () => InGameEntryPoint.Features.Resolve(FeatureKey.HideOilAreaEffect) as HideOilAreaEffect
    );
        private static readonly HideOilAreaEffect HideOilAreaEffect = _hideOilAreaEffect.Value;
        #endregion
        #region HideSandAreaEffect { get; }
        private static readonly Cached<HideSandAreaEffect> _hideSandAreaEffect = new Cached<HideSandAreaEffect>(
    () => InGameEntryPoint.Features.Resolve(FeatureKey.HideSandAreaEffect) as HideSandAreaEffect
    );
        private static readonly HideSandAreaEffect HideSandAreaEffect = _hideSandAreaEffect.Value;
        #endregion
        #region HideFertilityAreaEffect { get; }
        private static readonly Cached<HideFertilityAreaEffect> _hideFertilityAreaEffect = new Cached<HideFertilityAreaEffect>(
    () => InGameEntryPoint.Features.Resolve(FeatureKey.HideFertilityAreaEffect) as HideFertilityAreaEffect
    );
        private static readonly HideFertilityAreaEffect HideFertilityAreaEffect = _hideFertilityAreaEffect.Value;
        #endregion
        #region HideForestAreaEffect { get; }
        private static readonly Cached<HideForestAreaEffect> _hideForestAreaEffect = new Cached<HideForestAreaEffect>(
    () => InGameEntryPoint.Features.Resolve(FeatureKey.HideForestAreaEffect) as HideForestAreaEffect
    );
        private static readonly HideForestAreaEffect HideForestAreaEffect = _hideForestAreaEffect.Value;
        #endregion
        #region HideShoreAreaEffect { get; }
        private static readonly Cached<HideShoreAreaEffect> _hideShoreAreaEffect = new Cached<HideShoreAreaEffect>(
    () => InGameEntryPoint.Features.Resolve(FeatureKey.HideShoreAreaEffect) as HideShoreAreaEffect
    );
        private static readonly HideShoreAreaEffect HideShoreAreaEffect = _hideShoreAreaEffect.Value;
        #endregion

        private static readonly Lazy<FieldInfo> m_modifiedX1_field = new Lazy<FieldInfo>(() => typeof(NaturalResourceManager).GetField("m_modifiedX1", BindingFlags.NonPublic | BindingFlags.Instance));
        private static readonly Lazy<FieldInfo> m_modifiedX2_field = new Lazy<FieldInfo>(() => typeof(NaturalResourceManager).GetField("m_modifiedX2", BindingFlags.NonPublic | BindingFlags.Instance));

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static bool Prefix(NaturalResourceManager __instance)
        {
            if (!Data.IsPatchApplied) return true;
            try
            {
                int[] m_modifiedX1 = (int[])m_modifiedX1_field.Value.GetValue(__instance);
                int[] m_modifiedX2 = (int[])m_modifiedX2_field.Value.GetValue(__instance);

                for (int i = 0; i < 512; i++)
                {
                    if (m_modifiedX2[i] >= m_modifiedX1[i])
                    {
                        while (!Monitor.TryEnter(__instance.m_naturalResources, SimulationManager.SYNCHRONIZE_TIMEOUT))
                        {
                        }
                        int num1;
                        int num2;
                        try
                        {
                            num1 = m_modifiedX1[i];
                            num2 = m_modifiedX2[i];
                            m_modifiedX1[i] = 10000;
                            m_modifiedX2[i] = -10000;
                        }
                        finally
                        {
                            Monitor.Exit(__instance.m_naturalResources);
                        }
                        for (int j = num1; j <= num2; j++)
                        {
                            Color color = default;
                            if (i == 0 || j == 0 || i == 511 || j == 511)
                            {
                                color = new Color(0.5f, 0.5f, 0.5f, 0f);
                                InfoViewUpdater.ResourceTexture.SetPixel(j, i, color);
                            }
                            else
                            {
                                int ore = 0;
                                int oil = 0;
                                int sand = 0;
                                int fertility = 0;
                                int forest = 0;
                                int shore = 0;

                                AddResource(j - 1, i - 1, 5, ref ore, ref oil, ref sand, ref fertility, ref forest, ref shore);
                                AddResource(j, i - 1, 7, ref ore, ref oil, ref sand, ref fertility, ref forest, ref shore);
                                AddResource(j + 1, i - 1, 5, ref ore, ref oil, ref sand, ref fertility, ref forest, ref shore);
                                AddResource(j - 1, i, 7, ref ore, ref oil, ref sand, ref fertility, ref forest, ref shore);
                                AddResource(j, i, 14, ref ore, ref oil, ref sand, ref fertility, ref forest, ref shore);
                                AddResource(j + 1, i, 7, ref ore, ref oil, ref sand, ref fertility, ref forest, ref shore);
                                AddResource(j - 1, i + 1, 5, ref ore, ref oil, ref sand, ref fertility, ref forest, ref shore);
                                AddResource(j, i + 1, 7, ref ore, ref oil, ref sand, ref fertility, ref forest, ref shore);
                                AddResource(j + 1, i + 1, 5, ref ore, ref oil, ref sand, ref fertility, ref forest, ref shore);

                                color = CalculateColorComponents(ore, oil, sand, fertility, forest, shore);
                                InfoViewUpdater.ResourceTexture.SetPixel(j, i, color);

                                ore = ((HideOreAreaEffect?.IsEnabled) ?? false) ? 0 : ore;
                                oil = ((HideOilAreaEffect?.IsEnabled) ?? false) ? 0 : oil;
                                sand = ((HideSandAreaEffect?.IsEnabled) ?? false) ? 0 : sand;
                                fertility = ((HideFertilityAreaEffect?.IsEnabled) ?? false) ? 0 : fertility;
                                forest = ((HideForestAreaEffect?.IsEnabled) ?? false) ? 0 : forest;
                                shore = ((HideShoreAreaEffect?.IsEnabled) ?? false) ? 0 : shore;

                                color = CalculateColorComponents(ore, oil, sand, fertility, forest, shore);
                            }
                            __instance.m_resourceTexture.SetPixel(j, i, color);
                        }
                    }
                }
                __instance.m_resourceTexture.Apply();

                InfoViewUpdater.ResourceTexture.Apply();

                return false;
            }
            catch (Exception e)
            {
                Log.Error($"{nameof(NaturalResourceManagerUpdateTexturePatch)}.{nameof(Prefix)} failed", e);
                return true;
            }
        }

        #region Helpers
        private static Color CalculateColorComponents(int ore, int oil, int sand, int fertility, int forest, int shore)
        {
            Color color;

            color.r = (ore - oil + 15810) * 3.16255537E-05f;
            color.g = (sand - fertility + 15810) * 3.16255537E-05f;
            int num3 = shore * 4 - forest;
            if (num3 > 0)
            {
                color.b = (15810 + num3 / 4) * 3.16255537E-05f;
            }
            else
            {
                color.b = (15810 + num3) * 3.16255537E-05f;
            }
            color.a = 1f;

            return color;
        }

        private static void AddResource(int x, int z, int multiplier, ref int ore, ref int oil, ref int sand, ref int fertility, ref int forest, ref int shore)
        {
            try
            {
                if (!Singleton<NaturalResourceManager>.exists)
                {
#if DEV
                    Log.Warning($"{nameof(NaturalResourceManagerUpdateTexturePatch)}.{nameof(AddResource)} instance of {nameof(NaturalResourceManager)} does not exist");
#endif
                    return;
                }

                x = Mathf.Clamp(x, 0, 511);
                z = Mathf.Clamp(z, 0, 511);
                var resourceCell = Singleton<NaturalResourceManager>.instance.m_naturalResources[z * 512 + x];
                ore += resourceCell.m_ore * multiplier;
                oil += resourceCell.m_oil * multiplier;
                sand += resourceCell.m_sand * multiplier;
                fertility += resourceCell.m_fertility * multiplier;
                forest += resourceCell.m_forest * multiplier;
                shore += resourceCell.m_shore * multiplier;

            }
            catch (Exception e)
            {
                Log.Error($"{nameof(NaturalResourceManagerUpdateTexturePatch)}.{nameof(AddResource)} failed", e);
            }
        }
        #endregion
    }
}