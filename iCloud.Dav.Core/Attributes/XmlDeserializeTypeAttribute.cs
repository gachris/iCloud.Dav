using System;

namespace iCloud.Dav.Core.Attributes
{
    public class XmlDeserializeTypeAttribute : Attribute
    {
        public XmlDeserializeTypeAttribute(Type type) : base()
        {
            Type = type;
        }

        public Type Type { get; }
    }
}
