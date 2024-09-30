namespace iCloud.Dav.Calendar.Extensions;

internal static class DateTimeExtensions
{
    public static string ToFilterTime(this DateTime dateTime)
    {
        var universalTime = dateTime.ToUniversalTime();
        return universalTime.ToString("yyyyMMddTHHmmssZ");
    }

    public static string ToFilterTime(this DateTime? dateTime)
    {
        return !dateTime.HasValue ? default : dateTime.Value.ToFilterTime();
    }
}