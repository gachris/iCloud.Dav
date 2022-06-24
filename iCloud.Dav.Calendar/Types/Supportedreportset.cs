using System.Collections.Generic;
using System.Xml.Serialization;

namespace iCloud.Dav.ICalendar.Types
{
    [XmlRoot(ElementName = "supported-report-set", Namespace = "DAV:")]
    public class Supportedreportset
    {
        public static Supportedreportset Default = new Supportedreportset();

        [XmlElement(ElementName = "supported-report", Namespace = "DAV:")]
        public List<Supportedreport> Supportedreport { get; set; }
    }
}
