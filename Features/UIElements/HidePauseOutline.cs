using HideItBobby.Common.Logging;
using UnityEngine;

namespace HideItBobby.Features.UIElements
{
    internal sealed class HidePauseOutline : FeatureBase
    {
        public override FeatureKey Key => FeatureKey.HidePauseOutline;

        protected override bool EnableImpl()
        {
            var component = GetObject();
            if (component is null)
            {
                return false;
            }
            component.SetActive(false);
            return true;
        }
        protected override bool DisableImpl()
        {
            var component = GetObject();
            if (component is null)
            {
                return false;
            }
            component.SetActive(true);
            return true;
        }

        private GameObject GetObject()
        {
            var component = GameObject.Find("PauseOutline");
            if (component is null)
            {
                IncreaseErrorCount();
#if DEV || PREVIEW
                Log.Warning($"{nameof(HidePauseOutline)}.{nameof(GetObject)} could not find {nameof(PauseOutline)}, current error count is {ErrorCount}.");
#endif
            }
            return component;
        }
    }
}