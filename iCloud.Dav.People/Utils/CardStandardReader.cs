using iCloud.Dav.Core.Utils;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text;

namespace iCloud.Dav.People.Utils
{
    /// <summary>
    ///     Reads a Person written in the standard 2.0 or 3.0 text formats.
    ///     This is the primary (standard) Person format used by most
    ///     applications.
    /// </summary>
    /// <seealso cref="CardReader" />
    internal class CardStandardReader : CardReader
    {
        /// <summary>
        ///     The DeliveryAddressTypeNames array contains the recognized
        ///     TYPE values for an ADR (delivery address).
        /// </summary>
        private readonly string[] DeliveryAddressTypeNames = new string[7]
        {
            "DOM",
            "INTL",
            "POSTAL",
            "PARCEL",
            "HOME",
            "WORK",
            "PREF"
        };

        /// <summary>
        ///     The PhoneTypeNames constant defines the recognized
        ///     subproperty names that identify the category or
        ///     classification of a phone.  The names are used with
        ///     the TEL property.
        /// </summary>
        private readonly string[] PhoneTypeNames = new string[13]
        {
            "BBS",
            "CAR",
            "CELL",
            "FAX",
            "HOME",
            "ISDN",
            "MODEM",
            "MSG",
            "PAGER",
            "PREF",
            "VIDEO",
            "VOICE",
            "WORK"
        };

        /// <summary>Decodes a string containing BASE64 characters.</summary>
        /// <param name="value">
        ///     A string containing data that has been encoded with
        ///     the BASE64 format.
        /// </param>
        /// <returns>The decoded data as a byte array.</returns>
        public static byte[] DecodeBase64(string value) => Convert.FromBase64String(value);

        /// <summary>
        ///     Converts BASE64 data that has been stored in a
        ///     character array.
        /// </summary>
        /// <param name="value">
        ///     A character array containing BASE64 data.
        /// </param>
        /// <returns>A byte array containing the decoded BASE64 data.</returns>
        public static byte[] DecodeBase64(char[] value) => value != null ? Convert.FromBase64CharArray(value, 0, value.Length) : throw new ArgumentNullException(nameof(value));

        /// <summary>Parses the name of an email address type.</summary>
        /// <param name="keyword">
        ///     The email address type keyword found in the Person file (e.g. AOL or INTERNET).
        /// </param>
        /// <returns>
        ///     Null or the decoded <see cref="EmailAddressType" />.
        /// </returns>
        /// <seealso cref="EmailAddress" />
        /// <seealso cref="EmailAddressType" />
        public static EmailAddressType? DecodeEmailAddressType(string keyword)
        {
            if (string.IsNullOrEmpty(keyword)) return null;

            return keyword.ToUpperInvariant() switch
            {
                "INTERNET" => EmailAddressType.Internet,
                "AOL" => EmailAddressType.AOL,
                "APPLELINK" => EmailAddressType.AppleLink,
                "ATTMAIL" => EmailAddressType.AttMail,
                "CIS" => EmailAddressType.CompuServe,
                "EWORLD" => EmailAddressType.eWorld,
                "IBMMAIL" => EmailAddressType.IBMMail,
                "MCIMAIL" => EmailAddressType.MCIMail,
                "POWERSHARE" => EmailAddressType.PowerShare,
                "PRODIGY" => EmailAddressType.Prodigy,
                "TLX" => EmailAddressType.Telex,
                "X400" => EmailAddressType.X400,
                _ => null,
            };
        }

        /// <summary>
        ///     Decodes a string that has been encoded with the standard
        ///     Person escape codes.
        /// </summary>
        /// <param name="value">
        ///     A string encoded with Person escape codes.
        /// </param>
        /// <returns>The decoded string.</returns>
        public static string DecodeEscaped(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            var stringBuilder = new StringBuilder(value.Length);
            var startIndex = 0;
            do
            {
                var num1 = value.IndexOf('\\', startIndex);
                if (num1 == -1 || num1 == value.Length - 1)
                {
                    stringBuilder.Append(value, startIndex, value.Length - startIndex);
                    break;
                }
                var ch = value[num1 + 1];
                stringBuilder.Append(value, startIndex, num1 - startIndex);
                int num2;
                switch (ch)
                {
                    case ',':
                    case ';':
                    case '\\':
                        stringBuilder.Append(ch);
                        num2 = num1 + 2;
                        break;
                    case 'N':
                    case 'n':
                        stringBuilder.Append('\n');
                        num2 = num1 + 2;
                        break;
                    case 'R':
                    case 'r':
                        stringBuilder.Append('\r');
                        num2 = num1 + 2;
                        break;
                    default:
                        stringBuilder.Append('\\');
                        stringBuilder.Append(ch);
                        num2 = num1 + 2;
                        break;
                }
                startIndex = num2;
            }
            while (startIndex < value.Length);
            return stringBuilder.ToString();
        }

