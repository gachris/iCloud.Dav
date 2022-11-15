using iCloud.Dav.Core.Logger;
using iCloud.Dav.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace iCloud.Dav.Core;

public class ConfigurableMessageHandler : DelegatingHandler
{
    /// <summary>The class logger.</summary>
    private static readonly ILogger _logger = ApplicationContext.Logger.ForType<ConfigurableMessageHandler>();

    /// <summary>The current API version of this client library.</summary>
    private static readonly string ApiVersion = Utilities.GetLibraryVersion();

    /// <summary>The User-Agent suffix header which contains the <see cref="ApiVersion" />.</summary>
    private static readonly string UserAgentSuffix = "icloud-api-dotnet-client/" + ApiVersion;

    private readonly object _unsuccessfulResponseHandlersLock = new();
    private readonly object _exceptionHandlersLock = new();
    private readonly object _executeInterceptorsLock = new();

    /// <summary>A list of <see cref="IHttpUnsuccessfulResponseHandler" />.</summary>
    private readonly IList<IHttpUnsuccessfulResponseHandler> _unsuccessfulResponseHandlers = new List<IHttpUnsuccessfulResponseHandler>();

    /// <summary>A list of <see cref="IHttpExceptionHandler" />.</summary>
    private readonly IList<IHttpExceptionHandler> _exceptionHandlers = new List<IHttpExceptionHandler>();

    /// <summary>A list of <see cref="IHttpExecuteInterceptor" />.</summary>
    private readonly IList<IHttpExecuteInterceptor> _executeInterceptors = new List<IHttpExecuteInterceptor>();

    /// <summary>Number of tries. Default is <c>3</c>.</summary>
    private int _numTries = 3;

    /// <summary>Number of redirects allowed. Default is <c>10</c>.</summary>
    private int _numRedirects = 10;

    /// <summary>Maximum allowed number of tries.</summary>
    public const int _MaxAllowedNumTries = 20;

    /// <summary>
    /// Gets a list of <see cref="IHttpUnsuccessfulResponseHandler" />s.
    /// <remarks>
    /// Since version 1.10, <see cref="AddUnsuccessfulResponseHandler(IHttpUnsuccessfulResponseHandler)" /> and
    /// <see cref="RemoveUnsuccessfulResponseHandler(IHttpUnsuccessfulResponseHandler)" /> were added in order to keep this class thread-safe.
    /// More information is available on
    /// <a href="https://github.com/ICloud/ICloud-api-dotnet-client/issues/592">#592</a>.
    /// </remarks>
    /// </summary>
    [Obsolete("Use AddUnsuccessfulResponseHandler or RemoveUnsuccessfulResponseHandler instead.")]
    public IList<IHttpUnsuccessfulResponseHandler> UnsuccessfulResponseHandlers => _unsuccessfulResponseHandlers;

    /// <summary>Adds the specified handler to the list of unsuccessful response handlers.</summary>
    public void AddUnsuccessfulResponseHandler(IHttpUnsuccessfulResponseHandler handler)
    {
        lock (_unsuccessfulResponseHandlersLock)
            _unsuccessfulResponseHandlers.Add(handler);
    }

    /// <summary>Removes the specified handler from the list of unsuccessful response handlers.</summary>
    public void RemoveUnsuccessfulResponseHandler(IHttpUnsuccessfulResponseHandler handler)
    {
        lock (_unsuccessfulResponseHandlersLock)
            _unsuccessfulResponseHandlers.Remove(handler);
    }

    /// <summary>
    /// Gets a list of <see cref="IHttpExceptionHandler" />s.
    /// <remarks>
    /// Since version 1.10, <see cref="AddExceptionHandler(IHttpExceptionHandler)" /> and <see cref="RemoveExceptionHandler(IHttpExceptionHandler)" /> were added
    /// in order to keep this class thread-safe.  More information is available on
    /// <a href="https://github.com/ICloud/iCloud-api-dotnet-client/issues/592">#592</a>.
    /// </remarks>
    /// </summary>
    [Obsolete("Use AddExceptionHandler or RemoveExceptionHandler instead.")]
    public IList<IHttpExceptionHandler> ExceptionHandlers => _exceptionHandlers;

    /// <summary>Adds the specified handler to the list of exception handlers.</summary>
    public void AddExceptionHandler(IHttpExceptionHandler handler)
    {
        lock (_exceptionHandlersLock)
            _exceptionHandlers.Add(handler);
    }

    /// <summary>Removes the specified handler from the list of exception handlers.</summary>
    public void RemoveExceptionHandler(IHttpExceptionHandler handler)
    {
        lock (_exceptionHandlersLock)
            _exceptionHandlers.Remove(handler);
    }

