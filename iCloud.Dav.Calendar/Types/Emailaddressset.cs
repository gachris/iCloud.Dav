using System.Xml.Serialization;

namespace iCloud.Dav.ICalendar.Types
{
    [XmlRoot(ElementName = "email-address-set", Namespace = "http://calendarserver.org/ns/")]
    public class Emailaddressset
    {
        public static Emailaddressset Default = new Emailaddressset();

        [XmlText]
        public string Value { get; set; }
    }
}
