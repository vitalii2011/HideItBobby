using HideItBobby.Features.UIElements.Base;

namespace HideItBobby.Features.UIElements
{
    internal sealed class HideRadioButton : HideUIComponentByName
    {
        public override FeatureKey Key => FeatureKey.HideRadioButton;

        public HideRadioButton() : base("RadioButton") { }
    }
}