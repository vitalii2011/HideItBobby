using ColossalFramework.UI;
using HideItBobby.Common.Logging;
using UnityEngine;

namespace HideItBobby.Features.UIElements.Base
{
    internal abstract class HideUIComponentByName : HideUIComponent
    {
        protected virtual string ComponentName { get; private set; }

        public HideUIComponentByName(string componentName) : base()
        {
            ComponentName = componentName;
        }

        protected override UIComponent GetComponent()
        {
            var component = GameObject.Find(ComponentName)?.GetComponent<UIComponent>();
            if (component is null)
            {
                IncreaseErrorCount();
#if DEV || PREVIEW
                Log.Warning($"{GetType().Name}.{nameof(GetComponent)} could not find {ComponentName}, current error count is {ErrorCount}.");
#endif
            }
            return component;
        }
    }
}