        /// <summary>
        ///     Converts a single hexadecimal character to
        ///     its integer value.
        /// </summary>
        /// <param name="value">A Unicode character.</param>
        public static int DecodeHexadecimal(char value)
        {
            if (char.IsDigit(value))
                return Convert.ToInt32(char.GetNumericValue(value));
            if (value >= 'A' && value <= 'F')
                return Convert.ToInt32(value) - 55;
            if (value >= 'a' && value <= 'f')
                return Convert.ToInt32(value) - 87;
            throw new ArgumentOutOfRangeException(nameof(value));
        }

        /// <summary>
        ///     Decodes a string that has been encoded in QUOTED-PRINTABLE format.
        /// </summary>
        /// <param name="value">
        ///     A string that has been encoded in QUOTED-PRINTABLE.
        /// </param>
        /// <returns>The decoded string.</returns>
        public static string DecodeQuotedPrintable(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            var stringBuilder = new StringBuilder();
            var ch1 = char.MinValue;
            var quotedPrintableState = QuotedPrintableState.None;
            foreach (var ch2 in value)
            {
                switch (quotedPrintableState)
                {
                    case QuotedPrintableState.None:
                        if (ch2 == '=')
                        {
                            quotedPrintableState = QuotedPrintableState.ExpectingHexChar1;
                            break;
                        }
                        stringBuilder.Append(ch2);
                        break;
                    case QuotedPrintableState.ExpectingHexChar1:
                        if (IsHexDigit(ch2))
                        {
                            ch1 = ch2;
                            quotedPrintableState = QuotedPrintableState.ExpectingHexChar2;
                            break;
                        }
                        switch (ch2)
                        {
                            case '\r':
                                quotedPrintableState = QuotedPrintableState.ExpectingLineFeed;
                                continue;
                            case '=':
                                stringBuilder.Append('=');
                                quotedPrintableState = QuotedPrintableState.ExpectingHexChar1;
                                continue;
                            default:
                                stringBuilder.Append('=');
                                stringBuilder.Append(ch2);
                                quotedPrintableState = QuotedPrintableState.None;
                                continue;
                        }
                    case QuotedPrintableState.ExpectingHexChar2:
                        if (IsHexDigit(ch2))
                        {
                            var num = (DecodeHexadecimal(ch1) << 4) + DecodeHexadecimal(ch2);
                            stringBuilder.Append((char)num);
                            quotedPrintableState = QuotedPrintableState.None;
                            break;
                        }
                        stringBuilder.Append('=');
                        stringBuilder.Append(ch1);
                        stringBuilder.Append(ch2);
                        quotedPrintableState = QuotedPrintableState.None;
                        break;
                    case QuotedPrintableState.ExpectingLineFeed:
                        switch (ch2)
                        {
                            case '\n':
                                quotedPrintableState = QuotedPrintableState.None;
                                continue;
                            case '=':
                                quotedPrintableState = QuotedPrintableState.ExpectingHexChar1;
                                continue;
                            default:
                                stringBuilder.Append(ch2);
                                quotedPrintableState = QuotedPrintableState.None;
                                continue;
                        }
                }
            }
            switch (quotedPrintableState)
            {
                case QuotedPrintableState.ExpectingHexChar1:
                    stringBuilder.Append('=');
                    break;
                case QuotedPrintableState.ExpectingHexChar2:
                    stringBuilder.Append('=');
                    stringBuilder.Append(ch1);
                    break;
                case QuotedPrintableState.ExpectingLineFeed:
                    stringBuilder.Append('=');
                    stringBuilder.Append('\r');
                    break;
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        ///     Indicates whether the specified character is
        ///     a hexadecimal digit.
        /// </summary>
        /// <param name="value">A unicode character</param>
        public static bool IsHexDigit(char value)
        {
            if (char.IsDigit(value) || value >= 'A' && value <= 'F')
                return true;
            return value >= 'a' && value <= 'f';
        }

        /// <summary>Parses a string containing a date/time value.</summary>
        /// <param name="value">A string containing a date/time value.</param>
        /// <returns>
        ///     The parsed date, or null if no date could be parsed.
        /// </returns>
        /// <remarks>
        ///     Some revision dates, such as those generated by Outlook,
        ///     are not directly supported by the .NET DateTime parser.
        ///     This function attempts to accomodate the non-standard formats.
        /// </remarks>
        public static DateTime? ParseDate(string value)
        {
            if (DateTime.TryParse(value, out var result))
                return new DateTime?(result);
            return DateTime.TryParseExact(value, "yyyyMMdd\\THHmmss\\Z", null, DateTimeStyles.AssumeUniversal, out result) ? new DateTime?(result) : new DateTime?();
        }

        /// <summary>
        ///     Parses an encoding name (such as "BASE64") and returns
        ///     the corresponding <see cref="CardEncoding" /> value.
        /// </summary>
        /// <param name="name">
        ///     The name of an encoding from a standard Person property.
        /// </param>
        /// <returns>The enumerated value of the encoding.</returns>
        public static CardEncoding ParseEncoding(string name)
        {
            if (string.IsNullOrEmpty(name))
                return CardEncoding.Unknown;
            return name.ToUpperInvariant() switch
            {
                "B" => CardEncoding.Base64,
                "BASE64" => CardEncoding.Base64,
                "QUOTED-PRINTABLE" => CardEncoding.QuotedPrintable,
                _ => CardEncoding.Unknown,
            };
        }

        /// <summary>
        ///     Parses the name of a phone type and returns the
        ///     corresponding <see cref="PhoneTypes" /> value.
        /// </summary>
        /// <param name="name">
        ///     The name of a phone type from a TEL Person property.
        /// </param>
        /// <returns>
        ///     The enumerated value of the phone type, or Default
        ///     if the phone type could not be determined.
        /// </returns>
        public static PhoneTypes ParsePhoneType(string name)
        {
            if (string.IsNullOrEmpty(name))
                return PhoneTypes.Default;
            return name.Trim().ToUpperInvariant() switch
            {
                "BBS" => PhoneTypes.BBS,
                "CAR" => PhoneTypes.Car,
                "CELL" => PhoneTypes.Cellular,
                "FAX" => PhoneTypes.Fax,
                "HOME" => PhoneTypes.Home,
                "ISDN" => PhoneTypes.ISDN,
                "MODEM" => PhoneTypes.Modem,
                "MSG" => PhoneTypes.MessagingService,
                "PAGER" => PhoneTypes.Pager,
                "PREF" => PhoneTypes.Preferred,
                "VIDEO" => PhoneTypes.Video,
                "VOICE" => PhoneTypes.Voice,
                "WORK" => PhoneTypes.Work,
                _ => PhoneTypes.Default,
            };
        }

        /// <summary>
        ///     Decodes the bitmapped phone type given an array of
        ///     phone type names.
        /// </summary>
        /// <param name="names">
        ///     An array containing phone type names such as BBS or VOICE.
        /// </param>
        /// <returns>
        ///     The phone type value that represents the combination
        ///     of all names defined in the array.  Unknown names are
        ///     ignored.
        /// </returns>
        public static PhoneTypes ParsePhoneType(string[] names)
        {
            var cardPhoneTypes = PhoneTypes.Default;
            foreach (var name in names)
                cardPhoneTypes |= ParsePhoneType(name);
            return cardPhoneTypes;
        }

        /// <summary>Parses the type of postal address.</summary>
        /// <param name="value">
        ///     The single value of a TYPE subproperty for the ADR property.
        /// </param>
        /// <returns>
        ///     The <see cref="AddressTypes" /> that corresponds
        ///     with the TYPE keyword, or PostalAddressType.Default if
        ///     the type could not be identified.
        /// </returns>
        public static AddressTypes ParseDeliveryAddressType(string value)
        {
            if (string.IsNullOrEmpty(value))
                return AddressTypes.Default;
            return value.ToUpperInvariant() switch
            {
                "DOM" => AddressTypes.Domestic,
                "HOME" => AddressTypes.Home,
                "INTL" => AddressTypes.International,
                "PARCEL" => AddressTypes.Parcel,
                "POSTAL" => AddressTypes.Postal,
                "WORK" => AddressTypes.Work,
                _ => AddressTypes.Default,
            };
        }

        /// <summary>
        ///     Parses a string array containing one or more
        ///     postal address types.
        /// </summary>
        /// <param name="typeNames">
        ///     A string array containing zero or more keywords
        ///     used with the TYPE subproperty of the ADR property.
        /// </param>
        /// <returns>
        ///     A <see cref="AddressTypes" /> flags enumeration
        ///     that corresponds with all known type names from the array.
        /// </returns>
        public static AddressTypes ParseDeliveryAddressType(string[] typeNames)
        {
            var deliveryAddressTypes = AddressTypes.Default;
            foreach (var typeName in typeNames)
                deliveryAddressTypes |= ParseDeliveryAddressType(typeName);
            return deliveryAddressTypes;
        }

        /// <summary>Reads a Person (VCF) file from an input stream.</summary>
        /// <param name="card">An initialized Person.</param>
        /// <param name="reader">
        ///     A text reader pointing to the beginning of
        ///     a standard Person file.
        /// </param>
        /// <returns>The Person with values updated from the file.</returns>
        public override void ReadInto(Person card, TextReader reader)
        {
            CardProperty property;
            do
            {
                property = ReadProperty(reader);
                if (property != null)
                {
                    if (string.Compare("END", property.Name, StringComparison.OrdinalIgnoreCase) == 0 && string.Compare("VCARD", property.ToString(), StringComparison.OrdinalIgnoreCase) == 0)
                        break;
                    ReadInto(card, property);
                }
            }
            while (property != null);
        }

        /// <summary>
        ///     Updates a Person object based on the contents of a CardProperty.
        /// </summary>
        /// <param name="card">An initialized Person object.</param>
        /// <param name="property">An initialized CardProperty object.</param>
        /// <remarks>
        ///     <para>
        ///         This method examines the contents of a property
        ///         and attempts to update an existing Person based on
        ///         the property name and value.  This function must
        ///         be updated when new Person properties are implemented.
        ///     </para>
        /// </remarks>
        public void ReadInto(Person card, CardProperty property)
        {
            card.ThrowIfNull(nameof(card));
            property.ThrowIfNull(nameof(property));

            if (string.IsNullOrEmpty(property.Name)) return;

            switch (property.Name.ToUpperInvariant())
            {
                case "ADR":
                    ReadInto_ADR(card, property);
                    break;
                case "BDAY":
                    ReadInto_BDAY(card, property);
                    break;
                case "CATEGORIES":
                    ReadInto_CATEGORIES(card, property);
                    break;
                case "CLASS":
                    ReadInto_CLASS(card, property);
                    break;
                case "EMAIL":
                    ReadInto_EMAIL(card, property);
                    break;
                case "FN":
                    ReadInto_FN(card, property);
                    break;
                case "GEO":
                    ReadInto_GEO(card, property);
                    break;
                case "KEY":
                    ReadInto_KEY(card, property);
                    break;
                case "LABEL":
                    ReadInto_LABEL(card, property);
                    break;
                case "MAILER":
                    ReadInto_MAILER(card, property);
                    break;
                case "N":
                    ReadInto_N(card, property);
                    break;
                case "NAME":
                    ReadInto_NAME(card, property);
                    break;
                case "NICKNAME":
                    ReadInto_NICKNAME(card, property);
                    break;
                case "NOTE":
                    ReadInto_NOTE(card, property);
                    break;
                case "ORG":
                    ReadInto_ORG(card, property);
                    break;
                case "PHOTO":
                    ReadInto_PHOTO(card, property);
                    break;
                case "X-MS-CARDPICTURE":
                    ReadInto_PHOTO(card, property);
                    break;
                case "PRODID":
                    ReadInto_PRODID(card, property);
                    break;
                case "REV":
                    ReadInto_REV(card, property);
                    break;
                case "ROLE":
                    ReadInto_ROLE(card, property);
                    break;
                case "SOURCE":
                    ReadInto_SOURCE(card, property);
                    break;
                case "TEL":
                    ReadInto_TEL(card, property);
                    break;
                case "TITLE":
                    ReadInto_TITLE(card, property);
                    break;
                case "TZ":
                    ReadInto_TZ(card, property);
                    break;
                case "UID":
                    ReadInto_UID(card, property);
                    break;
                case "URL":
                    ReadInto_URL(card, property);
                    break;
                case "X-WAB-GENDER":
                    ReadInto_X_WAB_GENDER(card, property);
                    break;
                case "X-ADDRESSBOOKSERVER-KIND":
                    ReadInto_XADDRESSBOOKSERVERKIND((ContactGroup)card, property);
                    break;
                case "X-ADDRESSBOOKSERVER-MEMBER":
                    ReadInto_XADDRESSBOOKSERVERMEMBER((ContactGroup)card, property);
                    break;
                case var value when value.Contains(".EMAIL"):
                    ReadInto_EMAIL(card, property);
                    break;
                case var value when value.Contains(".URL"):
                    ReadInto_URL(card, property);
                    break;
                case var value when value.Contains(".ADR"):
                    ReadInto_ADR(card, property);
                    break;
            }
        }

        /// <summary>Reads an ADR property.</summary>
        private void ReadInto_ADR(Person card, CardProperty property)
        {
            var strArray = property.Value.ToString().Split(';');
            var cardAddress = new Address();
            if (strArray.Length >= 7)
                cardAddress.Country = strArray[6].Trim();
            if (strArray.Length >= 6)
                cardAddress.PostalCode = strArray[5].Trim();
            if (strArray.Length >= 5)
                cardAddress.Region = strArray[4].Trim();
            if (strArray.Length >= 4)
                cardAddress.City = strArray[3].Trim();
            if (strArray.Length >= 3)
                cardAddress.Street = strArray[2].Trim();
            if (string.IsNullOrEmpty(cardAddress.City) && string.IsNullOrEmpty(cardAddress.Country) && string.IsNullOrEmpty(cardAddress.PostalCode) && string.IsNullOrEmpty(cardAddress.Region) && string.IsNullOrEmpty(cardAddress.Street))
                return;
            cardAddress.AddressType = ParseDeliveryAddressType(property.Subproperties.GetNames(DeliveryAddressTypeNames));
            foreach (var subproperty in property.Subproperties)
            {
                if (!string.IsNullOrEmpty(subproperty.Value) && string.Compare("TYPE", subproperty.Name, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    cardAddress.AddressType = (AddressTypes)((int)cardAddress.AddressType | (int)ParseDeliveryAddressType(subproperty.Value.Split(',')));
                }
                if (subproperty.Name.ToUpperInvariant() == "PREF")
                    cardAddress.IsPreferred = true;
            }
            card.Addresses.Add(cardAddress);
        }

        /// <summary>Reads the BDAY property.</summary>
        private static void ReadInto_BDAY(Person card, CardProperty property)
        {
            if (DateTime.TryParse(property.ToString(), out var result))
                card.BirthDate = new DateTime?(result);
            else if (DateTime.TryParseExact(property.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
                card.BirthDate = new DateTime?(result);
            else
                card.BirthDate = new DateTime?();
        }

        /// <summary>Reads the CATEGORIES property.</summary>
        private static void ReadInto_CATEGORIES(Person card, CardProperty property)
        {
            var str1 = property.ToString();
            var chArray = new char[1] { ',' };
            foreach (var str2 in str1.Split(chArray))
            {
                if (str2.Length > 0)
                    card.Categories.Add(str2);
            }
        }

        /// <summary>Reads the CLASS property.</summary>
        private static void ReadInto_CLASS(Person card, CardProperty property)
        {
            if (property.Value == null)
                return;
            switch (property.ToString().ToUpperInvariant())
            {
                case "PUBLIC":
                    card.AccessClassification = AccessClassification.Public;
                    break;
                case "PRIVATE":
                    card.AccessClassification = AccessClassification.Private;
                    break;
                case "CONFIDENTIAL":
                    card.AccessClassification = AccessClassification.Confidential;
                    break;
            }
        }

        /// <summary>Reads an EMAIL property.</summary>
        private static void ReadInto_EMAIL(Person card, CardProperty property)
        {
            var cardEmailAddress = new EmailAddress
            {
                Address = property.Value.ToString()
            };
            foreach (var subproperty in (Collection<Subproperty>)property.Subproperties)
            {
                switch (subproperty.Name.ToUpperInvariant())
                {
                    case "PREF":
                        cardEmailAddress.IsPreferred = true;
                        continue;
                    case "TYPE":
                        var str1 = subproperty.Value;
                        var chArray = new char[1] { ',' };
                        foreach (var str2 in str1.Split(chArray))
                        {
                            if (string.Compare("PREF", str2, StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                cardEmailAddress.IsPreferred = true;
                            }
                            else
                            {
                                var nullable = DecodeEmailAddressType(str2);
                                if (nullable.HasValue)
                                    cardEmailAddress.EmailType = nullable.Value;
                            }
                        }
                        continue;
                    default:
                        var nullable1 = DecodeEmailAddressType(subproperty.Name);
                        if (nullable1.HasValue)
                        {
                            cardEmailAddress.EmailType = nullable1.Value;
                            continue;
                        }
                        continue;
                }
            }
            card.EmailAddresses.Add(cardEmailAddress);
        }

        /// <summary>Reads the FN property.</summary>
        private static void ReadInto_FN(Person card, CardProperty property) => card.FormattedName = property.Value.ToString();

        /// <summary>Reads the GEO property.</summary>
        private static void ReadInto_GEO(Person card, CardProperty property)
        {
            var strArray = property.Value.ToString().Split(';');
            if (strArray.Length != 2 || !float.TryParse(strArray[0], out float result1) || !float.TryParse(strArray[1], out float result2))
                return;
            card.Latitude = new float?(result1);
            card.Longitude = new float?(result2);
        }

        /// <summary>Reads the KEY property.</summary>
        private static void ReadInto_KEY(Person card, CardProperty property)
        {
            var certificate = new Certificate
            {
                Data = (byte[])property.Value
            };
            if (property.Subproperties.Contains("X509"))
                certificate.KeyType = "X509";
            card.Certificates.Add(certificate);
        }

        /// <summary>Reads the LABEL property.</summary>
        private void ReadInto_LABEL(Person card, CardProperty property)
        {
            var cardDeliveryLabel1 = new Label
            {
                Text = property.Value.ToString(),
                AddressType = ParseDeliveryAddressType(property.Subproperties.GetNames(DeliveryAddressTypeNames))
            };
            foreach (var subproperty in (Collection<Subproperty>)property.Subproperties)
            {
                if (!string.IsNullOrEmpty(subproperty.Value) && string.Compare("TYPE", subproperty.Name, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    var cardDeliveryLabel2 = cardDeliveryLabel1;
                    cardDeliveryLabel2.AddressType = (AddressTypes)((int)cardDeliveryLabel2.AddressType | (int)ParseDeliveryAddressType(subproperty.Value.Split(',')));
                }
            }
            card.Labels.Add(cardDeliveryLabel1);
        }

        /// <summary>Reads the MAILER property.</summary>
        private static void ReadInto_MAILER(Person card, CardProperty property) => card.Mailer = property.Value.ToString();

        /// <summary>Reads the N property.</summary>
        private static void ReadInto_N(Person card, CardProperty property)
        {
            var strArray = property.ToString().Split(';');
            card.FamilyName = strArray[0];
            if (strArray.Length == 1)
                return;
            card.GivenName = strArray[1];
            if (strArray.Length == 2)
                return;
            card.AdditionalNames = strArray[2];
            if (strArray.Length == 3)
                return;
            card.NamePrefix = strArray[3];
            if (strArray.Length == 4)
                return;
            card.NameSuffix = strArray[4];
        }

        /// <summary>Reads the NAME property.</summary>
        private static void ReadInto_NAME(Person card, CardProperty property) => card.DisplayName = property.ToString().Trim();

        /// <summary>Reads the NICKNAME property.</summary>
        private static void ReadInto_NICKNAME(Person card, CardProperty property)
        {
            if (property.Value == null)
                return;
            var str1 = property.Value.ToString();
            var chArray = new char[1] { ',' };
            foreach (var str2 in str1.Split(chArray))
            {
                var str3 = str2.Trim();
                if (str3.Length > 0)
                    card.Nicknames.Add(str3);
            }
        }

        /// <summary>Reads the NOTE property.</summary>
        private static void ReadInto_NOTE(Person card, CardProperty property)
        {
            if (property.Value == null)
                return;
            var note = new Note
            {
                Language = property.Subproperties.GetValue("language"),
                Text = property.Value.ToString()
            };
            if (string.IsNullOrEmpty(note.Text))
                return;
            card.Notes.Add(note);
        }

        /// <summary>Reads the ORG property.</summary>
        private static void ReadInto_ORG(Person card, CardProperty property) => card.Organization = property.Value.ToString();

        /// <summary>Reads the PHOTO property.</summary>
        private static void ReadInto_PHOTO(Person card, CardProperty property)
        {
            if (string.Compare(property.Subproperties.GetValue("VALUE"), "URI", StringComparison.OrdinalIgnoreCase) == 0)
            {
                card.Photos.Add(new Photo(new Uri(property.ToString())));
            }
            else
            {
                var str = property.Subproperties.GetValue("TYPE");
                if (str != null && str.ToUpper() == "JPEG")
                    card.Photos.Add(new Photo((byte[])property.Value, PhotoImageFormat.Jpeg));
                else
                    card.Photos.Add(new Photo((byte[])property.Value, PhotoImageFormat.Bmp));
            }
        }

        /// <summary>Reads the PRODID property.</summary>
        private static void ReadInto_PRODID(Person card, CardProperty property) => card.ProductId = property.ToString();

        /// <summary>Reads the REV property.</summary>
        private static void ReadInto_REV(Person card, CardProperty property) => card.RevisionDate = ParseDate(property.Value.ToString());

        /// <summary>Reads the ROLE property.</summary>
        private static void ReadInto_ROLE(Person card, CardProperty property) => card.Role = property.Value.ToString();

        /// <summary>Reads the SOURCE property.</summary>
        private static void ReadInto_SOURCE(Person card, CardProperty property) => card.Sources.Add(new Source()
        {
            Context = property.Subproperties.GetValue("CONTEXT"),
            Uri = new Uri(property.Value.ToString())
        });

        /// <summary>Reads the TEL property.</summary>
        private static void ReadInto_TEL(Person card, CardProperty property)
        {
            var phone1 = new Phone
            {
                FullNumber = property.ToString()
            };
            if (string.IsNullOrEmpty(phone1.FullNumber))
                return;
            foreach (var subproperty in (Collection<Subproperty>)property.Subproperties)
            {
                if (string.Compare(subproperty.Name, "TYPE", StringComparison.OrdinalIgnoreCase) == 0 && !string.IsNullOrEmpty(subproperty.Value))
                {
                    var phone2 = phone1;
                    phone2.PhoneType = (PhoneTypes)((int)phone2.PhoneType | (int)ParsePhoneType(subproperty.Value.Split(',')));
                }
                else
                    phone1.PhoneType |= ParsePhoneType(subproperty.Name);
            }
            card.Phones.Add(phone1);
        }

        /// <summary>Reads the TITLE property.</summary>
        private static void ReadInto_TITLE(Person card, CardProperty property) => card.Title = property.ToString();

        /// <summary>Reads a TZ property.</summary>
        private static void ReadInto_TZ(Person card, CardProperty property) => card.TimeZone = property.ToString();

        /// <summary>Reads the UID property.</summary>
        private static void ReadInto_UID(Person card, CardProperty property) => card.UniqueId = property.ToString();

        /// <summary>Reads the URL property.</summary>
        private static void ReadInto_URL(Person card, CardProperty property)
        {
            var website = new Website
            {
                Url = property.ToString()
            };
            if (property.Subproperties.Contains("HOME"))
                website.WebsiteType = WebsiteTypes.Personal;
            if (property.Subproperties.Contains("WORK"))
                website.WebsiteType |= WebsiteTypes.Work;
            card.Websites.Add(website);
        }

        /// <summary>Reads the X-WAB-GENDER property.</summary>
        private static void ReadInto_X_WAB_GENDER(Person card, CardProperty property)
        {
            if (!int.TryParse(property.ToString(), out var result))
                return;
            switch (result)
            {
                case 1:
                    card.Gender = Gender.Female;
                    break;
                case 2:
                    card.Gender = Gender.Male;
                    break;
            }
        }

        private static void ReadInto_XADDRESSBOOKSERVERKIND(ContactGroup card, CardProperty property)
        {
            card.GroupType = property.Value.ToString();
        }

        private static void ReadInto_XADDRESSBOOKSERVERMEMBER(ContactGroup card, CardProperty property)
        {
            card.MemberResourceNames.Add(property.Value.ToString().Replace("urn:uuid:", string.Empty));
        }

        /// <summary>Reads a property from a string.</summary>
        public CardProperty ReadProperty(string text)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException(nameof(text));
            using var stringReader = new StringReader(text);
            return ReadProperty(stringReader);
        }

        /// <summary>Reads a property from a text reader.</summary>
        public CardProperty ReadProperty(TextReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            string str1;
            int length;
            string[] strArray1;
            while (true)
            {
                var str2 = reader.ReadLine();
                if (str2 != null)
                {
                    str1 = str2.Trim();
                    if (str1.Length == 0)
                    {
                        Warnings.Add("WarningMessages.BlankLine");
                    }
                    else
                    {
                        length = str1.IndexOf(':');
                        if (length == -1)
                        {
                            Warnings.Add("WarningMessages.ColonMissing");
                        }
                        else
                        {
                            var str3 = str1[..length].Trim();
                            if (string.IsNullOrEmpty(str3))
                            {
                                Warnings.Add("WarningMessages.EmptyName");
                            }
                            else
                            {
                                strArray1 = str3.Split(';');
                                for (var index = 0; index < strArray1.Length; ++index)
                                    strArray1[index] = strArray1[index].Trim();
                                if (strArray1[0].Length == 0)
                                    Warnings.Add("WarningMessages.EmptyName");
                                else
                                    goto label_15;
                            }
                        }
                    }
                }
                else
                    break;
            }
            return null;
        label_15:
            var cardProperty = new CardProperty();
            cardProperty.Name = strArray1[0];
            for (var index = 1; index < strArray1.Length; ++index)
            {
                var strArray2 = strArray1[index].Split(new char[1] { '=' }, 2);
                if (strArray2.Length == 1)
                    cardProperty.Subproperties.Add(strArray1[index].Trim());
                else
                    cardProperty.Subproperties.Add(strArray2[0].Trim(), strArray2[1].Trim());
            }
            var encoding = ParseEncoding(cardProperty.Subproperties.GetValue("ENCODING", new string[3]
            {
                "B",
                "BASE64",
                "QUOTED-PRINTABLE"
            }));
            var str4 = str1[(length + 1)..];
            while (true)
            {
                switch (reader.Peek())
                {
                    case 9:
                    case 32:
                        var str5 = reader.ReadLine();
                        str4 += str5[1..];
                        continue;
                    default:
                        goto label_24;
                }
            }
        label_24:
            if (encoding == CardEncoding.QuotedPrintable && str4.Length > 0)
            {
                while (str4[^1] == '=')
                    str4 = str4 + "\r\n" + reader.ReadLine();
            }
            cardProperty.Value = encoding switch
            {
                CardEncoding.Escaped => DecodeEscaped(str4),
                CardEncoding.Base64 => DecodeBase64(str4),
                CardEncoding.QuotedPrintable => DecodeQuotedPrintable(str4),
                _ => DecodeEscaped(str4),
            };
            var name = cardProperty.Subproperties.GetValue("CHARSET");
            if (name != null)
                cardProperty.Value = Encoding.GetEncoding(name).GetString(Encoding.UTF8.GetBytes((string)cardProperty.Value));
            return cardProperty;
        }

        /// <summary>
        ///     The state of the quoted-printable decoder (private).
        /// </summary>
        /// <remarks>
        ///     The <see cref="DecodeQuotedPrintable(string)" /> function
        ///     is a utility function that parses a string that
        ///     has been encoded with the QUOTED-PRINTABLE format.
        ///     The function is implemented as a state-pased parser
        ///     where the state is updated after examining each
        ///     character of the input string.  This enumeration
        ///     defines the various states of the parser.
        /// </remarks>
        private enum QuotedPrintableState
        {
            None,
            ExpectingHexChar1,
            ExpectingHexChar2,
            ExpectingLineFeed,
        }
    }
}
