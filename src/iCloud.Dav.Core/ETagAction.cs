namespace iCloud.Dav.Core;

/// <summary>
/// Defines the behaviour/header used for sending an etag along with a request.
/// </summary>
public enum ETagAction
{
    /// <summary>
    /// The default etag behaviour will be determined by the type of the request.
    /// </summary>
    Default,
    /// <summary>The ETag won't be added to the header of the request.</summary>
    Ignore,
    /// <summary>
    /// The ETag will be added as an "If-Match" header.
    /// A request sent with an "If-Match" header will only succeed if both ETags are identical.
    /// </summary>
    IfMatch,
    /// <summary>
    /// The ETag will be added as an "If-None-Match" header.
    /// A request sent with an "If-Match" header will only succeed if both ETags are not identical.
    /// </summary>
    IfNoneMatch,
}