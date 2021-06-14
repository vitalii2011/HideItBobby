using HideItBobby.Features.Ruining.Compatibility;

namespace HideItBobby.Features.Ruining
{
    internal sealed class HidePropRuining : FeatureBase
    {
        public override FeatureKey Key => FeatureKey.HidePropRuining;

        public override bool IsAvailable => HideRuiningCompatibility.Instance.IsCompatible;

        protected override bool EnableImpl()
        {
            PropInfo propInfo;
            for (uint i = 0; i < PrefabCollection<PropInfo>.LoadedCount(); i++)
            {
                propInfo = PrefabCollection<PropInfo>.GetPrefab(i);
                if (!(propInfo is null) && propInfo.m_createRuining)
                {
                    propInfo.m_createRuining = false;
                }
            }
            return true;
        }
        protected override bool DisableImpl()
        {
            PropInfo propInfo;
            for (uint i = 0; i < PrefabCollection<PropInfo>.LoadedCount(); i++)
            {
                propInfo = PrefabCollection<PropInfo>.GetPrefab(i);
                if (!(propInfo is null) && !propInfo.m_createRuining)
                {
                    propInfo.m_createRuining = true;
                }
            }
            return true;
        }
    }
}