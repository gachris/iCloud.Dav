using iCloud.vCard.Net.Serialization.Mapping;
using iCloud.vCard.Net.Utils;
using System.Collections.Generic;
using System;
using iCloud.vCard.Net.Data;

namespace iCloud.vCard.Net.Serialization;

public class WebsiteSerializer : SerializerBase<Website>
{
    /// <summary>Converts the URL property to website.</summary>
    public override void Deserialize(CardPropertyList properties, Website website)
    {
        var urlProperty = properties.FindByName(Constants.Contact.Website.Property.URL).ThrowIfNull(Constants.Contact.Website.Property.URL);
        website.Url = urlProperty.ToString();

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
            var labelProperty = properties.FindByName(Constants.Contact.Website.Property.X_ABLABEL);
            if (labelProperty is not null && labelProperty.Value?.ToString() is string label)
            {
                switch (label)
                {
                    case Constants.Contact.Website.CustomType.HomePage:
                        websiteTypeFromInternal = WebsiteType.HomePage;
                        break;
                    case Constants.Contact.Website.CustomType.School:
                        websiteTypeFromInternal = WebsiteType.School;
                        break;
                    case Constants.Contact.Website.CustomType.Blog:
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
    }

    /// <summary>Converts the website to URL property.</summary>
    public override CardPropertyList Serialize(Website website)
    {
        var properties = new List<CardProperty>();
        website.Url = website.Url.ThrowIfNull(nameof(website.Url));
        var groupId = Guid.NewGuid().ToString();
        var urlProperty = new CardProperty(Constants.Contact.Website.Property.URL, website.Url.ToString(), groupId);
        properties.Add(urlProperty);

        var websiteTypeInternal = WebsiteTypeMapping.GetType(website.WebsiteType);
        if (website.IsPreferred)
        {
            websiteTypeInternal = websiteTypeInternal.AddFlags(WebsiteTypeInternal.Pref);
        }

        if (websiteTypeInternal is not 0)
        {
            websiteTypeInternal.StringArrayFlags()?.
                ForEach(type => urlProperty.Subproperties.Add(Constants.Contact.Website.Property.TYPE, type.ToUpper()));
        }

        var label = website.WebsiteType switch
        {
            WebsiteType.HomePage => Constants.Contact.Website.CustomType.HomePage,
            WebsiteType.School => Constants.Contact.Website.CustomType.School,
            WebsiteType.Blog => Constants.Contact.Website.CustomType.Blog,
            WebsiteType.Custom => website.Label,
            _ => null,
        };

        if (label is not null)
        {
            var labelProperty = new CardProperty(Constants.Contact.Website.Property.X_ABLABEL, label, groupId);
            properties.Add(labelProperty);
        }

        return new(properties);
    }
}
