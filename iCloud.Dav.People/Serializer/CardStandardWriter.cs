using System;
using System.IO;
using vCards;

namespace iCloud.Dav.People.Serializer
{
    internal class CardStandardWriter : vCardStandardWriter
    {
        public CardStandardWriter() : base()
        {
        }

        private vCardPropertyCollection BuildProperties(vCard card)
        {
            var properties = new vCardPropertyCollection
            {
                new vCardProperty("BEGIN", "VCARD"),
                new vCardProperty("VERSION", "3.0")
            };
            BuildProperties_NAME(properties, card);
            BuildProperties_SOURCE(properties, card);
            BuildProperties_N(properties, card);
            BuildProperties_FN(properties, card);
            BuildProperties_ADR(properties, card);
            BuildProperties_XABADR(properties, card);
            BuildProperties_BDAY(properties, card);
            BuildProperties_CATEGORIES(properties, card);
            BuildProperties_CLASS(properties, card);
            BuildProperties_EMAIL(properties, card);
            BuildProperties_GEO(properties, card);
            BuildProperties_KEY(properties, card);
            BuildProperties_LABEL(properties, card);
            BuildProperties_MAILER(properties, card);
            BuildProperties_NICKNAME(properties, card);
            BuildProperties_NOTE(properties, card);
            BuildProperties_ORG(properties, card);
            BuildProperties_PHOTO(properties, card);
            BuildProperties_PRODID(properties, card);
            BuildProperties_REV(properties, card);
            BuildProperties_ROLE(properties, card);
            BuildProperties_TEL(properties, card);
            BuildProperties_TITLE(properties, card);
            BuildProperties_TZ(properties, card);
            BuildProperties_UID(properties, card);
            BuildProperties_XADDRESSBOOKSERVERKIND(properties, card);
            BuildProperties_XADDRESSBOOKSERVERMEMBER(properties, card);
            BuildProperties_URL(properties, card);
            BuildProperties_X_WAB_GENDER(properties, card);
            properties.Add(new vCardProperty("END", "VCARD"));
            return properties;
        }

        private static void BuildProperties_ADR(vCardPropertyCollection properties, vCard card)
        {
            if (card is Person person)
            {
                foreach (var address in person.Addresses)
                {
                    if (!string.IsNullOrEmpty(address.City) || !string.IsNullOrEmpty(address.Country) || !string.IsNullOrEmpty(address.PostalCode) || !string.IsNullOrEmpty(address.Region) || !string.IsNullOrEmpty(address.Street))
                    {
                        if (string.IsNullOrEmpty(address.Id))
                            address.Id = string.Concat("item", person.Addresses.IndexOf(address) + 1);

                        var values = new vCardValueCollection(';')
                        {
                            string.Empty,
                            string.Empty,
                            address.Street,
                            address.City,
                            address.Region,
                            address.PostalCode,
                            address.Country
                        };
                        var vCardProperty = new vCardProperty(string.Concat(address.Id, ".ADR"), values);
                        if (address.IsDomestic)
                            vCardProperty.Subproperties.Add("TYPE", "DOM");
                        if (address.IsInternational)
                            vCardProperty.Subproperties.Add("TYPE", "INTL");
                        if (address.IsParcel)
                            vCardProperty.Subproperties.Add("TYPE", "PARCEL");
                        if (address.IsPostal)
                            vCardProperty.Subproperties.Add("TYPE", "POSTAL");
                        if (address.IsHome)
                            vCardProperty.Subproperties.Add("TYPE", "HOME");
                        if (address.IsWork)
                            vCardProperty.Subproperties.Add("TYPE", "WORK");
                        if (address.IsPreferred)
                            vCardProperty.Subproperties.Add("TYPE", "PREF");
                        properties.Add(vCardProperty);
                    }
                }
            }
        }

