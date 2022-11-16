namespace iCloud.Dav.Calendar.CalDav.Types;

internal sealed class Attribute
{
    public string Name { get; }

    public string Namespace { get; }

    public string Value { get; }

    public Attribute(string name, string nameSpace, string value) => (Name, Namespace, Value) = (name, nameSpace, value);
}
