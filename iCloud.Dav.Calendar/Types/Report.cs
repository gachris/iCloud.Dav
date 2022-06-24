using System.Xml;
using System.Xml.Serialization;

namespace iCloud.Dav.ICalendar.Types
{
    [XmlRoot(ElementName = "report", Namespace = "DAV:")]
    public class Report
    {
        public Report()
        {
        }

        public Report(XmlNode valueObject)
        {
            Value = valueObject;
        }

        private XmlNode _xmlNode;
        [XmlAnyElement]
        public object Value
        {
            get { return _xmlNode.Name; }
            set
            {
                if (value is XmlNode xmlNode)
                    _xmlNode = xmlNode;
            }
        }

        public static implicit operator string(Report objectToCast)
        {
            return objectToCast.Value.ToString();
        }

        public static implicit operator Report(XmlNode objectToCast)
        {
            return new Report(objectToCast);
        }
    }
}
