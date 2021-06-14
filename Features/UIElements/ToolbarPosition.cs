using HideItBobby.Features.UIElements.Base;
using HideItBobby.Settings;
using UnityEngine;

namespace HideItBobby.Features.UIElements
{
    internal sealed class ToolbarPosition : ModifyUIComponentPositionByName
    {
        public override FeatureKey Key => FeatureKey.ToolbarPosition;

        private readonly Vector3 defaultToolbarPosition = new Vector3(-1.056944f, -0.8314814f, 0);

        public ToolbarPosition() : base("MainToolstrip") { }

        protected override Vector3? GetDesiredComponentPosition() => new Vector3(defaultToolbarPosition.x + ModSettings.Data.ToolbarPosition, defaultToolbarPosition.y, defaultToolbarPosition.z);
        protected override Vector3? GetDefaultComponentPosition() => defaultToolbarPosition;
    }
}