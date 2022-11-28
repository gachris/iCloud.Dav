using iCloud.Dav.People.Serialization.DataTypes;
using System.IO;
using vCard.Net;
using vCard.Net.DataTypes;

namespace iCloud.Dav.People.DataTypes
{
    public class X_ABAddress : EncodableDataType
    {
        public virtual string Value { get; set; }

        public X_ABAddress()
        {
        }

        public X_ABAddress(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            var serializer = new X_ABAddressSerializer();
            CopyFrom(serializer.Deserialize(new StringReader(value)) as ICopyable);
        }
    }
}