using System;
using iCloud.Dav.People.DataTypes;
using iCloud.Dav.People.Serialization.DataTypes;
using vCard.Net.DataTypes;
using vCard.Net.Serialization;
using vCard.Net.Serialization.DataTypes;

namespace iCloud.Dav.People.Serialization;

/// <summary>
/// Factory class for creating serializers for extended vCard data types.
/// </summary>
internal class ExtendedDataTypeSerializerFactory : DataTypeSerializerFactory, ISerializerFactory
{
    /// <summary>
    /// Builds a serializer for the specified object type and serialization context.
    /// </summary>
    /// <param name="objectType">The type of object to serialize or deserialize.</param>
    /// <param name="ctx">The serialization context.</param>
    /// <returns>A serializer for the specified object type and serialization context.</returns>
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
        else if (typeof(IDateTime).IsAssignableFrom(objectType))
        {
            s = new DateTimeSerializer(ctx);
        }

        return s ?? base.Build(objectType, ctx);
    }
}