using iCloud.Dav.Core.Serialization;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace iCloud.Dav.Core;

/// <summary>
/// Interface for HTTP client services.
/// </summary>
public interface IClientService : IDisposable
{
    /// <summary>
    /// Gets the HTTP client which is used to create requests.
    /// </summary>
    ConfigurableHttpClient HttpClient { get; }

    /// <summary>
    /// Gets a HTTP client initializer which is able to custom properties on
    /// <see cref="ConfigurableHttpClient" /> and
    /// <see cref="ConfigurableMessageHandler" />.
    /// </summary>
    IConfigurableHttpClientCredentialInitializer HttpClientInitializer { get; }

    /// <summary>
    /// Gets the service name.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the BasePath of the service.
    /// </summary>
    string BasePath { get; }

    /// <summary>
    /// Gets the application name to be used in the User-Agent header.
    /// </summary>
    string ApplicationName { get; }

    /// <summary>
    /// Sets the content of the request by the given body and the this service's configuration.
    /// The body object is serialized by the Serializer
    /// </summary>
    void SetRequestSerializedContent(HttpRequestMessage request, object body);

    /// <summary>
    /// Gets the Serializer used by this service.
    /// </summary>
    ISerializer Serializer { get; }

    /// <summary>
    /// Serializes an object into a string representation.
    /// </summary>
    string SerializeObject(object data);

    /// <summary>
    /// Deserializes a response into the specified object.
    /// </summary>
    Task<T> DeserializeResponse<T>(HttpResponseMessage response);

    /// <summary>
    /// Deserializes an error response into a string.
    /// </summary>
    Task<string> DeserializeError(HttpResponseMessage response);
}