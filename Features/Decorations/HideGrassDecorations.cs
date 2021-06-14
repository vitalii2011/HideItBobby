using UnityEngine;

namespace HideItBobby.Features.Decorations
{
    internal sealed class HideGrassDecorations : FeatureBase
    {
        public override FeatureKey Key => FeatureKey.HideGrassDecorations;
        private TerrainProperties TerrainProperties;

        protected override bool InitializeImpl()
        {
            TerrainProperties = Object.FindObjectOfType<TerrainProperties>();
            return !(TerrainProperties is null);
        }
        protected override bool TerminateImpl()
        {
            TerrainProperties = null;
            return true;
        }

        protected override bool EnableImpl()
        {
            if (TerrainProperties is null) return false;

            TerrainProperties.m_useGrassDecorations = false;
            return true;
        }
        protected override bool DisableImpl()
        {
            if (TerrainProperties is null) return false;

            TerrainProperties.m_useGrassDecorations = true;
            return true;
        }
    }
}