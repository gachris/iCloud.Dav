namespace iCloud.Dav.Calendar.CalDav.Types
{
    internal sealed class ResourceType
    {
        public string Name { get; }

        public string NameSpace { get; }

        public ResourceType(string name, string nameSpace)
        {
            Name = name;
            NameSpace = nameSpace;
        }
    }
}