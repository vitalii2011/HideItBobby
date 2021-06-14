using HideItBobby.Features.UIElements.Base;

namespace HideItBobby.Features.UIElements
{
    internal sealed class HideChirperButton : HideUIComponentByName
    {
        public override FeatureKey Key => FeatureKey.HideChirperButton;

        public HideChirperButton() : base("ChirperPanel") { }
    }
}