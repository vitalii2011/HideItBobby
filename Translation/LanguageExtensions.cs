using LanguageDictionary = System.Collections.Generic.IReadOnlyDictionary<HideItBobby.Translation.Phrase, string>;

namespace HideItBobby.Translation
{
    internal static class LanguageExtensions
    {
        public static string Translate(this LanguageDictionary instance, Phrase phrase, params string[] values)
        {
            { if (!(instance is null) && instance.TryGetValue(phrase, out var translatedPhrase)) return SubstituteTokens(translatedPhrase, values); }
            { if (DefaultLanguage.Dictionary.TryGetValue(phrase, out var translatedPhrase)) return SubstituteTokens(translatedPhrase, values); }

            return phrase;
        }

        private static string SubstituteTokens(string instance, params string[] values)
        {
            if (string.IsNullOrEmpty(instance)) return string.Empty;
            if (values is null || values.Length == 0) return instance;
            string result = instance;
            for (int i = 0; i < values.Length; i++)
            {
                result = result.Replace($"%{i + 1}", values[i]);
            }
            return result;
        }
    }
}
