using HideItBobby.Common;
using HideItBobby.Common.Logging;
using HideItBobby.Features;
using System;
using static HideItBobby.Common.Patcher;
using IFeaturesContainer = System.Collections.Generic.IDictionary<HideItBobby.Features.FeatureKey, HideItBobby.Features.IFeature>;
using IFeaturesSettingsContainer = System.Collections.Generic.IDictionary<HideItBobby.Features.FeatureKey, System.Func<bool>>;

namespace HideItBobby.EntryPoints
{
    internal static class DictionaryExtensions
    {
        #region IFeaturesContainer
        public static IFeaturesContainer Register<TFeature>(this IFeaturesContainer instance)
            where TFeature : IFeature, new()
        {
            if (instance is null) throw new ArgumentNullException(nameof(instance), $"Parameter {nameof(instance)} cannot be null");

            var feature = new TFeature();
            if (instance.TryGetValue(feature.Key, out IFeature existingFeature))
            {
#if DEV
                Log.Warning($"{nameof(DictionaryExtensions)}.{nameof(Resolve)} feature {feature.Key} registered twice");
#endif
                if (existingFeature is IDisposable disposable) disposable.Dispose();
            }
            instance[feature.Key] = feature;
            return instance;
        }

        public static IFeature Resolve(this IFeaturesContainer instance, FeatureKey key)
        {
            if (instance is null) throw new ArgumentNullException(nameof(instance), $"Parameter {nameof(instance)} cannot be null");
            if (instance.TryGetValue(key, out IFeature feature)) return feature;
#if DEV
            Log.Warning($"{nameof(DictionaryExtensions)}.{nameof(Resolve)} trying to resolve missing feature {key}");
#endif
            return null;
        }

        public static void InitializeAll(this IFeaturesContainer instance)
        {
            if (instance is null) throw new ArgumentNullException(nameof(instance), $"Parameter {nameof(instance)} cannot be null");

            foreach (var feature in instance.Values)
                if (feature is IInitializable initializable)
                    initializable.Initialize();
        }

        public static void TerminateAll(this IFeaturesContainer instance)
        {
            if (instance is null) throw new ArgumentNullException(nameof(instance), $"Parameter {nameof(instance)} cannot be null");

            foreach (var feature in instance.Values)
                if (feature is IToggleable<FeatureFlags> toggleable)
                {
#if DEV
                    Log.Info($"disabled {feature.Key}, result: {toggleable.Disable()}");
#else
                    toggleable.Disable();
#endif
                }
            foreach (var feature in instance.Values)
                if (feature is IInitializable<FeatureFlags> initializable)
                {
#if DEV
                    Log.Info($"terminated {feature.Key}, result: {initializable.Terminate()}");
#else
                    initializable.Terminate();
#endif
                }
        }

        public static void ResetErrors(this IFeaturesContainer instance)
        {
            if (instance is null) throw new ArgumentNullException(nameof(instance), $"Parameter {nameof(instance)} cannot be null");

            foreach (var feature in instance.Values)
                if (feature is IErrorInfo errorInfo)
                    errorInfo.IsError = false;
        }
        #endregion

        #region IFeaturesSettingsContainer
        public static IFeaturesSettingsContainer Register(this IFeaturesSettingsContainer instance, FeatureKey key, Func<bool> function)
        {
            if (instance is null) throw new ArgumentNullException(nameof(instance), $"Parameter {nameof(instance)} cannot be null");

            instance[key] = function;
            return instance;
        }

        public static Func<bool> Resolve(this IFeaturesSettingsContainer instance, FeatureKey key)
        {
            if (instance is null) throw new ArgumentNullException(nameof(instance), $"Parameter {nameof(instance)} cannot be null");

            if (instance.TryGetValue(key, out Func<bool> function)) return function;
#if DEV
            Log.Warning($"{nameof(DictionaryExtensions)}.{nameof(Resolve)} trying to resolve missing feature settings {key}");
#endif
            return null;
        }

        public static bool Get(this IFeaturesSettingsContainer instance, FeatureKey key)
        {
            if (instance is null) return false;
            var function = instance.Resolve(key);
            if (function is null) return false;
            return function();
        }
        #endregion
    }
}