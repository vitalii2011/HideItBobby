using HideItBobby.Common.Logging;
using HideItBobby.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;
using LanguageDictionary = System.Collections.Generic.IReadOnlyDictionary<HideItBobby.Translation.Phrase, string>;
using Library = System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IReadOnlyDictionary<HideItBobby.Translation.Phrase, string>>;

namespace HideItBobby.Translation.Serialization
{
    internal sealed class TranslationsReader
    {
        private static readonly XmlSerializer Serializer;
        private static readonly XmlSerializerNamespaces Namespaces;

        static TranslationsReader()
        {
            Serializer = new XmlSerializer(typeof(TranslationFile));
            var noNamespaces = new XmlSerializerNamespaces();
            noNamespaces.Add("", "");
            Namespaces = noNamespaces;
        }

        public static Library Load()
        {
#if DEV
            Log.Info($"{nameof(TranslationsReader)}.{nameof(Load)} loading translations files from {Paths.TranslationsDir}");
#endif
            try
            {
                var library = new Dictionary<string, LanguageDictionary>();
                foreach (var fileName in Directory
                    .GetFiles(Paths.TranslationsDir)
                    .Where(fn => Path.GetExtension(fn) == ".xml"))
                {
#if DEV
                    Log.Info($"{nameof(TranslationsReader)}.{nameof(Load)} loading {fileName}");
#endif
                    TranslationFile file;
                    using (var streamReader = new StreamReader(fileName))
                    {
                        file = (Serializer.Deserialize(streamReader) as TranslationFile);
                    }
                    library.Add(
                        Path.GetFileNameWithoutExtension(fileName).Split('.').Last().ToLower(),
                        file.Where(item => !(Phrase.TryParse(item.Name) is null)).ToDictionary(item => Phrase.Parse(item.Name), item => item.Value).AsReadOnly());
                }
                return library.AsReadOnly();
            }
            catch (Exception e)
            {
                Log.Error($"{nameof(TranslationsReader)}.{nameof(Load)} failed", e);
                return new Dictionary<string, LanguageDictionary>().AsReadOnly();
            }
        }
    }
}