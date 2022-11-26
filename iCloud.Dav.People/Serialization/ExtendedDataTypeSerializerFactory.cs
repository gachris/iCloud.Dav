using iCloud.Dav.People.DataTypes;
using iCloud.Dav.People.Serialization.DataTypes;
using System;
using vCard.Net.Serialization;

namespace iCloud.Dav.People.Serialization
{
    /// <inheritdoc/>
    public class ExtendedDataTypeSerializerFactory : DataTypeSerializerFactory, ISerializerFactory
    {
        /// <inheritdoc/>
        public override ISerializer Build(Type objectType, SerializationContext ctx)
        {
            ISerializer s = null;

            if (typeof(Address).IsAssignableFrom(objectType))
            {
                s = new AddressSerializer(ctx);
            }
            else if (typeof(Email).IsAssignableFrom(objectType))
            {
                s = new EmailSerializer(ctx);
            }
            else if (typeof(Phone).IsAssignableFrom(objectType))
            {
                s = new PhoneSerializer(ctx);
            }
            else if (typeof(Photo).IsAssignableFrom(objectType))
            {
                s = new PhotoSerializer(ctx);
            }
            else if (typeof(Website).IsAssignableFrom(objectType))
            {
                s = new WebsiteSerializer(ctx);
            }
            else if (typeof(X_ABDate).IsAssignableFrom(objectType))
            {
                s = new X_ABDateSerializer(ctx);
            }
            else if (typeof(X_ABRelatedNames).IsAssignableFrom(objectType))
            {
                s = new X_ABRelatedNamesSerializer(ctx);
            }
            else if (typeof(X_SocialProfile).IsAssignableFrom(objectType))
            {
                s = new X_SocialProfileSerializer(ctx);
            }
            else if (typeof(vCard.Net.DataTypes.IDateTime).IsAssignableFrom(objectType))
            {
                s = new vCard.Net.Serialization.DataTypes.DateTimeSerializer(ctx);
            }

            return s ?? base.Build(objectType, ctx);
        }
    }
}