using iCloud.Dav.People.DataTypes;
using iCloud.Dav.People.DataTypes.Mapping;
using iCloud.Dav.People.Utils;
using System;
using System.IO;
using vCard.Net.Serialization;
using vCard.Net.Serialization.DataTypes;

namespace iCloud.Dav.People.Serialization.DataTypes
{
    public class EmailSerializer : EncodableDataTypeSerializer
    {
        public EmailSerializer()
        {
        }

        public EmailSerializer(SerializationContext ctx) : base(ctx)
        {
        }

        public override Type TargetType => typeof(Email);

        public override string SerializeToString(object obj)
        {
            if (!(obj is Email email))
            {
                return null;
            }

            //    var properties = new List<CardProperty>();
            //    emailAddress.Address = emailAddress.Address.ThrowIfNull(nameof(emailAddress.Address));
            //    var groupId = Guid.NewGuid().ToString();
            //    var emailProperty = new CardProperty(Constants.Contact.EmailAddress.Property.EMAIL, emailAddress.Address, groupId);
            //    properties.Add(emailProperty);

            //    var emailTypeInternal = EmailTypeMapping.GetType(emailAddress.EmailType);
            //    if (emailAddress.IsPreferred)
            //    {
            //        emailTypeInternal = emailTypeInternal.AddFlags(EmailTypeInternal.Pref);
            //    }

            //    if (emailTypeInternal is not 0)
            //    {
            //        emailTypeInternal.StringArrayFlags()?
            //            .ForEach(type => emailProperty.Subproperties.Add(Constants.Contact.EmailAddress.Property.TYPE, type.ToUpper()));
            //    }

            //    var label = emailAddress.EmailType switch
            //    {
            //        EmailType.School => Constants.Contact.EmailAddress.CustomType.School,
            //        EmailType.Custom => emailAddress.Label,
            //        _ => null,
            //    };

            //    if (label is not null)
            //    {
            //        var labelProperty = new CardProperty(Constants.Contact.EmailAddress.Property.X_ABLABEL, label, groupId);
            //        properties.Add(labelProperty);
            //    }

            //    return new(properties);

            var value = email.Address;
            return Encode(email, value);
        }

        public Email Deserialize(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            if (!(CreateAndAssociate() is Email email))
            {
                return null;
            }

            // Decode the value, if necessary!
            value = Decode(email, value);

            if (value is null)
            {
                return null;
            }

            email.Address = value;

            var types = email.Parameters.GetMany("TYPE");

            _ = types.TryParse<EmailTypeInternal>(out var emailTypeInternal);
            var isPreferred = emailTypeInternal.HasFlag(EmailTypeInternal.Pref);
            if (isPreferred)
            {
                email.IsPreferred = true;
                emailTypeInternal = emailTypeInternal.RemoveFlags(EmailTypeInternal.Pref);
            }

            var emailTypeFromInternal = EmailTypeMapping.GetType(emailTypeInternal);
            if (emailTypeFromInternal is 0)
            {
                //var labelProperty = properties.FindByName(Constants.Contact.EmailAddress.Property.X_ABLABEL);
                //if (labelProperty is not null && labelProperty.Value?.ToString() is string label)
                //{
                //    switch (label)
                //    {
                //        case Constants.Contact.EmailAddress.CustomType.School:
                //            emailTypeFromInternal = EmailType.School;
                //            break;
                //        default:
                //            emailTypeFromInternal = EmailType.Custom;
                //            email.Label = label;
                //            break;
                //    }
                //}
            }

            email.EmailType = emailTypeFromInternal;

            return email;
        }

        public override object Deserialize(TextReader tr) => Deserialize(tr.ReadToEnd());
    }
}