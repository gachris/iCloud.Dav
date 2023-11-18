namespace iCloud.Dav.Core;

/// <summary>
/// Interface for defining a response object that includes an e-tag.
/// </summary>
public interface IDirectResponseSchema
{
    /// <summary>
    /// The e-tag of this response.
    /// </summary>
    /// <remarks>
    /// Will be set by the service deserialization method,
    /// or the by xml response parser if implemented on service.
    /// </remarks>
    string ETag { get; set; }
}