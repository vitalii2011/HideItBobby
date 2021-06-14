using HideItBobby.Features.Menu.Base;

namespace HideItBobby.Features.Menu
{
    internal sealed class HideMainMenuLogo : HideMainMenuElement
    {
        public override FeatureKey Key => FeatureKey.HideMainMenuLogo;

        public HideMainMenuLogo() : base("Logo") { }
    }
}