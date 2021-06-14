using HideItBobby.Common.Logging;
using HideItBobby.Settings.SettingsFiles;
using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace HideItBobby.Settings.Providers
{
    internal sealed class Provider_1_17
    {
        private const string FileName = "HideItConfig.xml";

        private static readonly XmlSerializer Serializer;
        private static readonly XmlSerializerNamespaces Namespaces;

        static Provider_1_17()
        {
            Serializer = new XmlSerializer(typeof(File_1_17));
            var noNamespaces = new XmlSerializerNamespaces();
            noNamespaces.Add("", "");
            Namespaces = noNamespaces;
        }

        public static void Save(File_1_17 data)
        {
            if (data is null)
            {
#if DEV
                Log.Warning($"{nameof(Provider_1_17)}.{nameof(Save)} nothing to save");
#endif
                return;
            }

#if DEV
            Log.Info($"{nameof(Provider_1_17)}.{nameof(Save)} saving {FileName}");
#endif
            try
            {
                using (var streamWriter = new StreamWriter(FileName))
                {
                    Serializer.Serialize(streamWriter, data, Namespaces);
                }
            }
            catch (Exception e)
            {
                Log.Error($"{nameof(Provider_1_17)}.{nameof(Save)} failed", e);
            }
        }

        public static File_1_17 Load()
        {
#if DEV
            Log.Info($"{nameof(Provider_1_17)}.{nameof(Load)} loading settings");
#endif
            try
            {
                if (File.Exists(FileName))
                {
#if DEV
                    Log.Info($"{nameof(Provider_1_17)}.{nameof(Load)} loading {FileName}");
#endif
                    using (var streamReader = new StreamReader(FileName))
                    {
                        return Serializer.Deserialize(streamReader) as File_1_17;
                    }
                }
#if DEV
                Log.Info($"{nameof(Provider_1_17)}.{nameof(Load)} using new settings file");
#endif
                return null;
            }
            catch (Exception e)
            {
                Log.Error($"{nameof(Provider_1_17)}.{nameof(Load)} failed", e);
                return null;
            }
        }
    }
}