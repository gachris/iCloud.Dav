using System.Net;
using iCloud.Dav.Core.Extensions;

namespace iCloud.Dav.Core;

/// <summary>
/// Represents an exception thrown by the iCloud API Service.
/// </summary>
public class ICloudApiException : Exception
{
    /// <summary>
    /// Gets the name of the service related to this exception.
    /// </summary>
    public string ServiceName { get; }

    /// <summary>
    /// Gets the HTTP status code related to this exception.
    /// </summary>
    public HttpStatusCode HttpStatusCode { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ICloudApiException"/> class with the specified service name, message, and inner exception.
    /// </summary>
    /// <param name="serviceName">The name of the service that caused the exception.</param>
    /// <param name="httpStatusCode">The HTTP status code related to this exception.</param>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="inner">The exception that is the cause of the current exception.</param>
    public ICloudApiException(string serviceName, HttpStatusCode httpStatusCode, string message, Exception inner) : base(message, inner)
    {
        ServiceName = serviceName.ThrowIfNullOrEmpty(nameof(serviceName));
        HttpStatusCode = httpStatusCode;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ICloudApiException"/> class with the specified service name, message, and HTTP status code.
    /// </summary>
    /// <param name="serviceName">The name of the service that caused the exception.</param>
    /// <param name="httpStatusCode">The HTTP status code related to this exception.</param>
    /// <param name="message">The message that describes the error.</param>
    public ICloudApiException(string serviceName, HttpStatusCode httpStatusCode, string message) : this(serviceName, httpStatusCode, message, null)
    {
    }

    /// <summary>
    /// Returns a string that represents the current exception.
    /// </summary>
    /// <returns>A string that represents the current exception, including the service name and the base exception message.</returns>
    public override string ToString() => string.Format("The service {1} has thrown an exception: {0}", base.ToString(), ServiceName);
}