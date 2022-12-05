namespace iCloud.Dav.People.DataTypes
{
    public enum RelatedNamesType
    {
        Father = 1,
        Mother = 2,
        Parent = 3,
        Brother = 4,
        Sister = 5,
        Child = 6,
        Friend = 7,
        Spouse = 8,
        Partner = 9,
        Assistant = 10,
        Manager = 11,
        Other = 12,
        Custom = 13,
    }

    internal enum RelatedNamesTypeInternal
    {
        /// <summary>Indicates an other related person type.</summary>
        Other = 1,
        /// <summary>Indicates a pref related person type.</summary>
        Pref = 2,
    }
}