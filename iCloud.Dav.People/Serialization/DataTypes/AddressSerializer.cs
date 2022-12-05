using iCloud.Dav.People.DataTypes;
using System;
using System.IO;
using vCard.Net.Serialization;
using vCard.Net.Serialization.DataTypes;
using vCard.Net.Utility;
using System.Text.RegularExpressions;

namespace iCloud.Dav.People.Serialization.DataTypes
{
    public class AddressSerializer : StringSerializer
    {
        private static readonly Regex _reSplitSemiColon = new Regex("(?:^[;])|(?<=(?:[^\\\\]))[;]");

        public AddressSerializer()
        {
        }

        public AddressSerializer(SerializationContext ctx) : base(ctx)
        {
        }

        public override Type TargetType => typeof(Address);

        public override string SerializeToString(object obj)
        {
            if (!(obj is Address address))
            {
                return null;
            }

            var array = new string[7];
            var num = 0;
            if (address.POBox != null && address.POBox.Length > 0)
            {
                num = 1;
                array[0] = address.POBox.Escape();
            }

            if (address.ExtendedAddress != null && address.ExtendedAddress.Length > 0)
            {
                num = 2;
                array[1] = address.ExtendedAddress.Escape();
            }

            if (address.StreetAddress != null && address.StreetAddress.Length > 0)
            {
                num = 3;
                array[2] = address.StreetAddress.Escape()   ;
            }

            if (address.Locality != null && address.Locality.Length > 0)
            {
                num = 4;
                array[3] = address.Locality.Escape();
            }

            if (address.Region != null && address.Region.Length > 0)
            {
                num = 5;
                array[4] = address.Region.Escape();
            }

            if (address.PostalCode != null && address.PostalCode.Length > 0)
            {
                num = 6;
                array[5] = address.PostalCode.Escape();
            }

            if (address.Country != null && address.Country.Length > 0)
            {
                num = 7;
                array[6] = address.Country.Escape();
            }

            return num == 0 ? null : Encode(address, string.Join(";", array, 0, num));
        }

        public Address Deserialize(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            if (!(CreateAndAssociate() is Address address))
            {
                return null;
            }

            // Decode the value as needed
            value = Decode(address, value);

            if (value is null)
            {
                return null;
            }

            var parts = value.Split(';');
            if (value != null && value.Length > 0)
            {
                string[] array = _reSplitSemiColon.Split(value);
                if (array.Length != 0)
                {
                    address.POBox = array[0].Unescape();
                }

                if (array.Length > 1)
                {
                    address.ExtendedAddress = array[1].Unescape();
                }

                if (array.Length > 2)
                {
                    address.StreetAddress = array[2].Unescape();
                }

                if (array.Length > 3)
                {
                    address.Locality = array[3].Unescape();
                }

                if (array.Length > 4)
                {
                    address.Region = array[4].Unescape();
                }

                if (array.Length > 5)
                {
                    address.PostalCode = array[5].Unescape();
                }

                if (array.Length > 6)
                {
                    address.Country = array[6].Unescape();
                }
            }
            return address;
        }

        public override object Deserialize(TextReader tr) => Deserialize(tr.ReadToEnd());
    }
}