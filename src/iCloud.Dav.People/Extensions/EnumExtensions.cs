namespace iCloud.Dav.People.Extensions;

internal static class EnumExtensions
{
    public static bool HasFlags<T>(this T value, T flags) where T : struct
    {
        if (!typeof(T).IsEnum)
            throw new ArgumentException();

        var longFlags = Convert.ToInt64(flags);
        return (Convert.ToInt64(value) & longFlags) == longFlags;
    }

    public static T AddFlags<T>(this T value, T flags) where T : struct
    {
        if (!typeof(T).IsEnum)
            throw new ArgumentException();

        var longFlags = Convert.ToInt64(flags);
        var longValue = Convert.ToInt64(value);
        longValue |= longFlags;
        value = (T)Enum.ToObject(typeof(T), longValue);
        return value;
    }

    public static T RemoveFlags<T>(this T value, T flags) where T : struct
    {
        if (!typeof(T).IsEnum)
            throw new ArgumentException();

        var longFlags = Convert.ToInt64(flags);
        var longValue = Convert.ToInt64(value);
        longValue &= ~longFlags;
        value = (T)Enum.ToObject(typeof(T), longValue);
        return value;
    }

    public static string[] StringArrayFlags<T>(this T value) where T : struct
    {
        return !typeof(T).IsEnum
            ? throw new ArgumentException()
            : (value.ToString()?.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).ToArray());
    }
}