    /// <summary>
    /// Gets a list of <see cref="IHttpExecuteInterceptor" />s.
    /// <remarks>
    /// Since version 1.10, <see cref="AddExecuteInterceptor(IHttpExecuteInterceptor)" /> and <see cref="RemoveExecuteInterceptor(IHttpExecuteInterceptor)" /> were
    /// added in order to keep this class thread-safe.  More information is available on
    /// <a href="https://github.com/ICloud/ICloud-api-dotnet-client/issues/592">#592</a>.
    /// </remarks>
    /// </summary>
    [Obsolete("Use AddExecuteInterceptor or RemoveExecuteInterceptor instead.")]
    public IList<IHttpExecuteInterceptor> ExecuteInterceptors => _executeInterceptors;

    /// <summary>Adds the specified interceptor to the list of execute interceptors.</summary>
    public void AddExecuteInterceptor(IHttpExecuteInterceptor interceptor)
    {
        lock (_executeInterceptorsLock)
            _executeInterceptors.Add(interceptor);
    }

    /// <summary>Removes the specified interceptor from the list of execute interceptors.</summary>
    public void RemoveExecuteInterceptor(IHttpExecuteInterceptor interceptor)
    {
        lock (_executeInterceptorsLock)
            _executeInterceptors.Remove(interceptor);
    }

    /// <summary>
    /// Gets or sets the number of tries that will be allowed to execute. Retries occur as a result of either
    /// <see cref="IHttpUnsuccessfulResponseHandler" /> or <see cref="IHttpExceptionHandler" /> which handles the
    /// abnormal HTTP response or exception before being terminated.
    /// Set <c>1</c> for not retrying requests. The default value is <c>3</c>.
    /// <remarks>
    /// The number of allowed redirects (3xx) is defined by <see cref="NumRedirects" />. This property defines
    /// only the allowed tries for &gt;=400 responses, or when an exception is thrown. For example if you set
    /// <see cref="NumTries" /> to 1 and <see cref="NumRedirects" /> to 5, the library will send up to five redirect
    /// requests, but will not send any retry requests due to an error HTTP status code.
    /// </remarks>
    /// </summary>
    public int NumTries
    {
        get => _numTries;
        set
        {
            if (value > 20 || value < 1)
                throw new ArgumentOutOfRangeException(nameof(NumTries));
            _numTries = value;
        }
    }

    /// <summary>
    /// Gets or sets the number of redirects that will be allowed to execute. The default value is <c>10</c>.
    /// See <see cref="P:ICloud.Api.Http.ConfigurableMessageHandler.NumTries" /> for more information.
    /// </summary>
    public int NumRedirects
    {
        get => _numRedirects;
        set
        {
            if (value > 20 || value < 1)
                throw new ArgumentOutOfRangeException(nameof(NumRedirects));
            _numRedirects = value;
        }
    }

    /// <summary>
    /// Gets or sets whether the handler should follow a redirect when a redirect response is received. Default
    /// value is <c>true</c>.
    /// </summary>
    public bool FollowRedirect { get; set; }

    /// <summary>Gets or sets whether logging is enabled. Default value is <c>true</c>.</summary>
    public bool IsLoggingEnabled { get; set; }

    /// <summary>Gets or sets the application name which will be used on the User-Agent header.</summary>
    public string? ApplicationName { get; set; }

    public static ILogger Logger => _logger;

    /// <summary>Constructs a new configurable message handler.</summary>
    public ConfigurableMessageHandler(HttpMessageHandler httpMessageHandler) : base(httpMessageHandler)
    {
        FollowRedirect = true;
        IsLoggingEnabled = true;
    }

