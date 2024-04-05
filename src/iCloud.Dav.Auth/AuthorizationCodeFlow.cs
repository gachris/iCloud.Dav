using iCloud.Dav.Auth.Store;
using iCloud.Dav.Core;
using iCloud.Dav.Core.Extensions;
using iCloud.Dav.Core.Logger;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace iCloud.Dav.Auth;

/// <summary>ICloud specific authorization code flow.</summary>
public class AuthorizationCodeFlow : IAuthorizationCodeFlow
{
    private static readonly ILogger _logger = ApplicationContext.Logger.ForType<AuthorizationCodeFlow>();
    private readonly IAccessMethod _accessMethod;
    private readonly IDataStore _dataStore;
    private readonly ConfigurableHttpClient _httpClient;
    private readonly IClock _clock;

    /// <summary>Gets the data store used to store the credentials.</summary>
    public IDataStore DataStore => _dataStore;

    /// <summary>Gets the HTTP client used to make authentication requests to the server.</summary>
    public ConfigurableHttpClient HttpClient => _httpClient;

    /// <summary>Gets the method for presenting the access token to the resource server.</summary>
    public IAccessMethod AccessMethod => _accessMethod;

    /// <summary>Gets the clock.</summary>
    public IClock Clock => _clock;

    /// <summary>Constructs a new ICloud authorization code flow.</summary>
    public AuthorizationCodeFlow(Initializer initializer)
    {
        _accessMethod = initializer.AccessMethod.ThrowIfNull(nameof(initializer.AccessMethod));
        _clock = initializer.Clock.ThrowIfNull(nameof(initializer.Clock));
        _dataStore = initializer.DataStore;
        var args = new CreateHttpClientArgs();
        if (initializer.DefaultExponentialBackOffPolicy != ExponentialBackOffPolicy.None)
        {
            var exponentialBackOffInitializer = new ExponentialBackOffInitializer(initializer.DefaultExponentialBackOffPolicy, () => new BackOffHandler(new ExponentialBackOff()));
            args.Initializers.Add(exponentialBackOffInitializer);
        }
        _httpClient = initializer.HttpClientFactory.CreateHttpClient(args);
    }

    /// <summary>
    /// Asynchronously loads the user's token using the flow's
    /// <see cref="IDataStore" />.
    /// </summary>
    /// <param name="userId">User identifier</param>
    /// <param name="taskCancellationToken">Cancellation token to cancel operation</param>
    /// <returns>Token response</returns>
    public async Task<Token> LoadTokenAsync(string userId, CancellationToken taskCancellationToken)
    {
        taskCancellationToken.ThrowIfCancellationRequested();
        return DataStore == null ? null : await DataStore.GetAsync<Token>(userId).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously deletes the user's token using the flow's
    /// <see cref="IDataStore" />.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <param name="taskCancellationToken">Cancellation token to cancel operation.</param>
    public async Task DeleteTokenAsync(string userId, CancellationToken taskCancellationToken)
    {
        taskCancellationToken.ThrowIfCancellationRequested();
        if (DataStore == null)
            return;
        await DataStore.DeleteAsync<Token>(userId).ConfigureAwait(false);
    }

    /// <summary>Asynchronously exchanges code with a token.</summary>
    /// <param name="userId">User identifier.</param>
    /// <param name="code">Authorization Basic Token.</param>
    /// <param name="taskCancellationToken">Cancellation token to cancel operation.</param>
    /// <returns>Token response which contains the access token.</returns>
    public async Task<Token> ExchangeCodeForTokenAsync(string userId, string code, CancellationToken taskCancellationToken)
    {
        var token = await FetchTokenAsync(userId, code, taskCancellationToken).ConfigureAwait(false);
        await StoreTokenAsync(userId, token, taskCancellationToken).ConfigureAwait(false);
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
    public async Task RevokeTokenAsync(string userId, string token, CancellationToken taskCancellationToken) => await DeleteTokenAsync(userId, taskCancellationToken);

    /// <summary>Stores the token in the <see cref="DataStore" />.</summary>
    /// <param name="userId">User identifier.</param>
    /// <param name="token">Token to store.</param>
    /// <param name="cancellationToken">Cancellation token to cancel operation.</param>
    private async Task StoreTokenAsync(string userId, Token token, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (DataStore == null)
            return;
        await DataStore.StoreAsync(userId, token).ConfigureAwait(false);
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
            return await _httpClient.ExecuteAsync(code, Clock, cancellationToken).ConfigureAwait(false);
        }
        catch (TokenException ex)
        {
            tokenException = ex;
        }
        await DeleteTokenAsync(userId, cancellationToken).ConfigureAwait(false);
        throw tokenException;
    }

    /// <summary>
    /// Dispose.
    /// </summary>
    public void Dispose()
    {
        GC.SuppressFinalize(this);

        if (HttpClient == null)
            return;
        HttpClient.Dispose();
    }

    /// <summary>An initializer class for the authorization code flow. </summary>
    public class Initializer
    {
        /// <summary>
        /// Gets or sets the method for presenting the access token to the resource server.
        /// The default value is
        /// <see cref="BasicAuthentication.AuthorizationHeaderAccessMethod" />.
        /// </summary>
        public IAccessMethod AccessMethod { get; set; }

        /// <summary>Gets or sets the data store used to store the token response.</summary>
        public IDataStore DataStore { get; }

        /// <summary>
        /// Gets or sets the factory for creating <see cref="System.Net.Http.HttpClient" /> instance.
        /// </summary>
        public IHttpClientFactory HttpClientFactory { get; set; }

        /// <summary>
        /// Get or sets the exponential back-off policy. Default value is  <c>UnsuccessfulResponse503</c>, which
        /// means that exponential back-off is used on 503 abnormal HTTP responses.
        /// If the value is set to <c>None</c>, no exponential back-off policy is used, and it's up to user to
        /// configure the <see cref="ConfigurableMessageHandler" /> in an
        /// <see cref="IConfigurableHttpClientInitializer" /> to set a specific back-off
        /// implementation (using <see cref="BackOffHandler" />).
        /// </summary>
        public ExponentialBackOffPolicy DefaultExponentialBackOffPolicy { get; set; }

        /// <summary>
        /// Gets or sets the clock. The clock is used to determine if the token has expired, if so we will try to
        /// refresh it. The default value is <see cref="SystemClock.Default" />.
        /// </summary>
        public IClock Clock { get; set; }

        /// <summary>Constructs a new initializer.</summary>
        public Initializer(IDataStore dataStore)
        {
            DataStore = dataStore;
            HttpClientFactory = new HttpClientFactory();
            AccessMethod = new BasicAuthentication.AuthorizationHeaderAccessMethod();
            DefaultExponentialBackOffPolicy = ExponentialBackOffPolicy.UnsuccessfulResponse503;
            Clock = SystemClock.Default;
        }
    }
}