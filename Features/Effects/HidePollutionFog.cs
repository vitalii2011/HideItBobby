
using UnityEngine;

namespace HideItBobby.Features.Effects
{
    internal sealed class HidePollutionFog : FeatureBase
    {
        public override FeatureKey Key => FeatureKey.HidePollutionFog;
        private FogProperties FogProperties;
        private float Default;

        protected override bool InitializeImpl()
        {
            FogProperties = Object.FindObjectOfType<FogProperties>();
            if (FogProperties is null) return false;

            Default = FogProperties.m_PollutionAmount;
            return true;
        }
        protected override bool TerminateImpl()
        {
            FogProperties = null;
            return true;
        }

        protected override bool EnableImpl()
        {
            if (FogProperties is null) return false;

            FogProperties.m_PollutionAmount = 0f;
            return true;
        }

        protected override bool DisableImpl()
        {
            if (FogProperties is null) return false;

            FogProperties.m_PollutionAmount = Default;
            return true;
        }

    }
}