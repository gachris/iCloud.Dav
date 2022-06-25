using System.Xml.Serialization;

namespace iCloud.Dav.People.Types
{
    [XmlRoot(ElementName = "displayname", Namespace = "DAV:")]
    public sealed class Displayname
    {
        public static readonly Displayname Default = new Displayname();

        [XmlText]
        public string Value { get; set; }
    }
}
