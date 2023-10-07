using iCloud.Dav.Core;
using iCloud.Dav.Core.Request;

namespace iCloud.Dav.Calendar.Request;

/// <summary>
/// An abstract base class for service requests used in the Apple iCloud Calendar.
/// </summary>
public abstract class CalendarBaseServiceRequest<TResponse> : ClientServiceRequest<TResponse>
{
    /// <summary>
    /// Constructs a new instance of the <see cref="CalendarBaseServiceRequest{TResponse}"/> class.
    /// </summary>
    /// <param name="service">The client service instance used to make the request.</param>
    protected CalendarBaseServiceRequest(IClientService service) : base(service)
    {
    }
}