    /// <summary>
    /// The main logic of sending a request to the server. This send method adds the User-Agent header to a request
    /// with <see cref="ApplicationName" /> and the library version. It also calls interceptors before each attempt,
    /// and unsuccessful response handler or exception handlers when abnormal response or exception occurred.
    /// </summary>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Exception? lastException;
        var response = default(HttpResponseMessage);
        var loggable = IsLoggingEnabled && Logger.IsDebugEnabled;
        var triesRemaining = NumTries;
        var redirectRemaining = NumRedirects;
        request.Headers.Add("User-Agent", (ApplicationName == null ? "" : ApplicationName + " ") + UserAgentSuffix);
        do
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (response != null)
            {
                response.Dispose();
                response = null;
            }
            lastException = null;
            IEnumerable<IHttpExecuteInterceptor> list1;
            lock (_executeInterceptorsLock)
                list1 = _executeInterceptors.ToList();
            foreach (IHttpExecuteInterceptor executeInterceptor in list1)
            {
                await executeInterceptor.InterceptAsync(request, cancellationToken).ConfigureAwait(false);
            }
            try
            {
                response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                lastException = ex;
            }
            if (response == null || response.StatusCode >= HttpStatusCode.BadRequest || response.StatusCode < HttpStatusCode.OK)
                --triesRemaining;
            bool flag;
            if (response == null)
            {
                var flag1 = false;
                IEnumerable<IHttpExceptionHandler> list2;
                lock (_exceptionHandlersLock)
                    list2 = _exceptionHandlers.ToList();
                foreach (IHttpExceptionHandler exceptionHandler in list2)
                {
                    flag = flag1;
                    var num = await exceptionHandler.HandleExceptionAsync(new HandleExceptionArgs(request, lastException)
                    {
                        TotalTries = NumTries,
                        CurrentFailedTry = NumTries - triesRemaining,
                        CancellationToken = cancellationToken
                    }).ConfigureAwait(false) ? 1 : 0;
                    flag1 = flag | num != 0;
                }
                if (!flag1)
                {
                    Logger.Error(lastException, "Exception was thrown while executing a HTTP request and it wasn't handled");
                    throw lastException;
                }
                if (loggable)
                    Logger.Debug("Exception {0} was thrown, but it was handled by an exception handler", lastException?.Message);
            }
            else if (response.IsSuccessStatusCode)
            {
                triesRemaining = 0;
            }
            else
            {
                var flag1 = false;
                IEnumerable<IHttpUnsuccessfulResponseHandler> list2;
                lock (_unsuccessfulResponseHandlersLock)
                    list2 = _unsuccessfulResponseHandlers.ToList();
                foreach (IHttpUnsuccessfulResponseHandler unsuccessfulResponseHandler in list2)
                {
                    flag = flag1;
                    int num = await unsuccessfulResponseHandler.HandleResponseAsync(new HandleUnsuccessfulResponseArgs(request, response)
                    {
                        TotalTries = NumTries,
                        CurrentFailedTry = NumTries - triesRemaining,
                        CancellationToken = cancellationToken
                    }).ConfigureAwait(false) ? 1 : 0;
                    flag1 = flag | num != 0;
                }
                if (!flag1)
                {
                    if (FollowRedirect && HandleRedirect(response))
                    {
                        if (redirectRemaining-- == 0)
                            triesRemaining = 0;
                        if (loggable)
                            Logger.Debug("Redirect response was handled successfully. Redirect to {0}", response.Headers.Location);
                    }
                    else
                    {
                        if (loggable)
                            Logger.Debug("An abnormal response wasn't handled. Status code is {0}", response.StatusCode);
                        triesRemaining = 0;
                    }
                }
                else if (loggable)
                    Logger.Debug("An abnormal response was handled by an unsuccessful response handler. Status Code is {0}", response.StatusCode);
            }
        }
        while (triesRemaining > 0);
        if (response == null)
        {
            Logger.Error(lastException, "Exception was thrown while executing a HTTP request");
            throw lastException;
        }
        if (!response.IsSuccessStatusCode)
            Logger.Debug("Abnormal response is being returned. Status Code is {0}", response.StatusCode);
        return response;
    }

    /// <summary>
    /// Handles redirect if the response's status code is redirect, redirects are turned on, and the header has
    /// a location.
    /// When the status code is <c>303</c> the method on the request is changed to a GET as per the RFC2616
    /// specification. On a redirect, it also removes the <c>Authorization</c> and all <c>If-*</c> request headers.
    /// </summary>
    /// <returns> Whether this method changed the request and handled redirect successfully. </returns>
    private static bool HandleRedirect(HttpResponseMessage message)
    {
        var location = message.Headers.Location;
        if (!message.IsRedirectStatusCode() || location == null)
            return false;
        var requestMessage = message.RequestMessage.ThrowIfNull(nameof(message.RequestMessage));
        requestMessage.RequestUri = new Uri(requestMessage.RequestUri, location);
        if (message.StatusCode == HttpStatusCode.RedirectMethod)
            requestMessage.Method = HttpMethod.Get;
        requestMessage.Headers.Remove("Authorization");
        requestMessage.Headers.IfMatch.Clear();
        requestMessage.Headers.IfNoneMatch.Clear();
        requestMessage.Headers.IfModifiedSince = new DateTimeOffset?();
        requestMessage.Headers.IfUnmodifiedSince = new DateTimeOffset?();
        requestMessage.Headers.Remove("If-Range");
        return true;
    }
}
