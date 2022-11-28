using System;

namespace iCloud.Dav.People.DataTypes
{
    public enum WebsiteType
    {
        /// <summary>Indicates a homepage web site type.</summary>
        HomePage = 1,
        /// <summary>Indicates a home web site type.</summary>
        Home = 2,
        /// <summary>Indicates a work web site type.</summary>
        Work = 3,
        /// <summary>Indicates a school web site type.</summary>
        School = 4,
        /// <summary>Indicates a blog web site type.</summary>
        Blog = 5,
        /// <summary>Indicates an other web site type.</summary>
        Other = 6,
        /// <summary>Indicates an custom web site type.</summary>
        Custom = 7,
    }

    /// <summary>The type of a web site.</summary>
    [Flags]
    public enum WebsiteTypeInternal
    {
        /// <summary>Indicates a home web site type.</summary>
        Home = 1,
        /// <summary>Indicates a work web site type.</summary>
        Work = 2,
        /// <summary>Indicates an other web site type.</summary>
        Other = 4,
        /// <summary>Indicates an pref web site type.</summary>
        Pref = 8,
    }
}