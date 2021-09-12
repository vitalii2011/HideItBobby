using HideItBobby.Common.Logging;
using HideItBobby.Settings.SettingsFiles;
using System;
using System.IO;
using System.Xml.Serialization;

namespace HideItBobby.Settings.Providers
{
    internal sealed class Provider_1_21
    {
        public static readonly Lazy<string> FileName = new Lazy<string>(() => Path.Combine(Paths.ConfigDir, "HideItBobbyConfig.xml"));

        private static readonly XmlSerializer Serializer;
        private static readonly XmlSerializerNamespaces Namespaces;

        static Provider_1_21()
        {
            Serializer = new XmlSerializer(typeof(File_1_21));
            var noNamespaces = new XmlSerializerNamespaces();
            noNamespaces.Add("", "");
            Namespaces = noNamespaces;
        }

        public static void Save(File_1_21 data)
        {
            if (data is null)
            {
#if DEV
                Log.Warning($"{nameof(Provider_1_21)}.{nameof(Save)} nothing to save");
#endif
                return;
            }

#if DEV
            Log.Info($"{nameof(Provider_1_21)}.{nameof(Save)} saving {FileName.Value}");
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
                Log.Error($"{nameof(Provider_1_21)}.{nameof(Save)} failed", e);
            }
        }

        public static File_1_21 Load()
        {
#if DEV
            Log.Info($"{nameof(Provider_1_21)}.{nameof(Load)} loading settings");
#endif
            try
            {
                if (File.Exists(FileName.Value))
                {
#if DEV
                    Log.Info($"{nameof(Provider_1_21)}.{nameof(Load)} loading {FileName.Value}");
#endif
                    using (var streamReader = new StreamReader(FileName.Value))
                    {
                        return Serializer.Deserialize(streamReader) as File_1_21;
                    }
                }
#if DEV
                Log.Info($"{nameof(Provider_1_21)}.{nameof(Load)} using new settings file");
#endif
                return null;
            }
            catch (Exception e)
            {
                Log.Error($"{nameof(Provider_1_21)}.{nameof(Load)} failed", e);
                return null;
            }
        }

        public static void Delete()
        {
#if DEV
            Log.Info($"{nameof(Provider_1_21)}.{nameof(Delete)} deleting file {FileName.Value}");
#endif
            try
            {
#if DEV || PREVIEW
                Log.Info($"{nameof(Provider_1_19)}.{nameof(Delete)} config files are preserved in dev and preview builds");
#else
                if (File.Exists(FileName.Value)) File.Delete(FileName.Value);
#endif
            }
            catch (Exception e)
            {
                Log.Error($"{nameof(Provider_1_21)}.{nameof(Delete)} failed", e);
            }
        }
    }
}