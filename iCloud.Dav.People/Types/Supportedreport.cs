using System.Xml.Serialization;

namespace iCloud.Dav.People.Types
{
    [XmlRoot(ElementName = "supported-report", Namespace = "DAV:")]
    public class Supportedreport
    {
        [XmlElement(ElementName = "report", Namespace = "DAV:")]
        public Report Report { get; set; }
    }
}
