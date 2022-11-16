namespace iCloud.Dav.Calendar.CalDav.Types;

internal sealed class TimeRange
{
    public string? Start { get; }

    public string? End { get; }

    public TimeRange(string? start, string? end) => (Start, End) = (start, end);
}
