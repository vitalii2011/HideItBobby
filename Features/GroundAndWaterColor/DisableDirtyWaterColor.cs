
using ColossalFramework;
using HideItBobby.Common.Logging;
using UnityEngine;

namespace HideItBobby.Features.GroundAndWaterColor
{
    internal sealed class DisableDirtyWaterColor : FeatureBase
    {
        public override FeatureKey Key => FeatureKey.DisableDirtyWaterColor;

        protected override bool EnableImpl()
        {
            if (!Singleton<TerrainManager>.exists)
            {
#if DEV
                Log.Warning($"{nameof(DisableDirtyWaterColor)}.{nameof(EnableImpl)} instance of {nameof(TerrainManager)} does not exist");
#endif
                return false;
            }

            var properties = Singleton<TerrainManager>.instance.m_properties;
            var cleanWaterColor = new Color(
                properties.m_waterColorClean.r,
                properties.m_waterColorClean.g,
                properties.m_waterColorClean.b,
                properties.m_waterRainFoam);
            Shader.SetGlobalColor("_WaterColorDirty", cleanWaterColor);
            return true;
        }
        protected override bool DisableImpl()
        {
            if (!Singleton<TerrainManager>.exists)
            {
#if DEV
                Log.Warning($"{nameof(DisableDirtyWaterColor)}.{nameof(DisableImpl)} instance of {nameof(TerrainManager)} does not exist");
#endif
                return false;
            }

            var properties = Singleton<TerrainManager>.instance.m_properties;
            Shader.SetGlobalColor("_WaterColorDirty", properties.m_waterColorDirty);
            return true;
        }

    }
}