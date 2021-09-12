using System.Xml.Serialization;

namespace HideItBobby.Settings.SettingsFiles
{
    [XmlType("ModVersionInfo")]
    public sealed class File_Version
    {
        [XmlElement("Version")]
        public int Version;
    }
}