        private static void BuildProperties_XABADR(vCardPropertyCollection properties, vCard card)
        {
            if (card is Person person)
            {
                foreach (var address in person.Addresses)
                {
                    if (!string.IsNullOrEmpty(address.Id) && !string.IsNullOrEmpty(address.CountryCode))
                    {
                        var values = new vCardValueCollection() { address.CountryCode };
                        var vCardProperty = new vCardProperty(string.Concat(address.Id, ".X-ABADR"), values);
                        properties.Add(vCardProperty);
                    }
                }
            }
        }

        private static void BuildProperties_BDAY(vCardPropertyCollection properties, vCard card)
        {
            if (!card.BirthDate.HasValue)
                return;
            var vCardProperty = new vCardProperty("BDAY", card.BirthDate.Value);
            properties.Add(vCardProperty);
        }

        private static void BuildProperties_CATEGORIES(vCardPropertyCollection properties, vCard card)
        {
            if (card.Categories.Count <= 0)
                return;
            var values = new vCardValueCollection(',');
            foreach (var category in card.Categories)
            {
                if (!string.IsNullOrEmpty(category))
                    values.Add(category);
            }
            properties.Add(new vCardProperty("CATEGORIES", values));
        }

        private static void BuildProperties_CLASS(vCardPropertyCollection properties, vCard card)
        {
            var vCardProperty = new vCardProperty("CLASS");
            switch (card.AccessClassification)
            {
                case vCardAccessClassification.Unknown:
                    return;
                case vCardAccessClassification.Public:
                    vCardProperty.Value = "PUBLIC";
                    break;
                case vCardAccessClassification.Private:
                    vCardProperty.Value = "PRIVATE";
                    break;
                case vCardAccessClassification.Confidential:
                    vCardProperty.Value = "CONFIDENTIAL";
                    break;
                default:
                    throw new NotSupportedException();
            }
            properties.Add(vCardProperty);
        }

        private static void BuildProperties_EMAIL(vCardPropertyCollection properties, vCard card)
        {
            foreach (var emailAddress in card.EmailAddresses)
            {
                var vCardProperty = new vCardProperty
                {
                    Name = "EMAIL",
                    Value = emailAddress.Address
                };
                if (emailAddress.IsPreferred)
                    vCardProperty.Subproperties.Add("PREF");
                switch (emailAddress.EmailType)
                {
                    case vCardEmailAddressType.Internet:
                        vCardProperty.Subproperties.Add("INTERNET");
                        break;
                    case vCardEmailAddressType.AOL:
                        vCardProperty.Subproperties.Add("AOL");
                        break;
                    case vCardEmailAddressType.AppleLink:
                        vCardProperty.Subproperties.Add("AppleLink");
                        break;
                    case vCardEmailAddressType.AttMail:
                        vCardProperty.Subproperties.Add("ATTMail");
                        break;
                    case vCardEmailAddressType.CompuServe:
                        vCardProperty.Subproperties.Add("CIS");
                        break;
                    case vCardEmailAddressType.eWorld:
                        vCardProperty.Subproperties.Add("eWorld");
                        break;
                    case vCardEmailAddressType.IBMMail:
                        vCardProperty.Subproperties.Add("IBMMail");
                        break;
                    case vCardEmailAddressType.MCIMail:
                        vCardProperty.Subproperties.Add("MCIMail");
                        break;
                    case vCardEmailAddressType.PowerShare:
                        vCardProperty.Subproperties.Add("POWERSHARE");
                        break;
                    case vCardEmailAddressType.Prodigy:
                        vCardProperty.Subproperties.Add("PRODIGY");
                        break;
                    case vCardEmailAddressType.Telex:
                        vCardProperty.Subproperties.Add("TLX");
                        break;
                    case vCardEmailAddressType.X400:
                        vCardProperty.Subproperties.Add("X400");
                        break;
                    default:
                        vCardProperty.Subproperties.Add("INTERNET");
                        break;
                }
                properties.Add(vCardProperty);
            }
        }

