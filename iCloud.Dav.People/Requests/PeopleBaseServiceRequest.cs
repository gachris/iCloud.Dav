using iCloud.Dav.Core.Request;
using iCloud.Dav.Core.Services;

namespace iCloud.Dav.People.Request
{
    public abstract class PeopleBaseServiceRequest<TResponse> : ClientServiceRequest<TResponse>
    {
        ///<summary>Constructs a new PeopleBaseServiceRequest instance.</summary>
        protected PeopleBaseServiceRequest(IClientService service) : base(service)
        {
        }
    }
}
