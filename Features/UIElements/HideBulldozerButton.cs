using HideItBobby.Features.UIElements.Base;

namespace HideItBobby.Features.UIElements
{
    internal sealed class HideBulldozerButton : HideUIComponentByName
    {
        public override FeatureKey Key => FeatureKey.HideBulldozerButton;

        public HideBulldozerButton() : base("BulldozerButton") { }
    }
}