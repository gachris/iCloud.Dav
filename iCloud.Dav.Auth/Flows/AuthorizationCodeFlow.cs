using iCloud.Dav.Auth.Utils;
using iCloud.Dav.Auth.Utils.Store;
using iCloud.Dav.Core.Args;
using iCloud.Dav.Core.Enums;
using iCloud.Dav.Core.Logger;
using iCloud.Dav.Core.Services;
using iCloud.Dav.Core.Utils;
using System.Threading;
using System.Threading.Tasks;

namespace iCloud.Dav.Auth.Flows
{
    /// <summary>ICloud specific authorization code flow.</summary>
    public class AuthorizationCodeFlow : IAuthorizationCodeFlow
    {
        private static readonly ILogger Logger = ApplicationContext.Logger.ForType<AuthorizationCodeFlow>();
        private readonly IAccessMethod accessMethod;
        private readonly IDataStore dataStore;
        private readonly ConfigurableHttpClient httpClient;
        private readonly IClock clock;

        /// <summary>Gets the data store used to store the credentials.</summary>
        public IDataStore DataStore
        {
            get { return this.dataStore; }
        }

        /// <summary>Gets the HTTP client used to make authentication requests to the server.</summary>
        public ConfigurableHttpClient HttpClient
        {
            get { return this.httpClient; }
        }

        /// <summary>Gets the method for presenting the access token to the resource server.</summary>
        public IAccessMethod AccessMethod
        {
            get { return this.accessMethod; }
        }

        /// <summary>Gets the clock.</summary>
        public IClock Clock
        {
            get { return this.clock; }
        }

        /// <summary>Constructs a new ICloud authorization code flow.</summary>
        public AuthorizationCodeFlow(AuthorizationCodeFlow.Initializer initializer)
        {
            this.accessMethod = initializer.AccessMethod.ThrowIfNull("Initializer.AccessMethod");
            this.clock = initializer.Clock.ThrowIfNull("Initializer.Clock");
            this.dataStore = initializer.DataStore;
            if (this.dataStore == null)
                AuthorizationCodeFlow.Logger.Warning("Datastore is null, as a result the user's credential will not be stored");
            CreateHttpClientArgs args = new CreateHttpClientArgs();
            if (initializer.DefaultExponentialBackOffPolicy != ExponentialBackOffPolicy.None)
                args.Initializers.Add(new ExponentialBackOffInitializer(initializer.DefaultExponentialBackOffPolicy, () => new BackOffHandler(new ExponentialBackOff())));
            this.httpClient = (initializer.HttpClientFactory ?? new HttpClientFactory()).CreateHttpClient(args);
        }

        /// <summary>
        /// Asynchronously loads the user's token using the flow's
        /// <see cref="T:iCloud.dav.Auth.Util.Store.IDataStore" />.
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="taskCancellationToken">Cancellation token to cancel operation</param>
        /// <returns>Token response</returns>
        public async Task<Token> LoadTokenAsync(string userId, CancellationToken taskCancellationToken)
        {
            taskCancellationToken.ThrowIfCancellationRequested();
            if (this.DataStore == null)
                return null;
            return await this.DataStore.GetAsync<Token>(userId).ConfigureAwait(false);
        }

        /// <summary>
        /// Asynchronously deletes the user's token using the flow's
        /// <see cref="T:iCloud.dav.Auth.Util.Store.IDataStore" />.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="taskCancellationToken">Cancellation token to cancel operation.</param>
        public async Task DeleteTokenAsync(string userId, CancellationToken taskCancellationToken)
        {
            taskCancellationToken.ThrowIfCancellationRequested();
            if (this.DataStore == null)
                return;
            await this.DataStore.DeleteAsync<Token>(userId).ConfigureAwait(false);
        }

        /// <summary>Asynchronously exchanges code with a token.</summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="code">Authorization Basic Token.</param>
        /// <param name="taskCancellationToken">Cancellation token to cancel operation.</param>
        /// <returns>Token response which contains the access token.</returns>
        public async Task<Token> ExchangeCodeForTokenAsync(string userId, string code, CancellationToken taskCancellationToken)
        {
            Token token = await this.FetchTokenAsync(userId, code, taskCancellationToken).ConfigureAwait(false);
            await this.StoreTokenAsync(userId, token, taskCancellationToken).ConfigureAwait(false);
            return token;
        }

