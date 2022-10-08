using System;
using System.Text;

namespace iCloud.Dav.People.Utils;

internal class EncodeHelper
{
    private static readonly string _basis_64 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/???????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????";

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
