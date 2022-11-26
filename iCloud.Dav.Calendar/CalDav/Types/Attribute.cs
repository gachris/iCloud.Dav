namespace iCloud.Dav.Calendar.CalDav.Types
{
    internal sealed class Attribute
    {
        public string Name { get; }

        public string NameSpace { get; }

        public string Value { get; }

        public Attribute(string name, string nameSpace, string value)
        {
            Name = name;
            NameSpace = nameSpace;
            Value = value;
        }
    }
}