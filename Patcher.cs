using HarmonyLib;
using System.Reflection;

namespace HideItBobby
{
    static class Patcher
    {

        private static readonly string _harmonyId = "local.csl.HideItBobbyby";
        private static Harmony _harmony;

        public static void PatchAll()
        {
            _harmony = new Harmony(_harmonyId);

            if (_harmony != null)
            {
                _harmony.PatchAll(typeof(Patcher).GetType().Assembly);
            }
        }

        public static void UnpatchAll()
        {
            if (_harmony != null)
            {
                _harmony.UnpatchAll(_harmonyId);
                _harmony = null;
            }
        }
    }
}
