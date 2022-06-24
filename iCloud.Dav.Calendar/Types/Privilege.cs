using System.Xml;
using System.Xml.Serialization;

namespace iCloud.Dav.ICalendar.Types
{
    [XmlRoot(ElementName = "privilege", Namespace = "DAV:")]
    public class Privilege
    {
        public Privilege()
        {
        }

        public Privilege(XmlNode valueObject)
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

        public static implicit operator string(Privilege objectToCast)
        {
            return objectToCast.Value.ToString();
        }

        public static implicit operator Privilege(XmlNode objectToCast)
        {
            return new Privilege(objectToCast);
        }
    }
}
