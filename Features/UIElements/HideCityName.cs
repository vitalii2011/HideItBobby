using ColossalFramework.UI;
using HideItBobby.Common.Logging;
using UnityEngine;

namespace HideItBobby.Features.UIElements
{
    internal sealed class HideCityName : FeatureBase
    {
        public override FeatureKey Key => FeatureKey.HideCityName;

        private string _backgroundSprite = null;
        private string _text = null;

        protected override bool EnableImpl()
        {
            var cityNameLabel = GetComponent();
            if (cityNameLabel is null) return false;
            _backgroundSprite = cityNameLabel.backgroundSprite;
            cityNameLabel.backgroundSprite = null;
            _text = cityNameLabel.text;
            cityNameLabel.text = "";
            return true;
        }
        protected override bool DisableImpl()
        {
            var cityNameLabel = GetComponent();
            if (cityNameLabel is null) return false;
            cityNameLabel.backgroundSprite = _backgroundSprite;
            cityNameLabel.text = _text;
            return true;
        }

        private UILabel GetComponent()
        {
            var infoPanel = GameObject.Find("InfoPanel")?.GetComponent<UIComponent>();
            if (infoPanel is null)
            {
                IncreaseErrorCount();
#if DEV || PREVIEW
                Log.Warning($"{GetType().Name}.{nameof(GetComponent)} could not find InfoPanel, current error count is {ErrorCount}.");
#endif
                return null;
            }
            var name = infoPanel.Find("Name")?.GetComponentInChildren<UILabel>();
            if (name is null)
            {
                IncreaseErrorCount();
#if DEV || PREVIEW
                Log.Warning($"{GetType().Name}.{nameof(GetComponent)} could not find Name, current error count is {ErrorCount}.");
#endif
            }
            return name;
        }
    }
}