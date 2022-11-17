using iCloud.vCard.Net.Types;
using iCloud.vCard.Net.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

namespace iCloud.vCard.Net.Serialization.Converters;

internal class PhotoConverter : TypeConverter
{
    /// <inheritdoc/>
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) => sourceType.IsGenericType && sourceType.GetGenericTypeDefinition().IsAssignableFrom(typeof(IEnumerable<>));

    /// <inheritdoc/>
    public override bool CanConvertTo(ITypeDescriptorContext? context, [NotNullWhen(true)] Type? destinationType) => (destinationType?.IsGenericType ?? false) && destinationType.GetGenericTypeDefinition().IsAssignableFrom(typeof(IEnumerable<>));

    /// <inheritdoc/>
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (!CanConvertFrom(context, typeof(IEnumerable<CardProperty>)) || value is not IEnumerable<CardProperty> cardProperties) throw GetConvertFromException(value);
        return Convert(cardProperties);
    }

    /// <inheritdoc/>
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if (!CanConvertTo(context, destinationType) || value is not Photo photo) throw GetConvertToException(value, destinationType);
        return Convert(photo);
    }

    /// <summary>Converts the PHOTO property to photo.</summary>
    private static Photo? Convert(IEnumerable<CardProperty> properties)
    {
        var property = properties.FindByName(Constants.Contact.Property.Photo.Property.PHOTO).ThrowIfNull(Constants.Contact.Property.Photo.Property.PHOTO);

        if (!property.Subproperties.GetValue(Constants.Contact.Property.Photo.Subproperty.VALUE)
            .Equals(Constants.Contact.Property.Photo.Subproperty.Value.URI, StringComparison.OrdinalIgnoreCase)) return null;

        var rectangleSubproperty = property.Subproperties.GetValue(Constants.Contact.Property.Photo.Subproperty.X_ABCROP_RECTANGLE)
                .Replace(Constants.Contact.Property.Photo.Subproperty.Value.ABClipRect, string.Empty)
                .Split('&', StringSplitOptions.RemoveEmptyEntries)
                .Take(4)
                .Select(x => System.Convert.ToInt32(x)).ToArray();

        var x = rectangleSubproperty[0];
        var y = rectangleSubproperty[1];
        var width = rectangleSubproperty[2];
        var height = rectangleSubproperty[3];

        var value = property.ToString();
        if (string.IsNullOrEmpty(value)) return null;

        var rectangle = new System.Drawing.Rectangle(x, y, width, height);

        if (Uri.TryCreate(value, UriKind.RelativeOrAbsolute, out var uri))
            return new Photo(uri, rectangle);
        else if (IsBase64String(value))
        {
            var bytes = System.Convert.FromBase64String(value);
            return new Photo(bytes, rectangle);
        }

        return null;
    }

    /// <summary>Converts the photo to PHOTO property.</summary>
    private static IEnumerable<CardProperty> Convert(Photo photo)
    {
        var value = photo.Url?.ToString() ?? System.Convert.ToBase64String(photo.Data ?? Array.Empty<byte>());

        if (string.IsNullOrEmpty(value)) return Enumerable.Empty<CardProperty>();

        var rectangle = string.Format(Constants.Contact.Property.Photo.Subproperty.Value.ABClipRectFormat,
                                      photo.Rectangle.X,
                                      photo.Rectangle.Y,
                                      photo.Rectangle.Width,
                                      photo.Rectangle.Height);

        var photoProperty = new CardProperty(Constants.Contact.Property.Photo.Property.PHOTO, value);
        photoProperty.Subproperties.Add(Constants.Contact.Property.Photo.Subproperty.X_ABCROP_RECTANGLE, rectangle);
        photoProperty.Subproperties.Add(Constants.Contact.Property.Photo.Subproperty.VALUE, Constants.Contact.Property.Photo.Subproperty.Value.URI.ToLower());

        return new[] { photoProperty };
    }

    private static bool IsBase64String(string base64)
    {
        var buffer = new Span<byte>(new byte[base64.Length]);
        return System.Convert.TryFromBase64String(base64, buffer, out int _);
    }
}
