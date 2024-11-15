namespace iCloud.Dav.Core.Request;

/// <summary>
/// Represents a client service request which supports both synchronous and asynchronous execution to get the stream.
/// </summary>
public interface IClientServiceRequest
{
    /// <summary>
    /// Gets the name of the method to which this request belongs.
    /// </summary>
    string MethodName { get; }

    /// <summary>
    /// Gets the REST path of this request.
    /// </summary>
    string RestPath { get; }

    /// <summary>
    /// Gets the HTTP method of this request.
    /// </summary>
    string HttpMethod { get; }

    /// <summary>
    /// Gets the Content-Type header of this request.
    /// </summary>
    string ContentType { get; }

    /// <summary>
    /// Gets the Depth header of this request.
    /// </summary>
    string Depth { get; }

    /// <summary>
    /// Gets the parameters information for this specific request.
    /// </summary>
    IDictionary<string, IParameter> RequestParameters { get; }

    /// <summary>
    /// Gets the service which is related to this request.
    /// </summary>
    IClientService Service { get; }

    /// <summary>
    /// Creates an HTTP request message with all path and query parameters, ETag, etc.
    /// </summary>
    HttpRequestMessage CreateRequest();

    /// <summary>
    /// Executes the request asynchronously and returns the result stream.
    /// </summary>
    /// <returns>A task representing the asynchronous operation that returns the result stream.</returns>
    Task<Stream> ExecuteAsStreamAsync();

    /// <summary>
    /// Executes the request asynchronously and returns the result stream.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation that returns the result stream.</returns>
    Task<Stream> ExecuteAsStreamAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Executes the request and returns the result stream.
    /// </summary>
    /// <returns>The result stream.</returns>
    Stream ExecuteAsStream();
}

/// <summary>
/// Represents a client service request which inherits from <see cref="IClientServiceRequest"/> and represents a specific
/// service request with the given response type. It supports both synchronous and asynchronous execution to get the response.
/// </summary>
/// <typeparam name="TResponse">The type of the response object.</typeparam>
public interface IClientServiceRequest<TResponse> : IClientServiceRequest
{
    /// <summary>
    /// Executes the request asynchronously and returns the result object.
    /// </summary>
    /// <returns>A task representing the asynchronous operation that returns the result object.</returns>
    Task<TResponse> ExecuteAsync();

    /// <summary>
    /// Executes the request asynchronously and returns the result object.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation that returns the result object.</returns>
    Task<TResponse> ExecuteAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Executes the request and returns the result object.
    /// </summary>
    /// <returns>The result object.</returns>
    TResponse Execute();
}