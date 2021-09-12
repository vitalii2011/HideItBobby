using HideItBobby.Common.Logging;
using HideItBobby.Settings.SettingsFiles;
using System;
using System.IO;
using System.Xml.Serialization;

namespace HideItBobby.Settings.Providers
{
    internal sealed class Provider_Verison
    {
        public static readonly Lazy<string> FileName = new Lazy<string>(() => Path.Combine(Paths.ConfigDir, "HideItBobbyVersion.xml"));

        private static readonly XmlSerializer Serializer;
        private static readonly XmlSerializerNamespaces Namespaces;

        static Provider_Verison()
        {
            Serializer = new XmlSerializer(typeof(File_Version));
            var noNamespaces = new XmlSerializerNamespaces();
            noNamespaces.Add("", "");
            Namespaces = noNamespaces;
        }

        public static void Save(File_Version data)
        {
            if (data is null)
            {
#if DEV
                Log.Warning($"{nameof(Provider_Verison)}.{nameof(Save)} nothing to save");
#endif
                return;
            }

#if DEV
            Log.Info($"{nameof(Provider_Verison)}.{nameof(Save)} saving {FileName.Value}");
#endif
            try
            {
                using (var streamWriter = new StreamWriter(FileName.Value))
                {
                    Serializer.Serialize(streamWriter, data, Namespaces);
                }
            }
            catch (Exception e)
            {
                Log.Error($"{nameof(Provider_Verison)}.{nameof(Save)} failed", e);
            }
        }

        public static File_Version Load()
        {
#if DEV
            Log.Info($"{nameof(Provider_Verison)}.{nameof(Load)} loading settings");
#endif
            try
            {
                if (File.Exists(FileName.Value))
                {
#if DEV
                    Log.Info($"{nameof(Provider_Verison)}.{nameof(Load)} loading {FileName.Value}");
#endif
                    using (var streamReader = new StreamReader(FileName.Value))
                    {
                        return Serializer.Deserialize(streamReader) as File_Version;
                    }
                }
#if DEV
                Log.Info($"{nameof(Provider_Verison)}.{nameof(Load)} using new settings file");
#endif
                return null;
            }
            catch (Exception e)
            {
                Log.Error($"{nameof(Provider_Verison)}.{nameof(Load)} failed", e);
                return null;
            }
        }
    }
}