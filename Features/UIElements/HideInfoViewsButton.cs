using HideItBobby.Features.UIElements.Base;

namespace HideItBobby.Features.UIElements
{
    internal sealed class HideInfoViewsButton : HideUIComponentByName
    {
        public override FeatureKey Key => FeatureKey.HideInfoViewsButton;

        public HideInfoViewsButton() : base("InfoMenu") { }
    }
}