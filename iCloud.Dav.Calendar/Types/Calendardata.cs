using System.Xml.Serialization;

namespace iCloud.Dav.Calendar.Types
{
    [XmlRoot(ElementName = "calendar-data", Namespace = "DAV:")]
    public class Calendardata
    {
        public static Calendardata Default = new Calendardata();

        [XmlText]
        public string Value { get; set; }
    }
}
