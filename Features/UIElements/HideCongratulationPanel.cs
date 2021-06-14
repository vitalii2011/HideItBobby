using HideItBobby.Common;
using HideItBobby.Common.Logging;
using HideItBobby.EntryPoints;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using static HideItBobby.Common.Patcher;

namespace HideItBobby.Features.UIElements
{
    internal sealed class HideCongratulationPanel : FeatureBase
    {
        public override FeatureKey Key => FeatureKey.HideCongratulationPanel;

        protected override bool InitializeImpl()
        {
            Patch(HideCongratulationsPanelUnlockingPanelShowModalPatch.Data);
            return true;
        }
        protected override bool TerminateImpl()
        {
            Unpatch(HideCongratulationsPanelUnlockingPanelShowModalPatch.Data);
            return true;
        }
    }

    #region Harmony patch
    internal static class HideCongratulationsPanelUnlockingPanelShowModalPatch
    {
        public static readonly PatchData Data = new PatchData(
                patchId: $"{nameof(HideCongratulationPanel)}.{nameof(HideCongratulationsPanelUnlockingPanelShowModalPatch)}.{nameof(Prefix)}",
                target: () => typeof(UnlockingPanel).GetMethod("ShowModal", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance),
                prefix: () => typeof(HideCongratulationsPanelUnlockingPanelShowModalPatch).GetMethod(nameof(Prefix), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static),
                onUnpatch: () =>
                {
                    _hideCongratulationPanel.Invalidate();
                }
            );

        #region HideCongratulationPanel { get; }
        private static readonly Cached<HideCongratulationPanel> _hideCongratulationPanel = new Cached<HideCongratulationPanel>(
            () => InGameEntryPoint.Features.Resolve(FeatureKey.HideCongratulationPanel) as HideCongratulationPanel
            );
        private static readonly HideCongratulationPanel HideCongratulationPanel = _hideCongratulationPanel.Value;
        #endregion

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static bool Prefix()
        {
            if (!Data.IsPatchApplied) return true;
            try
            {
                var isEnabled = (HideCongratulationPanel?.IsEnabled) ?? false;
                return !isEnabled;
            }
            catch (Exception e)
            {
                Log.Error($"{nameof(HideCongratulationsPanelUnlockingPanelShowModalPatch)}.{nameof(Prefix)} failed", e);
                return true;
            }
        }
    }
    #endregion
}