using System.Xml.Serialization;

namespace HideItBobby.Translation.Serialization
{
    [XmlType("string")]
    public sealed class StringItem
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlText]
        public string Value { get; set; }
    }
}
