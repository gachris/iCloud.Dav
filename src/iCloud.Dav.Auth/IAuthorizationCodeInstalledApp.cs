using System.Net;

namespace iCloud.Dav.Auth;

/// <summary>
/// Authorization code flow for an installed application that persists end-user credentials.
/// </summary>
public interface IAuthorizationCodeInstalledApp
{
    /// <summary>Gets the authorization code flow.</summary>
    IAuthorizationCodeFlow Flow { get; }

    /// <summary>Gets the code receiver.</summary>
    ICodeReceiver CodeReceiver { get; }

    /// <summary>Asynchronously authorizes the installed application to access user's protected data.</summary>
    /// <param name="userId">User identifier</param>
    /// <param name="networkCredential">User Credentials</param>
    /// <param name="taskCancellationToken">Cancellation token to cancel an operation</param>
    /// <returns>The user's credential</returns>
    Task<UserCredential> AuthorizeAsync(string userId, NetworkCredential networkCredential, CancellationToken taskCancellationToken);
}