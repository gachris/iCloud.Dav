﻿using iCloud.Dav.People.DataTypes;
using System;
using System.IO;
using System.Linq;
using vCard.Net.Serialization;
using vCard.Net.Serialization.DataTypes;

namespace iCloud.Dav.People.Serialization.DataTypes
{
    public class PhotoSerializer : EncodableDataTypeSerializer
    {
        public PhotoSerializer()
        {
        }

        public PhotoSerializer(SerializationContext ctx) : base(ctx)
        {
        }

        public override Type TargetType => typeof(Photo);

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

        public override object Deserialize(TextReader tr) => Deserialize(tr.ReadToEnd());
    }
}