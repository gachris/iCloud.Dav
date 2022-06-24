using System.Collections.Generic;
using System.Xml.Serialization;

namespace iCloud.Dav.Calendar.Types
{
    [XmlRoot(ElementName = "multistatus", Namespace = "DAV:")]
    public class Multistatus<TProp>
    {
        [XmlElement(ElementName = "response", Namespace = "DAV:")]
        public virtual List<Response<TProp>> Responses { get; set; }
    }
}
