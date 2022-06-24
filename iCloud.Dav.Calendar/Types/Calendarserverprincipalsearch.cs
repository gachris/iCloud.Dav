using System.Xml.Serialization;

namespace iCloud.Dav.Calendar.Types
{
    [XmlRoot(ElementName = "calendarserver-principal-search", Namespace = "http://calendarserver.org/ns/")]
    public class Calendarserverprincipalsearch
    {
        public static readonly Calendarserverprincipalsearch Default = new Calendarserverprincipalsearch();

        [XmlText]
        public string Value { get; set; }
    }
}
