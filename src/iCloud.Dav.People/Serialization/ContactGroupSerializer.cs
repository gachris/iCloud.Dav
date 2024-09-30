using System;
using System.Linq;
using System.Text;
using iCloud.Dav.People.DataTypes;
using iCloud.Dav.People.Utils;
using vCard.Net;
using vCard.Net.CardComponents;
using vCard.Net.Serialization;

namespace iCloud.Dav.People.Serialization;

/// <summary>
/// Serializer for the <see cref="ContactGroup"/> class.
/// </summary>
public class ContactGroupSerializer : ComponentSerializer
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ContactGroupSerializer"/> class.
    /// </summary>
    public ContactGroupSerializer()
    {
        SetService(new ContactGroupDataTypeMapper());
        SetService(new ExtendedSerializerFactory());
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ContactGroupSerializer"/> class with the given <see cref="SerializationContext"/>.
    /// </summary>
    /// <param name="ctx">The <see cref="SerializationContext"/> to use.</param>
    public ContactGroupSerializer(SerializationContext ctx) : base(ctx)
    {
        SetService(new ContactGroupDataTypeMapper());
        SetService(new ExtendedSerializerFactory());
    }

    /// <summary>
    /// Gets the Type that this <see cref="ContactGroupSerializer"/> can serialize and deserialize, which is <see cref="ContactGroup"/>.
    /// </summary>
    public override Type TargetType => typeof(ContactGroup);

    /// <summary>
    /// Serialize a <see cref="ContactGroup"/> object to a string.
    /// </summary>
    /// <param name="obj">The <see cref="ContactGroup"/> object to serialize.</param>
    /// <returns>The serialized string.</returns>
    public override string SerializeToString(object obj)
    {
        if (obj is not IVCardComponent c)
        {
            return null;
        }

        var sb = new StringBuilder();
        var upperName = c.Name.ToUpperInvariant();
        sb.Append(TextUtil.FoldLines($"BEGIN:{upperName}"));

        // Get a serializer factory
        var sf = GetService<ISerializerFactory>();

        // Sort the vCard properties in alphabetical order before serializing them!
        var properties = c.Properties.OrderBy(p => p, PropertySorter).ToList();

        // Serialize properties
        foreach (var p in properties)
        {
            // Get a serializer for each property.
            var serializer = sf.Build(p.GetType(), SerializationContext) as IStringSerializer;
            sb.Append(serializer.SerializeToString(p));
        }

        // Serialize child objects
        foreach (var child in c.Children)
        {
            // Get a serializer for each child object.
            var serializer = sf.Build(child.GetType(), SerializationContext) as IStringSerializer;
            sb.Append(serializer.SerializeToString(child));
        }

        sb.Append(TextUtil.FoldLines($"END:{upperName}"));
        return sb.ToString();
    }
}