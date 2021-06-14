using HideItBobby.Features.UIElements.Base;

namespace HideItBobby.Features.UIElements
{
    internal sealed class HideAdvisorButton : HideUIComponentByName
    {
        public override FeatureKey Key => FeatureKey.HideAdvisorButton;

        public HideAdvisorButton() : base("AdvisorButton") { }
    }
}