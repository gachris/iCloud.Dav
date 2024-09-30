using System.Net;
using iCloud.Dav.Auth.Store;

namespace iCloud.Dav.Auth;

/// <summary>
/// Represents an Authorization Broker for handling Basic Authentication.
/// </summary>
public class AuthorizationBroker
{
    /// <summary>
    /// The folder name used for storing credentials.
    /// </summary>
    /// <remarks>
    /// This is not marked as 'private const' to allow users to customize the storage location.
    /// </remarks>
    public const string Folder = "iCloudAuthentication";

    /// <summary>
    /// Authorizes a user asynchronously using Basic Authentication.
    /// </summary>
    /// <param name="user">The user's identifier.</param>
    /// <param name="networkCredentials">The user's network credentials (username and password).</param>
    /// <param name="dataStore">Optional: A custom data store (defaults to a <see cref="FileDataStore"/>).</param>
    /// <param name="cancellationToken">Optional: A cancellation token to cancel the operation.</param>
    /// <returns>The user's credentials upon successful authorization.</returns>
    public static async Task<UserCredential> AuthorizeAsync(string user, NetworkCredential networkCredentials, IDataStore dataStore = null, CancellationToken cancellationToken = default)
    {
        var initializer = new AuthorizationCodeFlow.Initializer(dataStore ?? new FileDataStore(Folder, false));
        var authorizationCodeFlow = new AuthorizationCodeFlow(initializer);
        var codeReceiver = new CodeReceiver();
        var authorizationCodeInstalledApp = new AuthorizationCodeInstalledApp(authorizationCodeFlow, codeReceiver);
        return await authorizationCodeInstalledApp.AuthorizeAsync(user, networkCredentials, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Authorizes a user asynchronously using Basic Authentication and retrieves the credentials from cache.
    /// </summary>
    /// <param name="user">The user's identifier.</param>
    /// <param name="dataStore">The data store used for caching credentials (defaults to a <see cref="FileDataStore"/>).</param>
    /// <param name="cancellationToken">Optional: A cancellation token to cancel the operation.</param>
    /// <returns>The user's credentials retrieved from cache upon successful authorization.</returns>
    public static async Task<UserCredential> AuthorizeFromCacheAsync(string user, IDataStore dataStore = null, CancellationToken cancellationToken = default)
    {
        var initializer = new AuthorizationCodeFlow.Initializer(dataStore ?? new FileDataStore(Folder, false));
        var authorizationCodeFlow = new AuthorizationCodeFlow(initializer);
        var codeReceiver = new CodeReceiver();
        var authorizationCodeInstalledApp = new AuthorizationCodeInstalledApp(authorizationCodeFlow, codeReceiver);
        return await authorizationCodeInstalledApp.AuthorizeAsync(user, cancellationToken).ConfigureAwait(false);
    }
}