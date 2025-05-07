using iCloud.Dav.People.DataTypes;
using iCloud.Dav.People.Serialization.DataTypes;
using vCard.Net.DataTypes;
using vCard.Net.Serialization;
using vCard.Net.Serialization.DataTypes;
using Address = iCloud.Dav.People.DataTypes.Address;
using AddressSerializer = iCloud.Dav.People.Serialization.DataTypes.AddressSerializer;
using Email = iCloud.Dav.People.DataTypes.Email;
using EmailSerializer = iCloud.Dav.People.Serialization.DataTypes.EmailSerializer;
using Label = iCloud.Dav.People.DataTypes.Label;
using LabelSerializer = iCloud.Dav.People.Serialization.DataTypes.LabelSerializer;
using Photo = iCloud.Dav.People.DataTypes.Photo;
using PhotoSerializer = iCloud.Dav.People.Serialization.DataTypes.PhotoSerializer;

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
        else if (typeof(Member).IsAssignableFrom(objectType))
        {
            s = new MemberSerializer(ctx);
        }
        else if (typeof(Birthdate).IsAssignableFrom(objectType))
        {
            s = new BirthdateSerializer(ctx);
        }

        return s ?? base.Build(objectType, ctx);
    }
}