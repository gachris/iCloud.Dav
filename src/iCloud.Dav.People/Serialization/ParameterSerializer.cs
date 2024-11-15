using System.Text;
using vCard.Net;
using vCard.Net.Serialization;

namespace iCloud.Dav.People.Serialization;

/// <summary>
/// Serializes a <see cref="VCardParameter"/> to a string representation, according to the vCard specification.
/// </summary>
public class ParameterSerializer : SerializerBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ParameterSerializer"/> class.
    /// </summary>
    public ParameterSerializer() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ParameterSerializer"/> class with the specified <see cref="SerializationContext"/>.
    /// </summary>
    /// <param name="ctx">The <see cref="SerializationContext"/>.</param>
    public ParameterSerializer(SerializationContext ctx) : base(ctx) { }

    /// <summary>
    /// Gets the Type that this <see cref="ParameterSerializer"/> can serialize and deserialize, which is <see cref="VCardParameter"/>.
    /// </summary>
    public override Type TargetType => typeof(VCardParameter);

    /// <summary>
    /// Converts a <see cref="VCardParameter"/> to a string representation.
    /// </summary>
    /// <param name="obj">The <see cref="VCardParameter"/> object to be serialized.</param>
    /// <returns>A string representation of the <see cref="VCardParameter"/> object.</returns>
    public override string SerializeToString(object obj)
    {
        if (obj is not VCardParameter p)
        {
            return null;
        }

        var builder = new StringBuilder();

        if (p.Name.Equals("TYPE", StringComparison.OrdinalIgnoreCase))
        {
            var typeValues = p.Values.Select(type => string.Concat(p.Name, "=", type));
            var values = string.Join(";", typeValues);

            // Surround the parameter value with double quotes, if the value
            // contains any problematic characters.
            if (values.IndexOfAny([':', ',']) >= 0)
            {
                values = "\"" + values + "\"";
            }
            builder.Append(values);
        }
        else
        {
            builder.Append(p.Name + "=");

            // "Section 3.2:  Property parameter values MUST NOT contain the DQUOTE character."
            // Therefore, let's strip any double quotes from the value.
            var values = string.Join(",", p.Values).Replace("\"", string.Empty);

            // Surround the parameter value with double quotes, if the value
            // contains any problematic characters.
            if (values.IndexOfAny([';', ':', ',']) >= 0)
            {
                values = "\"" + values + "\"";
            }
            builder.Append(values);
        }

        return builder.ToString();
    }

    /// <summary>
    /// This method is not implemented for the <see cref="ParameterSerializer"/> class.
    /// </summary>
    /// <param name="tr">The <see cref="TextReader"/>.</param>
    /// <returns>null</returns>
    public override object Deserialize(TextReader tr) => null;
}