using Ical.Net.CalendarComponents;
using iCloud.Dav.Core;

namespace iCloud.Dav.Calendar.DataTypes;

/// <summary>
/// Represents an item in a sync collection.
/// </summary>
public class SyncCollectionItem : IDirectResponseSchema
{
    /// <summary>
    /// A value that uniquely identifies the component. It is used for requests and in most cases has the same value as the <seealso cref="UniqueComponent.Uid"/>.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// ETag of the collection.
    /// </summary>
    public virtual string ETag { get; set; }

    /// <summary>
    /// Whether this object has been deleted from the list. Read-only.
    /// The default is null.
    /// </summary>
    public virtual bool? Deleted { get; set; }
}