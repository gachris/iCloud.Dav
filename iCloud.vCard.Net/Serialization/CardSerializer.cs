using iCloud.vCard.Net.Data;
using iCloud.vCard.Net.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace iCloud.vCard.Net.Serialization;

/// <summary>
/// Serializer for vCard.
/// </summary>
public class CardSerializer
{
    #region Fields/Consts

    private static readonly string _basis_64 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/???????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????";

    /// <summary>
    ///     The characters that are escaped per the original
    ///     vCard specification.
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

    /// <summary>
    ///    A default serializer of the <see cref="CardSerializer"/>.
    /// </summary>
    public static readonly CardSerializer Default = new();

    #endregion

    #region Properties

    /// <summary>
    ///     Extended options for the vCard writer.
    /// </summary>
    public CardStandardWriterOptions Options { get; set; }

    #endregion

    #region Methods

    /// <summary>
    ///     Converts a byte to a BASE64 string.
    /// </summary>
    public static string EncodeBase64(byte value) => Convert.ToBase64String(new byte[1] { value });

    /// <summary>
    ///     Converts a byte array to a BASE64 string.
    /// </summary>
    public static string EncodeBase64(byte[] value)
    {
        var bytes = EncodeB64(value);
        return Encoding.ASCII.GetString(bytes, 0, bytes.Length);
    }

    /// <summary>
    ///     Converts an integer to a BASE64 string.
    /// </summary>
    public static string EncodeBase64(int value) => Convert.ToBase64String(new byte[4] { (byte)value, (byte)(value >> 8), (byte)(value >> 16), (byte)(value >> 24) });

    /// <summary>
    ///     Encodes a string using simple escape codes.
    /// </summary>
    public string EncodeEscaped(string value) => (Options & CardStandardWriterOptions.IgnoreCommas) == CardStandardWriterOptions.IgnoreCommas ? EncodeEscaped(value, _outlookEscapedCharacters) : EncodeEscaped(value, _standardEscapedCharacters);

    /// <summary>
    ///     Encodes a character array using simple escape sequences.
    /// </summary>
    public static string EncodeEscaped(string value, char[] escaped)
    {
        if (escaped == null) throw new ArgumentNullException(nameof(escaped));
        if (string.IsNullOrEmpty(value)) return value;
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

    /// <summary>
    ///     Converts a string to quoted-printable format.
    /// </summary>
    /// <param name="value">
    ///     The value to encode in Quoted Printable format.
    /// </param>
    /// <param name="enc">
    ///     The encoding in Quoted Printable format.
    /// </param>
    /// <returns>The value encoded in Quoted Printable format.</returns>
    public static string EncodeQuotedPrintable(string value, Encoding enc)
    {
        if (string.IsNullOrEmpty(value)) return value;

        var stringBuilder = new StringBuilder();

        foreach (var c in value)
        {
            if (c is (char)9 or >= (char)32 and <= (char)60 or >= (char)62 and <= (char)126)
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
    ///     Returns property encoded into a standard vCard NAME:VALUE format.
    /// </summary>
    public string EncodeProperty(CardProperty property) => EncodeProperty(property, null);

    /// <summary>
    ///     Returns property encoded into a standard vCard NAME:VALUE format.
    /// </summary>
    public string EncodeProperty(CardProperty property, string? charsetName)
    {
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
                var cardValueCollection = ((ValueCollection)property.Value).ThrowIfNull(nameof(property.Value));
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
                        stringBuilder.Append(EncodeQuotedPrintable(cardValueCollection[index] ?? string.Empty, enc));
                    else
                        stringBuilder.Append(EncodeEscaped(cardValueCollection[index] ?? string.Empty));
                    if (index < cardValueCollection.Count - 1)
                        stringBuilder.Append(cardValueCollection.Separator);
                }
            }
            else
            {
                var s = (type != typeof(char[]) ? property.Value.ToString() : new string((char[])property.Value)) ?? string.Empty;
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
    public virtual void Serialize<T>(T card, TextWriter output, string? charsetName = null) where T : Card
    {
        var properties = new List<CardProperty>
        {
            new CardProperty(Constants.Card.BEGIN, Constants.Card.vCard_Type),
            new CardProperty(Constants.Card.VERSION, Constants.Card.vCard_Version)
        };

        properties.AddRange(card.Properties);
        properties.Add(new CardProperty(Constants.Card.END, Constants.Card.vCard_Type));

        var groups = properties.GroupBy(x => x.Group).Where(x => x.Count() > 1 && x.Key != null);
        var current = 0;
        foreach (var group in groups)
        {
            current++;
            group.ForEach(x => x.Name = string.Concat($"item{current}.", x.Name));
        }

        foreach (var property in properties)
            output.WriteLine(EncodeProperty(property, charsetName));
    }

    public virtual string SerializeToString<T>(T card, string charsetName) where T : Card
    {
        using var stream = new MemoryStream();
        using var textWriter = new StreamWriter(stream);
        Serialize(card, textWriter, charsetName);
        textWriter.Flush();
        stream.Seek(0, SeekOrigin.Begin);
        using var streamReader = new StreamReader(stream);
        return streamReader.ReadToEnd();
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
        if (lineLength <= 0 || insertCount % lineLength != 0) return;
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

    #endregion
}
