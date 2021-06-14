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
    internal static class NaturalResourceManagerUpdateTextureBPatch
    {
        public static readonly PatchData Data = new PatchData(
           patchId: $"SharedPatch.{nameof(NaturalResourceManagerUpdateTextureBPatch)}.{nameof(Prefix)}",
           target: () => typeof(NaturalResourceManager).GetMethod("UpdateTextureB", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance),
           prefix: () => typeof(NaturalResourceManagerUpdateTextureBPatch).GetMethod(nameof(Prefix), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static),
           onUnpatch: () =>
           {
               _hidePollutedAreaEffect.Invalidate();
               _hideBurnedAreaEffect.Invalidate();
               _hideDestroyedAreaEffect.Invalidate();
           }
       );

        #region HidePollutedAreaEffect { get; }
        private static readonly Cached<HidePollutedAreaEffect> _hidePollutedAreaEffect = new Cached<HidePollutedAreaEffect>(
            () => InGameEntryPoint.Features.Resolve(FeatureKey.HidePollutedAreaEffect) as HidePollutedAreaEffect
            );
        private static readonly HidePollutedAreaEffect HidePollutedAreaEffect = _hidePollutedAreaEffect.Value;
        #endregion
        #region HideBurnedAreaEffect { get; }
        private static readonly Cached<HideBurnedAreaEffect> _hideBurnedAreaEffect = new Cached<HideBurnedAreaEffect>(
            () => InGameEntryPoint.Features.Resolve(FeatureKey.HideBurnedAreaEffect) as HideBurnedAreaEffect
            );
        private static readonly HideBurnedAreaEffect HideBurnedAreaEffect = _hideBurnedAreaEffect.Value;
        #endregion
        #region HideDestroyedAreaEffect { get; }
        private static readonly Cached<HideDestroyedAreaEffect> _hideDestroyedAreaEffect = new Cached<HideDestroyedAreaEffect>(
            () => InGameEntryPoint.Features.Resolve(FeatureKey.HideDestroyedAreaEffect) as HideDestroyedAreaEffect
            );
        private static readonly HideDestroyedAreaEffect HideDestroyedAreaEffect = _hideDestroyedAreaEffect.Value;
        #endregion

        private static readonly Lazy<FieldInfo> m_modifiedBX1_field = new Lazy<FieldInfo>(() => typeof(NaturalResourceManager).GetField("m_modifiedBX1", BindingFlags.NonPublic | BindingFlags.Instance));
        private static readonly Lazy<FieldInfo> m_modifiedBX2_field = new Lazy<FieldInfo>(() => typeof(NaturalResourceManager).GetField("m_modifiedBX2", BindingFlags.NonPublic | BindingFlags.Instance));

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static bool Prefix(NaturalResourceManager __instance)
        {
            if (!Data.IsPatchApplied) return true;
            try
            {
                int[] m_modifiedBX1 = (int[])m_modifiedBX1_field.Value.GetValue(__instance);
                int[] m_modifiedBX2 = (int[])m_modifiedBX2_field.Value.GetValue(__instance);

                for (int i = 0; i < 512; i++)
                {
                    if (m_modifiedBX2[i] >= m_modifiedBX1[i])
                    {
                        while (!Monitor.TryEnter(__instance.m_naturalResources, SimulationManager.SYNCHRONIZE_TIMEOUT))
                        {
                        }
                        int num1;
                        int num2;
                        try
                        {
                            num1 = m_modifiedBX1[i];
                            num2 = m_modifiedBX2[i];
                            m_modifiedBX1[i] = 10000;
                            m_modifiedBX2[i] = -10000;
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
                                color = new Color(0f, 0f, 0f, 1f);
                                InfoViewUpdater.DestructionTexture.SetPixel(j, i, color);
                            }
                            else
                            {
                                int pollution = 0;
                                int burned = 0;
                                int destroyed = 0;

                                AddResource(j - 1, i - 1, 5, ref pollution, ref burned, ref destroyed);
                                AddResource(j, i - 1, 7, ref pollution, ref burned, ref destroyed);
                                AddResource(j + 1, i - 1, 5, ref pollution, ref burned, ref destroyed);
                                AddResource(j - 1, i, 7, ref pollution, ref burned, ref destroyed);
                                AddResource(j, i, 14, ref pollution, ref burned, ref destroyed);
                                AddResource(j + 1, i, 7, ref pollution, ref burned, ref destroyed);
                                AddResource(j - 1, i + 1, 5, ref pollution, ref burned, ref destroyed);
                                AddResource(j, i + 1, 7, ref pollution, ref burned, ref destroyed);
                                AddResource(j + 1, i + 1, 5, ref pollution, ref burned, ref destroyed);

                                color = CalculateColorComponents(pollution, burned, destroyed);
                                InfoViewUpdater.DestructionTexture.SetPixel(j, i, color);

                                pollution = (HidePollutedAreaEffect?.IsEnabled ?? false) ? 0 : pollution;
                                burned = (HideBurnedAreaEffect?.IsEnabled ?? false) ? 0 : burned;
                                destroyed = (HideDestroyedAreaEffect?.IsEnabled ?? false) ? 0 : destroyed;

                                color = CalculateColorComponents(pollution, burned, destroyed);
                            }
                            __instance.m_destructionTexture.SetPixel(j, i, color);
                        }
                    }
                }
                __instance.m_destructionTexture.Apply();

                InfoViewUpdater.DestructionTexture.Apply();

                return false;
            }
            catch (Exception e)
            {
                Log.Error($"{nameof(NaturalResourceManagerUpdateTextureBPatch)}.{nameof(Prefix)} failed", e);
                return true;
            }
        }

        #region Helpers
        private static Color CalculateColorComponents(int pollution, int burned, int destroyed)
        {
            Color color;
            color.r = pollution * 6.325111E-05f;
            color.g = burned * 6.325111E-05f;
            color.b = destroyed * 6.325111E-05f;
            color.a = 1f;
            return color;
        }

        private static void AddResource(int x, int z, int multiplier, ref int pollution, ref int burned, ref int destroyed)
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
                pollution += resourceCell.m_pollution * multiplier;
                burned += resourceCell.m_burned * multiplier;
                destroyed += resourceCell.m_destroyed * multiplier;
            }
            catch (Exception e)
            {
                Log.Error($"{nameof(NaturalResourceManagerUpdateTextureBPatch)}.{nameof(AddResource)} failed", e);
            }
        }
        #endregion
    }
}