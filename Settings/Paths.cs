using ColossalFramework.IO;
using HideItBobby.Properties;
using System.IO;

namespace HideItBobby.Settings
{
    internal static class Paths
    {
        private static readonly Lazy<string> _configDir = new Lazy<string>(GetConfigDir);
        public static string ConfigDir => _configDir.Value;

        private static readonly Lazy<string> _translationsDir = new Lazy<string>(GetTranslationsDir);
        public static string TranslationsDir => _translationsDir.Value;

        private static readonly Lazy<string> _logsDir = new Lazy<string>(GetLogsDir);
        public static string LogsDir => _logsDir.Value;

        private static string GetConfigDir()
        {
            var modsConfigPath = Path.Combine(DataLocation.localApplicationData, "ModConfig");
            if (!Directory.Exists(modsConfigPath)) Directory.CreateDirectory(modsConfigPath);
            var path = Path.Combine(modsConfigPath, ModProperties.ShortName);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            return path;
        }

        private static string GetTranslationsDir()
        {
            var path = Path.Combine(_configDir.Value, "Translations");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            return path;
        }

        private static string GetLogsDir()
        {
            var path = Path.Combine(_configDir.Value, "Logs");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            return path;
        }
    }
}
