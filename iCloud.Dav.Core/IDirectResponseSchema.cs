namespace iCloud.Dav.Core;

public interface IDirectResponseSchema
{
    /// <summary>The e-tag of this response.</summary>
    /// <remarks>
    /// Will be set by the service deserialization method,
    /// or the by json response parser if implemented on service.
    /// </remarks>
    string? ETag { get; set; }
}
