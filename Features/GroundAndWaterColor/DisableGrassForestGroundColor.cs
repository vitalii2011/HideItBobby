using HideItBobby.Features.GroundAndWaterColor.Shared;
using UnityEngine;

namespace HideItBobby.Features.GroundAndWaterColor
{
    internal sealed class DisableGrassForestGroundColor : FeatureBase
    {
        private const string ShaderName = "_GrassForestColorOffset";

        public override FeatureKey Key => FeatureKey.DisableGrassForestGroundColor;
        private Vector4 DefaultColorOffset;

        protected override bool EnableImpl()
        {
            if (DefaultColorOffset == GroundColorOffset.None) DefaultColorOffset = Shader.GetGlobalVector(ShaderName);
            Shader.SetGlobalVector(ShaderName, GroundColorOffset.None);
            return true;
        }
        protected override bool DisableImpl()
        {
            Shader.SetGlobalVector(ShaderName, DefaultColorOffset);
            return true;
        }

    }
}