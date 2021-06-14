using ColossalFramework.UI;
using HideItBobby.Common;
using HideItBobby.Common.Logging;
using HideItBobby.EntryPoints;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace HideItBobby.Features.Menu.Shared.Patches
{
    internal static class HideMainMenuItemsOnVisibilityChangedPatch
    {
        public static readonly PatchData Data = new PatchData(
                patchId: $"SharedPatch.{nameof(HideMainMenuItemsOnVisibilityChangedPatch)}.{nameof(Postfix)}",
                target: () => typeof(MainMenu).GetMethod("OnVisibilityChanged", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance),
                postfix: () => typeof(HideMainMenuItemsOnVisibilityChangedPatch).GetMethod(nameof(Postfix), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static),
                onUnpatch: () =>
                {
                    _hideMainMenuDLCPanel.Invalidate();
                    _hideMainMenuNewsPanel.Invalidate();
                    _hideMainMenuParadoxAccountPanel.Invalidate();
                    _hideMainMenuWorkshopPanel.Invalidate();
                }
            );

        #region HideMainMenuDLCPanel { get; }
        private static readonly Cached<HideMainMenuDLCPanel> _hideMainMenuDLCPanel = new Cached<HideMainMenuDLCPanel>(
    () => MainMenuEntryPoint.Features.Resolve(FeatureKey.HideMainMenuDLCPanel) as HideMainMenuDLCPanel
    );
        private static readonly HideMainMenuDLCPanel HideMainMenuDLCPanel = _hideMainMenuDLCPanel.Value;
        #endregion
        #region HideMainMenuNewsPanel { get; }
        private static readonly Cached<HideMainMenuNewsPanel> _hideMainMenuNewsPanel = new Cached<HideMainMenuNewsPanel>(
    () => MainMenuEntryPoint.Features.Resolve(FeatureKey.HideMainMenuNewsPanel) as HideMainMenuNewsPanel
    );
        private static readonly HideMainMenuNewsPanel HideMainMenuNewsPanel = _hideMainMenuNewsPanel.Value;
        #endregion
        #region HideMainMenuParadoxAccountPanel { get; }
        private static readonly Cached<HideMainMenuParadoxAccountPanel> _hideMainMenuParadoxAccountPanel = new Cached<HideMainMenuParadoxAccountPanel>(
    () => MainMenuEntryPoint.Features.Resolve(FeatureKey.HideMainMenuParadoxAccountPanel) as HideMainMenuParadoxAccountPanel
    );
        private static readonly HideMainMenuParadoxAccountPanel HideMainMenuParadoxAccountPanel = _hideMainMenuParadoxAccountPanel.Value;
        #endregion
        #region HideMainMenuWorkshopPanel { get; }
        private static readonly Cached<HideMainMenuWorkshopPanel> _hideMainMenuWorkshopPanel = new Cached<HideMainMenuWorkshopPanel>(
    () => MainMenuEntryPoint.Features.Resolve(FeatureKey.HideMainMenuWorkshopPanel) as HideMainMenuWorkshopPanel
    );
        private static readonly HideMainMenuWorkshopPanel HideMainMenuWorkshopPanel = _hideMainMenuWorkshopPanel.Value;
        #endregion

        private static readonly Lazy<FieldInfo> m_DLCPanel_field = new Lazy<FieldInfo>(() => typeof(MainMenu).GetField("m_DLCPanel", BindingFlags.NonPublic | BindingFlags.Instance));
        private static readonly Lazy<FieldInfo> m_NewsFeedPanel_field = new Lazy<FieldInfo>(() => typeof(MainMenu).GetField("m_NewsFeedPanel", BindingFlags.NonPublic | BindingFlags.Instance));
        private static readonly Lazy<FieldInfo> m_PDXPanel_field = new Lazy<FieldInfo>(() => typeof(MainMenu).GetField("m_PDXPanel", BindingFlags.NonPublic | BindingFlags.Instance));
        private static readonly Lazy<FieldInfo> m_WorkshopPanel_field = new Lazy<FieldInfo>(() => typeof(MainMenu).GetField("m_WorkshopPanel", BindingFlags.NonPublic | BindingFlags.Instance));

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Postfix(MainMenu __instance, UIComponent comp, bool visible)
        {
            if (!Data.IsPatchApplied) return;
#if DEV
            Log.Info($"{nameof(HideMainMenuItemsOnVisibilityChangedPatch)}.{nameof(Postfix)} setting visibility.");
#endif
            var isHideMainMenuDLCPanelEnabled = (HideMainMenuDLCPanel?.IsEnabled) ?? false;
            if (isHideMainMenuDLCPanelEnabled)
            {
                var isDLCPanelVisible = (m_DLCPanel_field.Value.GetValue(__instance) as UIComponent)?.isVisible ?? false;
                if (isDLCPanelVisible) HideMainMenuDLCPanel?.Enable(true);
            }

            var isHideMainMenuNewsPanelEnabled = (HideMainMenuNewsPanel?.IsEnabled) ?? false;
            if (isHideMainMenuNewsPanelEnabled)
            {
                var isNewsFeedPanelVisible = (m_NewsFeedPanel_field.Value.GetValue(__instance) as UIComponent)?.isVisible ?? false;
                if (isNewsFeedPanelVisible) HideMainMenuNewsPanel?.Enable(true);
            }

            var isHideMainMenuParadoxAccountPanelEnabled = (HideMainMenuParadoxAccountPanel?.IsEnabled) ?? false;
            if (isHideMainMenuParadoxAccountPanelEnabled)
            {
                var isPDXPanelVisible = (m_PDXPanel_field.Value.GetValue(__instance) as UIComponent)?.isVisible ?? false;
                if (isPDXPanelVisible) HideMainMenuParadoxAccountPanel?.Enable(true);
            }

            var isHideMainMenuWorkshopPanelEnabled = (HideMainMenuWorkshopPanel?.IsEnabled) ?? false;
            if (isHideMainMenuWorkshopPanelEnabled)
            {
                var isWorkshopPanelVisible = (m_WorkshopPanel_field.Value.GetValue(__instance) as UIComponent)?.isVisible ?? false;
                if (isWorkshopPanelVisible) HideMainMenuWorkshopPanel?.Enable(true);
            }
        }
    }
}