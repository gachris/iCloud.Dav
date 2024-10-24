using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using iCloud.Dav.Core.Extensions;
using iCloud.Dav.Core.Logger;
using iCloud.Dav.Core.Utils;

namespace iCloud.Dav.Core.Request;

/// <summary>
/// Represents an abstract, strongly typed request base class to make requests to a service.
/// Supports a strongly typed response.
/// </summary>
/// <typeparam name="TResponse">The type of the response object</typeparam>
public abstract class ClientServiceRequest<TResponse> : IClientServiceRequest<TResponse>, IClientServiceRequest
{
    /// <summary>
    /// The class logger.
    /// </summary>
    private static readonly ILogger _logger = ApplicationContext.Logger.ForType<ClientServiceRequest<TResponse>>();

    private readonly Dictionary<string, IParameter> _requestParameters = new Dictionary<string, IParameter>();

    /// <summary>
    /// The service on which this request will be executed.
    /// </summary>
    private readonly IClientService _service;

    /// <summary>
    /// Defines whether the E-Tag will be used in a specified way or be ignored.
    /// </summary>
    public ETagAction ETagAction { get; set; }

    /// <inheritdoc/>
    public abstract string MethodName { get; }

    /// <inheritdoc/>
    public abstract string RestPath { get; }

    /// <inheritdoc/>
    public abstract string HttpMethod { get; }

    /// <inheritdoc/>
    public virtual string Depth { get; }

    /// <inheritdoc/>
    public virtual string ContentType { get; }

    /// <inheritdoc/>
    public IDictionary<string, IParameter> RequestParameters => _requestParameters;

    /// <inheritdoc/>
    public IClientService Service => _service;

    /// <summary>Creates a new service request.</summary>
    protected ClientServiceRequest(IClientService service)
    {
        _service = service;
        InitParameters();
    }

    /// <summary>
    /// Initializes request's parameters. Inherited classes MUST override this method to add parameters to the
    /// <see cref="RequestParameters" /> dictionary.
    /// </summary>
    protected virtual void InitParameters()
    {
    }

    /// <inheritdoc/>
    public TResponse Execute()
    {
        try
        {
            using var result = ExecuteUnparsedAsync(CancellationToken.None).Result;
            return ParseResponse(result).Result;
        }
        catch (AggregateException ex)
        {
            throw ex.InnerException ?? ex;
        }
        catch
        {
            throw;
        }
    }

