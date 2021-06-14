using HideItBobby.Features.GroundAndWaterColor.Shared;
using UnityEngine;

namespace HideItBobby.Features.GroundAndWaterColor
{
    internal sealed class DisableGrassFertilityGroundColor : FeatureBase
    {
        private const string ShaderName = "_GrassFertilityColorOffset";

        public override FeatureKey Key => FeatureKey.DisableGrassFertilityGroundColor;
        private Vector4 DefaultColorOffset = GroundColorOffset.None;

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