        /// <summary>
        /// Asynchronously revokes the specified token. This method disconnects the user's account.
        /// application. It should be called upon removing the user account from the site.</summary>
        /// <remarks>
        /// If revoking the token succeeds, the user's credential is removed from the data store and the user MUST
        /// authorize the application again before the application can access the user's private resources.
        /// </remarks>
        /// <param name="userId">User identifier.</param>
        /// <param name="token">Access token to be revoked.</param>
        /// <param name="taskCancellationToken">Cancellation token to cancel operation.</param>
        /// <returns><c>true</c> if the token was revoked successfully.</returns>
        public async Task RevokeTokenAsync(string userId, string token, CancellationToken taskCancellationToken)
        {
            await this.DeleteTokenAsync(userId, taskCancellationToken);
        }

        /// <summary>Stores the token in the <see cref="P:iCloud.dav.Auth.AuthorizationCodeFlow.DataStore" />.</summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="token">Token to store.</param>
        /// <param name="cancellationToken">Cancellation token to cancel operation.</param>
        private async Task StoreTokenAsync(string userId, Token token, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (this.DataStore == null)
                return;
            await this.DataStore.StoreAsync(userId, token).ConfigureAwait(false);
        }

        /// <summary>Retrieve a new token from the server using the specified request.</summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="code">Authorization Basic Token.</param>
        /// <param name="cancellationToken">Cancellation token to cancel operation.</param>
        /// <returns>Token response with the access token.</returns>
        public async Task<Token> FetchTokenAsync(string userId, string code, CancellationToken cancellationToken)
        {
            TokenException tokenException;
            try
            {
                return await this.httpClient.ExecuteAsync(code, cancellationToken, this.Clock).ConfigureAwait(false);
            }
            catch (TokenException ex)
            {
                tokenException = ex;
            }
            await this.DeleteTokenAsync(userId, cancellationToken).ConfigureAwait(false);
            throw tokenException;
        }

        /// <summary>Despose.</summary>
        public void Dispose()
        {
            if (this.HttpClient == null)
                return;
            this.HttpClient.Dispose();
        }

        /// <summary>An initializer class for the authorization code flow. </summary>
        public class Initializer
        {
            /// <summary>
            /// Gets or sets the method for presenting the access token to the resource server.
            /// The default value is
            /// <see cref="T:iCloud.dav.Auth.BasicAuthentication.AuthorizationHeaderAccessMethod" />.
            /// </summary>
            public IAccessMethod AccessMethod { get; set; }

            /// <summary>Gets or sets the data store used to store the token response.</summary>
            public IDataStore DataStore { get; set; }

            /// <summary>
            /// Gets or sets the factory for creating <see cref="T:System.Net.Http.HttpClient" /> instance.
            /// </summary>
            public IHttpClientFactory HttpClientFactory { get; set; }

            /// <summary>
            /// Get or sets the exponential back-off policy. Default value is  <c>UnsuccessfulResponse503</c>, which
            /// means that exponential back-off is used on 503 abnormal HTTP responses.
            /// If the value is set to <c>None</c>, no exponential back-off policy is used, and it's up to user to
            /// configure the <see cref="T:iCloud.dav.Core.ConfigurableMessageHandler" /> in an
            /// <see cref="T:iCloud.dav.Core.IConfigurableHttpClientInitializer" /> to set a specific back-off
            /// implementation (using <see cref="T:iCloud.dav.Core.BackOffHandler" />).
            /// </summary>
            public ExponentialBackOffPolicy DefaultExponentialBackOffPolicy { get; set; }

            /// <summary>
            /// Gets or sets the clock. The clock is used to determine if the token has expired, if so we will try to
            /// refresh it. The default value is <see cref="F:iCloud.dav.Auth.Util.SystemClock.Default" />.
            /// </summary>
            public IClock Clock { get; set; }

            /// <summary>Constructs a new initializer.</summary>
            public Initializer()
            {
                this.AccessMethod = new BasicAuthentication.AuthorizationHeaderAccessMethod();
                this.DefaultExponentialBackOffPolicy = ExponentialBackOffPolicy.UnsuccessfulResponse503;
                this.Clock = SystemClock.Default;
            }
        }
    }
}
