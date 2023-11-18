using iCloud.Dav.People.DataTypes;
using System;
using System.IO;
using System.Text.RegularExpressions;
using vCard.Net.Serialization;
using vCard.Net.Serialization.DataTypes;
using vCard.Net.Utility;

namespace iCloud.Dav.People.Serialization.DataTypes;

/// <summary>
/// Serializes and deserializes an <see cref="Address"/> object to and from a string representation, according to the vCard specification.
/// </summary>
public class AddressSerializer : StringSerializer
{
    /// <summary>
    /// Regular expression pattern used to split a semicolon-separated list of Address values
    /// </summary>
    private static readonly Regex _reSplitSemiColon = new Regex("(?:^[;])|(?<=(?:[^\\\\]))[;]");

    /// <summary>
    /// Initializes a new instance of the <see cref="AddressSerializer"/> class.
    /// </summary>
    public AddressSerializer() : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AddressSerializer"/> class with the given <see cref="SerializationContext"/>.
    /// </summary>
    /// <param name="ctx">The <see cref="SerializationContext"/> to use.</param>
    public AddressSerializer(SerializationContext ctx) : base(ctx)
    {
    }

    /// <summary>
    /// Gets the Type that this <see cref="AddressSerializer"/> can serialize and deserialize, which is <see cref="Address"/>.
    /// </summary>
    public override Type TargetType => typeof(Address);

    /// <summary>
    /// Converts an <see cref="Address"/> object to a semicolon-separated string representation.
    /// </summary>
    /// <param name="obj">The <see cref="Address"/> object to be serialized.</param>
    /// <returns>A semicolon-separated string representation of the <see cref="Address"/> object.</returns>
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
            array[2] = address.StreetAddress.Escape();
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

    /// <summary>
    /// Converts a semicolon-separated string representation of an <see cref="Address"/> object to an <see cref="Address"/> object.
    /// </summary>
    /// <param name="value">The semicolon-separated string representation of the <see cref="Address"/> object to be deserialized.</param>
    /// <returns>An <see cref="Address"/> object.</returns>
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

    /// <summary>
    /// This method deserializes an <see cref="Address"/> object from the given <see cref="TextReader"/>.
    /// </summary>
    /// <param name="tr">The <see cref="TextReader"/> to deserialize the <see cref="Address"/> object from.</param>
    /// <returns>An <see cref="Address"/> object.</returns>
    public override object Deserialize(TextReader tr) => Deserialize(tr.ReadToEnd());
}