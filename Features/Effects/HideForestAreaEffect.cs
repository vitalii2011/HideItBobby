using HideItBobby.Features.Effects.Shared.Patches;
using static HideItBobby.Common.Patcher;

namespace HideItBobby.Features.Effects
{
    internal sealed class HideForestAreaEffect : FeatureBase
    {
        public override FeatureKey Key => FeatureKey.HideForestAreaEffect;

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