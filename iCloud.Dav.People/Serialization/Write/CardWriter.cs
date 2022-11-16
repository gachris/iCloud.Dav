using iCloud.Dav.People.Utils;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;

namespace iCloud.Dav.People.Serialization.Write;

/// <summary>Base class for generators.</summary>
public abstract class CardWriter<T>
{
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

    /// <summary>Extended options for the Person writer.</summary>
    public CardStandardWriterOptions Options { get; set; }

    /// <summary>Holds output warnings.</summary>
    private readonly StringCollection _warnings = new();

    /// <summary>
    ///     A collection of warning messages that were generated
    ///     during the output.
    /// </summary>
    public StringCollection Warnings => _warnings;

    /// <summary>
    ///     Writes to an I/O stream using the format
    ///     implemented by the class.
    /// </summary>
    /// <param name="card">The object to write the I/O string.</param>
    /// <param name="output">The text writer to use for output.</param>
    /// <param name="charsetName">The charsetName to use for output.</param>
    /// <remarks>
    ///     The implementor should not close or flush the stream.
    ///     The caller owns the stream and may not wish for the
    ///     stream to be closed (e.g. the caller may call the
    ///     function again with a different object).
    /// </remarks>
    public abstract void Write(T card, TextWriter output, string charsetName);

    /// <summary>Writes the object to the specified filename.</summary>
    public virtual void Write(T card, string filename, string charsetName)
    {
        if (card == null)
            throw new ArgumentNullException(nameof(card));
        using var streamWriter = new StreamWriter(filename);
        Write(card, streamWriter, charsetName);
    }

    /// <summary>Encodes a string using simple escape codes.</summary>
    public string EncodeEscaped(string value) => (Options & CardStandardWriterOptions.IgnoreCommas) == CardStandardWriterOptions.IgnoreCommas ? EncodeHelper.EncodeEscaped(value, _outlookEscapedCharacters) : EncodeHelper.EncodeEscaped(value, _standardEscapedCharacters);

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
                stringBuilder.Append(EncodeHelper.EncodeBase64((byte[])property.Value));
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
                        stringBuilder.Append(EncodeHelper.EncodeQuotedPrintable(cardValueCollection[index], enc));
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
                        stringBuilder.Append(EncodeHelper.EncodeQuotedPrintable(s, enc));
                        break;
                    default:
                        stringBuilder.Append(EncodeEscaped(s));
                        break;
                }
            }
        }
        return stringBuilder.ToString();
    }
}
