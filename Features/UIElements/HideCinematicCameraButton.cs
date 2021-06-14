using HideItBobby.Features.UIElements.Base;

namespace HideItBobby.Features.UIElements
{
    internal sealed class HideCinematicCameraButton : HideUIComponentByName
    {
        public override FeatureKey Key => FeatureKey.HideCinematicCameraButton;

        public HideCinematicCameraButton() : base("CinematicCameraPanel") { }
    }
}