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
            else if (typeof(Date).IsAssignableFrom(objectType))
            {
                s = new DateSerializer(ctx);
            }
            else if (typeof(RelatedNames).IsAssignableFrom(objectType))
            {
                s = new RelatedNamesSerializer(ctx);
            }
            else if (typeof(SocialProfile).IsAssignableFrom(objectType))
            {
                s = new SocialProfileSerializer(ctx);
            }
            else if (typeof(Label).IsAssignableFrom(objectType))
            {
                s = new LabelSerializer(ctx);
            }
            else if (typeof(InstantMessage).IsAssignableFrom(objectType))
            {
                s = new InstantMessageSerializer(ctx);
            }
            else if (typeof(X_ABAddress).IsAssignableFrom(objectType))
            {
                s = new X_ABAddressSerializer(ctx);
            }
            else if (typeof(vCard.Net.DataTypes.IDateTime).IsAssignableFrom(objectType))
            {
                s = new vCard.Net.Serialization.DataTypes.DateTimeSerializer(ctx);
            }

            return s ?? base.Build(objectType, ctx);
        }
    }
}