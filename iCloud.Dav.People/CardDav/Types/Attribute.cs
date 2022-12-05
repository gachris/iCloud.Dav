namespace iCloud.Dav.People.CardDav.Types
{
    internal sealed class Attribute
    {
        public string Name { get; }

        public string Namespace { get; }

        public string Value { get; }

        public Attribute InnerAttribute { get; }

        public Attribute(string name, string ns, string value)
        {
            Name = name;
            Namespace = ns;
            Value = value;
        }

        public Attribute(string name, string ns, string value, Attribute innerAttribute) : this(name, ns, value) => InnerAttribute = innerAttribute;
    }
}