using ColossalFramework;
using HideItBobby.Common;
using HideItBobby.Common.Logging;
using HideItBobby.EntryPoints;
using System;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using static HideItBobby.Common.Patcher;

namespace HideItBobby.Features.Objects
{
    internal sealed class HideSeagulls : FeatureBase
    {
        public override FeatureKey Key => FeatureKey.HideSeagulls;

        protected override bool InitializeImpl()
        {
            Patch(CargoHarborAICountAnimalsPatch.Data);
            Patch(HarborAICountAnimalsPatch.Data);
            Patch(IndustryBuildingAICountAnimalsPatch.Data);
            Patch(LandfillSiteAICountAnimalsPatch.Data);
            Patch(ParkAICountAnimalsPatch.Data);
            Patch(ParkBuildingAICountAnimalsPatch.Data);
            Patch(WarehouseAICountAnimalsPatch.Data);
            return true;
        }
        protected override bool TerminateImpl()
        {
            Unpatch(CargoHarborAICountAnimalsPatch.Data);
            Unpatch(HarborAICountAnimalsPatch.Data);
            Unpatch(IndustryBuildingAICountAnimalsPatch.Data);
            Unpatch(LandfillSiteAICountAnimalsPatch.Data);
            Unpatch(ParkAICountAnimalsPatch.Data);
            Unpatch(ParkBuildingAICountAnimalsPatch.Data);
            Unpatch(WarehouseAICountAnimalsPatch.Data);
            return true;
        }

        protected override bool EnableImpl()
        {
            if (!SimulationManager.exists) return false;

            SimulationManager.instance.AddAction(OnSeagullsRefresh());
            return true;
        }
        protected override bool DisableImpl() => true;

        private static IEnumerator OnSeagullsRefresh()
        {
            try
            {
                if (Singleton<CitizenManager>.exists)
                {
                    var citizenManager = Singleton<CitizenManager>.instance;
                    for (int i = 1; i < citizenManager.m_instances.m_buffer.Length; i++)
                    {
                        if ((citizenManager.m_instances.m_buffer[i].m_flags & CitizenInstance.Flags.Created) != CitizenInstance.Flags.None)
                        {
                            if (citizenManager.m_instances.m_buffer[i].Info.m_citizenAI != null && citizenManager.m_instances.m_buffer[i].Info.m_citizenAI is BirdAI)
                            {
                                citizenManager.ReleaseCitizenInstance((ushort)i);
                            }
                        }
                    }
                }
                else
                {
#if DEV
                    Log.Warning($"{nameof(HideSeagulls)}.{nameof(OnSeagullsRefresh)} instance of {nameof(CitizenManager)} does not exist");
#endif
                }
            }
            catch (Exception e)
            {
                Log.Error($"{nameof(HideSeagulls)}.{nameof(OnSeagullsRefresh)} failed", e);
            }

            yield return null;
        }
    }

    #region Harmony patches
    internal static class CargoHarborAICountAnimalsPatch
    {
        public static readonly PatchData Data = new PatchData(
            patchId: $"{nameof(HideSeagulls)}.{nameof(CargoHarborAICountAnimalsPatch)}.{nameof(Postfix)}",
            target: () => typeof(CargoHarborAI).GetMethod("CountAnimals", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance),
            postfix: () => typeof(CargoHarborAICountAnimalsPatch).GetMethod(nameof(Postfix), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static),
            onUnpatch: () =>
            {
                _hideSeagulls.Invalidate();
            }
        );

        #region HideSeagulls { get; }
        private static readonly Cached<HideSeagulls> _hideSeagulls = new Cached<HideSeagulls>(
            () => InGameEntryPoint.Features.Resolve(FeatureKey.HideSeagulls) as HideSeagulls
            );
        private static readonly HideSeagulls HideSeagulls = _hideSeagulls.Value;
        #endregion

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int Postfix(int __result)
        {
            if (!Data.IsPatchApplied) return __result;
            try
            {
                var isEnabled = (HideSeagulls?.IsEnabled) ?? false;
                return isEnabled ? int.MaxValue : __result;
            }
            catch (Exception e)
            {
                Log.Error($"{nameof(HideSeagulls)}.{nameof(CargoHarborAICountAnimalsPatch)}.{nameof(Postfix)} failed", e);
                return __result;
            }
        }
    }

