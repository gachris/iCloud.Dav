using System.Collections.Generic;
using System.Xml.Serialization;

namespace iCloud.Dav.Core.WebDav.Cal
{
    /// <summary>
    /// Represents a filter used in constructing XML representations of filters in the CalDAV namespace.
    /// </summary>
    public interface IFilter : IXmlSerializable
    {
        /// <summary>
        /// Gets the collection of child filter elements.
        /// </summary>
        IEnumerable<IFilter> Children { get; }
    }
}