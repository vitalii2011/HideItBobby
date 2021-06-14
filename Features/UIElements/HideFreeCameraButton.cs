using HideItBobby.Features.UIElements.Base;

namespace HideItBobby.Features.UIElements
{
    internal sealed class HideFreeCameraButton : HideUIComponentByName
    {
        public override FeatureKey Key => FeatureKey.HideFreeCameraButton;

        public HideFreeCameraButton() : base("Freecamera") { }
    }
}