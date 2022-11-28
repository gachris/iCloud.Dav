using iCloud.Dav.People.DataTypes;
using System;
using System.IO;
using vCard.Net.Serialization;
using vCard.Net.Serialization.DataTypes;

namespace iCloud.Dav.People.Serialization.DataTypes
{
    public class WebsiteSerializer : EncodableDataTypeSerializer
    {
        public WebsiteSerializer() : base()
        {
        }

        public WebsiteSerializer(SerializationContext ctx) : base(ctx)
        {
        }

        public override Type TargetType => typeof(Website);

        public override string SerializeToString(object obj)
        {
            if (!(obj is Website email))
            {
                return null;
            }

            var value = email.Url;

            return Encode(email, value);
        }

        public Website Deserialize(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            if (!(CreateAndAssociate() is Website website))
            {
                return null;
            }

            // Decode the value, if necessary!
            value = Decode(website, value);

            if (value is null)
            {
                return null;
            }

            website.Url = value;

            return website;
        }

        public override object Deserialize(TextReader tr) => Deserialize(tr.ReadToEnd());
    }
}