namespace iCloud.Dav.Calendar.CalDav.Types;

internal sealed class SupportedReport
{
    public string Name { get; }

    public string NameSpace { get; }

    public SupportedReport(string name, string nameSpace) => (Name, NameSpace) = (name, nameSpace);
}
