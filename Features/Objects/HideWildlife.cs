using ColossalFramework;
using HideItBobby.Common;
using HideItBobby.Common.Logging;
using HideItBobby.EntryPoints;
using System;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using static HideItBobby.Common.Patcher;

namespace HideItBobby.Features.Objects
{
    internal sealed class HideWildlife : FeatureBase
    {
        public override FeatureKey Key => FeatureKey.HideWildlife;

        protected override bool InitializeImpl()
        {
            Patch(WildlifeSpawnPointAICountAnimalsPatch.Data);
            return true;
        }
        protected override bool TerminateImpl()
        {
            Unpatch(WildlifeSpawnPointAICountAnimalsPatch.Data);
            return true;
        }

        protected override bool EnableImpl()
        {
            if (!SimulationManager.exists) return false;
            SimulationManager.instance.AddAction(OnWildlifeRefresh());
            return true;
        }
        protected override bool DisableImpl() => true;

        private static IEnumerator OnWildlifeRefresh()
        {
            try
            {
                if (Singleton<CitizenManager>.exists)
                {
                    var citizenManager = Singleton<CitizenManager>.instance;
                    for (int i = 1; i < citizenManager.m_instances.m_buffer.Length; i++)
                    {
                        if ((citizenManager.m_instances.m_buffer[i].m_flags & CitizenInstance.Flags.Created) != CitizenInstance.Flags.None)
                        {
                            if (citizenManager.m_instances.m_buffer[i].Info.m_citizenAI != null && citizenManager.m_instances.m_buffer[i].Info.m_citizenAI is WildlifeAI)
                            {
                                citizenManager.ReleaseCitizenInstance((ushort)i);
                            }
                        }
                    }
                }
                else
                {
#if DEV
                    Log.Warning($"{nameof(HideWildlife)}.{nameof(OnWildlifeRefresh)} instance of {nameof(CitizenManager)} does not exist");
#endif
                }
            }
            catch (Exception e)
            {
                Log.Error($"{nameof(HideWildlife)}.{nameof(OnWildlifeRefresh)} failed", e);
            }
            yield return null;
        }
    }

    #region Harmony patch
    internal static class WildlifeSpawnPointAICountAnimalsPatch
    {
        public static readonly PatchData Data = new PatchData(
            patchId: $"{nameof(HideWildlife)}.{nameof(WildlifeSpawnPointAICountAnimalsPatch)}.{nameof(Postfix)}",
            target: () => typeof(WildlifeSpawnPointAI).GetMethod("CountAnimals", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance),
            postfix: () => typeof(WildlifeSpawnPointAICountAnimalsPatch).GetMethod(nameof(Postfix), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static),
            onUnpatch: () =>
            {
                _hideWildlife.Invalidate();
            }
        );

        #region HideWildlife { get; }
        private static readonly Cached<HideWildlife> _hideWildlife = new Cached<HideWildlife>(
            () => InGameEntryPoint.Features.Resolve(FeatureKey.HideWildlife) as HideWildlife
            );
        private static readonly HideWildlife HideWildlife = _hideWildlife.Value;
        #endregion

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int Postfix(int __result)
        {
            if (!Data.IsPatchApplied) return __result;
            try
            {
                var isEnabled = (HideWildlife?.IsEnabled) ?? false;
                return isEnabled ? int.MaxValue : __result;
            }
            catch (Exception e)
            {
                Log.Error($"{nameof(HideWildlife)}.{nameof(WildlifeSpawnPointAICountAnimalsPatch)}.{nameof(Postfix)} failed", e);
                return __result;
            }
        }
    }
    #endregion
}