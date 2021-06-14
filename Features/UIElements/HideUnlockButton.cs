using HideItBobby.Features.UIElements.Base;

namespace HideItBobby.Features.UIElements
{
    internal sealed class HideUnlockButton : HideUIComponentByName
    {
        public override FeatureKey Key => FeatureKey.HideUnlockButton;

        public HideUnlockButton() : base("UnlockButton") { }
    }
}