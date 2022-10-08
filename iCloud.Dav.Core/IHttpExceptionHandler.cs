using System.Threading.Tasks;

namespace iCloud.Dav.Core;

/// <summary>Exception handler is invoked when an exception is thrown during a HTTP request.</summary>
public interface IHttpExceptionHandler
{
    /// <summary>
    /// Handles an exception thrown when sending a HTTP request.
    /// A simple rule must be followed, if you modify the request object in a way that the exception can be
    /// resolved, you must return <c>true</c>.
    /// </summary>
    /// <param name="args">
    /// Handle exception argument which properties such as the request, exception, current failed try.
    /// </param>
    /// <returns>Whether this handler has made a change that requires the request to be resent.</returns>
    Task<bool> HandleExceptionAsync(HandleExceptionArgs args);
}
