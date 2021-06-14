using UnityEngine;

namespace HideItBobby.Features.Effects
{
    internal sealed class HideDistanceFog : FeatureBase
    {
        public override FeatureKey Key => FeatureKey.HideDistanceFog;
        private FogProperties FogProperties;
        private float Default;

        protected override bool InitializeImpl()
        {
            FogProperties = Object.FindObjectOfType<FogProperties>();
            if (FogProperties is null) return false;

            Default = FogProperties.m_ColorDecay;
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

            FogProperties.m_ColorDecay = 1f;
            return true;
        }
        protected override bool DisableImpl()
        {
            if (FogProperties is null) return false;

            FogProperties.m_ColorDecay = Default;
            return true;
        }

    }
}