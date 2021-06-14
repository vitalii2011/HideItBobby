using ColossalFramework.UI;
using HideItBobby.Common;

namespace HideItBobby.Features.UIElements.Base
{
    internal abstract class HideUIComponent : FeatureBase
    {
        protected readonly Cached<UIComponent> Component;

        public HideUIComponent()
        {
            Component = new Cached<UIComponent>(GetComponent);
        }

        protected override bool EnableImpl()
        {
            var component = Component.Value;
            if (component is null) return false;
            component.isVisible = false;
            return true;
        }
        protected override bool DisableImpl()
        {
            var component = Component.Value;
            if (component is null) return false;
            component.isVisible = true;
            Component.Invalidate();
            return true;
        }

        protected abstract UIComponent GetComponent();
    }
}