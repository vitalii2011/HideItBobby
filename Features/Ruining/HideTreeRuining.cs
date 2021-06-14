using HideItBobby.Features.Ruining.Compatibility;

namespace HideItBobby.Features.Ruining
{
    internal sealed class HideTreeRuining : FeatureBase
    {
        public override FeatureKey Key => FeatureKey.HideTreeRuining;

        public override bool IsAvailable => HideRuiningCompatibility.Instance.IsCompatible;

        protected override bool EnableImpl()
        {
            TreeInfo treeInfo;
            for (uint i = 0; i < PrefabCollection<TreeInfo>.LoadedCount(); i++)
            {
                treeInfo = PrefabCollection<TreeInfo>.GetPrefab(i);
                if (!(treeInfo is null) && treeInfo.m_createRuining)
                {
                    treeInfo.m_createRuining = false;
                }
            }
            return true;
        }
        protected override bool DisableImpl()
        {
            TreeInfo treeInfo;
            for (uint i = 0; i < PrefabCollection<TreeInfo>.LoadedCount(); i++)
            {
                treeInfo = PrefabCollection<TreeInfo>.GetPrefab(i);
                if (!(treeInfo is null) && !treeInfo.m_createRuining)
                {
                    treeInfo.m_createRuining = true;
                }
            }
            return true;
        }
    }
}