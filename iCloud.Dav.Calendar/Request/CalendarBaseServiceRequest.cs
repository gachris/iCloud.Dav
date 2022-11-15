using iCloud.Dav.Core;
using iCloud.Dav.Core.Request;

namespace iCloud.Dav.Calendar.Request;

/// <inheritdoc/>
public abstract class CalendarBaseServiceRequest<TResponse> : ClientServiceRequest<TResponse>
{
    /// <inheritdoc/>
    protected CalendarBaseServiceRequest(IClientService service) : base(service)
    {
    }
}
