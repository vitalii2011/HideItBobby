using HideItBobby.Features.GroundAndWaterColor.Shared;
using UnityEngine;

namespace HideItBobby.Features.GroundAndWaterColor
{
    internal sealed class DisableGrassFieldGroundColor : FeatureBase
    {
        private const string ShaderName = "_GrassFieldColorOffset";

        public override FeatureKey Key => FeatureKey.DisableGrassFieldGroundColor;
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