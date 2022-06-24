using iCloud.Dav.Core.Request;
using iCloud.Dav.Core.Services;

namespace iCloud.Dav.Calendar.Request
{
    public abstract class CalendarBaseServiceRequest<TResponse> : ClientServiceRequest<TResponse>
    {
        ///<summary>Constructs a new CalendarBaseServiceRequest instance.</summary>
        protected CalendarBaseServiceRequest(IClientService service) : base(service)
        {
        }
    }
}
