using HideItBobby.Common;
using HideItBobby.Settings;
using HideItBobby.Settings.SettingsFiles;
using System.IO;
using System.Security.Cryptography;

namespace HideItBobby.VersionMigrations
{
    internal static class Migrate_1_24_to_1_25
    {
        private const string HashValue = "12c13330511cb18972e566aafb37f85d566d67576fda04f383dc30cff6e3febb";
        public static void Migrate(File_Version versionInfo)
        {
            if (versionInfo?.Version >= 25) return;

            var dePath = Path.Combine(Paths.TranslationsDir, "hide_it_bobby.de.xml");
            if (File.Exists(dePath))
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    if (Hash.VerifyHash(sha256, dePath, HashValue)) File.Delete(dePath);
                }
            }

            var esPath = Path.Combine(Paths.TranslationsDir, "hide_it_bobby.es.xml");
            var esMovePath = Path.Combine(Paths.TranslationsDir, "hide_it_bobby.es.xml.backup");
            if (File.Exists(esPath))
            {
                if (File.Exists(esMovePath)) File.Delete(esMovePath);
                File.Move(esPath, esMovePath);
            }
        }
    }
}