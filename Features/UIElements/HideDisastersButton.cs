using HideItBobby.Features.UIElements.Base;
using HideItBobby.Features.UIElements.Compatibility;

namespace HideItBobby.Features.UIElements
{
    internal sealed class HideDisastersButton : HideUIComponentByName
    {
        public override bool IsAvailable => HideDisastersButtonCompatibility.Instance.IsCompatible;
        public override FeatureKey Key => FeatureKey.HideDisastersButton;

        public HideDisastersButton() : base("WarningPhasePanel") { }
    }
}