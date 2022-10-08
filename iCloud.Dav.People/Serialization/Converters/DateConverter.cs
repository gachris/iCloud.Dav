using iCloud.Dav.Core.Utils;
using iCloud.Dav.People.Serialization.Mapping;
using iCloud.Dav.People.Types;
using iCloud.Dav.People.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace iCloud.Dav.People.Serialization.Converters;

internal class DateConverter : TypeConverter
{
    /// <inheritdoc/>
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) =>
        sourceType.IsGenericType && sourceType.GetGenericTypeDefinition().IsAssignableFrom(typeof(IEnumerable<>));

    /// <inheritdoc/>
    public override bool CanConvertTo(ITypeDescriptorContext? context, [NotNullWhen(true)] Type? destinationType) =>
        (destinationType?.IsGenericType ?? false) && destinationType.GetGenericTypeDefinition().IsAssignableFrom(typeof(IEnumerable<>));

    /// <inheritdoc/>
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (!CanConvertFrom(context, typeof(IEnumerable<CardProperty>)) || value is not IEnumerable<CardProperty> cardProperties) throw GetConvertFromException(value);
        return Convert(cardProperties);
    }

    /// <inheritdoc/>
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if (!CanConvertTo(context, destinationType) || value is not Date date) throw GetConvertToException(value, destinationType);
        return Convert(date);
    }

    /// <summary>Converts the X-ABDATE property to date.</summary>
    private static Date? Convert(IEnumerable<CardProperty> properties)
    {
        var dateProperty = properties.FindByName(Constants.Contact.Property.Date.Property.X_ABDATE).ThrowIfNull(Constants.Contact.Property.Date.Property.X_ABDATE);
        var date = new Date() { DateTime = DateTimeHelper.TryParseDate(dateProperty.ToString()) };

        _ = dateProperty.Subproperties.TryParse<DateTypeInternal>(out var dateTypeInternal);
        var isPreferred = dateTypeInternal.HasFlag(DateTypeInternal.Pref);
        if (isPreferred)
        {
            date.IsPreferred = true;
            dateTypeInternal = dateTypeInternal.RemoveFlags(DateTypeInternal.Pref);
        }

        var dateTypeFromInternal = DateTypeMapping.GetType(dateTypeInternal);
        if (dateTypeFromInternal is 0)
        {
            var labelProperty = properties.FindByName(Constants.Contact.Property.Date.Property.X_ABLABEL);
            if (labelProperty is not null && labelProperty.Value?.ToString() is string label)
            {
                switch (label)
                {
                    case Constants.Contact.Property.Date.CustomType.Anniversary:
                        dateTypeFromInternal = DateType.Anniversary;
                        break;
                    default:
                        dateTypeFromInternal = DateType.Custom;
                        date.Label = label;
                        break;
                }
            }
        }

        date.DateType = dateTypeFromInternal;

        return date;
    }

    /// <summary>Converts the date to X-ABDATE property.</summary>
    private static IEnumerable<CardProperty> Convert(Date date)
    {
        var properties = new List<CardProperty>();
        date.DateTime = date.DateTime.ThrowIfNull(nameof(date.DateTime));
        var groupId = Guid.NewGuid().ToString();
        var urlProperty = new CardProperty(Constants.Contact.Property.Date.Property.X_ABDATE, date.DateTime.ToString()!, groupId);
        properties.Add(urlProperty);

        var dateTypeInternal = DateTypeMapping.GetType(date.DateType);
        if (date.IsPreferred)
        {
            dateTypeInternal = dateTypeInternal.AddFlags(DateTypeInternal.Pref);
        }

        if (dateTypeInternal is not 0)
        {
            dateTypeInternal.StringArrayFlags()?.
                ForEach(type => urlProperty.Subproperties.Add(Constants.Contact.Property.Date.Property.TYPE, type.ToUpper()));
        }

        var label = date.DateType switch
        {
            DateType.Anniversary => Constants.Contact.Property.Date.CustomType.Anniversary,
            DateType.Custom => date.Label,
            _ => null,
        };

        if (label is not null)
        {
            var labelProperty = new CardProperty(Constants.Contact.Property.Date.Property.X_ABLABEL, label, groupId);
            properties.Add(labelProperty);
        }

        return properties;
    }
}
