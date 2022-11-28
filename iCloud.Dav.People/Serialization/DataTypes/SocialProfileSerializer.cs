using iCloud.Dav.People.DataTypes;
using System;
using System.IO;
using vCard.Net.Serialization;
using vCard.Net.Serialization.DataTypes;

namespace iCloud.Dav.People.Serialization.DataTypes
{
    public class SocialProfileSerializer : EncodableDataTypeSerializer
    {
        public SocialProfileSerializer() : base()
        {
        }

        public SocialProfileSerializer(SerializationContext ctx) : base(ctx)
        {
        }

        public override Type TargetType => typeof(SocialProfile);

        public override string SerializeToString(object obj)
        {
            if (!(obj is SocialProfile socialProfile))
            {
                return null;
            }

            var value = socialProfile.Url;

            return Encode(socialProfile, value);
        }

        public SocialProfile Deserialize(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            if (!(CreateAndAssociate() is SocialProfile socialProfile))
            {
                return null;
            }

            // Decode the value, if necessary!
            value = Decode(socialProfile, value);

            if (value is null)
            {
                return null;
            }

            socialProfile.Url = value;

            return socialProfile;
        }

        public override object Deserialize(TextReader tr) => Deserialize(tr.ReadToEnd());
    }
}