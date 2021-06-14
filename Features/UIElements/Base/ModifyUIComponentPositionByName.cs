using HideItBobby.Common.Logging;
using UnityEngine;

namespace HideItBobby.Features.UIElements.Base
{
    internal abstract class ModifyUIComponentPositionByName : ModifyUIComponentPosition
    {
        protected virtual string ComponentName { get; private set; }

        public ModifyUIComponentPositionByName(string componentName)
        {
            ComponentName = componentName;
        }

        protected override GameObject GetGameObject()
        {
            var component = UnityEngine.GameObject.Find(ComponentName);
            if (component is null)
            {
                IncreaseErrorCount();
#if DEV || PREVIEW
                Log.Warning($"{GetType().Name}.{nameof(GetGameObject)} could not find {ComponentName}, current error count is {ErrorCount}.");
#endif
            }
            return component;
        }
    }
}