﻿using System.Xml.Serialization;

namespace iCloud.Dav.ICalendar.Types
{
    [XmlRoot(ElementName = "propertyupdate", Namespace = "DAV:")]
    public class PropertyUpdate
    {
        [XmlElement(ElementName = "set", Namespace = "DAV:")]
        public Set Set { get; set; }
    }
}
