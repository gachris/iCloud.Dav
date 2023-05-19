using iCloud.Dav.Core;
using iCloud.Dav.Core.Request;

namespace iCloud.Dav.People.Requests
{
    /// <summary>
    /// An abstract base class for service requests used in the Apple iCloud People.
    /// </summary>
    public abstract class PeopleBaseServiceRequest<TResponse> : ClientServiceRequest<TResponse>
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="PeopleBaseServiceRequest{TResponse}"/> class.
        /// </summary>
        /// <param name="service">The client service instance used to make the request.</param>
        protected PeopleBaseServiceRequest(IClientService service) : base(service)
        {
        }
    }
}