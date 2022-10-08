using iCloud.Dav.People.Serialization;
using System;
using System.Text;

namespace iCloud.Dav.People.Utils;

internal class DecodeHelper
{
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
