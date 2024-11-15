internal static class Base64Helper
{
    public static bool TryFromBase64String(this string base64String, out byte[] bytes)
    {
        bytes = Array.Empty<byte>();
        var isNotBase64 = string.IsNullOrEmpty(base64String)
            || base64String.Length % 4 != 0
            || base64String.Contains(" ")
            || base64String.Contains("\t")
            || base64String.Contains("\r")
            || base64String.Contains("\n");

        if (isNotBase64)
            return false;

        try
        {
            bytes = Convert.FromBase64String(base64String);
            return true;
        }
        catch
        {
        }

        return false;
    }
}