        private static void BuildProperties_FN(vCardPropertyCollection properties, vCard card)
        {
            if (string.IsNullOrEmpty(card.FormattedName))
                return;
            var vCardProperty = new vCardProperty("FN", card.FormattedName);
            properties.Add(vCardProperty);
        }

        private static void BuildProperties_GEO(vCardPropertyCollection properties, vCard card)
        {
            if (!card.Latitude.HasValue || !card.Longitude.HasValue)
                return;
            properties.Add(new vCardProperty()
            {
                Name = "GEO",
                Value = card.Latitude.ToString() + ";" + card.Longitude.ToString()
            });
        }

        private static void BuildProperties_KEY(vCardPropertyCollection properties, vCard card)
        {
            foreach (var certificate in card.Certificates)
                properties.Add(new vCardProperty()
                {
                    Name = "KEY",
                    Value = certificate.Data,
                    Subproperties = { certificate.KeyType }
                });
        }

        private static void BuildProperties_LABEL(vCardPropertyCollection properties, vCard card)
        {
            foreach (var deliveryLabel in card.DeliveryLabels)
            {
                if (deliveryLabel.Text.Length > 0)
                {
                    var vCardProperty = new vCardProperty("LABEL", deliveryLabel.Text);
                    if (deliveryLabel.IsDomestic)
                        vCardProperty.Subproperties.Add("DOM");
                    if (deliveryLabel.IsInternational)
                        vCardProperty.Subproperties.Add("INTL");
                    if (deliveryLabel.IsParcel)
                        vCardProperty.Subproperties.Add("PARCEL");
                    if (deliveryLabel.IsPostal)
                        vCardProperty.Subproperties.Add("POSTAL");
                    if (deliveryLabel.IsHome)
                        vCardProperty.Subproperties.Add("HOME");
                    if (deliveryLabel.IsWork)
                        vCardProperty.Subproperties.Add("WORK");
                    vCardProperty.Subproperties.Add("ENCODING", "QUOTED-PRINTABLE");
                    properties.Add(vCardProperty);
                }
            }
        }

        private static void BuildProperties_MAILER(vCardPropertyCollection properties, vCard card)
        {
            if (string.IsNullOrEmpty(card.Mailer))
                return;
            var vCardProperty = new vCardProperty("MAILER", card.Mailer);
            properties.Add(vCardProperty);
        }

        private static void BuildProperties_N(vCardPropertyCollection properties, vCard card)
        {
            var values = new vCardValueCollection(';')
            {
                card.FamilyName,
                card.GivenName,
                card.AdditionalNames,
                card.NamePrefix,
                card.NameSuffix
            };
            var vCardProperty = new vCardProperty("N", values);
            properties.Add(vCardProperty);
        }

        private static void BuildProperties_NAME(vCardPropertyCollection properties, vCard card)
        {
            if (string.IsNullOrEmpty(card.DisplayName))
                return;
            var vCardProperty = new vCardProperty("NAME", card.DisplayName);
            properties.Add(vCardProperty);
        }

        private static void BuildProperties_NICKNAME(vCardPropertyCollection properties, vCard card)
        {
            if (card.Nicknames.Count <= 0)
                return;
            var vCardProperty = new vCardProperty("NICKNAME", new vCardValueCollection(',') { card.Nicknames });
            properties.Add(vCardProperty);
        }

        private static void BuildProperties_NOTE(vCardPropertyCollection properties, vCard card)
        {
            foreach (var note in card.Notes)
            {
                if (!string.IsNullOrEmpty(note.Text))
                {
                    var vCardProperty = new vCardProperty
                    {
                        Name = "NOTE",
                        Value = note.Text
                    };
                    if (!string.IsNullOrEmpty(note.Language))
                        vCardProperty.Subproperties.Add("language", note.Language);
                    properties.Add(vCardProperty);
                }
            }
        }

