using System.Collections.Generic;
using System.Xml.Serialization;

namespace iCloud.Dav.People.Types
{
    [XmlRoot(ElementName = "multistatus", Namespace = "DAV:")]
    public sealed class Multistatus<TProp>
    {
        [XmlElement(ElementName = "response", Namespace = "DAV:")]
        public List<Response<TProp>> Responses { get; set; }
    }
}
