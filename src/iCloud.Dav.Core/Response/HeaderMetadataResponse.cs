namespace iCloud.Dav.Core.Response;

/// <summary>
/// Represents a void response that returns etag.
/// </summary>
public class HeaderMetadataResponse : IDirectResponseSchema
{
    /// <inheritdoc/>
    public string ETag { get; set; }
}