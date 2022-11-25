using iCloud.Dav.People.DataTypes.Mapping;
using iCloud.Dav.People.DataTypes;
using System;
using System.IO;
using vCard.Net.Serialization;
using vCard.Net.Serialization.DataTypes;
using iCloud.Dav.People.Utils;

namespace iCloud.Dav.People.Serialization.DataTypes;

public class WebsiteSerializer : EncodableDataTypeSerializer
{
    public WebsiteSerializer() : base()
    {
    }

    public WebsiteSerializer(SerializationContext ctx) : base(ctx)
    {
    }

    public override Type TargetType => typeof(Website);

    public override string? SerializeToString(object obj)
    {
        if (obj is not Website email)
        {
            return null;
        }

        //    var properties = new List<CardProperty>();
        //    website.Url = website.Url.ThrowIfNull(nameof(website.Url));
        //    var groupId = Guid.NewGuid().ToString();
        //    var urlProperty = new CardProperty(Constants.Contact.Website.Property.URL, website.Url.ToString(), groupId);
        //    properties.Add(urlProperty);

        //    var websiteTypeInternal = WebsiteTypeMapping.GetType(website.WebsiteType);
        //    if (website.IsPreferred)
        //    {
        //        websiteTypeInternal = websiteTypeInternal.AddFlags(WebsiteTypeInternal.Pref);
        //    }

        //    if (websiteTypeInternal is not 0)
        //    {
        //        websiteTypeInternal.StringArrayFlags()?.
        //            ForEach(type => urlProperty.Subproperties.Add(Constants.Contact.Website.Property.TYPE, type.ToUpper()));
        //    }

        //    var label = website.WebsiteType switch
        //    {
        //        WebsiteType.HomePage => Constants.Contact.Website.CustomType.HomePage,
        //        WebsiteType.School => Constants.Contact.Website.CustomType.School,
        //        WebsiteType.Blog => Constants.Contact.Website.CustomType.Blog,
        //        WebsiteType.Custom => website.Label,
        //        _ => null,
        //    };

        //    if (label is not null)
        //    {
        //        var labelProperty = new CardProperty(Constants.Contact.Website.Property.X_ABLABEL, label, groupId);
        //        properties.Add(labelProperty);
        //    }

        //    return new(properties);

        var value = email.Label;
        return Encode(email, value);
    }

    public Website? Deserialize(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        if (CreateAndAssociate() is not Website website)
        {
            return null;
        }

        // Decode the value, if necessary!
        value = Decode(website, value);

        if (value is null)
        {
            return null;
        }

        website.Url = value;

        var types = website.Parameters.GetMany("TYPE");

        _ = types.TryParse<WebsiteTypeInternal>(out var websiteTypeInternal);
        var isPreferred = websiteTypeInternal.HasFlag(WebsiteTypeInternal.Pref);
        if (isPreferred)
        {
            website.IsPreferred = true;
            websiteTypeInternal = websiteTypeInternal.RemoveFlags(WebsiteTypeInternal.Pref);
        }

        var websiteTypeFromInternal = WebsiteTypeMapping.GetType(websiteTypeInternal);
        if (websiteTypeFromInternal is 0)
        {
            //var labelProperty = properties.FindByName(Constants.Contact.Website.Property.X_ABLABEL);
            //if (labelProperty is not null && labelProperty.Value?.ToString() is string label)
            //{
            //    switch (label)
            //    {
            //        case Constants.Contact.Website.CustomType.HomePage:
            //            websiteTypeFromInternal = WebsiteType.HomePage;
            //            break;
            //        case Constants.Contact.Website.CustomType.School:
            //            websiteTypeFromInternal = WebsiteType.School;
            //            break;
            //        case Constants.Contact.Website.CustomType.Blog:
            //            websiteTypeFromInternal = WebsiteType.Blog;
            //            break;
            //        default:
            //            websiteTypeFromInternal = WebsiteType.Custom;
            //            website.Label = label;
            //            break;
            //    }
            //}
        }

        website.WebsiteType = websiteTypeFromInternal;

        return website;
    }

    public override object? Deserialize(TextReader tr) => Deserialize(tr.ReadToEnd());
}
