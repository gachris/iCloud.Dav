using iCloud.Dav.People.DataTypes;
using iCloud.Dav.People.DataTypes.Mapping;
using iCloud.Dav.People.Utils;
using System;
using System.IO;
using vCard.Net.Serialization;
using vCard.Net.Serialization.DataTypes;

namespace iCloud.Dav.People.Serialization.DataTypes
{
    public class X_ABDateSerializer : EncodableDataTypeSerializer
    {
        public X_ABDateSerializer()
        {
        }

        public X_ABDateSerializer(SerializationContext ctx) : base(ctx)
        {
        }

        public override Type TargetType => typeof(X_ABDate);

        public override string SerializeToString(object obj)
        {
            if (!(obj is X_ABDate date))
            {
                return null;
            }

            // var properties = new List<CardProperty>();
            // date.DateTime = date.DateTime.ThrowIfNull(nameof(date.DateTime));
            // var groupId = Guid.NewGuid().ToString();
            // var urlProperty = new CardProperty(Constants.Contact.Date.Property.X_ABDATE, date.DateTime.ToString()!, groupId);
            // properties.Add(urlProperty);

            // var dateTypeInternal = DateTypeMapping.GetType(date.DateType);
            // if (date.IsPreferred)
            // {
            //     dateTypeInternal = dateTypeInternal.AddFlags(DateTypeInternal.Pref);
            // }

            // if (dateTypeInternal is not 0)
            // {
            //     dateTypeInternal.StringArrayFlags()?.
            //         ForEach(type => urlProperty.Subproperties.Add(Constants.Contact.Date.Property.TYPE, type.ToUpper()));
            // }

            // var label = date.DateType switch
            // {
            //     DateType.Anniversary => Constants.Contact.Date.CustomType.Anniversary,
            //     DateType.Custom => date.Label,
            //     _ => null,
            // };

            // if (label is not null)
            // {
            //     var labelProperty = new CardProperty(Constants.Contact.Date.Property.X_ABLABEL, label, groupId);
            //     properties.Add(labelProperty);
            // }

            // return new(properties);

            var value = date.Label;
            return Encode(date, value);
        }

        public X_ABDate Deserialize(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            if (!(CreateAndAssociate() is X_ABDate date))
            {
                return null;
            }

            // Decode the value, if necessary!
            value = Decode(date, value);

            if (value is null)
            {
                return null;
            }

            date.DateTime = DateTimeHelper.TryParseDate(value);

            var types = date.Parameters.GetMany("TYPE");

            _ = types.TryParse<DateTypeInternal>(out var dateTypeInternal);
            var isPreferred = dateTypeInternal.HasFlag(DateTypeInternal.Pref);
            if (isPreferred)
            {
                date.IsPreferred = true;
                dateTypeInternal = dateTypeInternal.RemoveFlags(DateTypeInternal.Pref);
            }

            var dateTypeFromInternal = DateTypeMapping.GetType(dateTypeInternal);
            if (dateTypeFromInternal is 0)
            {
                //var labelProperty = properties.FindByName(Constants.Contact.Date.Property.X_ABLABEL);
                //if (labelProperty is not null && labelProperty.Value?.ToString() is string label)
                //{
                //    switch (label)
                //    {
                //        case Constants.Contact.Date.CustomType.Anniversary:
                //            dateTypeFromInternal = DateType.Anniversary;
                //            break;
                //        default:
                //            dateTypeFromInternal = DateType.Custom;
                //            date.Label = label;
                //            break;
                //    }
                //}
            }

            date.DateType = dateTypeFromInternal;

            return date;
        }

        public override object Deserialize(TextReader tr) => Deserialize(tr.ReadToEnd());
    }
}