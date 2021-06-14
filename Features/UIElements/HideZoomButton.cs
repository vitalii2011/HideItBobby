using HideItBobby.Features.UIElements.Base;

namespace HideItBobby.Features.UIElements
{
    internal sealed class HideZoomButton : HideUIComponentByName
    {
        public override FeatureKey Key => FeatureKey.HideZoomButton;

        public HideZoomButton() : base("ZoomComposite") { }
    }
}