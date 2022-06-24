using System.Xml.Serialization;

namespace iCloud.Dav.ICalendar.Types
{
    [XmlRoot(ElementName = "displayname", Namespace = "DAV:")]
    public class Displayname
    {
        public static readonly Displayname Default = new Displayname();

        [XmlText]
        public string Value { get; set; }
    }
}
