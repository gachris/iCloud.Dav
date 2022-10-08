using iCloud.Dav.Core;
using iCloud.Dav.Core.Request;

namespace iCloud.Dav.Calendar.Request;

public abstract class CalendarBaseServiceRequest<TResponse> : ClientServiceRequest<TResponse>
{
    ///<summary>Constructs a new CalendarBaseServiceRequest instance.</summary>
    protected CalendarBaseServiceRequest(IClientService service) : base(service)
    {
    }
}
