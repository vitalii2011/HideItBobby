using HideItBobby.Common;
using UnityEngine;

namespace HideItBobby.Features.UIElements.Base
{
    internal abstract class ModifyUIComponentPosition : UpdatableFeatureBase
    {
        protected virtual Vector3? ComponentPosition
        {
            get
            {
                return GameObject.Value?.transform.position;
            }
            set
            {
                var obj = GameObject.Value;
                if (!(obj is null) && value.HasValue) obj.transform.position = value.Value;
            }
        }
        protected readonly Cached<GameObject> GameObject;

        public ModifyUIComponentPosition()
        {
            GameObject = new Cached<GameObject>(GetGameObject);
        }

        protected abstract GameObject GetGameObject();
        protected abstract Vector3? GetDesiredComponentPosition();
        protected abstract Vector3? GetDefaultComponentPosition();

        #region Updatable
        public override bool IsCurrent
        {
            get
            {
                if (!IsEnabled) return true;
                var currentPosition = ComponentPosition;
                var desiredPosition = GetDesiredComponentPosition();
                if (!currentPosition.HasValue || !desiredPosition.HasValue) return false;
                return currentPosition.Value == desiredPosition.Value;
            }
        }
        protected override bool UpdateImpl()
        {
            ComponentPosition = GetDesiredComponentPosition();
            return IsCurrent;
        }
        #endregion

        #region Togglable
        protected override bool EnableImpl()
        {
            ComponentPosition = GetDesiredComponentPosition();
            return IsCurrent;
        }
        protected override bool DisableImpl()
        {
            var defaultposition = GetDefaultComponentPosition();
            ComponentPosition = defaultposition;

            var currentPosition = ComponentPosition;
            if (!currentPosition.HasValue || !defaultposition.HasValue) return false;
            var result = currentPosition.Value == defaultposition.Value;
            if (result) GameObject.Invalidate();
            return result;
        } 
        #endregion
    }
}