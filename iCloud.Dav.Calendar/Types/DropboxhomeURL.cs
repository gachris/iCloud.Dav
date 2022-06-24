using System.Xml.Serialization;

namespace iCloud.Dav.ICalendar.Types
{
    [XmlRoot(ElementName = "dropbox-home-URL", Namespace = "http://calendarserver.org/ns/")]
    public class DropboxhomeURL
    {
        public static readonly DropboxhomeURL Default = new DropboxhomeURL();

        [XmlElement(ElementName = "href", Namespace = "http://calendarserver.org/ns/")]
        public Url Url { get; set; }
    }
}
