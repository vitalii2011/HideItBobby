using HarmonyLib;
using HideItBobby.Common.Logging;
using HideItBobby.Properties;
using System;
using System.Collections.Generic;
using System.Reflection;
using static CitiesHarmony.API.HarmonyHelper;

namespace HideItBobby.Common
{
    internal sealed class PatchData
    {
        public readonly string PatchId;
        public readonly Lazy<MethodInfo> Target;
        public readonly Lazy<MethodInfo> Prefix;
        public readonly Lazy<MethodInfo> Postfix;
        public readonly Lazy<MethodInfo> Transpiler;
        public readonly Lazy<MethodInfo> Finalizer;

        public readonly Action OnUnpatch;
        public readonly Action OnPatch;

        public bool IsPatchApplied { get; set; }

        public PatchData(
            string patchId,
            Func<MethodInfo> target,
            Func<MethodInfo> prefix = null,
            Func<MethodInfo> postfix = null,
            Func<MethodInfo> transpiler = null,
            Func<MethodInfo> finalizer = null,
            Action onPatch = null,
            Action onUnpatch = null)
        {
            PatchId = string.IsNullOrEmpty(patchId.Trim()) ? Guid.NewGuid().ToString() : patchId.Trim();
            Target = target is null ? null : new Lazy<MethodInfo>(target);
            Prefix = prefix is null ? null : new Lazy<MethodInfo>(prefix);
            Postfix = postfix is null ? null : new Lazy<MethodInfo>(postfix);
            Transpiler = transpiler is null ? null : new Lazy<MethodInfo>(transpiler);
            Finalizer = finalizer is null ? null : new Lazy<MethodInfo>(finalizer);
            OnPatch = onPatch;
            OnUnpatch = onUnpatch;
        }
    }

    internal static class Patcher
    {
        private static readonly Dictionary<string, int> Patches = new Dictionary<string, int>();

        public static bool IsPatched(string patchId)
        {
            if (Patches.TryGetValue(patchId, out int value)) return value > 0;
            return false;
        }

        public static bool CanPatch { get; set; } = true;

        public static void Patch(PatchData patch)
        {
            if (patch is null) throw new ArgumentNullException("patch");
            var target = patch.Target?.Value;
            if (target is null)
            {
                Log.Error($"{nameof(Patcher)}.{nameof(Patch)} target for {patch.PatchId} is null.");
                return;
            }
            var prefix = patch.Prefix?.Value;
            var postfix = patch.Postfix?.Value;
            var transpiler = patch.Transpiler?.Value;
            var finalizer = patch.Finalizer?.Value;
            if (prefix is null
                && postfix is null
                && transpiler is null
                && finalizer is null)
            {
                Log.Error($"{nameof(Patcher)}.{nameof(Patch)} all patch methods for {patch.PatchId} are null");
                return;
            }

            if (IsPatched(patch.PatchId))
            {
#if DEV
                Log.Info($"{nameof(Patcher)}.{nameof(Patch)} patch {patch.PatchId} already applied.");
#endif
                return;
            }

            if (!CanPatch)
            {
#if DEV
                Log.Info($"{nameof(Patcher)}.{nameof(Patch)} patch {patch.PatchId} will not be applied. CanPatch set to {CanPatch}.");
#endif
                return;
            }
#if DEV
            Log.Info($"{nameof(Patcher)}.{nameof(Patch)} applying harmony patch {patch.PatchId}.");
#endif
            if (Patches.ContainsKey(patch.PatchId))
            {
                Patches[patch.PatchId]++;
                if (Patches[patch.PatchId] > 1) return;
            }
            else
            {
                Patches.Add(patch.PatchId, 1);
            }

            DoOnHarmonyReady(() =>
            {
                try
                {
                    if (patch.IsPatchApplied) return;
#if DEV
                    Log.Info($"{nameof(Patcher)}.{nameof(Patch)} Harmony ready, patching {patch.PatchId}.");
#endif
                    patch.IsPatchApplied = true;
                    var harmony = new Harmony(ModProperties.HarmonyId);
                    harmony.Patch(target,
                        prefix is null ? null : new HarmonyMethod(prefix),
                        postfix is null ? null : new HarmonyMethod(postfix),
                        transpiler is null ? null : new HarmonyMethod(transpiler),
                        finalizer is null ? null : new HarmonyMethod(finalizer));
                    if (!(patch.OnPatch is null)) patch.OnPatch();
                }
                catch
                {
                    Log.Error($"{nameof(Patcher)}.{nameof(Patch)} DoOnHarmonyReady for {patch.PatchId} failed");
                    patch.IsPatchApplied = false;
                    throw;
                }
            });
        }

        public static void Unpatch(PatchData patch, bool force = false)
        {
            if (patch is null) throw new ArgumentNullException("patch");
            var target = patch.Target?.Value;
            if (target is null)
            {
                Log.Error($"{nameof(Patcher)}.{nameof(Unpatch)} target for {patch.PatchId} is null.");
                return;
            }
            var prefix = patch.Prefix?.Value;
            var postfix = patch.Postfix?.Value;
            var transpiler = patch.Transpiler?.Value;
            var finalizer = patch.Finalizer?.Value;
            if (prefix is null
                && postfix is null
                && transpiler is null
                && finalizer is null)
            {
                Log.Error($"{nameof(Patcher)}.{nameof(Unpatch)} all patch methods for {patch.PatchId} are null");
                return;
            }

            if (!IsPatched(patch.PatchId))
            {
#if DEV
                Log.Info($"{nameof(Patcher)}.{nameof(Unpatch)} patch {patch.PatchId} not applied.");
#endif
                return;
            }
#if DEV
            Log.Info($"{nameof(Patcher)}.{nameof(Unpatch)} removing harmony patch {patch.PatchId}.");
#endif
            Patches[patch.PatchId]--;
            var useCount = Patches[patch.PatchId];

            if (useCount > 0 && !force) return;

            DoOnHarmonyReady(() =>
            {
                try
                {
                    if (!patch.IsPatchApplied) return;
#if DEV
                    Log.Info($"{nameof(Patcher)}.{nameof(Unpatch)} Harmony ready, removing patch {patch.PatchId}.");
#endif
                    patch.IsPatchApplied = false;
                    var harmony = new Harmony(ModProperties.HarmonyId);
                    if (!(prefix is null)) harmony.Unpatch(target, prefix);
                    if (!(postfix is null)) harmony.Unpatch(target, postfix);
                    if (!(transpiler is null)) harmony.Unpatch(target, transpiler);
                    if (!(finalizer is null)) harmony.Unpatch(target, finalizer);
                    if (!(patch.OnUnpatch is null)) patch.OnUnpatch();
                }
                catch
                {
                    Log.Error($"{nameof(Patcher)}.{nameof(Unpatch)} DoOnHarmonyReady for {patch.PatchId} failed");
                    throw;
                }
            });
        }
    }
}