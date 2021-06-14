using HideItBobby.Common;
using HideItBobby.Common.Logging;
using HideItBobby.EntryPoints;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using static HideItBobby.Common.Patcher;

namespace HideItBobby.Features.UIElements
{
    internal sealed class HideAdvisorPanel : FeatureBase
    {
        public override FeatureKey Key => FeatureKey.HideAdvisorPanel;

        protected override bool InitializeImpl()
        {
            Patch(HideAdvisorPanelTutorialAdvisorPanelShowPatch.Data);
            return true;
        }
        protected override bool TerminateImpl()
        {
            Unpatch(HideAdvisorPanelTutorialAdvisorPanelShowPatch.Data);
            return true;
        }
    }

    #region Harmony patch
    internal static class HideAdvisorPanelTutorialAdvisorPanelShowPatch
    {
        public static readonly PatchData Data = new PatchData(
                patchId: $"{nameof(HideAdvisorPanel)}.{nameof(HideAdvisorPanelTutorialAdvisorPanelShowPatch)}.{nameof(Prefix)}",
                target: () => typeof(TutorialAdvisorPanel).GetMethod(
                    "Show",
                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
                    null,
                    new Type[] { typeof(string), typeof(string), typeof(string), typeof(float), typeof(bool), typeof(bool) },
                    null),
                prefix: () => typeof(HideAdvisorPanelTutorialAdvisorPanelShowPatch).GetMethod(nameof(Prefix), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static),
                onUnpatch: () =>
                {
                    _hideAdvisorPanel.Invalidate();
                }
            );

        #region HideAdvisorPanel { get; }
        private static readonly Cached<HideAdvisorPanel> _hideAdvisorPanel = new Cached<HideAdvisorPanel>(
            () => InGameEntryPoint.Features.Resolve(FeatureKey.HideAdvisorPanel) as HideAdvisorPanel
            );
        private static HideAdvisorPanel HideAdvisorPanel => _hideAdvisorPanel.Value;
        #endregion

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static bool Prefix()
        {
            if (!Data.IsPatchApplied) return true;
            try
            {
                var isEnabled = (HideAdvisorPanel?.IsEnabled) ?? false;
                return !isEnabled;
            }
            catch (Exception e)
            {
                Log.Error($"{nameof(HideAdvisorPanelTutorialAdvisorPanelShowPatch)}.{nameof(Prefix)} failed", e);
                return true;
            }
        }
    }
    #endregion
}