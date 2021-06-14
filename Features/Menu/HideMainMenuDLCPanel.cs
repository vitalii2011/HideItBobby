using ColossalFramework.UI;
using HideItBobby.Common.Logging;
using HideItBobby.Features.Menu.Shared.Patches;
using HideItBobby.Features.UIElements.Base;
using static HideItBobby.Common.Patcher;

namespace HideItBobby.Features.Menu
{
    internal sealed class HideMainMenuDLCPanel : HideUIComponent
    {
        public override FeatureKey Key => FeatureKey.HideMainMenuDLCPanel;

        public override bool IsInitialized
            => HideMainMenuItemsCreditsEndedPatch.Data.IsPatchApplied
            && HideMainMenuItemsOnVisibilityChangedPatch.Data.IsPatchApplied;

        public HideMainMenuDLCPanel() : base() { }

        protected override bool InitializeImpl()
        {
            Patch(HideMainMenuItemsCreditsEndedPatch.Data);
            Patch(HideMainMenuItemsOnVisibilityChangedPatch.Data);
            return true;
        }
        protected override bool TerminateImpl()
        {
            Unpatch(HideMainMenuItemsCreditsEndedPatch.Data);
            Unpatch(HideMainMenuItemsOnVisibilityChangedPatch.Data);
            return true;
        }

        protected override UIComponent GetComponent()
        {
            var menuContainer = UIView.Find("MenuContainer")?.GetComponent<UIPanel>();
            if (menuContainer is null)
            {
                IncreaseErrorCount();
#if DEV || PREVIEW
                Log.Warning($"{nameof(HideMainMenuDLCPanel)}.{nameof(GetComponent)} could not find MenuContainer.");
#endif
                return null;
            }
            var dlcPanelNew = menuContainer.Find("DLCPanelNew")?.GetComponent<UIComponent>();
#if DEV || PREVIEW
            if (dlcPanelNew is null)
            {
                Log.Warning($"{nameof(HideMainMenuDLCPanel)}.{nameof(GetComponent)} could not find DLCPanelNew.");
            }
#endif
            if (!(dlcPanelNew is null)) return dlcPanelNew;
            var dlcPanel = menuContainer.Find("DLCPanel")?.GetComponent<UIComponent>();

            if (dlcPanel is null)
            {
                IncreaseErrorCount();
#if DEV || PREVIEW
                Log.Warning($"{nameof(HideMainMenuDLCPanel)}.{nameof(GetComponent)} could not find DLCPanel.");
#endif
            }

            return dlcPanel;
        }
    }
}