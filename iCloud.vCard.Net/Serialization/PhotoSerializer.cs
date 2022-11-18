using iCloud.vCard.Net.Data;
using System;
using System.Linq;

namespace iCloud.vCard.Net.Serialization;

public class PhotoSerializer : SerializerBase<Photo>
{
    /// <summary>Reads the PHOTO property.</summary>
    public override void Deserialize(CardPropertyList properties, Photo photo)
    {
        var property = properties.FindByName(Constants.Contact.Photo.Property.PHOTO);

        if (property == null) return;

        if (!property.Subproperties.GetValue(Constants.Contact.Photo.Subproperty.VALUE)
            .Equals(Constants.Contact.Photo.Subproperty.Value.URI, StringComparison.OrdinalIgnoreCase)) return;

        var rectangleSubproperty = property.Subproperties.GetValue(Constants.Contact.Photo.Subproperty.X_ABCROP_RECTANGLE)
                .Replace(Constants.Contact.Photo.Subproperty.Value.ABClipRect, string.Empty)
                .Split('&', StringSplitOptions.RemoveEmptyEntries)
                .Take(4)
                .Select(x => Convert.ToInt32(x)).ToArray();

        var x = rectangleSubproperty[0];
        var y = rectangleSubproperty[1];
        var width = rectangleSubproperty[2];
        var height = rectangleSubproperty[3];

        var value = property.ToString();
        if (string.IsNullOrEmpty(value)) return;

        var rectangle = new System.Drawing.Rectangle(x, y, width, height);

        if (Uri.TryCreate(value, UriKind.RelativeOrAbsolute, out var uri))
        {
            photo.Url = uri;
            photo.Rectangle = rectangle;
        }
        else if (IsBase64String(value))
        {
            var bytes = Convert.FromBase64String(value);
            photo.Data = bytes;
            photo.Rectangle = rectangle;
        }
    }

    public override CardPropertyList Serialize(Photo? photo)
    {
        if (photo is null) return new(Enumerable.Empty<CardProperty>());

        var value = photo.Url?.ToString() ?? Convert.ToBase64String(photo.Data ?? Array.Empty<byte>());

        if (string.IsNullOrEmpty(value)) return new(Enumerable.Empty<CardProperty>());

        var rectangle = string.Format(Constants.Contact.Photo.Subproperty.Value.ABClipRectFormat,
                                      photo.Rectangle.X,
                                      photo.Rectangle.Y,
                                      photo.Rectangle.Width,
                                      photo.Rectangle.Height);

        var photoProperty = new CardProperty(Constants.Contact.Photo.Property.PHOTO, value);
        photoProperty.Subproperties.Add(Constants.Contact.Photo.Subproperty.X_ABCROP_RECTANGLE, rectangle);
        photoProperty.Subproperties.Add(Constants.Contact.Photo.Subproperty.VALUE, Constants.Contact.Photo.Subproperty.Value.URI.ToLower());

        return new(new[] { photoProperty });
    }

    private static bool IsBase64String(string base64)
    {
        var buffer = new Span<byte>(new byte[base64.Length]);
        return Convert.TryFromBase64String(base64, buffer, out int _);
    }
}
