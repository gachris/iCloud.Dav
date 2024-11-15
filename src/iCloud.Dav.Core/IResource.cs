namespace iCloud.Dav.Core;

/// <summary>
/// Interface for an object representing a URL path segment, which typically contains an identifier.
/// </summary>
public interface IResource
{
    /// <summary>
    /// Gets or sets the URL of the resource.
    /// </summary>
    string Href { get; set; }
}