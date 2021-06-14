using UnityEngine;

namespace HideItBobby.Features.Effects
{
    internal sealed class HideVolumeFog : FeatureBase
    {
        public override FeatureKey Key => FeatureKey.HideVolumeFog;
        private FogProperties FogProperties;
        private float Default;

        protected override bool InitializeImpl()
        {
            FogProperties = Object.FindObjectOfType<FogProperties>();
            if (FogProperties is null) return false;

            Default = FogProperties.m_FogDensity;
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

            FogProperties.m_FogDensity = 0f;
            return true;
        }
        protected override bool DisableImpl()
        {
            if (FogProperties is null) return false;

            FogProperties.m_FogDensity = Default;
            return true;
        }

    }
}