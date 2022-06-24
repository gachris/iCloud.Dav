﻿using System.Xml.Serialization;

namespace iCloud.Dav.Calendar.Types
{
    [XmlRoot(ElementName = "resource-id", Namespace = "DAV:")]
    public class ResourceId

    {
        public static readonly ResourceId Default = new ResourceId();

        [XmlText]
        public string Value { get; set; }
    }
}
