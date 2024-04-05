using iCloud.Dav.Core;

namespace iCloud.Dav.People.DataTypes;

/// <summary>
/// Represents an identity card, which contains information about a contact or user.
/// </summary>
public class IdentityCard : IDirectResponseSchema
{
    /// <summary>
    /// The resource name of the identity card.
    /// </summary>
    public virtual string ResourceName { get; set; }

    /// <summary>
    /// The ETag of the collection.
    /// </summary>
    public virtual string ETag { get; set; }

    /// <summary>
    /// A token that can be sent as `sync_token` to retrieve changes since the last request.
    /// The request must set `request_sync_token` to return the sync token. When the response is paginated,
    /// only the last page will contain `nextSyncToken`.
    /// </summary>
    public virtual string NextSyncToken { get; set; }
}