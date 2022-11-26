namespace iCloud.Dav.Calendar.CalDav.Types
{
    internal sealed class Privilege
    {
        public string Name { get; }

        public string NameSpace { get; }

        public Privilege(string name, string nameSpace)
        {
            Name = name;
            NameSpace = nameSpace;
        }
    }
}