using System;

namespace iCloud.Dav.People.Utils
{
    /// <summary>
    ///     Extended options for the <see cref="CardStandardWriter" /> class.
    /// </summary>
    [Flags]
    internal enum CardStandardWriterOptions
    {
        /// <summary>No options.</summary>
        None = 0,
        /// <summary>
        ///     Indicates whether or not commas should be escaped in values.
        /// </summary>
        /// <remarks>
        ///     The Person specification requires that commas be escaped
        ///     in values (e.g. a "," is translated to "\,").  However, Microsoft
        ///     Outlook(tm) does not properly decode these escaped commas.  This
        ///     option instruct the writer to ignored (not translate) embedded
        ///     commas for better compatibility with Outlook.
        /// </remarks>
        IgnoreCommas = 1,
    }
}
