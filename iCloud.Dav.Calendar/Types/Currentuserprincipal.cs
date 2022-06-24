using System.Xml.Serialization;

namespace iCloud.Dav.ICalendar.Types
{
    [XmlRoot(ElementName = "current-user-principal", Namespace = "DAV:")]
    public class Currentuserprincipal
    {
        public static Currentuserprincipal Default = new Currentuserprincipal();

        [XmlElement(ElementName = "href", Namespace = "DAV:")]
        public Url Url { get; set; }
    }
}
