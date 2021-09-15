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
            vCardPropertyCollection properties = new vCardPropertyCollection
            {
                new vCardProperty("BEGIN", "VCARD"),
                new vCardProperty("VERSION", "3.0")
            };
            this.BuildProperties_NAME(properties, card);
            this.BuildProperties_SOURCE(properties, card);
            this.BuildProperties_N(properties, card);
            this.BuildProperties_FN(properties, card);
            this.BuildProperties_ADR(properties, card);
            this.BuildProperties_XABADR(properties, card);
            this.BuildProperties_BDAY(properties, card);
            this.BuildProperties_CATEGORIES(properties, card);
            this.BuildProperties_CLASS(properties, card);
            this.BuildProperties_EMAIL(properties, card);
            this.BuildProperties_GEO(properties, card);
            this.BuildProperties_KEY(properties, card);
            this.BuildProperties_LABEL(properties, card);
            this.BuildProperties_MAILER(properties, card);
            this.BuildProperties_NICKNAME(properties, card);
            this.BuildProperties_NOTE(properties, card);
            this.BuildProperties_ORG(properties, card);
            this.BuildProperties_PHOTO(properties, card);
            this.BuildProperties_PRODID(properties, card);
            this.BuildProperties_REV(properties, card);
            this.BuildProperties_ROLE(properties, card);
            this.BuildProperties_TEL(properties, card);
            this.BuildProperties_TITLE(properties, card);
            this.BuildProperties_TZ(properties, card);
            this.BuildProperties_UID(properties, card);
            this.BuildProperties_XADDRESSBOOKSERVERKIND(properties, card);
            this.BuildProperties_XADDRESSBOOKSERVERMEMBER(properties, card);
            this.BuildProperties_URL(properties, card);
            this.BuildProperties_X_WAB_GENDER(properties, card);
            properties.Add(new vCardProperty("END", "VCARD"));
            return properties;
        }

        private void BuildProperties_ADR(vCardPropertyCollection properties, vCard card)
        {
            if (card is Person person)
            {
                foreach (VCardAddress address in person.Addresses)
                {
                    if (!string.IsNullOrEmpty(address.City) || !string.IsNullOrEmpty(address.Country) || !string.IsNullOrEmpty(address.PostalCode) || !string.IsNullOrEmpty(address.Region) || !string.IsNullOrEmpty(address.Street))
                    {
                        if (string.IsNullOrEmpty(address.Id))
                            address.Id = string.Concat("item", person.Addresses.IndexOf(address) + 1);

                        vCardValueCollection values = new vCardValueCollection(';')
                        {
                            string.Empty,
                            string.Empty,
                            address.Street,
                            address.City,
                            address.Region,
                            address.PostalCode,
                            address.Country
                        };
                        vCardProperty vCardProperty = new vCardProperty(string.Concat(address.Id, ".ADR"), values);
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

        private void BuildProperties_XABADR(vCardPropertyCollection properties, vCard card)
        {
            if (card is Person person)
            {
                foreach (VCardAddress address in person.Addresses)
                {
                    if (!string.IsNullOrEmpty(address.Id) && !string.IsNullOrEmpty(address.CountryCode))
                    {
                        vCardValueCollection values = new vCardValueCollection() { address.CountryCode };
                        vCardProperty vCardProperty = new vCardProperty(string.Concat(address.Id, ".X-ABADR"), values);
                        properties.Add(vCardProperty);
                    }
                }
            }
        }

        private void BuildProperties_BDAY(vCardPropertyCollection properties, vCard card)
        {
            if (!card.BirthDate.HasValue)
                return;
            vCardProperty vCardProperty = new vCardProperty("BDAY", card.BirthDate.Value);
            properties.Add(vCardProperty);
        }

        private void BuildProperties_CATEGORIES(vCardPropertyCollection properties, vCard card)
        {
            if (card.Categories.Count <= 0)
                return;
            vCardValueCollection values = new vCardValueCollection(',');
            foreach (string category in card.Categories)
            {
                if (!string.IsNullOrEmpty(category))
                    values.Add(category);
            }
            properties.Add(new vCardProperty("CATEGORIES", values));
        }

        private void BuildProperties_CLASS(vCardPropertyCollection properties, vCard card)
        {
            vCardProperty vCardProperty = new vCardProperty("CLASS");
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

        private void BuildProperties_EMAIL(vCardPropertyCollection properties, vCard card)
        {
            foreach (vCardEmailAddress emailAddress in card.EmailAddresses)
            {
                vCardProperty vCardProperty = new vCardProperty
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

        private void BuildProperties_FN(vCardPropertyCollection properties, vCard card)
        {
            if (string.IsNullOrEmpty(card.FormattedName))
                return;
            vCardProperty vCardProperty = new vCardProperty("FN", card.FormattedName);
            properties.Add(vCardProperty);
        }

        private void BuildProperties_GEO(vCardPropertyCollection properties, vCard card)
        {
            if (!card.Latitude.HasValue || !card.Longitude.HasValue)
                return;
            properties.Add(new vCardProperty()
            {
                Name = "GEO",
                Value = card.Latitude.ToString() + ";" + card.Longitude.ToString()
            });
        }

        private void BuildProperties_KEY(vCardPropertyCollection properties, vCard card)
        {
            foreach (vCardCertificate certificate in card.Certificates)
                properties.Add(new vCardProperty()
                {
                    Name = "KEY",
                    Value = certificate.Data,
                    Subproperties = { certificate.KeyType }
                });
        }

        private void BuildProperties_LABEL(vCardPropertyCollection properties, vCard card)
        {
            foreach (vCardDeliveryLabel deliveryLabel in card.DeliveryLabels)
            {
                if (deliveryLabel.Text.Length > 0)
                {
                    vCardProperty vCardProperty = new vCardProperty("LABEL", deliveryLabel.Text);
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

        private void BuildProperties_MAILER(vCardPropertyCollection properties, vCard card)
        {
            if (string.IsNullOrEmpty(card.Mailer))
                return;
            vCardProperty vCardProperty = new vCardProperty("MAILER", card.Mailer);
            properties.Add(vCardProperty);
        }

        private void BuildProperties_N(vCardPropertyCollection properties, vCard card)
        {
            vCardValueCollection values = new vCardValueCollection(';')
            {
                card.FamilyName,
                card.GivenName,
                card.AdditionalNames,
                card.NamePrefix,
                card.NameSuffix
            };
            vCardProperty vCardProperty = new vCardProperty("N", values);
            properties.Add(vCardProperty);
        }

        private void BuildProperties_NAME(vCardPropertyCollection properties, vCard card)
        {
            if (string.IsNullOrEmpty(card.DisplayName))
                return;
            vCardProperty vCardProperty = new vCardProperty("NAME", card.DisplayName);
            properties.Add(vCardProperty);
        }

        private void BuildProperties_NICKNAME(vCardPropertyCollection properties, vCard card)
        {
            if (card.Nicknames.Count <= 0)
                return;
            vCardProperty vCardProperty = new vCardProperty("NICKNAME", new vCardValueCollection(',') { card.Nicknames });
            properties.Add(vCardProperty);
        }

        private void BuildProperties_NOTE(vCardPropertyCollection properties, vCard card)
        {
            foreach (vCardNote note in card.Notes)
            {
                if (!string.IsNullOrEmpty(note.Text))
                {
                    vCardProperty vCardProperty = new vCardProperty
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

        private void BuildProperties_ORG(vCardPropertyCollection properties, vCard card)
        {
            if (string.IsNullOrEmpty(card.Organization))
                return;
            vCardProperty vCardProperty = new vCardProperty("ORG", card.Organization);
            properties.Add(vCardProperty);
        }

        private void BuildProperties_PHOTO(vCardPropertyCollection properties, vCard card)
        {
            foreach (vCardPhoto photo in card.Photos)
            {
                if (photo.Url == null)
                {
                    if (photo.IsLoaded)
                        properties.Add(new vCardProperty("PHOTO", photo.GetBytes()));
                }
                else
                {
                    bool flag = photo.Url.IsFile ? this.EmbedLocalImages : this.EmbedInternetImages;
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
                        vCardProperty vCardProperty = new vCardProperty("PHOTO", photo.GetBytes());
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

        private void BuildProperties_PRODID(vCardPropertyCollection properties, vCard card)
        {
            if (string.IsNullOrEmpty(card.ProductId))
                return;
            properties.Add(new vCardProperty()
            {
                Name = "PRODID",
                Value = card.ProductId
            });
        }

        private void BuildProperties_REV(vCardPropertyCollection properties, vCard card)
        {
            if (!card.RevisionDate.HasValue)
                return;
            vCardProperty vCardProperty = new vCardProperty("REV", card.RevisionDate.Value.ToString());
            properties.Add(vCardProperty);
        }

        private void BuildProperties_ROLE(vCardPropertyCollection properties, vCard card)
        {
            if (string.IsNullOrEmpty(card.Role))
                return;
            vCardProperty vCardProperty = new vCardProperty("ROLE", card.Role);
            properties.Add(vCardProperty);
        }

        private void BuildProperties_SOURCE(vCardPropertyCollection properties, vCard card)
        {
            foreach (vCardSource source in card.Sources)
            {
                vCardProperty vCardProperty = new vCardProperty
                {
                    Name = "SOURCE",
                    Value = source.Uri.ToString()
                };
                if (!string.IsNullOrEmpty(source.Context))
                    vCardProperty.Subproperties.Add("CONTEXT", source.Context);
                properties.Add(vCardProperty);
            }
        }

        private void BuildProperties_TEL(vCardPropertyCollection properties, vCard card)
        {
            foreach (vCardPhone phone in card.Phones)
            {
                vCardProperty vCardProperty = new vCardProperty
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

        private void BuildProperties_TITLE(vCardPropertyCollection properties, vCard card)
        {
            if (string.IsNullOrEmpty(card.Title))
                return;
            vCardProperty vCardProperty = new vCardProperty("TITLE", card.Title);
            properties.Add(vCardProperty);
        }

        private void BuildProperties_TZ(vCardPropertyCollection properties, vCard card)
        {
            if (string.IsNullOrEmpty(card.TimeZone))
                return;
            properties.Add(new vCardProperty("TZ", card.TimeZone));
        }

        private void BuildProperties_UID(vCardPropertyCollection properties, vCard card)
        {
            if (string.IsNullOrEmpty(card.UniqueId))
                return;
            properties.Add(new vCardProperty()
            {
                Name = "UID",
                Value = card.UniqueId
            });
        }

        private void BuildProperties_XADDRESSBOOKSERVERMEMBER(vCardPropertyCollection properties, vCard card)
        {
            if (card is ContactGroup contactGroup)
            {
                foreach (var memberResourceName in contactGroup.MemberResourceNames)
                {
                    if (!string.IsNullOrEmpty(memberResourceName))
                    {
                        vCardProperty vCardProperty = new vCardProperty("X-ADDRESSBOOKSERVER-MEMBER", string.Concat("urn:uuid:", memberResourceName));
                        properties.Add(vCardProperty);
                    }
                }
            }
        }

        private void BuildProperties_XADDRESSBOOKSERVERKIND(vCardPropertyCollection properties, vCard card)
        {
            if (card is ContactGroup contactGroup)
            {
                if (!string.IsNullOrEmpty(contactGroup.GroupType))
                {
                    vCardProperty vCardProperty = new vCardProperty("X-ADDRESSBOOKSERVER-KIND", contactGroup.GroupType);
                    properties.Add(vCardProperty);
                }
            }
        }

        private void BuildProperties_URL(vCardPropertyCollection properties, vCard card)
        {
            foreach (vCardWebsite website in card.Websites)
            {
                if (!string.IsNullOrEmpty(website.Url))
                {
                    vCardProperty vCardProperty = new vCardProperty("URL", website.Url.ToString());
                    if (website.IsWorkSite)
                        vCardProperty.Subproperties.Add("WORK");
                    properties.Add(vCardProperty);
                }
            }
        }

        private void BuildProperties_X_WAB_GENDER(vCardPropertyCollection properties, vCard card)
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
            this.Write(this.BuildProperties(card), output, charsetName);
        }
    }
}
