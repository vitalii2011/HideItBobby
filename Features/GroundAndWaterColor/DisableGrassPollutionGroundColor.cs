using ColossalFramework;
using HideItBobby.Common.Logging;
using HideItBobby.Features.GroundAndWaterColor.Shared;
using UnityEngine;

namespace HideItBobby.Features.GroundAndWaterColor
{
    internal sealed class DisableGrassPollutionGroundColor : FeatureBase
    {
        private const string ShaderName = "_GrassPollutionColorOffset";

        public override FeatureKey Key => FeatureKey.DisableGrassPollutionGroundColor;
        private Vector4 DefaultColorOffset;

        protected override bool EnableImpl()
        {
            if (!Singleton<TerrainManager>.exists)
            {
#if DEV
                Log.Warning($"{nameof(DisableGrassPollutionGroundColor)}.{nameof(EnableImpl)} instance of {nameof(TerrainManager)} does not exist");
#endif
                return false;
            }

            var properties = Singleton<TerrainManager>.instance.m_properties;
            if (DefaultColorOffset == GroundColorOffset.None) DefaultColorOffset = Shader.GetGlobalVector(ShaderName);
            Shader.SetGlobalVector(ShaderName, new Vector4(GroundColorOffset.None.x, GroundColorOffset.None.y, GroundColorOffset.None.z, properties.m_cliffSandNormalTiling));
            return true;
        }
        protected override bool DisableImpl()
        {
            if (!Singleton<TerrainManager>.exists)
            {
#if DEV
                Log.Warning($"{nameof(DisableGrassPollutionGroundColor)}.{nameof(DisableImpl)} instance of {nameof(TerrainManager)} does not exist");
#endif
                return false;
            }

            var properties = Singleton<TerrainManager>.instance.m_properties;
            Shader.SetGlobalVector(ShaderName, new Vector4(DefaultColorOffset.x, DefaultColorOffset.y, DefaultColorOffset.z, properties.m_cliffSandNormalTiling));
            return true;
        }

    }
}