        private static void BuildProperties_ORG(vCardPropertyCollection properties, vCard card)
        {
            if (string.IsNullOrEmpty(card.Organization))
                return;
            var vCardProperty = new vCardProperty("ORG", card.Organization);
            properties.Add(vCardProperty);
        }

        private void BuildProperties_PHOTO(vCardPropertyCollection properties, vCard card)
        {
            foreach (var photo in card.Photos)
            {
                if (photo.Url == null)
                {
                    if (photo.IsLoaded)
                        properties.Add(new vCardProperty("PHOTO", photo.GetBytes()));
                }
                else
                {
                    bool flag = photo.Url.IsFile ? EmbedLocalImages : EmbedInternetImages;
                    if (flag)
                    {
                        try
                        {
                            photo.Fetch();
                        }
                        catch
                        {
                            flag = false;
                        }
                    }
                    if (flag)
                    {
                        var vCardProperty = new vCardProperty("PHOTO", photo.GetBytes());
                        switch (photo.vCardPhotoFormat)
                        {
                            case vCardPhoto.PhotoImageFormat.Bmp:
                                vCardProperty.Subproperties.Add("TYPE", "BMP");
                                break;
                            case vCardPhoto.PhotoImageFormat.Gif:
                                vCardProperty.Subproperties.Add("TYPE", "GIF");
                                break;
                            case vCardPhoto.PhotoImageFormat.Jpeg:
                                vCardProperty.Subproperties.Add("TYPE", "JPEG");
                                break;
                        }
                        properties.Add(vCardProperty);
                    }
                    else
                        properties.Add(new vCardProperty("PHOTO")
                        {
                            Subproperties = { { "VALUE", "URI" } },
                            Value = photo.Url.ToString()
                        });
                }
            }
        }

        private static void BuildProperties_PRODID(vCardPropertyCollection properties, vCard card)
        {
            if (string.IsNullOrEmpty(card.ProductId))
                return;
            properties.Add(new vCardProperty()
            {
                Name = "PRODID",
                Value = card.ProductId
            });
        }

        private static void BuildProperties_REV(vCardPropertyCollection properties, vCard card)
        {
            if (!card.RevisionDate.HasValue)
                return;
            var vCardProperty = new vCardProperty("REV", card.RevisionDate.Value.ToString());
            properties.Add(vCardProperty);
        }

        private static void BuildProperties_ROLE(vCardPropertyCollection properties, vCard card)
        {
            if (string.IsNullOrEmpty(card.Role))
                return;
            var vCardProperty = new vCardProperty("ROLE", card.Role);
            properties.Add(vCardProperty);
        }

        private static void BuildProperties_SOURCE(vCardPropertyCollection properties, vCard card)
        {
            foreach (var source in card.Sources)
            {
                var vCardProperty = new vCardProperty
                {
                    Name = "SOURCE",
                    Value = source.Uri.ToString()
                };
                if (!string.IsNullOrEmpty(source.Context))
                    vCardProperty.Subproperties.Add("CONTEXT", source.Context);
                properties.Add(vCardProperty);
            }
        }

        private static void BuildProperties_TEL(vCardPropertyCollection properties, vCard card)
        {
            foreach (var phone in card.Phones)
            {
                var vCardProperty = new vCardProperty
                {
                    Name = "TEL"
                };
                if (phone.IsBBS)
                    vCardProperty.Subproperties.Add("TYPE", "BBS");
                if (phone.IsCar)
                    vCardProperty.Subproperties.Add("TYPE", "CAR");
                if (phone.IsCellular)
                    vCardProperty.Subproperties.Add("TYPE", "CELL");
                if (phone.IsFax)
                    vCardProperty.Subproperties.Add("TYPE", "FAX");
                if (phone.IsHome)
                    vCardProperty.Subproperties.Add("TYPE", "HOME");
                if (phone.IsISDN)
                    vCardProperty.Subproperties.Add("TYPE", "ISDN");
                if (phone.IsMessagingService)
                    vCardProperty.Subproperties.Add("TYPE", "MSG");
                if (phone.IsModem)
                    vCardProperty.Subproperties.Add("TYPE", "MODEM");
                if (phone.IsPager)
                    vCardProperty.Subproperties.Add("TYPE", "PAGER");
                if (phone.IsPreferred)
                    vCardProperty.Subproperties.Add("TYPE", "PREF");
                if (phone.IsVideo)
                    vCardProperty.Subproperties.Add("TYPE", "VIDEO");
                if (phone.IsVoice)
                    vCardProperty.Subproperties.Add("TYPE", "VOICE");
                if (phone.IsWork)
                    vCardProperty.Subproperties.Add("TYPE", "WORK");
                if (phone.IsVoice && !phone.IsFax && !phone.IsCellular && !phone.IsHome && !phone.IsWork)
                    vCardProperty.Subproperties.Add("TYPE", "OTHER");
                vCardProperty.Value = phone.FullNumber;
                properties.Add(vCardProperty);
            }
        }

