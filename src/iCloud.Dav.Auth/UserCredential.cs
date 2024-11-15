using System.Net;
using System.Net.Http.Headers;
using iCloud.Dav.Core;
using iCloud.Dav.Core.Logger;

namespace iCloud.Dav.Auth;

/// <summary>
/// User Credentials.
/// </summary>
public class UserCredential : ICredential, IConfigurableHttpClientCredentialInitializer, ITokenAccess, IHttpExecuteInterceptor, IHttpUnsuccessfulResponseHandler
{
    /// <summary>Gets Logger.</summary>
    private static readonly ILogger _logger = ApplicationContext.Logger.ForType<UserCredential>();

    private readonly object _lockObject = new object();
    private readonly IAuthorizationCodeFlow _flow;
    private readonly string _userId;
    private Token _token;

    /// <summary>Gets or sets the token response which contains the access token.</summary>
    public Token Token
    {
        get
        {
            lock (_lockObject)
                return _token;
        }
        set
        {
            lock (_lockObject)
                _token = value;
        }
    }

    /// <summary>Gets the user identity.</summary>
    public string UserId => _userId;

    /// <summary>Constructs a new credential instance.</summary>
    /// <param name="flow">Authorization code flow.</param>
    /// <param name="userId">User identifier.</param>
    /// <param name="token">An initial token for the user.</param>
    public UserCredential(IAuthorizationCodeFlow flow, string userId, Token token)
    {
        _flow = flow;
        _userId = userId;
        _token = token;
    }

    /// <summary>
    /// <summary>Invoked before the request is being sent.</summary>
    /// </summary>
    /// <param name="request">The HTTP request message.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    public async Task InterceptAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        GetAccessTokenForRequestAsync();
        _flow.AccessMethod.Intercept(request, Token.AccessToken);
        await Task.Delay(0, cancellationToken);
    }

    /// <summary>
    /// Handles an abnormal response when sending a HTTP request.
    /// A simple rule must be followed, if you modify the request object in a way that the abnormal response can
    /// be resolved, you must return <c>true</c>.
    /// </summary>
    /// <param name="args">
    /// Handle response argument which contains properties such as the request, response, current failed try.
    /// </param>
    /// <returns>Whether this handler has made a change that requires the request to be resent.</returns>
    public async Task<bool> HandleResponseAsync(HandleUnsuccessfulResponseArgs args)
    {
        if (args.Response.StatusCode != HttpStatusCode.Unauthorized)
            return false;
        var flag = !Token.AccessToken.Equals(_flow.AccessMethod.GetAccessToken(args.Request));
        await Task.Delay(0);
        return flag;
    }

    /// <summary>Initializes a HTTP client after it was created.</summary>
    public void Initialize(ConfigurableHttpClient httpClient)
    {
        httpClient.MessageHandler.AddExecuteInterceptor(this);
        httpClient.MessageHandler.AddUnsuccessfulResponseHandler(this);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Token.AccessToken);
    }

    /// <summary>
    /// Gets an access token to authorize a request.
    /// </summary>
    /// <returns>The access token.</returns>
    public virtual string GetAccessTokenForRequestAsync() => Token.AccessToken;

    /// <summary>
    /// Asynchronously revokes the token by calling
    /// <see cref="IAuthorizationCodeFlow.RevokeTokenAsync(String,String,CancellationToken)" />.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token to cancel an operation.</param>
    /// <returns><c>true</c> if the token was revoked successfully.</returns>
    public async Task<bool> RevokeTokenAsync(CancellationToken cancellationToken)
    {
        if (Token == null)
        {
            _logger.Warning("Token is already null, no need to revoke it.");
            return false;
        }
        await _flow.RevokeTokenAsync(_userId, Token.AccessToken, cancellationToken).ConfigureAwait(false);
        _logger.Info("Access token was revoked successfully");
        return true;
    }

    /// <summary>
    /// Get home set url by principal type.
    /// </summary>
    /// <param name="principal"></param>
    /// <returns>Principal home set url.</returns>
    public Uri GetUri(PrincipalHomeSet principal)
    {
        return principal switch
        {
            PrincipalHomeSet.Calendar => new Uri(Token.CalendarPrincipal.CalendarHomeSet),
            PrincipalHomeSet.AddressBook => new Uri(Token.PeoplePrincipal.AddressBookHomeSet),
            _ => throw new ArgumentOutOfRangeException(nameof(PrincipalHomeSet)),
        };
    }
}