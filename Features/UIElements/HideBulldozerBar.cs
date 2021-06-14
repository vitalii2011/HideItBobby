using HideItBobby.Common.Logging;
using UnityEngine;

namespace HideItBobby.Features.UIElements
{
    internal sealed class HideBulldozerBar : FeatureBase
    {
        public override FeatureKey Key => FeatureKey.HideBulldozerBar;

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
            var component = GameObject.Find("BulldozerBar");
            if (component is null)
            {
                IncreaseErrorCount();
#if DEV || PREVIEW
                Log.Warning($"{nameof(HideBulldozerBar)}.{nameof(GetObject)} could not find BulldozerBar, current error count is {ErrorCount}.");
#endif
            }
            return component;
        }
    }
}