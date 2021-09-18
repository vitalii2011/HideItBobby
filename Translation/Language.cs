using ColossalFramework;
using ColossalFramework.Globalization;
using HideItBobby.Common.Logging;
using HideItBobby.Settings;
using HideItBobby.Translation.Serialization;
using System.IO;
using UnityEngine;
using LanguageDictionary = System.Collections.Generic.IReadOnlyDictionary<HideItBobby.Translation.Phrase, string>;
using Library = System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IReadOnlyDictionary<HideItBobby.Translation.Phrase, string>>;

namespace HideItBobby.Translation
{
    internal delegate void LanguageChangeEvent(string key);

    internal static class Language
    {
        public static string GameLanguage => SingletonLite<LocaleManager>.exists
                ? SingletonLite<LocaleManager>.instance.language
                : "";
        public static event LanguageChangeEvent LanugageChanged;

        private static readonly Lazy<Library> _library = new Lazy<Library>(TranslationsReader.Load);
        public static Library Library => _library.Value;

        public static string Key { get; private set; } = "en";
        public static LanguageDictionary Current { get; private set; } = DefaultLanguage.Dictionary;

        public static LanguageDictionary Get(string key)
        {
            var targetKey = string.IsNullOrEmpty(key) ? "en" : key;
            if (Library.TryGetValue(targetKey, out var dictionary))
            {
                return dictionary;
            }
            else
            {
                return DefaultLanguage.Dictionary;
            }
        }

        public static void ChangeTo(string key)
        {
#if DEV || PREVIEW
            Log.Info($"{nameof(Language)}.{nameof(ChangeTo)} setting language to {key}");
#endif
            var targetKey = string.IsNullOrEmpty(key) ? "en" : key;
            if (Library.TryGetValue(targetKey, out var dictionary))
            {
                Key = targetKey;
                Current = dictionary;
                var _event = LanugageChanged;
                if (!(_event is null)) _event(Key);
            }
            else
            {
                Key = "en";
                Current = DefaultLanguage.Dictionary;
                var _event = LanugageChanged;
                if (!(_event is null)) _event(Key);
            }
        }
        public static void ChangeToGameLanguage()
        {
#if DEV
            Log.Info($"{nameof(Language)}.{nameof(ChangeToGameLanguage)} setting language to game language");
#endif
            ChangeTo(GameLanguage);
        }

        public static string NameToKey(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;
            foreach (var item in Library)
            {
                if (item.Value.TryGetValue(Phrase.LanguageName, out var langName) && langName == name)
                    return item.Key;
            }
            return null;
        }
        public static string KeyToName(string key)
        {
            if (string.IsNullOrEmpty(key)) return null;
            if (Library.TryGetValue(key, out var dictionary)
                && dictionary.TryGetValue(Phrase.LanguageName, out var langName))
                return langName;
            return null;
        }

        #region Lifecycle
        public static bool IsInitialized { get; private set; }
        public static void Initialize()
        {
            if (IsInitialized) return;

            var dePath = Path.Combine(Paths.TranslationsDir, "hide_it_bobby.de.xml");
            if (!File.Exists(dePath))
            {
                File.WriteAllText(dePath, Properties.Resources.hide_it_bobby_de, System.Text.Encoding.UTF8);
            }

            var enPath = Path.Combine(Paths.TranslationsDir, "hide_it_bobby.en.xml");
            if (!File.Exists(enPath))
            {
                File.WriteAllText(enPath, Properties.Resources.hide_it_bobby_en, System.Text.Encoding.UTF8);
            }

            var esPath = Path.Combine(Paths.TranslationsDir, "hide_it_bobby.es.xml");
            if (!File.Exists(esPath))
            {
                File.WriteAllText(esPath, Properties.Resources.hide_it_bobby_es, System.Text.Encoding.UTF8);
            }

            var jaPath = Path.Combine(Paths.TranslationsDir, "hide_it_bobby.ja.xml");
            if (!File.Exists(jaPath))
            {
                File.WriteAllText(jaPath, Properties.Resources.hide_it_bobby_ja, System.Text.Encoding.UTF8);
            }

            var plPath = Path.Combine(Paths.TranslationsDir, "hide_it_bobby.pl.xml");
            if (!File.Exists(plPath))
            {
                File.WriteAllText(plPath, Properties.Resources.hide_it_bobby_pl, System.Text.Encoding.UTF8);
            }

            var zhPath = Path.Combine(Paths.TranslationsDir, "hide_it_bobby.zh.xml");
            if (!File.Exists(zhPath))
            {
                File.WriteAllText(zhPath, Properties.Resources.hide_it_bobby_zh, System.Text.Encoding.UTF8);
            }

            LocaleManager.eventLocaleChanged += OnEventLocaleChanged;
            LocaleManager.eventUIComponentLocaleChanged += OnEventUIComponentLocaleChanged;

            if (ModSettings.Data.UseGameLanguage) ChangeToGameLanguage();
            else ChangeTo(ModSettings.Data.SelectedLanguage);

            IsInitialized = true;
        }
        public static void Terminate()
        {
            if (!IsInitialized) return;

            LocaleManager.eventLocaleChanged -= OnEventLocaleChanged;
            LocaleManager.eventUIComponentLocaleChanged -= OnEventUIComponentLocaleChanged;

            IsInitialized = false;
        }
        #endregion

        #region Events handling
        private static void OnEventUIComponentLocaleChanged()
        {
#if DEV
            Log.Info($"{nameof(Language)}.{nameof(OnEventUIComponentLocaleChanged)} fired");
            Log.Info($"GameLanguage: {GameLanguage}");
#endif
            if (ModSettings.Data.UseGameLanguage) ChangeToGameLanguage();
            else ChangeTo(ModSettings.Data.SelectedLanguage);
        }

        private static void OnEventLocaleChanged()
        {
#if DEV
            Log.Info($"{nameof(Language)}.{nameof(OnEventLocaleChanged)} fired");
            Log.Info($"GameLanguage: {GameLanguage}");
#endif
            if (ModSettings.Data.UseGameLanguage) ChangeToGameLanguage();
            else ChangeTo(ModSettings.Data.SelectedLanguage);
        }
        #endregion
    }
}