    /// <inheritdoc/>
    public Stream ExecuteAsStream()
    {
        try
        {
            return ExecuteUnparsedAsync(CancellationToken.None).Result.Content.ReadAsStreamAsync().Result;
        }
        catch (AggregateException ex)
        {
            throw ex.InnerException ?? ex;
        }
        catch
        {
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<TResponse> ExecuteAsync() => await ExecuteAsync(CancellationToken.None).ConfigureAwait(false);

    /// <inheritdoc/>
    public async Task<TResponse> ExecuteAsync(CancellationToken cancellationToken)
    {
        TResponse response;
        using (var httpResponseMessage = await ExecuteUnparsedAsync(cancellationToken).ConfigureAwait(false))
        {
            cancellationToken.ThrowIfCancellationRequested();
            response = await ParseResponse(httpResponseMessage).ConfigureAwait(false);
        }
        return response;
    }

    /// <inheritdoc/>
    public async Task<Stream> ExecuteAsStreamAsync() => await ExecuteAsStreamAsync(CancellationToken.None).ConfigureAwait(false);

    /// <inheritdoc/>
    public async Task<Stream> ExecuteAsStreamAsync(CancellationToken cancellationToken)
    {
        var httpResponseMessage = await ExecuteUnparsedAsync(cancellationToken).ConfigureAwait(false);
        cancellationToken.ThrowIfCancellationRequested();
        return await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false);
    }

    /// <summary>Sync executes the request without parsing the result. </summary>
    private async Task<HttpResponseMessage> ExecuteUnparsedAsync(CancellationToken cancellationToken)
    {
        using var request = CreateRequest();
        return await _service.HttpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Parses the response and deserialize the content into the requested response object. </summary>
    private async Task<TResponse> ParseResponse(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return await _service.DeserializeResponse<TResponse>(response).ConfigureAwait(false);
        }
        var requestError = await _service.DeserializeError(response).ConfigureAwait(false);
        var errorResponse = new ErrorResponse(response.ReasonPhrase, response.StatusCode, response.RequestMessage?.RequestUri?.AbsoluteUri, requestError);
        throw new ICloudApiException(_service.Name, response.StatusCode, errorResponse.ToString());
    }

    /// <inheritdoc/>
    public HttpRequestMessage CreateRequest()
    {
        var request = CreateBuilder().CreateRequest();
        var body = GetBody();
        request.SetHttpRequestDepth(Depth);
        request.SetRequestSerializedContent(_service, body, ContentType);
        AddETag(request);
        return request;
    }

    /// <summary>
    /// Creates the <see cref="RequestBuilder" /> which is used to generate a request.
    /// </summary>
    /// <returns>
    /// A new builder instance which contains the HTTP method and the right Uri with its path and query parameters.
    /// </returns>
    private RequestBuilder CreateBuilder()
    {
        var requestBuilder = new RequestBuilder(new Uri(Service.BasePath), RestPath, HttpMethod);
        var parameterDictionary = ParameterUtils.CreateParameterDictionary(this);
        AddParameters(requestBuilder, ParameterCollection.FromDictionary(parameterDictionary));
        return requestBuilder;
    }

    ///<summary>Returns the body of the request.</summary>
    protected virtual object GetBody() => default;

    /// <summary>
    /// Adds the right ETag action (e.g. If-Match) header to the given HTTP request if the body contains ETag.
    /// </summary>
    private void AddETag(HttpRequestMessage request)
    {
        if (GetBody() is not IDirectResponseSchema body || string.IsNullOrEmpty(body.ETag)) return;
        var etag = body.ETag;
        var etagAction = ETagAction == ETagAction.Default ? GetDefaultETagAction(HttpMethod) : ETagAction;
        try
        {
            if (etagAction != ETagAction.IfMatch)
            {
                if (etagAction != ETagAction.IfNoneMatch) return;
                request.Headers.IfNoneMatch.Add(new EntityTagHeaderValue(etag));
            }
            else request.Headers.IfMatch.Add(new EntityTagHeaderValue(etag));
        }
        catch (FormatException ex)
        {
            _logger.Error(ex, "Can't set {0}. Etag is: {1}.", etagAction, etag);
        }
    }

    /// <summary>Returns the default ETagAction for a specific HTTP verb.</summary>
    public static ETagAction GetDefaultETagAction(string httpMethod)
    {
        return httpMethod is "GET"
            ? ETagAction.IfNoneMatch
            : httpMethod is "PUT" or "POST" or "PATCH" or "DELETE" ? ETagAction.IfMatch : ETagAction.Ignore;
    }

    /// <summary>Adds path and query parameters to the given <c>requestBuilder</c>.</summary>
    private void AddParameters(RequestBuilder requestBuilder, ParameterCollection inputParameters)
    {
        inputParameters.ForEach(inputParameter =>
        {
            if (!RequestParameters.TryGetValue(inputParameter.Key, out var parameter))
                throw new ICloudApiException(Service.Name, 0, string.Format("Invalid parameter \"{0}\" was specified", inputParameter.Key));

            var defaultValue = inputParameter.Value;

            if (!ParameterValidator.ValidateParameter(parameter, defaultValue))
                throw new ICloudApiException(Service.Name, 0, string.Format("Parameter validation failed for \"{0}\"", parameter.Name));

            defaultValue ??= parameter.DefaultValue;

            var parameterType = parameter.ParameterType;
            if (!(parameterType == "path"))
            {
                if (parameterType == "query")
                {
                    if (!Equals(defaultValue, parameter.DefaultValue) || parameter.IsRequired)
                        requestBuilder.AddParameter(RequestParameterType.Query, inputParameter.Key, defaultValue);
                }
                else throw new ICloudApiException(_service.Name, 
                                                  0,
                                                  string.Format("Unsupported parameter type \"{0}\" for \"{1}\"",
                                                                parameter.ParameterType,
                                                                parameter.Name));
            }
            else requestBuilder.AddParameter(RequestParameterType.Path, inputParameter.Key, defaultValue);
        });

        RequestParameters.Values.ForEach(parameter =>
        {
            if (parameter.IsRequired && !inputParameters.ContainsKey(parameter.Name))
                throw new ICloudApiException(_service.Name, 0, string.Format("Parameter \"{0}\" is missing", parameter.Name));
        });
    }
}