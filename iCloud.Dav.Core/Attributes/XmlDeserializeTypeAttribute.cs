using System;

namespace iCloud.Dav.Core.Attributes
{
    public class XmlDeserializeTypeAttribute : Attribute
    {
        public Type Type { get; }

        public XmlDeserializeTypeAttribute(Type type) : base()
        {
            Type = type;
        }
    }
}
