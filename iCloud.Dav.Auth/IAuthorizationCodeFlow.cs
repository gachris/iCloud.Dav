using iCloud.Dav.Auth.Store;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace iCloud.Dav.Auth;

/// <summary>Authorization code flow that manages and persists end-user credentials.</summary>
public interface IAuthorizationCodeFlow : IDisposable
{
    /// <summary>Gets the method for presenting the access token to the resource server.</summary>
    IAccessMethod AccessMethod { get; }

    /// <summary>Gets the clock.</summary>
    IClock Clock { get; }

    /// <summary>Gets the data store used to store the credentials.</summary>
    IDataStore DataStore { get; }

    /// <summary>
    /// Asynchronously loads the user's token using the flow's
    /// <see cref="IDataStore" />.
    /// </summary>
    /// <param name="userId">User identifier</param>
    /// <param name="taskCancellationToken">Cancellation token to cancel operation</param>
    /// <returns>Token response</returns>
    Task<Token?> LoadTokenAsync(string userId, CancellationToken taskCancellationToken);

    /// <summary>
    /// Asynchronously deletes the user's token using the flow's
    /// <see cref="IDataStore" />.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <param name="taskCancellationToken">Cancellation token to cancel operation.</param>
    Task DeleteTokenAsync(string userId, CancellationToken taskCancellationToken);

    /// <summary>Asynchronously exchanges code with a token.</summary>
    /// <param name="userId">User identifier.</param>
    /// <param name="code">Authorization Basic Token.</param>
    /// <param name="taskCancellationToken">Cancellation token to cancel operation.</param>
    /// <returns>Token response which contains the access token.</returns>
    Task<Token> ExchangeCodeForTokenAsync(string userId, string code, CancellationToken taskCancellationToken);

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
    Task RevokeTokenAsync(string userId, string token, CancellationToken taskCancellationToken);
}