    internal static class HarborAICountAnimalsPatch
    {
        public static readonly PatchData Data = new PatchData(
            patchId: $"{nameof(HideSeagulls)}.{nameof(HarborAICountAnimalsPatch)}.{nameof(Postfix)}",
            target: () => typeof(HarborAI).GetMethod("CountAnimals", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance),
            postfix: () => typeof(HarborAICountAnimalsPatch).GetMethod(nameof(Postfix), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static),
            onUnpatch: () =>
            {
                _hideSeagulls.Invalidate();
            }
        );

        #region HideSeagulls { get; }
        private static readonly Cached<HideSeagulls> _hideSeagulls = new Cached<HideSeagulls>(
            () => InGameEntryPoint.Features.Resolve(FeatureKey.HideSeagulls) as HideSeagulls
            );
        private static readonly HideSeagulls HideSeagulls = _hideSeagulls.Value;
        #endregion

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int Postfix(int __result)
        {
            if (!Data.IsPatchApplied) return __result;
            try
            {
                var isEnabled = (HideSeagulls?.IsEnabled) ?? false;
                return isEnabled ? int.MaxValue : __result;
            }
            catch (Exception e)
            {
                Log.Error($"{nameof(HideSeagulls)}.{nameof(HarborAICountAnimalsPatch)}.{nameof(Postfix)} failed", e);
                return __result;
            }
        }
    }

    internal static class IndustryBuildingAICountAnimalsPatch
    {
        public static readonly PatchData Data = new PatchData(
            patchId: $"{nameof(HideSeagulls)}.{nameof(IndustryBuildingAICountAnimalsPatch)}.{nameof(Postfix)}",
            target: () => typeof(IndustryBuildingAI).GetMethod("CountAnimals", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance),
            postfix: () => typeof(IndustryBuildingAICountAnimalsPatch).GetMethod(nameof(Postfix), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static),
            onUnpatch: () =>
            {
                _hideSeagulls.Invalidate();
            }
        );

        #region HideSeagulls { get; }
        private static readonly Cached<HideSeagulls> _hideSeagulls = new Cached<HideSeagulls>(
            () => InGameEntryPoint.Features.Resolve(FeatureKey.HideSeagulls) as HideSeagulls
            );
        private static readonly HideSeagulls HideSeagulls = _hideSeagulls.Value;
        #endregion

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int Postfix(int __result)
        {
            if (!Data.IsPatchApplied) return __result;
            try
            {
                var isEnabled = (HideSeagulls?.IsEnabled) ?? false;
                return isEnabled ? int.MaxValue : __result;
            }
            catch (Exception e)
            {
                Log.Error($"{nameof(HideSeagulls)}.{nameof(IndustryBuildingAICountAnimalsPatch)}.{nameof(Postfix)} failed", e);
                return __result;
            }
        }
    }

    internal static class LandfillSiteAICountAnimalsPatch
    {
        public static readonly PatchData Data = new PatchData(
            patchId: $"{nameof(HideSeagulls)}.{nameof(LandfillSiteAICountAnimalsPatch)}.{nameof(Postfix)}",
            target: () => typeof(LandfillSiteAI).GetMethod("CountAnimals", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance),
            postfix: () => typeof(LandfillSiteAICountAnimalsPatch).GetMethod(nameof(Postfix), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static),
            onUnpatch: () =>
            {
                _hideSeagulls.Invalidate();
            }
        );

        #region HideSeagulls { get; }
        private static readonly Cached<HideSeagulls> _hideSeagulls = new Cached<HideSeagulls>(
            () => InGameEntryPoint.Features.Resolve(FeatureKey.HideSeagulls) as HideSeagulls
            );
        private static readonly HideSeagulls HideSeagulls = _hideSeagulls.Value;
        #endregion

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int Postfix(int __result)
        {
            if (!Data.IsPatchApplied) return __result;
            try
            {
                var isEnabled = (HideSeagulls?.IsEnabled) ?? false;
                return isEnabled ? int.MaxValue : __result;
            }
            catch (Exception e)
            {
                Log.Error($"{nameof(HideSeagulls)}.{nameof(LandfillSiteAICountAnimalsPatch)}.{nameof(Postfix)} failed", e);
                return __result;
            }
        }
    }

