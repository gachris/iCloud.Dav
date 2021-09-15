using iCloud.Dav.Core.Args;
using iCloud.Dav.Core.Enums;
using iCloud.Dav.Core.Logger;
using iCloud.Dav.Core.Services;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace iCloud.Dav.Auth
{
    /// <summary>
    /// User Credentials.
    /// </summary>
    public class UserCredential : ICredential, IConfigurableHttpClientCredentialInitializer, ITokenAccess, IHttpExecuteInterceptor, IHttpUnsuccessfulResponseHandler
    {
        /// <summary>Gets Logger.</summary>
        protected static readonly ILogger Logger = ApplicationContext.Logger.ForType<UserCredential>();
        private readonly object _lockObject = new object();
        private readonly IAuthorizationCodeFlow _flow;
        private readonly string _userId;
        private Token _token;

        /// <summary>Gets or sets the token response which contains the access token.</summary>
        public Token Token
        {
            get
            {
                lock (this._lockObject)
                    return this._token;
            }
            set
            {
                lock (this._lockObject)
                    this._token = value;
            }
        }

        /// <summary>Gets the user identity.</summary>
        public string UserId
        {
            get { return this._userId; }
        }

        /// <summary>Constructs a new credential instance.</summary>
        /// <param name="flow">Authorization code flow.</param>
        /// <param name="userId">User identifier.</param>
        /// <param name="token">An initial token for the user.</param>
        public UserCredential(IAuthorizationCodeFlow flow, string userId, Token token)
        {
            this._flow = flow;
            this._userId = userId;
            this._token = token;
        }

        /// <summary>
        /// <summary>Invoked before the request is being sent.</summary>
        /// </summary>
        /// <param name="request">The HTTP request message.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        public async Task InterceptAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            this.GetAccessTokenForRequestAsync();
            this._flow.AccessMethod.Intercept(request, this.Token.AccessToken);
#if net40
            await TaskEx.Delay(0);
#endif
#if others_frameworks
            await Task.Delay(0);
#endif
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
            bool flag = !object.Equals(Token.AccessToken, this._flow.AccessMethod.GetAccessToken(args.Request));
#if net40
            await TaskEx.Delay(0);
#endif
#if others_frameworks
            await Task.Delay(0);
#endif
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
        public virtual string GetAccessTokenForRequestAsync()
        {
            if (this.Token != null)
                return this.Token.AccessToken;
            else return null;
        }

        /// <summary>
        /// Asynchronously revokes the token by calling
        /// <see cref="M:iCloud.dav.Auth.IAuthorizationCodeFlow.RevokeTokenAsync(System.String,System.String, System.Threading.CancellationToken)" />.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to cancel an operation.</param>
        /// <returns><c>true</c> if the token was revoked successfully.</returns>
        public async Task<bool> RevokeTokenAsync(CancellationToken cancellationToken)
        {
            if (this.Token == null)
            {
                UserCredential.Logger.Warning("Token is already null, no need to revoke it.");
                return false;
            }
            await this._flow.RevokeTokenAsync(this._userId, this.Token.AccessToken, cancellationToken).ConfigureAwait(false);
            UserCredential.Logger.Info("Access token was revoked successfully");
            return true;
        }

        /// <summary>
        /// Get home set url by principal type.
        /// </summary>
        /// <param name="principal"></param>
        /// <returns>Principal home set url.</returns>
        public string GetUriHomeSet(PrincipalHomeSet principal)
        {
            if (principal == PrincipalHomeSet.AddressBookHomeSet)
            {
                return this.Token?.PeoplePrincipal?.AddressBookHomeSet;
            }
            else
            {
                return this.Token?.CalendarPrincipal?.CalendarHomeSet;
            }
        }
    }
}