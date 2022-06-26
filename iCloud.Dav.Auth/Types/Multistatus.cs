using System.Collections.Generic;
using System.Xml.Serialization;

namespace iCloud.Dav.Auth.Types
{
    [XmlRoot(ElementName = "multistatus", Namespace = "DAV:")]
    public class Multistatus
    {
        [XmlElement(ElementName = "response", Namespace = "DAV:")]
        public virtual List<Response> Responses { get; set; }
    }
}
