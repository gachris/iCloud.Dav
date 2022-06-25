using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace iCloud.Dav.People.Utils
{
    /// <summary>
    ///     Implements the standard Person 2.1 and 3.0 text formats.
    /// </summary>
    internal class CardStandardWriter : CardWriter
    {
        private bool _embedInternetImages;
        private bool _embedLocalImages;
        private CardStandardWriterOptions _options;
        private string _productId;

        /// <summary>
        ///     The characters that are escaped per the original
        ///     Person specification.
        /// </summary>
        private readonly char[] _standardEscapedCharacters = new char[5] { ',', '\\', ';', '\r', '\n' };

        /// <summary>
        ///     The characters that are escaped by Microsoft Outlook.
        /// </summary>
        /// <remarks>
        ///     Microsoft Outlook does not property decode escaped
        ///     commas in values.
        /// </remarks>
        private readonly char[] _outlookEscapedCharacters = new char[4] { '\\', ';', '\r', '\n' };

        private static readonly string _basis_64 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/???????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????";

        /// <summary>Creates a new instance of the standard writer.</summary>
        /// <remarks>
        ///     The standard writer is configured to create Person
        ///     files in the highest supported version.  This is
        ///     currently version 3.0.
        /// </remarks>
        public CardStandardWriter() => _embedLocalImages = true;

        /// <summary>
        ///     Indicates whether images that reference Internet
        ///     URLs should be embedded in the output.  If not,
        ///     then a URL is written instead.
        /// </summary>
        public bool EmbedInternetImages
        {
            get => _embedInternetImages;
            set => _embedInternetImages = value;
        }

        /// <summary>
        ///     Indicates whether or not references to local
        ///     images should be embedded in the Person.  If not,
        ///     then a local file reference is written instead.
        /// </summary>
        public bool EmbedLocalImages
        {
            get => _embedLocalImages;
            set => _embedLocalImages = value;
        }

        /// <summary>Extended options for the Person writer.</summary>
        public CardStandardWriterOptions Options
        {
            get => _options;
            set => _options = value;
        }

        /// <summary>The product ID to use when writing a Person.</summary>
        public string ProductId
        {
            get => _productId;
            set => _productId = value;
        }

        /// <summary>
        ///     Builds a collection of standard properties based on
        ///     the specified Person.
        /// </summary>
        /// <returns>
        ///     A <see cref="List{CardProperty}" /> that contains all
        ///     properties for the current Person, including the header
        ///     and footer properties.
        /// </returns>
        /// <seealso cref="Person" />
        /// <seealso cref="CardProperty" />
        private IList<CardProperty> BuildProperties(Person card)
        {
            var properties = new List<CardProperty>
            {
                new CardProperty("BEGIN", "VCARD"),
                new CardProperty("VERSION", "3.0")
            };
            BuildProperties_NAME(properties, card);
            BuildProperties_SOURCE(properties, card);
            BuildProperties_N(properties, card);
            BuildProperties_FN(properties, card);
            BuildProperties_ADR(properties, card);
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
            properties.Add(new CardProperty("END", "VCARD"));
            return properties;
        }

        /// <summary>Builds ADR properties.</summary>
        private static void BuildProperties_ADR(IList<CardProperty> properties, Person card)
        {
            // DeliveryAddresses Or Addresses
            // TO DO Fix

            foreach (var deliveryAddress in card.Addresses)
            {
                if (!string.IsNullOrEmpty(deliveryAddress.City) || !string.IsNullOrEmpty(deliveryAddress.Country) || !string.IsNullOrEmpty(deliveryAddress.PostalCode) || !string.IsNullOrEmpty(deliveryAddress.Region) || !string.IsNullOrEmpty(deliveryAddress.Street))
                {
                    if (string.IsNullOrEmpty(deliveryAddress.Id))
                        deliveryAddress.Id = string.Concat("item", card.Addresses.IndexOf(deliveryAddress) + 1);

                    var values = new ValueCollection(';')
                    {
                        string.Empty,
                        string.Empty,
                        deliveryAddress.Street,
                        deliveryAddress.City,
                        deliveryAddress.Region,
                        deliveryAddress.PostalCode,
                        deliveryAddress.Country
                    };
                    var cardProperty = new CardProperty("ADR", values);
                    if (deliveryAddress.IsDomestic)
                        cardProperty.Subproperties.Add("DOM");
                    if (deliveryAddress.IsInternational)
                        cardProperty.Subproperties.Add("INTL");
                    if (deliveryAddress.IsParcel)
                        cardProperty.Subproperties.Add("PARCEL");
                    if (deliveryAddress.IsPostal)
                        cardProperty.Subproperties.Add("POSTAL");
                    if (deliveryAddress.IsHome)
                        cardProperty.Subproperties.Add("HOME");
                    if (deliveryAddress.IsWork)
                        cardProperty.Subproperties.Add("WORK");
                    cardProperty.Subproperties.Add("ENCODING", "QUOTED-PRINTABLE");
                    properties.Add(cardProperty);
                }
            }
        }

        /// <summary>Builds the BDAY property.</summary>
        private static void BuildProperties_BDAY(IList<CardProperty> properties, Person card)
        {
            if (!card.BirthDate.HasValue)
                return;
            var cardProperty = new CardProperty("BDAY", card.BirthDate.Value);
            properties.Add(cardProperty);
        }

        private static void BuildProperties_CATEGORIES(IList<CardProperty> properties, Person card)
        {
            if (card.Categories.Count <= 0)
                return;
            var values = new ValueCollection(',');
            foreach (var category in card.Categories)
            {
                if (!string.IsNullOrEmpty(category))
                    values.Add(category);
            }
            properties.Add(new CardProperty("CATEGORIES", values));
        }

        private static void BuildProperties_CLASS(IList<CardProperty> properties, Person card)
        {
            var cardProperty = new CardProperty("CLASS");
            switch (card.AccessClassification)
            {
                case AccessClassification.Unknown:
                    return;
                case AccessClassification.Public:
                    cardProperty.Value = "PUBLIC";
                    break;
                case AccessClassification.Private:
                    cardProperty.Value = "PRIVATE";
                    break;
                case AccessClassification.Confidential:
                    cardProperty.Value = "CONFIDENTIAL";
                    break;
                default:
                    throw new NotSupportedException();
            }
            properties.Add(cardProperty);
        }

        /// <summary>Builds EMAIL properties.</summary>
        private static void BuildProperties_EMAIL(IList<CardProperty> properties, Person card)
        {
            foreach (var emailAddress in card.EmailAddresses)
            {
                var cardProperty = new CardProperty
                {
                    Name = "EMAIL",
                    Value = emailAddress.Address
                };
                if (emailAddress.IsPreferred)
                    cardProperty.Subproperties.Add("PREF");
                switch (emailAddress.EmailType)
                {
                    case EmailAddressType.Internet:
                        cardProperty.Subproperties.Add("INTERNET");
                        break;
                    case EmailAddressType.AOL:
                        cardProperty.Subproperties.Add("AOL");
                        break;
                    case EmailAddressType.AppleLink:
                        cardProperty.Subproperties.Add("AppleLink");
                        break;
                    case EmailAddressType.AttMail:
                        cardProperty.Subproperties.Add("ATTMail");
                        break;
                    case EmailAddressType.CompuServe:
                        cardProperty.Subproperties.Add("CIS");
                        break;
                    case EmailAddressType.eWorld:
                        cardProperty.Subproperties.Add("eWorld");
                        break;
                    case EmailAddressType.IBMMail:
                        cardProperty.Subproperties.Add("IBMMail");
                        break;
                    case EmailAddressType.MCIMail:
                        cardProperty.Subproperties.Add("MCIMail");
                        break;
                    case EmailAddressType.PowerShare:
                        cardProperty.Subproperties.Add("POWERSHARE");
                        break;
                    case EmailAddressType.Prodigy:
                        cardProperty.Subproperties.Add("PRODIGY");
                        break;
                    case EmailAddressType.Telex:
                        cardProperty.Subproperties.Add("TLX");
                        break;
                    case EmailAddressType.X400:
                        cardProperty.Subproperties.Add("X400");
                        break;
                    default:
                        cardProperty.Subproperties.Add("INTERNET");
                        break;
                }
                properties.Add(cardProperty);
            }
        }

        private static void BuildProperties_FN(IList<CardProperty> properties, Person card)
        {
            if (string.IsNullOrEmpty(card.FormattedName))
                return;
            var cardProperty = new CardProperty("FN", card.FormattedName);
            properties.Add(cardProperty);
        }

        /// <summary>Builds the GEO property.</summary>
        private static void BuildProperties_GEO(IList<CardProperty> properties, Person card)
        {
            if (!card.Latitude.HasValue || !card.Longitude.HasValue)
                return;
            properties.Add(new CardProperty()
            {
                Name = "GEO",
                Value = card.Latitude.ToString() + ";" + card.Longitude.ToString()
            });
        }

        /// <summary>Builds KEY properties.</summary>
        private static void BuildProperties_KEY(IList<CardProperty> properties, Person card)
        {
            foreach (var certificate in card.Certificates)
                properties.Add(new CardProperty()
                {
                    Name = "KEY",
                    Value = certificate.Data,
                    Subproperties = {
            certificate.KeyType
          }
                });
        }

        private static void BuildProperties_LABEL(IList<CardProperty> properties, Person card)
        {
            foreach (var deliveryLabel in card.Labels)
            {
                if (deliveryLabel.Text.Length > 0)
                {
                    var cardProperty = new CardProperty("LABEL", deliveryLabel.Text);
                    if (deliveryLabel.IsDomestic)
                        cardProperty.Subproperties.Add("DOM");
                    if (deliveryLabel.IsInternational)
                        cardProperty.Subproperties.Add("INTL");
                    if (deliveryLabel.IsParcel)
                        cardProperty.Subproperties.Add("PARCEL");
                    if (deliveryLabel.IsPostal)
                        cardProperty.Subproperties.Add("POSTAL");
                    if (deliveryLabel.IsHome)
                        cardProperty.Subproperties.Add("HOME");
                    if (deliveryLabel.IsWork)
                        cardProperty.Subproperties.Add("WORK");
                    cardProperty.Subproperties.Add("ENCODING", "QUOTED-PRINTABLE");
                    properties.Add(cardProperty);
                }
            }
        }

        /// <summary>Builds the MAILER property.</summary>
        private static void BuildProperties_MAILER(IList<CardProperty> properties, Person card)
        {
            if (string.IsNullOrEmpty(card.Mailer))
                return;
            var cardProperty = new CardProperty("MAILER", card.Mailer);
            properties.Add(cardProperty);
        }

        private static void BuildProperties_N(IList<CardProperty> properties, Person card)
        {
            var values = new ValueCollection(';')
            {
                card.FamilyName,
                card.GivenName,
                card.AdditionalNames,
                card.NamePrefix,
                card.NameSuffix
            };
            var cardProperty = new CardProperty("N", values);
            properties.Add(cardProperty);
        }

        private static void BuildProperties_NAME(IList<CardProperty> properties, Person card)
        {
            if (string.IsNullOrEmpty(card.DisplayName))
                return;
            var cardProperty = new CardProperty("NAME", card.DisplayName);
            properties.Add(cardProperty);
        }

        /// <summary>Builds the NICKNAME property.</summary>
        private static void BuildProperties_NICKNAME(IList<CardProperty> properties, Person card)
        {
            if (card.Nicknames.Count <= 0)
                return;
            var cardProperty = new CardProperty("NICKNAME", new ValueCollection(',') { card.Nicknames });
            properties.Add(cardProperty);
        }

        /// <summary>Builds the NOTE property.</summary>
        private static void BuildProperties_NOTE(IList<CardProperty> properties, Person card)
        {
            foreach (var note in card.Notes)
            {
                if (!string.IsNullOrEmpty(note.Text))
                {
                    var cardProperty = new CardProperty
                    {
                        Name = "NOTE",
                        Value = note.Text
                    };
                    if (!string.IsNullOrEmpty(note.Language))
                        cardProperty.Subproperties.Add("language", note.Language);
                    cardProperty.Subproperties.Add("ENCODING", "QUOTED-PRINTABLE"); // do I need it?
                    properties.Add(cardProperty);
                }
            }
        }

        /// <summary>Builds the ORG property.</summary>
        private static void BuildProperties_ORG(IList<CardProperty> properties, Person card)
        {
            if (string.IsNullOrEmpty(card.Organization))
                return;
            var cardProperty = new CardProperty("ORG", card.Organization);
            properties.Add(cardProperty);
        }

        private void BuildProperties_PHOTO(IList<CardProperty> properties, Person card)
        {
            foreach (var photo in card.Photos)
            {
                if (photo.Url == null)
                {
                    if (photo.IsLoaded)
                        properties.Add(new CardProperty("PHOTO", photo.GetBytes()));
                }
                else
                {
                    var flag = photo.Url.IsFile ? _embedLocalImages : _embedInternetImages;
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
                        var cardProperty = new CardProperty("PHOTO", photo.GetBytes());
                        switch (photo.PhotoFormat)
                        {
                            case PhotoImageFormat.Bmp:
                                cardProperty.Subproperties.Add("TYPE", "BMP");
                                break;
                            case PhotoImageFormat.Gif:
                                cardProperty.Subproperties.Add("TYPE", "GIF");
                                break;
                            case PhotoImageFormat.Jpeg:
                                cardProperty.Subproperties.Add("TYPE", "JPEG");
                                break;
                        }
                        properties.Add(cardProperty);
                    }
                    else properties.Add(new CardProperty("PHOTO") { Subproperties = { { "VALUE", "URI" } }, Value = photo.Url.ToString() });
                }
            }
        }

        /// <summary>Builds PRODID properties.</summary>
        private static void BuildProperties_PRODID(IList<CardProperty> properties, Person card)
        {
            if (string.IsNullOrEmpty(card.ProductId))
                return;
            properties.Add(new CardProperty()
            {
                Name = "PRODID",
                Value = card.ProductId
            });
        }

        /// <summary>Builds the REV property.</summary>
        private static void BuildProperties_REV(IList<CardProperty> properties, Person card)
        {
            if (!card.RevisionDate.HasValue)
                return;
            var cardProperty = new CardProperty("REV", card.RevisionDate.Value.ToString());
            properties.Add(cardProperty);
        }

        /// <summary>Builds the ROLE property.</summary>
        private static void BuildProperties_ROLE(IList<CardProperty> properties, Person card)
        {
            if (string.IsNullOrEmpty(card.Role))
                return;
            var cardProperty = new CardProperty("ROLE", card.Role);
            properties.Add(cardProperty);
        }

        /// <summary>Builds SOURCE properties.</summary>
        private static void BuildProperties_SOURCE(IList<CardProperty> properties, Person card)
        {
            foreach (var source in card.Sources)
            {
                var cardProperty = new CardProperty
                {
                    Name = "SOURCE",
                    Value = source.Uri.ToString()
                };
                if (!string.IsNullOrEmpty(source.Context))
                    cardProperty.Subproperties.Add("CONTEXT", source.Context);
                properties.Add(cardProperty);
            }
        }

        /// <summary>Builds TEL properties.</summary>
        private static void BuildProperties_TEL(IList<CardProperty> properties, Person card)
        {
            foreach (var phone in card.Phones)
            {
                var cardProperty = new CardProperty
                {
                    Name = "TEL"
                };
                if (phone.IsBBS)
                    cardProperty.Subproperties.Add("TYPE", "BBS");
                if (phone.IsCar)
                    cardProperty.Subproperties.Add("TYPE", "CAR");
                if (phone.IsCellular)
                    cardProperty.Subproperties.Add("TYPE", "CELL");
                if (phone.IsFax)
                    cardProperty.Subproperties.Add("TYPE", "FAX");
                if (phone.IsHome)
                    cardProperty.Subproperties.Add("TYPE", "HOME");
                if (phone.IsISDN)
                    cardProperty.Subproperties.Add("TYPE", "ISDN");
                if (phone.IsMessagingService)
                    cardProperty.Subproperties.Add("TYPE", "MSG");
                if (phone.IsModem)
                    cardProperty.Subproperties.Add("TYPE", "MODEM");
                if (phone.IsPager)
                    cardProperty.Subproperties.Add("TYPE", "PAGER");
                if (phone.IsPreferred)
                    cardProperty.Subproperties.Add("TYPE", "PREF");
                if (phone.IsVideo)
                    cardProperty.Subproperties.Add("TYPE", "VIDEO");
                if (phone.IsVoice)
                    cardProperty.Subproperties.Add("TYPE", "VOICE");
                if (phone.IsWork)
                    cardProperty.Subproperties.Add("TYPE", "WORK");
                if (phone.IsVoice && !phone.IsFax && !phone.IsCellular && !phone.IsHome && !phone.IsWork)
                    cardProperty.Subproperties.Add("TYPE", "OTHER");
                cardProperty.Value = phone.FullNumber;
                properties.Add(cardProperty);
            }
        }

        private static void BuildProperties_TITLE(IList<CardProperty> properties, Person card)
        {
            if (string.IsNullOrEmpty(card.Title))
                return;
            var cardProperty = new CardProperty("TITLE", card.Title);
            properties.Add(cardProperty);
        }

        private static void BuildProperties_TZ(IList<CardProperty> properties, Person card)
        {
            if (string.IsNullOrEmpty(card.TimeZone))
                return;
            properties.Add(new CardProperty("TZ", card.TimeZone));
        }

        private static void BuildProperties_UID(IList<CardProperty> properties, Person card)
        {
            if (string.IsNullOrEmpty(card.UniqueId))
                return;
            properties.Add(new CardProperty()
            {
                Name = "UID",
                Value = card.UniqueId
            });
        }

        private static void BuildProperties_XADDRESSBOOKSERVERKIND(IList<CardProperty> properties, Person card)
        {
            if (card is ContactGroup contactGroup)
            {
                if (!string.IsNullOrEmpty(contactGroup.GroupType))
                {
                    var cardProperty = new CardProperty("X-ADDRESSBOOKSERVER-KIND", contactGroup.GroupType);
                    properties.Add(cardProperty);
                }
            }
        }

        private static void BuildProperties_XADDRESSBOOKSERVERMEMBER(IList<CardProperty> properties, Person card)
        {
            if (card is ContactGroup contactGroup)
            {
                foreach (var memberResourceName in contactGroup.MemberResourceNames)
                {
                    if (!string.IsNullOrEmpty(memberResourceName))
                    {
                        var cardProperty = new CardProperty("X-ADDRESSBOOKSERVER-MEMBER", string.Concat("urn:uuid:", memberResourceName));
                        properties.Add(cardProperty);
                    }
                }
            }
        }

        private static void BuildProperties_URL(IList<CardProperty> properties, Person card)
        {
            foreach (var website in card.Websites)
            {
                if (!string.IsNullOrEmpty(website.Url))
                {
                    var cardProperty = new CardProperty("URL", website.Url.ToString());
                    if (website.IsWorkSite)
                        cardProperty.Subproperties.Add("WORK");
                    properties.Add(cardProperty);
                }
            }
        }

        private static void BuildProperties_X_WAB_GENDER(IList<CardProperty> properties, Person card)
        {
            switch (card.Gender)
            {
                case Gender.Female:
                    properties.Add(new CardProperty("X-WAB-GENDER", "1"));
                    break;
                case Gender.Male:
                    properties.Add(new CardProperty("X-WAB-GENDER", "2"));
                    break;
            }
        }

        /// <summary>Converts a byte to a BASE64 string.</summary>
        public static string EncodeBase64(byte value) => Convert.ToBase64String(new byte[1] { value });

        /// <summary>Converts a byte array to a BASE64 string.</summary>
        public static string EncodeBase64(byte[] value)
        {
            byte[] bytes = EncodeB64(value);
            return Encoding.ASCII.GetString(bytes, 0, bytes.Length);
        }

        /// <summary>Converts an integer to a BASE64 string.</summary>
        public static string EncodeBase64(int value) => Convert.ToBase64String(new byte[4] { (byte)value, (byte)(value >> 8), (byte)(value >> 16), (byte)(value >> 24) });

        /// <summary>Encodes a string using simple escape codes.</summary>
        public string EncodeEscaped(string value) => (_options & CardStandardWriterOptions.IgnoreCommas) == CardStandardWriterOptions.IgnoreCommas ? EncodeEscaped(value, _outlookEscapedCharacters) : EncodeEscaped(value, _standardEscapedCharacters);

        /// <summary>
        ///     Encodes a character array using simple escape sequences.
        /// </summary>
        public static string EncodeEscaped(string value, char[] escaped)
        {
            if (escaped == null)
                throw new ArgumentNullException(nameof(escaped));
            if (string.IsNullOrEmpty(value))
                return value;
            var stringBuilder = new StringBuilder();
            var startIndex = 0;
            do
            {
                var index = value.IndexOfAny(escaped, startIndex);
                if (index == -1)
                {
                    stringBuilder.Append(value, startIndex, value.Length - startIndex);
                    break;
                }

                var ch = value[index] switch
                {
                    '\n' => 'n',
                    '\r' => 'r',
                    _ => value[index],
                };
                stringBuilder.Append(value, startIndex, index - startIndex);
                stringBuilder.Append('\\');
                stringBuilder.Append(ch);
                startIndex = index + 1;
            }
            while (startIndex < value.Length);
            return stringBuilder.ToString();
        }

        private static string EncodeQuotedPrintableChar(char c, Encoding targetEncoding)
        {
            var bytes = targetEncoding.GetBytes(new char[1] { c });
            var str = string.Empty;
            foreach (var num in bytes)
                str = str + "=" + num.ToString("X2");
            return str;
        }

        /// <summary>Converts a string to quoted-printable format.</summary>
        /// <param name="value">
        ///     The value to encode in Quoted Printable format.
        /// </param>
        /// <param name="enc">
        ///     The encoding in Quoted Printable format.
        /// </param>
        /// <returns>The value encoded in Quoted Printable format.</returns>
        public static string EncodeQuotedPrintable(string value, Encoding enc)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            var stringBuilder = new StringBuilder();
            foreach (var c in value)
            {
                var num = c;
                if (num == 9 || num >= 32 && num <= 60 || num >= 62 && num <= 126)
                    stringBuilder.Append(c);
                else
                    stringBuilder.Append(EncodeQuotedPrintableChar(c, enc));
            }
            var c1 = stringBuilder[^1];
            if (char.IsWhiteSpace(c1))
            {
                stringBuilder.Remove(stringBuilder.Length - 1, 1);
                stringBuilder.Append(EncodeQuotedPrintableChar(c1, enc));
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        ///     Returns property encoded into a standard Person NAME:VALUE format.
        /// </summary>
        public string EncodeProperty(CardProperty property) => EncodeProperty(property, null);

        public string EncodeProperty(CardProperty property, string charsetName)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));
            if (string.IsNullOrEmpty(property.Name))
                throw new ArgumentException();
            var enc = Encoding.UTF8;
            if (charsetName != null && charsetName != string.Empty && charsetName.ToLower() != "utf-8")
                enc = Encoding.GetEncoding(charsetName);
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(property.Name);
            foreach (var subproperty in property.Subproperties)
            {
                stringBuilder.Append(';');
                stringBuilder.Append(subproperty.Name);
                if (!string.IsNullOrEmpty(subproperty.Value))
                {
                    stringBuilder.Append('=');
                    stringBuilder.Append(subproperty.Value);
                }
            }
            if (property.Value == null)
            {
                stringBuilder.Append(':');
            }
            else
            {
                var type = property.Value.GetType();
                if (type == typeof(byte[]))
                {
                    stringBuilder.Append(";ENCODING=BASE64:");
                    stringBuilder.Append("\r\n ");
                    stringBuilder.Append(EncodeBase64((byte[])property.Value));
                    stringBuilder.Append("\r\n");
                }
                else if (type == typeof(ValueCollection))
                {
                    var cardValueCollection = (ValueCollection)property.Value;
                    if (charsetName != null)
                    {
                        foreach (var s in cardValueCollection)
                        {
                            if (s == null) continue;
                            if (s != Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(s)))
                            {
                                stringBuilder.Append(";CHARSET=");
                                stringBuilder.Append(charsetName);
                                break;
                            }
                        }
                    }
                    stringBuilder.Append(':');
                    var flag = property.Subproperties.GetValue("ENCODING") == "QUOTED-PRINTABLE";
                    for (var index = 0; index < cardValueCollection.Count; ++index)
                    {
                        if (flag)
                            stringBuilder.Append(EncodeQuotedPrintable(cardValueCollection[index], enc));
                        else
                            stringBuilder.Append(EncodeEscaped(cardValueCollection[index]));
                        if (index < cardValueCollection.Count - 1)
                            stringBuilder.Append(cardValueCollection.Separator);
                    }
                }
                else
                {
                    var s = type != typeof(char[]) ? property.Value.ToString() : new string((char[])property.Value);
                    if (charsetName != null && s != Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(s)))
                    {
                        stringBuilder.Append(";CHARSET=");
                        stringBuilder.Append(charsetName);
                    }
                    stringBuilder.Append(':');
                    switch (property.Subproperties.GetValue("ENCODING"))
                    {
                        case "QUOTED-PRINTABLE":
                            stringBuilder.Append(EncodeQuotedPrintable(s, enc));
                            break;
                        default:
                            stringBuilder.Append(EncodeEscaped(s));
                            break;
                    }
                }
            }
            return stringBuilder.ToString();
        }

        /// <summary>Writes a Person to an output text writer.</summary>
        public override void Write(Person card, TextWriter output, string charsetName)
        {
            if (card == null)
                throw new ArgumentNullException(nameof(card));
            if (output == null)
                throw new ArgumentNullException(nameof(output));
            Write(BuildProperties(card), output, charsetName);
        }

        /// <summary>
        ///     Writes a collection of Person properties to an output text writer.
        /// </summary>
        public void Write(IList<CardProperty> properties, TextWriter output, string charsetName)
        {
            if (properties == null)
                throw new ArgumentNullException(nameof(properties));
            if (output == null)
                throw new ArgumentNullException(nameof(output));
            foreach (var property in properties)
                output.WriteLine(EncodeProperty(property, charsetName));
        }

        private static byte[] EncodeB64(byte[] srcBytes)
        {
            var numArray1 = EncodeAndWrapB64(srcBytes, 72, out var destBytesLength);
            var numArray2 = new byte[destBytesLength];
            Array.Copy(numArray1, 0, numArray2, 0, destBytesLength);
            return numArray2;
        }

        private static byte[] EncodeAndWrapB64(byte[] srcBytes, int nLineLen, out int destBytesLength)
        {
            destBytesLength = 0;
            if (srcBytes == null || srcBytes.Length == 0)
                return new byte[destBytesLength];
            var length = srcBytes.Length;
            destBytesLength = length >= 4 ? CalculateB64Size(length, nLineLen, 1f) : 4;
            var base64Array = new byte[destBytesLength];
            var arrayIndex = 0;
            var num1 = 0;
            var index = 0;
            for (; length >= 3; length -= 3)
            {
                base64Array[arrayIndex++] = Convert.ToByte(_basis_64[srcBytes[index] >> 2]);
                int num2;
                InsertLineBreakIfNeed(num2 = num1 + 1, ref base64Array, ref arrayIndex, nLineLen);
                base64Array[arrayIndex++] = Convert.ToByte(_basis_64[srcBytes[index] << 4 & 48 | srcBytes[index + 1] >> 4]);
                int num3;
                InsertLineBreakIfNeed(num3 = num2 + 1, ref base64Array, ref arrayIndex, nLineLen);
                base64Array[arrayIndex++] = Convert.ToByte(_basis_64[srcBytes[index + 1] << 2 & 60 | srcBytes[index + 2] >> 6]);
                int num4;
                InsertLineBreakIfNeed(num4 = num3 + 1, ref base64Array, ref arrayIndex, nLineLen);
                base64Array[arrayIndex++] = Convert.ToByte(_basis_64[srcBytes[index + 2] & 63]);
                InsertLineBreakIfNeed(num1 = num4 + 1, ref base64Array, ref arrayIndex, nLineLen);
                index += 3;
            }
            if (length > 0)
            {
                base64Array[arrayIndex++] = Convert.ToByte(_basis_64[srcBytes[index] >> 2]);
                int num5;
                InsertLineBreakIfNeed(num5 = num1 + 1, ref base64Array, ref arrayIndex, nLineLen);
                var num6 = Convert.ToByte(srcBytes[index] << 4 & 48);
                if (length > 1)
                    num6 |= Convert.ToByte(srcBytes[index + 1] >> 4);
                base64Array[arrayIndex++] = Convert.ToByte(_basis_64[num6]);
                int num7;
                InsertLineBreakIfNeed(num7 = num5 + 1, ref base64Array, ref arrayIndex, nLineLen);
                base64Array[arrayIndex++] = Convert.ToByte(length < 2 ? '=' : _basis_64[srcBytes[index + 1] << 2 & 60]);
                InsertLineBreakIfNeed(_ = num7 + 1, ref base64Array, ref arrayIndex, nLineLen);
                base64Array[arrayIndex++] = Convert.ToByte('=');
            }
            destBytesLength = arrayIndex;
            return base64Array;
        }

        private static void InsertLineBreakIfNeed(int insertCount, ref byte[] base64Array, ref int arrayIndex, int lineLength)
        {
            if (lineLength <= 0 || insertCount % lineLength != 0)
                return;
            base64Array[arrayIndex++] = 13;
            base64Array[arrayIndex++] = 10;
            base64Array[arrayIndex++] = 32;
        }

        private static int CalculateB64Size(int srcSize, int lineLength, float factor)
        {
            var num = (srcSize + 2 - (srcSize + 2) % 3) / 3 * 4;
            for (var index = 0; index < 4; ++index)
                num += num % 4 != 0 ? 1 : 0;
            if (lineLength > 0 && num / lineLength > 0)
                num += (int)Math.Floor((double)(num / lineLength)) * 3;
            return (double)factor <= 0.0 ? num : (int)Math.Floor(num * (double)factor);
        }
    }
}
