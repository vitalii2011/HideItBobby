using CitiesHarmony.API;
using HarmonyLib;
using System;

namespace HideItBobby.Features.Ruining.Compatibility
{
    internal sealed class HideRuiningCompatibility : CompatibilityCheckBase
    {
        #region Instance
        private static readonly Lazy<HideRuiningCompatibility> _instance = new Lazy<HideRuiningCompatibility>(() => new HideRuiningCompatibility());
        public static HideRuiningCompatibility Instance { get => _instance.Value; } 
        #endregion

        private const string bobHarmonyId = "com.github.algernon-A.csl.bob";

        private static readonly Type treeInfoType = typeof(TreeInfo);
        private static readonly Type propInfoType = typeof(PropInfo);

        protected override bool CheckCompatibility()
        {
            if (!HarmonyHelper.IsHarmonyInstalled) return true;
            if (!Harmony.HasAnyPatches(bobHarmonyId)) return true;

            var harmony = new Harmony(bobHarmonyId);
            var methods = harmony.GetPatchedMethods();
            foreach (var method in methods)
            {
                if ((method.DeclaringType == treeInfoType
                    || method.DeclaringType == propInfoType)
                    && method.Name == "InitializePrefab")
                {
                    return false;
                }
            }
            return true;
        }
    }
}