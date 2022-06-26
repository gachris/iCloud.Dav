using System;

namespace iCloud.Dav.People
{
    /// <summary>The type or classification of a web site.</summary>
    /// <remarks>
    ///     <para>
    ///         The Microsoft Outlook contact editor has a field for
    ///         entering a web site.  The default classification of
    ///         this web site is work-related.  A personal web site
    ///         can be viewed or entered through the All Fields tab.
    ///     </para>
    /// </remarks>
    [Flags]
    public enum WebsiteTypes
    {
        /// <summary>No web site designation.</summary>
        Default = 0,
        /// <summary>A personal home page.</summary>
        Personal = 1,
        /// <summary>A work-related web site.</summary>
        Work = 2,
    }
}
