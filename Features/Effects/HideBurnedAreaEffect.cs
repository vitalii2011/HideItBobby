using HideItBobby.Features.Effects.Shared.Patches;
using static HideItBobby.Common.Patcher;

namespace HideItBobby.Features.Effects
{
    internal sealed class HideBurnedAreaEffect : FeatureBase
    {
        public override FeatureKey Key => FeatureKey.HideBurnedAreaEffect;

        public override bool IsInitialized => NaturalResourceManagerUpdateTextureBPatch.Data.IsPatchApplied;

        protected override bool InitializeImpl()
        {
            Patch(NaturalResourceManagerUpdateTextureBPatch.Data);
            return true;
        }

        protected override bool TerminateImpl()
        {
            Unpatch(NaturalResourceManagerUpdateTextureBPatch.Data);
            return true;
        }
    }
}