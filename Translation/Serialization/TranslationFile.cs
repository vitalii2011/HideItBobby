using System.Collections.Generic;
using System.Xml.Serialization;

namespace HideItBobby.Translation.Serialization
{
    [XmlType("resources")]
    public sealed class TranslationFile : List<StringItem> { }
}
