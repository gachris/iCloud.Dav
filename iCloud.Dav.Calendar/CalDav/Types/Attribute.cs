namespace iCloud.Dav.Calendar.CalDav.Types;

internal sealed class Attribute
{
    public string Name { get; }

    public string Namespace { get; }

    public string Value { get; }

    public Attribute(string name, string ns, string value)
    {
        Name = name;
        Namespace = ns;
        Value = value;
    }
}
