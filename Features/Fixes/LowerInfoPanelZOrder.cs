using ColossalFramework.UI;
using HideItBobby.Common;
using HideItBobby.Common.Logging;
using UnityEngine;

namespace HideItBobby.Features.Fixes
{
    internal sealed class LowerInfoPanelZOrder : FeatureBase
    {
        public override FeatureKey Key => FeatureKey.LowerInfoPanelZOrder;

        private int _originalZOrder = 22;
        private readonly Cached<UIPanel> Component;

        public LowerInfoPanelZOrder() : base()
        {
            Component = new Cached<UIPanel>(GetComponent);
        }

        protected override bool EnableImpl()
        {
            var infoPanelUIPanel = Component.Value;
            if (infoPanelUIPanel is null) return false;
            _originalZOrder = infoPanelUIPanel.zOrder;
            infoPanelUIPanel.zOrder = 1;
            return true;
        }
        protected override bool DisableImpl()
        {
            var infoPanelUIPanel = Component.Value;
            if (infoPanelUIPanel is null) return false;
            infoPanelUIPanel.zOrder = _originalZOrder;
            Component.Invalidate();
            return true;
        }

        private UIPanel GetComponent()
        {
            var infoPanel = GameObject.Find("InfoPanel")?.GetComponent<UIComponent>();
            if (infoPanel is null)
            {
                IncreaseErrorCount();
#if DEV || PREVIEW
                Log.Warning($"{nameof(LowerInfoPanelZOrder)}.{nameof(GetComponent)} could not find InfoPanel, current error count is {ErrorCount}.");
#endif
                return null;
            }
            var panel = infoPanel.GetComponentInChildren<UIPanel>();
            if (panel is null)
            {
                IncreaseErrorCount();
#if DEV || PREVIEW
                Log.Warning($"{nameof(LowerInfoPanelZOrder)}.{nameof(GetComponent)} could not find {nameof(UIPanel)}, current error count is {ErrorCount}.");
#endif
            }
            return panel;
        }
    }
}