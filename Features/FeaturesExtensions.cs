using HideItBobby.Common;

namespace HideItBobby.Features
{
    internal static class FeaturesExtensions
    {
        public static FeatureFlags Set(this IFeature feature, bool enable)
        {
            if (feature is null) return FeatureFlags.False;
            if (enable)
            {
                if (feature is IToggleable<FeatureFlags> toggleable && !toggleable.IsEnabled) return toggleable.Enable();
                if (feature is IUpdatable<FeatureFlags> updateable && !updateable.IsCurrent) return updateable.Update();
            }
            else
            {
                if (feature is IToggleable<FeatureFlags> toggleable && toggleable.IsEnabled) return toggleable.Disable();
            }
            return FeatureFlags.True;
        }

        public static FeatureFlags Run(this IFeature feature)
        {
            if (feature is null) return FeatureFlags.False;
            if (feature is IForceToggleable<FeatureFlags> toDisable && !toDisable.IsEnabled) return toDisable.Disable(true);
            if (feature is IForceUpdatable<FeatureFlags> toUpdate && !toUpdate.IsCurrent) return toUpdate.Update(true);
            if (feature is IForceToggleable<FeatureFlags> toEnable) return toEnable.Enable(true);
            return FeatureFlags.True;
        }
    }
}