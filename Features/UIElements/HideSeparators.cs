using ColossalFramework.UI;
using System.Collections.Generic;
using UnityEngine;

namespace HideItBobby.Features.UIElements
{
    internal sealed class HideSeparators : FeatureBase
    {
        public override FeatureKey Key => FeatureKey.HideSeparators;

        protected override bool EnableImpl()
        {
            foreach (var component in GetComponents())
            {
                if (!(component is null))
                {
                    component.isVisible = false;
                }
            }
            return true;
        }
        protected override bool DisableImpl()
        {
            foreach (var component in GetComponents())
            {
                if (!(component is null))
                {
                    component.isVisible = true;
                }
            }
            return true;
        }

        private IEnumerable<UIComponent> GetComponents()
        {
            var parent = GameObject
                .Find("MainToolstrip")
                ?.GetComponent<UIComponent>();

            if (parent is null) yield break;

            foreach (var component in parent.components)
            {
                if (!(component is null)
                    && (component.name.Equals("Separator")
                    || component.name.Equals("SmallSeparator")))
                {
                    yield return component;
                }
            }
        }
    }
}