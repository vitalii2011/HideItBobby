using UnityEngine;

namespace HideItBobby.Features.Effects
{
    internal sealed class HideEdgeFog : FeatureBase
    {
        public override FeatureKey Key => FeatureKey.HideEdgeFog;
        private FogProperties FogProperties;

        protected override bool InitializeImpl()
        {
            FogProperties = Object.FindObjectOfType<FogProperties>();
            return !(FogProperties is null);
        }
        protected override bool TerminateImpl()
        {
            FogProperties = null;
            return true;
        }

        protected override bool EnableImpl()
        {
            if (FogProperties is null) return false;

            FogProperties.m_edgeFog = false;
            return true;
        }
        protected override bool DisableImpl()
        {
            if (FogProperties is null) return false;

            FogProperties.m_edgeFog = true;
            return true;
        }

    }
}