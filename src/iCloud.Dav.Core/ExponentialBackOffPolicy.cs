namespace iCloud.Dav.Core;

/// <summary>
/// An enumeration that represents the exponential back-off policy for retries.
/// </summary>
[Flags]
public enum ExponentialBackOffPolicy
{
    /// <summary>
    /// Exponential back-off is disabled.
    /// </summary>
    None = 0,
    /// <summary>
    /// Exponential back-off is enabled only for exceptions.
    /// </summary>
    Exception = 1,
    /// <summary>
    /// Exponential back-off is enabled only for 503 HTTP Status code.
    /// </summary>
    UnsuccessfulResponse503 = 2,
}