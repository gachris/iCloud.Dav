namespace iCloud.Dav.Core;

/// <summary>
/// Interface for an object representing a URL path segment, which typically contains an identifier.
/// </summary>
public interface IUrlPath
{
    /// <summary>
    /// Gets or sets the identifier of the URL path segment.
    /// </summary>
    string Id { get; set; }
}