    internal static class ParkAICountAnimalsPatch
    {
        public static readonly PatchData Data = new PatchData(
            patchId: $"{nameof(HideSeagulls)}.{nameof(ParkAICountAnimalsPatch)}.{nameof(Postfix)}",
            target: () => typeof(ParkAI).GetMethod("CountAnimals", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance),
            postfix: () => typeof(ParkAICountAnimalsPatch).GetMethod(nameof(Postfix), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static),
            onUnpatch: () =>
            {
                _hideSeagulls.Invalidate();
            }
        );

        #region HideSeagulls { get; }
        private static readonly Cached<HideSeagulls> _hideSeagulls = new Cached<HideSeagulls>(
            () => InGameEntryPoint.Features.Resolve(FeatureKey.HideSeagulls) as HideSeagulls
            );
        private static readonly HideSeagulls HideSeagulls = _hideSeagulls.Value;
        #endregion

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int Postfix(int __result)
        {
            if (!Data.IsPatchApplied) return __result;
            try
            {
                var isEnabled = (HideSeagulls?.IsEnabled) ?? false;
                return isEnabled ? int.MaxValue : __result;
            }
            catch (Exception e)
            {
                Log.Error($"{nameof(HideSeagulls)}.{nameof(ParkAICountAnimalsPatch)}.{nameof(Postfix)} failed", e);
                return __result;
            }
        }
    }

    internal static class ParkBuildingAICountAnimalsPatch
    {
        public static readonly PatchData Data = new PatchData(
            patchId: $"{nameof(HideSeagulls)}.{nameof(ParkBuildingAICountAnimalsPatch)}.{nameof(Postfix)}",
            target: () => typeof(ParkBuildingAI).GetMethod("CountAnimals", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance),
            postfix: () => typeof(ParkBuildingAICountAnimalsPatch).GetMethod(nameof(Postfix), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static),
            onUnpatch: () =>
            {
                _hideSeagulls.Invalidate();
            }
        );

        #region HideSeagulls { get; }
        private static readonly Cached<HideSeagulls> _hideSeagulls = new Cached<HideSeagulls>(
            () => InGameEntryPoint.Features.Resolve(FeatureKey.HideSeagulls) as HideSeagulls
            );
        private static readonly HideSeagulls HideSeagulls = _hideSeagulls.Value;
        #endregion

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int Postfix(int __result)
        {
            if (!Data.IsPatchApplied) return __result;
            try
            {
                var isEnabled = (HideSeagulls?.IsEnabled) ?? false;
                return isEnabled ? int.MaxValue : __result;
            }
            catch (Exception e)
            {
                Log.Error($"{nameof(HideSeagulls)}.{nameof(ParkBuildingAICountAnimalsPatch)}.{nameof(Postfix)} failed", e);
                return __result;
            }
        }
    }

    internal static class WarehouseAICountAnimalsPatch
    {
        public static readonly PatchData Data = new PatchData(
            patchId: $"{nameof(HideSeagulls)}.{nameof(WarehouseAICountAnimalsPatch)}.{nameof(Postfix)}",
            target: () => typeof(WarehouseAI).GetMethod("CountAnimals", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance),
            postfix: () => typeof(WarehouseAICountAnimalsPatch).GetMethod(nameof(Postfix), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static),
            onUnpatch: () =>
            {
                _hideSeagulls.Invalidate();
            }
        );

        #region HideSeagulls { get; }
        private static readonly Cached<HideSeagulls> _hideSeagulls = new Cached<HideSeagulls>(
            () => InGameEntryPoint.Features.Resolve(FeatureKey.HideSeagulls) as HideSeagulls
            );
        private static readonly HideSeagulls HideSeagulls = _hideSeagulls.Value;
        #endregion

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int Postfix(int __result)
        {
            if (!Data.IsPatchApplied) return __result;
            try
            {
                var isEnabled = (HideSeagulls?.IsEnabled) ?? false;
                return isEnabled ? int.MaxValue : __result;
            }
            catch (Exception e)
            {
                Log.Error($"{nameof(HideSeagulls)}.{nameof(WarehouseAICountAnimalsPatch)}.{nameof(Postfix)} failed", e);
                return __result;
            }
        }
    }
    #endregion
}