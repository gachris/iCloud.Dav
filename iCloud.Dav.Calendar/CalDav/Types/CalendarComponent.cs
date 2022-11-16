namespace iCloud.Dav.Calendar.CalDav.Types;

internal sealed class CalendarComponent
{
    public string Name { get; }

    public string NameSpace { get; }

    public CalendarComponent(string name, string nameSpace) => (Name, NameSpace) = (name, nameSpace);
}
