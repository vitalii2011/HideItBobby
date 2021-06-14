using HideItBobby.Features.UIElements.Base;

namespace HideItBobby.Features.UIElements
{
    internal sealed class HideTimePanel : HideUIComponentByName
    {
        public override FeatureKey Key => FeatureKey.HideTimePanel;

        public HideTimePanel() : base("PanelTime") { }
    }
}