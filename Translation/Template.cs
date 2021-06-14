using System;

namespace HideItBobby.Translation
{
    internal sealed class Template
    {
        private readonly Func<string[]> _getValues;

        public Phrase Phrase { get; }

        public Template(Phrase phrase)
        {
            Phrase = phrase;
        }
        public Template(Phrase phrase, params string[] values)
        {
            Phrase = phrase;
            if (!(values is null) && values.Length > 0)
            {
                _getValues = () => values;
            }
        }
        public Template(Phrase phrase, Func<string[]> getValues)
        {
            Phrase = phrase;
            _getValues = getValues;
        }

        public string Translate() => Language.Current.Translate(Phrase, _getValues is null ? null : _getValues());
        public string Translate(string languageKey) => Language.Get(languageKey).Translate(Phrase, _getValues is null ? null : _getValues());

        public static implicit operator Template(Phrase phrase) => new Template(phrase);
    }
}