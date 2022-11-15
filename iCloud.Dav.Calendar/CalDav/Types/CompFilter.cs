namespace iCloud.Dav.Calendar.CalDav.Types;

internal sealed class CompFilter
{
    public string Name { get; }

    public TimeRange? TimeRange { get; set; }

    public CompFilter? Child { get; set; }

    public CompFilter(string name) => Name = name;
}
