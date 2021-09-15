using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace iCloud.Dav.Calendar.Types
{
    [XmlRoot(ElementName = "current-user-privilege-set", Namespace = "DAV:")]
    public class Currentuserprivilegeset
    {
        public static readonly Currentuserprivilegeset Default = new Currentuserprivilegeset();

        [XmlElement(ElementName = "privilege", Type = typeof(Privilege), Namespace = "DAV:")]
        public List<Privilege> Privilege { get; set; }
    }
}