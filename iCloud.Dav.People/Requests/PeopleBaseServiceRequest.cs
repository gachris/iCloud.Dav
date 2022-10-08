using iCloud.Dav.Core;
using iCloud.Dav.Core.Request;

namespace iCloud.Dav.People.Requests;

public abstract class PeopleBaseServiceRequest<TResponse> : ClientServiceRequest<TResponse>
{
    ///<summary>Constructs a new PeopleBaseServiceRequest instance.</summary>
    protected PeopleBaseServiceRequest(IClientService service) : base(service)
    {
    }
}
