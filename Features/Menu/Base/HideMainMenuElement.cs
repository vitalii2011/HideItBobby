using ColossalFramework.UI;
using HideItBobby.Common.Logging;
using HideItBobby.Features.UIElements.Base;

namespace HideItBobby.Features.Menu.Base
{
    internal abstract class HideMainMenuElement : HideUIComponentByName
    {
        public HideMainMenuElement(string componentName) : base(componentName) { }

        protected override UIComponent GetComponent()
        {
            var menuContainer = UIView.Find("MenuContainer")?.GetComponent<UIPanel>();
            if (menuContainer is null)
            {
#if DEV
                Log.Warning($"{GetType().Name}.{nameof(GetComponent)} could not find MenuContainer.");
#endif
                return null;
            }
            var component = menuContainer.Find(ComponentName)?.GetComponent<UIComponent>();
#if DEV
            if (component is null)
            {
                Log.Warning($"{GetType().Name}.{nameof(GetComponent)} could not find {ComponentName}.");
            }
#endif
            return component;
        }
    }
}