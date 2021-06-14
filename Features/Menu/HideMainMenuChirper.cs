using HideItBobby.Features.Menu.Base;

namespace HideItBobby.Features.Menu
{
    internal sealed class HideMainMenuChirper : HideMainMenuElement
    {
        public override FeatureKey Key => FeatureKey.HideMainMenuChirper;

        public HideMainMenuChirper() : base("Chirper") { }
    }
}