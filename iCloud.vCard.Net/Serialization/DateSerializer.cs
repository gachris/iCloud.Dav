using iCloud.vCard.Net.Data;
using iCloud.vCard.Net.Serialization.Mapping;
using iCloud.vCard.Net.Utils;
using System;
using System.Collections.Generic;

namespace iCloud.vCard.Net.Serialization;

public class DateSerializer : SerializerBase<Date>
{
    /// <summary>Converts the X-ABDATE property to date.</summary>
    public override void Deserialize(CardPropertyList properties, Date date)
    {
        var dateProperty = properties.FindByName(Constants.Contact.Date.Property.X_ABDATE).ThrowIfNull(Constants.Contact.Date.Property.X_ABDATE);
        date.DateTime = DateTimeHelper.TryParseDate(dateProperty.ToString());

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
            var labelProperty = properties.FindByName(Constants.Contact.Date.Property.X_ABLABEL);
            if (labelProperty is not null && labelProperty.Value?.ToString() is string label)
            {
                switch (label)
                {
                    case Constants.Contact.Date.CustomType.Anniversary:
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
    }

    /// <summary>Converts the date to X-ABDATE property.</summary>
    public override CardPropertyList Serialize(Date date)
    {
        var properties = new List<CardProperty>();
        date.DateTime = date.DateTime.ThrowIfNull(nameof(date.DateTime));
        var groupId = Guid.NewGuid().ToString();
        var urlProperty = new CardProperty(Constants.Contact.Date.Property.X_ABDATE, date.DateTime.ToString()!, groupId);
        properties.Add(urlProperty);

        var dateTypeInternal = DateTypeMapping.GetType(date.DateType);
        if (date.IsPreferred)
        {
            dateTypeInternal = dateTypeInternal.AddFlags(DateTypeInternal.Pref);
        }

        if (dateTypeInternal is not 0)
        {
            dateTypeInternal.StringArrayFlags()?.
                ForEach(type => urlProperty.Subproperties.Add(Constants.Contact.Date.Property.TYPE, type.ToUpper()));
        }

        var label = date.DateType switch
        {
            DateType.Anniversary => Constants.Contact.Date.CustomType.Anniversary,
            DateType.Custom => date.Label,
            _ => null,
        };

        if (label is not null)
        {
            var labelProperty = new CardProperty(Constants.Contact.Date.Property.X_ABLABEL, label, groupId);
            properties.Add(labelProperty);
        }

        return new(properties);
    }
}
