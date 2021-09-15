using iCloud.Dav.Auth.Flows;
using iCloud.Dav.Auth.Utils.Store;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace iCloud.Dav.Auth
{
    /// <summary>
    /// Authorization Broker.
    /// </summary>
    public class AuthorizationBroker
    {
        /// <summary>The folder which is used by the <see cref="T:iCloud.dav.Auth.Utils.Store.FileDataStore" />.</summary>
        /// <remarks>
        /// The reason that this is not 'private const' is that a user can change it and store the credentials in a
        /// different location.
        /// </remarks>
        public static string Folder = "iCloud.dav.Auth";

        /// <summary>
        /// Authorize async.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="networdCredentials"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="dataStore"></param>
        /// <returns>User credential</returns>
        public static async Task<UserCredential> AuthorizeAsync(string user, NetworkCredential networdCredentials, CancellationToken cancellationToken, IDataStore dataStore)
        {
            AuthorizationCodeFlow.Initializer initializer = new AuthorizationCodeFlow.Initializer();
            initializer.DataStore = dataStore ?? new FileDataStore(AuthorizationBroker.Folder, false);
            AuthorizationCodeFlow authorizationCodeFlow = new AuthorizationCodeFlow(initializer);
            CodeReceiver codeReceiver = new CodeReceiver();
            AuthorizationCodeInstalledApp authorizationCodeInstalledApp = new AuthorizationCodeInstalledApp(authorizationCodeFlow, codeReceiver);
            return await authorizationCodeInstalledApp.AuthorizeAsync(user, networdCredentials, cancellationToken).ConfigureAwait(false);
        }
    }
}
