using ColossalFramework.UI;
using HideItBobby.Common.Logging;
using HideItBobby.Features.UIElements.Base;
using UnityEngine;

namespace HideItBobby.Features.UIElements
{
    internal sealed class HideZoomAndUnlockBackground : HideUIComponent
    {
        public override FeatureKey Key => FeatureKey.HideZoomAndUnlockBackground;

        protected override UIComponent GetComponent()
        {
            var tsbar = GameObject.Find("TSBar")?.GetComponent<UIComponent>();
            if (tsbar is null)
            {
                IncreaseErrorCount();
#if DEV || PREVIEW
                Log.Warning($"{GetType().Name}.{nameof(GetComponent)} could not find TSBar, current error count is {ErrorCount}.");
#endif
                return null;
            }
            var sprite = tsbar.Find("Sprite")?.GetComponent<UIComponent>();
            if (sprite is null)
            {
                IncreaseErrorCount();
#if DEV || PREVIEW
                Log.Warning($"{GetType().Name}.{nameof(GetComponent)} could not find Sprite, current error count is {ErrorCount}.");
#endif
            }
            return sprite;
        }
    }
}