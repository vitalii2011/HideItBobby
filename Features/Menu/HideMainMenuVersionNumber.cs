using HideItBobby.Features.UIElements.Base;

namespace HideItBobby.Features.Menu
{
    internal sealed class HideMainMenuVersionNumber : HideUIComponentByName
    {
        public override FeatureKey Key => FeatureKey.HideMainMenuVersionNumber;

        public HideMainMenuVersionNumber() : base("VersionNumber") { }
    }
}