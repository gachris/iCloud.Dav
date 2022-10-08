using iCloud.Dav.People.Types;
using iCloud.Dav.People.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace iCloud.Dav.People.Serialization.Read;

/// <summary>
///     An abstract reader for Person and Person-like file formats.
/// </summary>
/// <remarks>
///     <para>
///         The <see cref="Warnings" /> property is a string collection
///         containing a description of each warning encountered during
///         the read process.  An implementor of a card reader should
///         populate this collection as the Person data is being parsed.
///     </para>
/// </remarks>
internal abstract partial class CardReader<T>
{
    /// <summary>
    ///     Stores the warnings issued by the implementor
    ///     of the Person reader.  Currently warnings are
    ///     simple string messages; a future version will
    ///     store line numbers, severity levels, etc.
    /// </summary>
    /// <seealso cref="Warnings" />
    private readonly StringCollection _warnings;

    /// <summary>Initializes the base reader.</summary>
    protected CardReader() => _warnings = new StringCollection();

    /// <summary>Reads a Person from the specified input stream.</summary>
    /// <param name="reader">
    ///     A text reader that points to the beginning of
    ///     a Person in the format expected by the implementor.
    /// </param>
    /// <returns>
    ///     An initialized <see cref="Person" /> object.
    /// </returns>
    public T Read(TextReader reader)
    {
        var card = Activator.CreateInstance(typeof(T));
        if (card is not T tCard) throw new ArgumentNullException(nameof(T));
        ReadInto(tCard, reader);
        return tCard;
    }

    /// <summary>A collection of warning messages.</summary>
    /// <remarks>Reseved for future use.</remarks>
    public StringCollection Warnings => _warnings;

    /// <summary>Reads a group (VCF) file from an input stream.</summary>
    /// <param name="card">An initialized group.</param>
    /// <param name="reader">
    ///     A text reader pointing to the beginning of
    ///     a standard group file.
    /// </param>
    /// <returns>The group with values updated from the file.</returns>
    public void ReadInto(T card, TextReader reader)
    {
        CardProperty property;
        var properties = new List<CardProperty>();
        do
        {
            property = ReadProperty(reader);

            if (property is null) continue;

            if (property.Name.Equals("END", StringComparison.OrdinalIgnoreCase) &&
                property.ToString().Equals("VCARD", StringComparison.OrdinalIgnoreCase))
                break;

            properties.Add(property);
        }
        while (property != null);

        var cardProperties = properties.GroupBy(property =>
        {
            property.Name = Regex.Replace(property.Name, "^item(\\d+).", match =>
            {
                property.Group = match.Groups[1].Value;
                return string.Empty;
            });

            property.Group ??= Guid.NewGuid().ToString();
            return property.Group;
        });

        foreach (var cardPropertyGroup in cardProperties)
        {
            ReadInto(card, cardPropertyGroup.ToList());
        }
    }

    /// <summary>
    ///     Updates a group object based on the contents of a CardProperty.
    /// </summary>
    /// <param name="card">An initialized group object.</param>
    /// <param name="properties">An initialized card properties object.</param>
    /// <remarks>
    ///     <para>
    ///         This method examines the contents of a property
    ///         and attempts to update an existing group based on
    ///         the property name and value.  This function must
    ///         be updated when new group properties are implemented.
    ///     </para>
    /// </remarks>

    protected abstract void ReadInto(T card, IEnumerable<CardProperty> properties);

    /// <summary>Reads a property from a string.</summary>
    public CardProperty ReadProperty(string text)
    {
        if (string.IsNullOrEmpty(text))
            throw new ArgumentNullException(nameof(text));
        using var stringReader = new StringReader(text);
        return ReadProperty(stringReader);
    }

    /// <summary>Reads a property from a text reader.</summary>
    public virtual CardProperty ReadProperty(TextReader reader)
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
        var cardProperty = new CardProperty(strArray1[0]);
        for (var index = 1; index < strArray1.Length; ++index)
        {
            var strArray2 = strArray1[index].Split(new char[1] { '=' }, 2);
            if (strArray2.Length == 1)
                cardProperty.Subproperties.Add(strArray1[index].Trim());
            else
                cardProperty.Subproperties.Add(strArray2[0].Trim(), strArray2[1].Trim());
        }
        var encoding = DecodeHelper.ParseEncoding(cardProperty.Subproperties.GetValue("ENCODING", new string[3]
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
            CardEncoding.Escaped => DecodeHelper.DecodeEscaped(str4),
            CardEncoding.Base64 => DecodeHelper.DecodeBase64(str4),
            CardEncoding.QuotedPrintable => DecodeHelper.DecodeQuotedPrintable(str4),
            _ => DecodeHelper.DecodeEscaped(str4),
        };
        var name = cardProperty.Subproperties.GetValue("CHARSET");
        if (name != null)
            cardProperty.Value = Encoding.GetEncoding(name).GetString(Encoding.UTF8.GetBytes((string)cardProperty.Value));
        return cardProperty;
    }
}
