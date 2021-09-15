using System;
using System.IO;
using System.Linq;
using System.Text;
using vCards;

namespace iCloud.Dav.People.Serializer
{
    internal class CardStandardReader : vCardStandardReader
    {
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

        public override void ReadInto(vCard card, TextReader reader)
        {
            vCardProperty property;
            do
            {
                property = this.ReadProperty(reader);
                if (property != null)
                {
                    if (string.Compare("END", property.Name, StringComparison.OrdinalIgnoreCase) == 0 && string.Compare("VCARD", property.ToString(), StringComparison.OrdinalIgnoreCase) == 0)
                        break;
                    this.ReadInto(card, property);
                }
            }
            while (property != null);
        }

        public new vCardProperty ReadProperty(TextReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            string str1;
            int length;
            string[] strArray1;
            while (true)
            {
                string str2 = reader.ReadLine();
                if (str2 != null)
                {
                    str1 = str2.Trim();
                    if (str1.Length == 0)
                    {
                        this.Warnings.Add("WarningMessages.BlankLine");
                    }
                    else
                    {
                        length = str1.IndexOf(':');
                        if (length == -1)
                        {
                            this.Warnings.Add("WarningMessages.ColonMissing");
                        }
                        else
                        {
                            string str3 = str1.Substring(0, length).Trim();
                            if (string.IsNullOrEmpty(str3))
                            {
                                this.Warnings.Add("WarningMessages.EmptyName");
                            }
                            else
                            {
                                strArray1 = str3.Split(';');
                                for (int index = 0; index < strArray1.Length; ++index)
                                    strArray1[index] = strArray1[index].Trim();
                                if (strArray1[0].Length == 0)
                                    this.Warnings.Add("WarningMessages.EmptyName");
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
            vCardProperty vCardProperty = new vCardProperty();
            vCardProperty.Name = strArray1[0];
            for (int index = 1; index < strArray1.Length; ++index)
            {
                string[] strArray2 = strArray1[index].Split(new char[1] { '=' }, 2);
                if (strArray2.Length == 1)
                    vCardProperty.Subproperties.Add(strArray1[index].Trim());
                else
                    vCardProperty.Subproperties.Add(strArray2[0].Trim(), strArray2[1].Trim());
            }
            vCardEncoding encoding = vCardStandardReader.ParseEncoding(vCardProperty.Subproperties.GetValue("ENCODING", new string[3]
            {
                "B",
                "BASE64",
                "QUOTED-PRINTABLE"
            }));
            string str4 = str1.Substring(length + 1);
            while (true)
            {
                switch (reader.Peek())
                {
                    case 9:
                    case 32:
                        string str5 = reader.ReadLine();
                        str4 += str5.Substring(1);
                        continue;
                    default:
                        goto label_24;
                }
            }
        label_24:
            if (encoding == vCardEncoding.QuotedPrintable && str4.Length > 0)
            {
                while (str4[str4.Length - 1] == '=')
                    str4 = str4 + "\r\n" + reader.ReadLine();
            }
            switch (encoding)
            {
                case vCardEncoding.Escaped:
                    vCardProperty.Value = vCardStandardReader.DecodeEscaped(str4);
                    break;
                case vCardEncoding.Base64:
                    vCardProperty.Value = vCardStandardReader.DecodeBase64(str4);
                    break;
                case vCardEncoding.QuotedPrintable:
                    vCardProperty.Value = vCardStandardReader.DecodeQuotedPrintable(str4);
                    break;
                default:
                    vCardProperty.Value = vCardStandardReader.DecodeEscaped(str4);
                    break;
            }
            string name = vCardProperty.Subproperties.GetValue("CHARSET");
            if (name != null)
                vCardProperty.Value = Encoding.GetEncoding(name).GetString(Encoding.UTF8.GetBytes((string)vCardProperty.Value));
            return vCardProperty;
        }

        public new void ReadInto(vCard card, vCardProperty property)
        {
            base.ReadInto(card, property);
            switch (property.Name.ToUpperInvariant())
            {
                case "X-ADDRESSBOOKSERVER-KIND":
                    this.ReadInto_XADDRESSBOOKSERVERKIND((ContactGroup)card, property);
                    break;
                case "X-ADDRESSBOOKSERVER-MEMBER":
                    this.ReadInto_XADDRESSBOOKSERVERMEMBER((ContactGroup)card, property);
                    break;
                case string value when value.Contains(".EMAIL"):
                    this.ReadInto_EMAIL((Person)card, property);
                    break;
                case string value when value.Contains(".URL"):
                    this.ReadInto_URL((Person)card, property);
                    break;
                case string value when value.Contains(".ADR"):
                    this.ReadInto_ADR((Person)card, property);
                    break;
                case string value when value.Contains(".X-ABADR"):
                    this.ReadInto_XABADR((Person)card, property);
                    break;
                case string value when value.Contains("NOTE"):
                    this.ReadInto_XABADR((Person)card, property);
                    break;
            }
        }

        private void ReadInto_ADR(Person card, vCardProperty property)
        {
            string[] strArray = property.Value.ToString().Split(';');
            VCardAddress cardAddress = new VCardAddress() { Id = property.Name.Replace(".ADR", string.Empty) };
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
            if (string.IsNullOrEmpty(cardAddress.City) && string.IsNullOrEmpty(cardAddress.Country) && (string.IsNullOrEmpty(cardAddress.PostalCode) && string.IsNullOrEmpty(cardAddress.Region)) && string.IsNullOrEmpty(cardAddress.Street))
                return;
            cardAddress.AddressType = vCardStandardReader.ParseDeliveryAddressType(property.Subproperties.GetNames(this.DeliveryAddressTypeNames));
            foreach (vCardSubproperty subproperty in property.Subproperties)
            {
                if (!string.IsNullOrEmpty(subproperty.Value) && string.Compare("TYPE", subproperty.Name, StringComparison.OrdinalIgnoreCase) == 0)
                    cardAddress.AddressType = (vCardDeliveryAddressTypes)((int)cardAddress.AddressType | (int)vCardStandardReader.ParseDeliveryAddressType(subproperty.Value.Split(',')));
                if (subproperty.Name.ToUpperInvariant() == "PREF")
                    cardAddress.IsPreferred = true;
            }
            card.Addresses.Add(cardAddress);
        }

        private void ReadInto_EMAIL(vCard card, vCardProperty property)
        {
            vCardEmailAddress cardEmailAddress = new vCardEmailAddress { Address = property.Value.ToString() };
            foreach (vCardSubproperty subproperty in property.Subproperties)
            {
                switch (subproperty.Name.ToUpperInvariant())
                {
                    case "PREF":
                        cardEmailAddress.IsPreferred = true;
                        continue;
                    case "TYPE":
                        string str1 = subproperty.Value;
                        char[] chArray = new char[1] { ',' };
                        foreach (string str2 in str1.Split(chArray))
                        {
                            if (string.Compare("PREF", str2, StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                cardEmailAddress.IsPreferred = true;
                            }
                            else
                            {
                                vCardEmailAddressType? nullable = vCardStandardReader.DecodeEmailAddressType(str2);
                                if (nullable.HasValue)
                                    cardEmailAddress.EmailType = nullable.Value;
                            }
                        }
                        continue;
                    default:
                        vCardEmailAddressType? nullable1 = vCardStandardReader.DecodeEmailAddressType(subproperty.Name);
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

        private void ReadInto_URL(vCard card, vCardProperty property)
        {
            vCardWebsite vCardWebsite = new vCardWebsite { Url = property.ToString() };
            if (property.Subproperties.Contains("HOME"))
                vCardWebsite.IsPersonalSite = true;
            if (property.Subproperties.Contains("WORK"))
                vCardWebsite.IsWorkSite = true;
            card.Websites.Add(vCardWebsite);
        }

        private void ReadInto_XABADR(Person card, vCardProperty property)
        {
            card.Addresses.Where(x => x.Id.Contains(property.Name.Replace(".X-ABADR", string.Empty).ToString())).ToList().ForEach(x => x.CountryCode = property.Value.ToString());
        }

        private void ReadInto_XADDRESSBOOKSERVERKIND(ContactGroup card, vCardProperty property)
        {
            card.GroupType = property.Value.ToString();
        }

        private void ReadInto_XADDRESSBOOKSERVERMEMBER(ContactGroup card, vCardProperty property)
        {
            card.MemberResourceNames.Add(property.Value.ToString().Replace("urn:uuid:", string.Empty));
        }
    }
}
