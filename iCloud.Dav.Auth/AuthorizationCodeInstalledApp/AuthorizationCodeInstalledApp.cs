using iCloud.Dav.Core.Logger;
using iCloud.Dav.Core.Services;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace iCloud.Dav.Auth
{
    /// <summary>
    /// Thread-safe authorization code flow for an installed application that persists end-user credentials.
    /// </summary>
    public class AuthorizationCodeInstalledApp : IAuthorizationCodeInstalledApp
    {
        private static readonly ILogger Logger = ApplicationContext.Logger.ForType<AuthorizationCodeInstalledApp>();
        private readonly IAuthorizationCodeFlow _flow;
        private readonly ICodeReceiver _codeReceiver;

        /// <summary>
        /// Constructs a new authorization code installed application with the given flow and code receiver.
        /// </summary>
        public AuthorizationCodeInstalledApp(IAuthorizationCodeFlow flow, ICodeReceiver codeReceiver)
        {
            this._flow = flow;
            this._codeReceiver = codeReceiver;
        }

        /// <summary>Gets the authorization code flow.</summary>
        public IAuthorizationCodeFlow Flow
        {
            get { return this._flow; }
        }

        /// <summary>Gets the code receiver which is responsible for receiving the authorization code.</summary>
        public ICodeReceiver CodeReceiver
        {
            get { return this._codeReceiver; }
        }

        /// <summary>Asynchronously authorizes the installed application to access user's protected data.</summary>
        /// <param name="userId">User identifier</param>
        /// <param name="networkCredential">User Credentials</param>
        /// <param name="taskCancellationToken">Cancellation token to cancel an operation</param>
        /// <returns>The user's credential</returns>
        public async Task<UserCredential> AuthorizeAsync(string userId, NetworkCredential networkCredential, CancellationToken taskCancellationToken)
        {
            Token token = await this.Flow.LoadTokenAsync(userId, taskCancellationToken).ConfigureAwait(false);
            if (token == null)
            {
                string authorizationCode = this.CodeReceiver.ReceiveCode(networkCredential);
                AuthorizationCodeInstalledApp.Logger.Debug("Received \"{0}\" code", (object)authorizationCode);
                token = await this.Flow.ExchangeCodeForTokenAsync(userId, authorizationCode, taskCancellationToken).ConfigureAwait(false);
            }
            return new UserCredential(this._flow, userId, token);
        }
    }
}