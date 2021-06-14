using HideItBobby.Common;
using HideItBobby.Common.Logging;
using System;
using System.Collections;
using UnityEngine;

namespace HideItBobby.Features.Effects.Shared
{
    internal static class TexturesUpdater
    {
        private static Counter _counter = int.MaxValue;

        public static void ResetCounter() => _counter = int.MaxValue;

        public static void Update(Counter counter)
        {
            if (counter <= _counter) return;
            _counter = counter.Clone();
            try
            {
                if (SimulationManager.exists)
                    SimulationManager.instance.AddAction(SetAreaModified());
            }
            catch (Exception e)
            {
                Log.Error($"{nameof(TexturesUpdater)}.{nameof(Update)} failed", e);
            }
        }

        private static IEnumerator SetAreaModified()
        {
            try
            {
                if (NaturalResourceManager.exists)
                    NaturalResourceManager.instance.AreaModified(0, 0, 511, 511);
            }
            catch (Exception e)
            {
                Log.Error($"{nameof(TexturesUpdater)}.{nameof(SetAreaModified)} failed", e);
            }
            yield return null;
        }
    }
}