using iCloud.Dav.Auth.Store;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace iCloud.Dav.Auth;

/// <summary>
/// Authorization Broker.
/// </summary>
public class AuthorizationBroker
{
    /// <summary>The folder which is used by the <see cref="FileDataStore" />.</summary>
    /// <remarks>
    /// The reason that this is not 'private const' is that a user can change it and store the credentials in a
    /// different location.
    /// </remarks>
    public const string Folder = "iCloudAuthentication";

    /// <summary>
    /// Authorize async.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="networdCredentials"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="dataStore"></param>
    /// <returns>User credential</returns>
    public static async Task<UserCredential> AuthorizeAsync(string user, NetworkCredential networdCredentials, IDataStore dataStore = null, CancellationToken cancellationToken = default)
    {
        var initializer = new AuthorizationCodeFlow.Initializer { DataStore = dataStore ?? new FileDataStore(Folder, false) };
        var authorizationCodeFlow = new AuthorizationCodeFlow(initializer);
        var codeReceiver = new CodeReceiver();
        var authorizationCodeInstalledApp = new AuthorizationCodeInstalledApp(authorizationCodeFlow, codeReceiver);
        return await authorizationCodeInstalledApp.AuthorizeAsync(user, networdCredentials, cancellationToken).ConfigureAwait(false);
    }
}
