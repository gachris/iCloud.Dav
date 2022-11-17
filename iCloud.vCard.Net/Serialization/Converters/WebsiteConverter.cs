using iCloud.vCard.Net.Serialization.Mapping;
using iCloud.vCard.Net.Types;
using iCloud.vCard.Net.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace iCloud.vCard.Net.Serialization.Converters;

internal class WebsiteConverter : TypeConverter
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
        if (!CanConvertTo(context, destinationType) || value is not Website website) throw GetConvertToException(value, destinationType);
        return Convert(website);
    }

    /// <summary>Converts the URL property to website.</summary>
    private static Website? Convert(IEnumerable<CardProperty> properties)
    {
        var urlProperty = properties.FindByName(Constants.Contact.Property.Website.Property.URL).ThrowIfNull(Constants.Contact.Property.Website.Property.URL);
        var website = new Website { Url = urlProperty.ToString() };

        _ = urlProperty.Subproperties.TryParse<WebsiteTypeInternal>(out var websiteTypeInternal);
        var isPreferred = websiteTypeInternal.HasFlag(WebsiteTypeInternal.Pref);
        if (isPreferred)
        {
            website.IsPreferred = true;
            websiteTypeInternal = websiteTypeInternal.RemoveFlags(WebsiteTypeInternal.Pref);
        }

        var websiteTypeFromInternal = WebsiteTypeMapping.GetType(websiteTypeInternal);
        if (websiteTypeFromInternal is 0)
        {
            var labelProperty = properties.FindByName(Constants.Contact.Property.Website.Property.X_ABLABEL);
            if (labelProperty is not null && labelProperty.Value?.ToString() is string label)
            {
                switch (label)
                {
                    case Constants.Contact.Property.Website.CustomType.HomePage:
                        websiteTypeFromInternal = WebsiteType.HomePage;
                        break;
                    case Constants.Contact.Property.Website.CustomType.School:
                        websiteTypeFromInternal = WebsiteType.School;
                        break;
                    case Constants.Contact.Property.Website.CustomType.Blog:
                        websiteTypeFromInternal = WebsiteType.Blog;
                        break;
                    default:
                        websiteTypeFromInternal = WebsiteType.Custom;
                        website.Label = label;
                        break;
                }
            }
        }

        website.WebsiteType = websiteTypeFromInternal;

        return website;
    }

    /// <summary>Converts the website to URL property.</summary>
    private static IEnumerable<CardProperty> Convert(Website website)
    {
        var properties = new List<CardProperty>();
        website.Url = website.Url.ThrowIfNull(nameof(website.Url));
        var groupId = Guid.NewGuid().ToString();
        var urlProperty = new CardProperty(Constants.Contact.Property.Website.Property.URL, website.Url.ToString(), groupId);
        properties.Add(urlProperty);

        var websiteTypeInternal = WebsiteTypeMapping.GetType(website.WebsiteType);
        if (website.IsPreferred)
        {
            websiteTypeInternal = websiteTypeInternal.AddFlags(WebsiteTypeInternal.Pref);
        }

        if (websiteTypeInternal is not 0)
        {
            websiteTypeInternal.StringArrayFlags()?.
                ForEach(type => urlProperty.Subproperties.Add(Constants.Contact.Property.Website.Property.TYPE, type.ToUpper()));
        }

        var label = website.WebsiteType switch
        {
            WebsiteType.HomePage => Constants.Contact.Property.Website.CustomType.HomePage,
            WebsiteType.School => Constants.Contact.Property.Website.CustomType.School,
            WebsiteType.Blog => Constants.Contact.Property.Website.CustomType.Blog,
            WebsiteType.Custom => website.Label,
            _ => null,
        };

        if (label is not null)
        {
            var labelProperty = new CardProperty(Constants.Contact.Property.Website.Property.X_ABLABEL, label, groupId);
            properties.Add(labelProperty);
        }

        return properties;
    }
}
