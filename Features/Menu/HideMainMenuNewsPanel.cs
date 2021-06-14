using HideItBobby.Features.Menu.Base;
using HideItBobby.Features.Menu.Shared.Patches;
using static HideItBobby.Common.Patcher;

namespace HideItBobby.Features.Menu
{
    internal sealed class HideMainMenuNewsPanel : HideMainMenuElement
    {
        public override FeatureKey Key => FeatureKey.HideMainMenuNewsPanel;

        public override bool IsInitialized
            => HideMainMenuItemsCreditsEndedPatch.Data.IsPatchApplied
            && HideMainMenuItemsOnVisibilityChangedPatch.Data.IsPatchApplied;

        public HideMainMenuNewsPanel() : base("NewsFeedPanel") { }

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
    }
}