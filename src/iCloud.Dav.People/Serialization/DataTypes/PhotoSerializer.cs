using iCloud.Dav.People.DataTypes;
using vCard.Net.Serialization;
using vCard.Net.Serialization.DataTypes;

namespace iCloud.Dav.People.Serialization.DataTypes;

/// <summary>
/// Serializes and deserializes a <see cref="Photo"/> object to and from a string representation, according to the vCard specification.
/// </summary>
public class PhotoSerializer : EncodableDataTypeSerializer
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PhotoSerializer"/> class.
    /// </summary>
    public PhotoSerializer()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PhotoSerializer"/> class with the given <see cref="SerializationContext"/>.
    /// </summary>
    /// <param name="ctx">The <see cref="SerializationContext"/> to use.</param>
    public PhotoSerializer(SerializationContext ctx) : base(ctx)
    {
    }

    /// <summary>
    /// Gets the Type that this <see cref="PhotoSerializer"/> can serialize and deserialize, which is <see cref="Photo"/>.
    /// </summary>
    public override Type TargetType => typeof(Photo);

    /// <summary>
    /// Converts a <see cref="Photo"/> object to a string representation.
    /// </summary>
    /// <param name="obj">The <see cref="Photo"/> object to be serialized.</param>
    /// <returns>A string representation of the <see cref="Photo"/> object.</returns>
    public override string SerializeToString(object obj)
    {
        if (obj is not Photo photo)
        {
            return null;
        }

        var value = photo.Url?.ToString() ?? Convert.ToBase64String(photo.Data ?? Array.Empty<byte>());

        if (string.IsNullOrEmpty(value))
        {
            return null;
        }

        var rectangle = string.Format("ABClipRect_{0}&{1}&{2}&{3}&{4}&==",
                                      1,
                                      photo.Rectangle.X,
                                      photo.Rectangle.Y,
                                      photo.Rectangle.Width,
                                      photo.Rectangle.Height);

        photo.Parameters.Set("X-ABCROP-RECTANGLE", rectangle);

        if (photo.Url != null)
        {
            photo.SetValueType("uri");
        }

        return Encode(photo, value);
    }

    /// <summary>
    /// Converts a string representation of a <see cref="Photo"/> object to a <see cref="Photo"/> object.
    /// </summary>
    /// <param name="value">The string representation of the <see cref="Photo"/> object to be deserialized.</param>
    /// <returns>A <see cref="Photo"/> object.</returns>
    public Photo Deserialize(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        if (CreateAndAssociate() is not Photo photo)
        {
            return null;
        }

        // Decode the value, if necessary!
        value = Decode(photo, value);

        if (value is null)
        {
            return null;
        }

        if (Base64Helper.TryFromBase64String(value, out var data))
        {
            photo.Data = data;
        }
        else if (Uri.TryCreate(value, UriKind.RelativeOrAbsolute, out var uri))
        {
            photo.Url = uri;
        }

        var x_abCrop_rectangle = photo.Parameters.Get("X-ABCROP-RECTANGLE");

        var rectangleSubProperty = x_abCrop_rectangle
                .Replace("ABClipRect_", string.Empty)
                .Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries)
                .Take(5)
                .Select(property => Convert.ToInt32(property)).ToArray();

        var dimension = rectangleSubProperty[0];
        var x = rectangleSubProperty[1];
        var y = rectangleSubProperty[2];
        var width = rectangleSubProperty[3];
        var height = rectangleSubProperty[4];

        var rectangle = new System.Drawing.Rectangle(x, y, width, height);

        photo.Rectangle = rectangle;

        return photo;
    }

    /// <summary>
    /// This method deserializes a <see cref="Photo"/> object from the given <see cref="TextReader"/>.
    /// </summary>
    /// <param name="tr">The <see cref="TextReader"/> to deserialize the <see cref="Photo"/> object from.</param>
    /// <returns>A <see cref="Photo"/> object.</returns>
    public override object Deserialize(TextReader tr) => Deserialize(tr.ReadToEnd());
}
