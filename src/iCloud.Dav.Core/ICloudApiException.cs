using System.Runtime.Serialization;
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
    /// Gets the error response associated with this exception.
    /// </summary>
    public ErrorResponse ErrorResponse { get; }

    /// <summary>
    /// Gets the HTTP response message associated with this exception.
    /// </summary>
    public HttpResponseMessage ResponseMessage { get; }

    /// <summary>
    /// Constructs a new instance of <see cref="ICloudApiException"/> with a service name and message.
    /// </summary>
    public ICloudApiException(string serviceName, string message) : base(message)
    {
        ServiceName = serviceName.ThrowIfNullOrEmpty(nameof(serviceName));
    }

    /// <summary>
    /// Constructs a new instance of <see cref="ICloudApiException"/> with a service name, message, and error response.
    /// </summary>
    public ICloudApiException(string serviceName, string message, ErrorResponse errorResponse) : base(message)
    {
        ServiceName = serviceName.ThrowIfNullOrEmpty(nameof(serviceName));
        ErrorResponse = errorResponse;
    }

    /// <summary>
    /// Constructs a new instance of <see cref="ICloudApiException"/> with a service name, message, error response, and response message.
    /// </summary>
    public ICloudApiException(string serviceName, string message, ErrorResponse errorResponse, HttpResponseMessage responseMessage) : base(message)
    {
        ServiceName = serviceName.ThrowIfNullOrEmpty(nameof(serviceName));
        ErrorResponse = errorResponse;
        ResponseMessage = responseMessage;
    }

    /// <summary>
    /// Constructs a new instance of <see cref="ICloudApiException"/> with a service name, message, and inner exception.
    /// </summary>
    public ICloudApiException(string serviceName, string message, Exception inner) : base(message, inner)
    {
        ServiceName = serviceName.ThrowIfNullOrEmpty(nameof(serviceName));
    }

    /// <summary>
    /// Constructs a new instance of <see cref="ICloudApiException"/> for serialization purposes.
    /// </summary>
    protected ICloudApiException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        ServiceName = info.GetString(nameof(ServiceName));
        ErrorResponse = (ErrorResponse)info.GetValue(nameof(ErrorResponse), typeof(ErrorResponse));
    }

    /// <inheritdoc/>
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(ServiceName), ServiceName);
        info.AddValue(nameof(ErrorResponse), ErrorResponse);
    }

    /// <inheritdoc/>
    public override string ToString() => $"The service {ServiceName} has thrown an exception: {base.ToString()}";
}