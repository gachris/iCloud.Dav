using System.ComponentModel;
using iCloud.Dav.Calendar.Serialization.Converters;
using iCloud.Dav.Calendar.WebDav.DataTypes;
using iCloud.Dav.Core;
using iCloud.Dav.Core.Serialization;

namespace iCloud.Dav.Calendar.DataTypes;

/// <summary>
/// Represents a synchronization token used to retrieve only the entries that have changed since a previous sync.
/// </summary>
[XmlDeserializeType(typeof(MultiStatus))]
[TypeConverter(typeof(SyncTokenConverter))]
public class SyncToken : IDirectResponseSchema
{
    /// <inheritdoc/>
    public string ETag { get; set; }

    /// <summary>
    /// Token used to retrieve only the entries that have changed since this result was returned.
    /// </summary>
    public virtual string NextSyncToken { get; set; }
}