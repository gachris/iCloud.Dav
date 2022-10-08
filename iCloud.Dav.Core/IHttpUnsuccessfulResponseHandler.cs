using System.Threading.Tasks;

namespace iCloud.Dav.Core;

/// <summary>
/// Unsuccessful response handler which is invoked when an abnormal HTTP response is returned when sending a HTTP
/// request.
/// </summary>
public interface IHttpUnsuccessfulResponseHandler
{
    /// <summary>
    /// Handles an abnormal response when sending a HTTP request.
    /// A simple rule must be followed, if you modify the request object in a way that the abnormal response can
    /// be resolved, you must return <c>true</c>.
    /// </summary>
    /// <param name="args">
    /// Handle response argument which contains properties such as the request, response, current failed try.
    /// </param>
    /// <returns>Whether this handler has made a change that requires the request to be resent.</returns>
    Task<bool> HandleResponseAsync(HandleUnsuccessfulResponseArgs args);
}
