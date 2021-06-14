using HideItBobby.Features.Effects.Shared.Patches;
using static HideItBobby.Common.Patcher;

namespace HideItBobby.Features.Effects
{
    internal sealed class HideOreAreaEffect : FeatureBase
    {
        public override FeatureKey Key => FeatureKey.HideOreAreaEffect;

        public override bool IsInitialized => NaturalResourceManagerUpdateTexturePatch.Data.IsPatchApplied;

        protected override bool InitializeImpl()
        {
            Patch(NaturalResourceManagerUpdateTexturePatch.Data);
            return true;
        }

        protected override bool TerminateImpl()
        {
            Unpatch(NaturalResourceManagerUpdateTexturePatch.Data);
            return true;
        }
    }
}