        private static void BuildProperties_TITLE(vCardPropertyCollection properties, vCard card)
        {
            if (string.IsNullOrEmpty(card.Title))
                return;
            var vCardProperty = new vCardProperty("TITLE", card.Title);
            properties.Add(vCardProperty);
        }

        private static void BuildProperties_TZ(vCardPropertyCollection properties, vCard card)
        {
            if (string.IsNullOrEmpty(card.TimeZone))
                return;
            properties.Add(new vCardProperty("TZ", card.TimeZone));
        }

        private static void BuildProperties_UID(vCardPropertyCollection properties, vCard card)
        {
            if (string.IsNullOrEmpty(card.UniqueId))
                return;
            properties.Add(new vCardProperty()
            {
                Name = "UID",
                Value = card.UniqueId
            });
        }

        private static void BuildProperties_XADDRESSBOOKSERVERMEMBER(vCardPropertyCollection properties, vCard card)
        {
            if (card is ContactGroup contactGroup)
            {
                foreach (var memberResourceName in contactGroup.MemberResourceNames)
                {
                    if (!string.IsNullOrEmpty(memberResourceName))
                    {
                        var vCardProperty = new vCardProperty("X-ADDRESSBOOKSERVER-MEMBER", string.Concat("urn:uuid:", memberResourceName));
                        properties.Add(vCardProperty);
                    }
                }
            }
        }

        private static void BuildProperties_XADDRESSBOOKSERVERKIND(vCardPropertyCollection properties, vCard card)
        {
            if (card is ContactGroup contactGroup)
            {
                if (!string.IsNullOrEmpty(contactGroup.GroupType))
                {
                    var vCardProperty = new vCardProperty("X-ADDRESSBOOKSERVER-KIND", contactGroup.GroupType);
                    properties.Add(vCardProperty);
                }
            }
        }

        private static void BuildProperties_URL(vCardPropertyCollection properties, vCard card)
        {
            foreach (var website in card.Websites)
            {
                if (!string.IsNullOrEmpty(website.Url))
                {
                    var vCardProperty = new vCardProperty("URL", website.Url.ToString());
                    if (website.IsWorkSite)
                        vCardProperty.Subproperties.Add("WORK");
                    properties.Add(vCardProperty);
                }
            }
        }

        private static void BuildProperties_X_WAB_GENDER(vCardPropertyCollection properties, vCard card)
        {
            switch (card.Gender)
            {
                case vCardGender.Female:
                    properties.Add(new vCardProperty("X-WAB-GENDER", "1"));
                    break;
                case vCardGender.Male:
                    properties.Add(new vCardProperty("X-WAB-GENDER", "2"));
                    break;
            }
        }

        public override void Write(vCard card, TextWriter output, string charsetName)
        {
            if (card == null)
                throw new ArgumentNullException(nameof(card));
            if (output == null)
                throw new ArgumentNullException(nameof(output));
            Write(BuildProperties(card), output, charsetName);
        }
    }
}
