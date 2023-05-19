using iCloud.Dav.People.DataTypes;
using System;
using System.IO;
using System.Linq;
using vCard.Net.Serialization;
using vCard.Net.Serialization.DataTypes;

namespace iCloud.Dav.People.Serialization.DataTypes
{
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
            if (!(obj is Photo photo))
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

            if (!(CreateAndAssociate() is Photo photo))
            {
                return null;
            }

            // Decode the value, if necessary!
            value = Decode(photo, value);

            if (value is null)
            {
                return null;
            }

            if (Uri.TryCreate(value, UriKind.RelativeOrAbsolute, out var uri))
            {
                photo.Url = uri;
            }
            else
            {
                try
                {
                    var bytes = Convert.FromBase64String(value);
                    photo.Data = bytes;
                }
                catch
                {
                }
            }

            var x_abcrop_rectangle = photo.Parameters.Get("X-ABCROP-RECTANGLE");

            var rectangleSubproperty = x_abcrop_rectangle
                    .Replace("ABClipRect_", string.Empty)
                    .Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries)
                    .Take(5)
                    .Select(property => Convert.ToInt32(property)).ToArray();

            var dimension = rectangleSubproperty[0];
            var x = rectangleSubproperty[1];
            var y = rectangleSubproperty[2];
            var width = rectangleSubproperty[3];
            var height = rectangleSubproperty[4];

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
}