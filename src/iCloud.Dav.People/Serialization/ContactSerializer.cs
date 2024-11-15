using System.Text;
using iCloud.Dav.People.DataTypes;
using iCloud.Dav.People.Utils;
using vCard.Net;
using vCard.Net.DataTypes;
using vCard.Net.Serialization;

namespace iCloud.Dav.People.Serialization;

/// <summary>
/// Serializer for the <see cref="Contact"/> class.
/// </summary>
public class ContactSerializer : ComponentSerializer
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ContactSerializer"/> class.
    /// </summary>
    public ContactSerializer()
    {
        SetService(new ContactDataTypeMapper());
        SetService(new ExtendedSerializerFactory());
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ContactSerializer"/> class with the given <see cref="SerializationContext"/>.
    /// </summary>
    /// <param name="ctx">The <see cref="SerializationContext"/> to use.</param>
    public ContactSerializer(SerializationContext ctx) : base(ctx)
    {
        SetService(new ContactDataTypeMapper());
        SetService(new ExtendedSerializerFactory());
    }

    /// <summary>
    /// Gets the Type that this <see cref="ContactSerializer"/> can serialize and deserialize, which is <see cref="Contact"/>.
    /// </summary>
    public override Type TargetType => typeof(Contact);

    /// <summary>
    /// Serialize a <see cref="Contact"/> object to a string.
    /// </summary>
    /// <param name="obj">The <see cref="Contact"/> object to serialize.</param>
    /// <returns>The serialized string.</returns>
    public override string SerializeToString(object obj)
    {
        if (obj is not Contact c)
        {
            return null;
        }

        var sb = new StringBuilder();
        var upperName = c.Name.ToUpperInvariant();
        sb.Append(TextUtil.FoldLines($"BEGIN:{upperName}"));
        sb.Append(TextUtil.FoldLines($"VERSION:3.0"));

        // Get a serializer factory
        var sf = GetService<ISerializerFactory>();

        // Sort the vCard properties in alphabetical order before serializing them!
        var properties = c.Properties.OrderBy(p => p, PropertyComparer.Default).ToList();

        var index = 0;

        // Serialize properties
        foreach (var p in properties)
        {
            // Get a serializer for each property.
            var serializer = sf.Build(p.GetType(), SerializationContext) as IStringSerializer;

            if (p?.Values == null || !p.Values.Any())
            {
                continue;
            }

            // Push this object on the serialization context.
            SerializationContext.Push(p);

            var result = new StringBuilder();
            foreach (var v in p.Values.Where(value => value != null))
            {
                // Get a serializer to serialize the property's value.
                // If we can't serialize the property's value, the next step is worthless anyway.
                var valueSerializer = sf.Build(v.GetType(), SerializationContext) as IStringSerializer;

                // Iterate through each value to be serialized,
                // and give it a property (with parameters).
                // FIXME: this isn't always the way this is accomplished.
                // Multiple values can often be serialized within the
                // same property.  How should we fix this?

                // NOTE:
                // We Serialize the property's value first, as during 
                // serialization it may modify our parameters.
                // FIXME: the "parameter modification" operation should
                // be separated from serialization. Perhaps something
                // like PreSerialize(), etc.
                var value = valueSerializer.SerializeToString(v);

                // Get the list of parameters we'll be serializing
                var parameterList = p.Parameters;
                if (v is ICardDataType)
                {
                    parameterList = (v as ICardDataType).Parameters;
                }

                var stringBuilder = new StringBuilder();
                if (v is IRelatedDataType dataType && dataType.Properties.Any(x => x.Values.Any()))
                {
                    stringBuilder.Append($"item{++index}.");
                }
                stringBuilder.Append(p.Name);
                if (parameterList.Any())
                {
                    // Get a serializer for parameters
                    if (sf.Build(typeof(CardParameter), SerializationContext) is IStringSerializer parameterSerializer)
                    {
                        // Serialize each parameter
                        // Separate parameters with semicolons
                        stringBuilder.Append(";");
                        stringBuilder.Append(string.Join(";", parameterList.Select(param => parameterSerializer.SerializeToString(param))));
                    }
                }
                stringBuilder.Append(":");
                stringBuilder.Append(value);

                result.Append(TextUtil.FoldLines(stringBuilder.ToString()));

                if (v is IRelatedDataType relatedDataType)
                {
                    foreach (var property in relatedDataType.Properties)
                    {
                        var sb2 = new StringBuilder();
                        // Get a serializer for each property.
                        var serializer2 = sf.Build(property.GetType(), SerializationContext) as IStringSerializer;
                        sb2.Append($"item{index}.");
                        sb2.Append(serializer2.SerializeToString(property));

                        result.Append(TextUtil.FoldLines(sb2.ToString()));
                    }
                }

            }

            // Pop the object off the serialization context.
            SerializationContext.Pop();
            sb.Append(result.ToString());
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

    /// <summary>
    /// Deserializes a vCard component from a string.
    /// </summary>
    /// <param name="tr">The text reader to use for deserialization.</param>
    /// <returns>Always null.</returns>
    public override object Deserialize(TextReader tr) => null;

    /// <summary>
    /// Compares two ICardProperty objects based on their name, in a case-insensitive way.
    /// </summary>
    public class PropertyComparer : IComparer<ICardProperty>
    {
        /// <summary>
        /// The default instance of PropertyComparer.
        /// </summary>
        public static PropertyComparer Default = new PropertyComparer();

        /// <summary>
        /// Compares two ICardProperty objects based on their name, in a case-insensitive way.
        /// </summary>
        /// <param name="x">The first ICardProperty to compare.</param>
        /// <param name="y">The second ICardProperty to compare.</param>
        /// <returns>-1 if x is null, 1 if y is null, or the result of a case-insensitive string comparison of their names.</returns>
        public int Compare(ICardProperty x, ICardProperty y)
        {
            return x == y
                ? 0
                : x == null
                ? -1
                : y == null
                ? 1
                : string.Compare(x.Name, y.Name, StringComparison.OrdinalIgnoreCase);
        }
    }
}