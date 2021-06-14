using HideItBobby.Common;
using HideItBobby.Common.Logging;
using HideItBobby.Features.UIElements.Base;
using HideItBobby.Features.UIElements.Compatibility;
using HideItBobby.Settings;
using UnityEngine;

namespace HideItBobby.Features.UIElements
{
    internal sealed class HideThermometer : ModifyUIComponentPositionByName
    {
        public override bool IsAvailable => HideThermometerCompatibility.Instance.IsCompatible;
        public override FeatureKey Key => FeatureKey.HideThermometer;

        private readonly Vector3 defaultThermomenterPosition = new Vector3(-0.2209259f, -0.9351852f, 0);

        public HideThermometer() : base("Heat'o'meter")
        {
            TreeAnarchyPanelObject = new Cached<GameObject>(GetTreeAnarchyPanelObject);
        }

        protected override Vector3? GetDesiredComponentPosition() => new Vector3(defaultThermomenterPosition.x, ModSettings.Data.HideThermometer ? defaultThermomenterPosition.y - 0.1f : defaultThermomenterPosition.y, defaultThermomenterPosition.z);
        protected override Vector3? GetDefaultComponentPosition() => defaultThermomenterPosition;

        #region TreeAnarchy mod compatibility
        private readonly Vector3 treeAnarchyPanelPositionOnEnabled = new Vector3(-0.2f, 0.1f, 0f);
        private readonly Vector3 treeAnarchyPanelPositionOnDisabled = new Vector3(-0.2f, 0.0f, 0f);

        private readonly Cached<GameObject> TreeAnarchyPanelObject;

        public override bool IsCurrent
        {
            get
            {
                if (!base.IsCurrent) return false;

                var treeAnarchyPanel = TreeAnarchyPanelObject.Value;
                var isTreeAnarchyPanelPositionCurrent = treeAnarchyPanel is null || treeAnarchyPanel.transform.localPosition == (ModSettings.Data.HideThermometer ? treeAnarchyPanelPositionOnEnabled : treeAnarchyPanelPositionOnDisabled);
                return isTreeAnarchyPanelPositionCurrent;
            }
        }

        protected override bool UpdateImpl()
        {
            var result = base.UpdateImpl();
            var treeAnarchyPanel = TreeAnarchyPanelObject.Value;
            if (!(treeAnarchyPanel is null))
            {
                treeAnarchyPanel.transform.localPosition = ModSettings.Data.HideThermometer ? treeAnarchyPanelPositionOnEnabled : treeAnarchyPanelPositionOnDisabled;
            }
            return result;
        }
        protected override bool EnableImpl()
        {
            var result = base.EnableImpl();
            var treeAnarchyPanel = TreeAnarchyPanelObject.Value;
            if (!(treeAnarchyPanel is null))
            {
                treeAnarchyPanel.transform.localPosition = treeAnarchyPanelPositionOnEnabled;
            }
            return result;
        }
        protected override bool DisableImpl()
        {
            var treeAnarchyPanel = TreeAnarchyPanelObject.Value;
            if (!(treeAnarchyPanel is null))
            {
                treeAnarchyPanel.transform.localPosition = treeAnarchyPanelPositionOnDisabled;
            }
            var result = base.DisableImpl();
            if (result) TreeAnarchyPanelObject.Invalidate();
            return result;
        }

        private GameObject GetTreeAnarchyPanelObject()
        {
            var obj = UnityEngine.GameObject.Find("TreeAnarchyIndicatorPanel");
#if DEV || PREVIEW
            if (obj is null)
            {
                Log.Info($"{nameof(HideThermometer)}.{nameof(GetTreeAnarchyPanelObject)} could not find TreeAnarchyIndicatorPanel.");
            }
#endif
            return obj;
        }
        #endregion
    }
}