using HideItBobby.Common;
using HideItBobby.Settings;
using HideItBobby.Settings.SettingsFiles;
using System.IO;
using System.Security.Cryptography;

namespace HideItBobby.VersionMigrations
{
    internal static class Migrate_1_21_to_1_22
    {
        private const string HashValue = "f29d85c675c5666194d388d11b89834dc08bfd5a66d80df25c8cab5d2c58e8f1";
        public static void Migrate(File_Version versionInfo)
        {
            if (versionInfo?.Version >= 22) return;

            var plPath = Path.Combine(Paths.TranslationsDir, "hide_it_bobby.pl.xml");
            if (!File.Exists(plPath)) return;

            using (SHA256 sha256 = SHA256.Create())
            {
                if (Hash.VerifyHash(sha256, plPath, HashValue)) File.Delete(plPath);
            }
        }
    }
}