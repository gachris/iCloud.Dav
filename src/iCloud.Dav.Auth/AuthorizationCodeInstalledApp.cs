using System.Net;
using iCloud.Dav.Core;
using iCloud.Dav.Core.Logger;

namespace iCloud.Dav.Auth;

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
        _flow = flow;
        _codeReceiver = codeReceiver;
    }

    /// <summary>Gets the authorization code flow.</summary>
    public IAuthorizationCodeFlow Flow => _flow;

    /// <summary>Gets the code receiver which is responsible for receiving the authorization code.</summary>
    public ICodeReceiver CodeReceiver => _codeReceiver;

    /// <summary>Asynchronously authorizes the installed application to access user's protected data.</summary>
    /// <param name="userId">User identifier</param>
    /// <param name="networkCredential">User Credentials</param>
    /// <param name="taskCancellationToken">Cancellation token to cancel an operation</param>
    /// <returns>The user's credential</returns>
    public async Task<UserCredential> AuthorizeAsync(string userId, NetworkCredential networkCredential, CancellationToken taskCancellationToken)
    {
        var token = await Flow.LoadTokenAsync(userId, taskCancellationToken).ConfigureAwait(false);
        if (token == null)
        {
            var authorizationCode = CodeReceiver.ReceiveCode(networkCredential);
            AuthorizationCodeInstalledApp.Logger.Debug("Received \"{0}\" code", authorizationCode);
            token = await Flow.ExchangeCodeForTokenAsync(userId, authorizationCode, taskCancellationToken).ConfigureAwait(false);
        }
        return new UserCredential(_flow, userId, token);
    }

    /// <summary>Asynchronously authorizes the installed application to access user's protected data.</summary>
    /// <param name="userId">User identifier</param>
    /// <param name="taskCancellationToken">Cancellation token to cancel an operation</param>
    /// <returns>The user's credential</returns>
    public async Task<UserCredential> AuthorizeAsync(string userId, CancellationToken taskCancellationToken)
    {
        var token = await Flow.LoadTokenAsync(userId, taskCancellationToken).ConfigureAwait(false);
        return token == null ? throw new TokenException("User is not signed in.") : new UserCredential(_flow, userId, token);
    }
}