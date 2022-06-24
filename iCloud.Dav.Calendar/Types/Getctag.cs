using System.Xml.Serialization;

namespace iCloud.Dav.ICalendar.Types
{
    [XmlRoot(ElementName = "getctag", Namespace = "http://calendarserver.org/ns/")]
    public class Getctag
    {
        public static readonly Getctag Default = new Getctag();

        [XmlText]
        public string Value { get; set; }
    }
}
