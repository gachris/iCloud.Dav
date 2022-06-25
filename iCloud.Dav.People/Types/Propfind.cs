﻿using System.Xml.Serialization;

namespace iCloud.Dav.People.Types
{
    [XmlRoot(ElementName = "propfind", Namespace = "DAV:")]
    public sealed class Propfind<TProp>
    {
        [XmlElement(ElementName = "prop", Namespace = "DAV:")]
        public TProp Prop { get; set; }
    }
}
