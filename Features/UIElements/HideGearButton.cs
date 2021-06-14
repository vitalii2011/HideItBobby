using HideItBobby.Features.UIElements.Base;

namespace HideItBobby.Features.UIElements
{
    internal sealed class HideGearButton : HideUIComponentByName
    {
        public override FeatureKey Key => FeatureKey.HideGearButton;

        public HideGearButton() : base("Esc") { }
    }
}