using iCloud.Dav.Core.Services;
using iCloud.Dav.Core.Utils;
using iCloud.Dav.People.CardDav.Types;
using iCloud.Dav.People.Request;

namespace iCloud.Dav.People.Resources
{
    /// <summary>The "IdentityCard" collection of methods.</summary>
    public class IdentityCardResource
    {
        /// <summary>The service which this resource belongs to.</summary>
        private readonly IClientService _service;

        /// <summary>Constructs a new resource.</summary>
        public IdentityCardResource(IClientService service)
        {
            _service = service;
        }

        /// <summary>Returns the identity cards on the user's identity card list.</summary>
        public virtual ListRequest List()
        {
            return new ListRequest(_service);
        }

        /// <summary>Returns the identity cards on the user's identity card list.</summary>
        public class ListRequest : PeopleBaseServiceRequest<IdentityCardList>
        {
            /// <summary>Constructs a new Get request.</summary>
            public ListRequest(IClientService service) : base(service)
            {
                InitParameters();
            }

            ///<summary>Gets the method name.</summary>
            public override string MethodName => "list";

            ///<summary>Gets the HTTP method.</summary>
            public override string HttpMethod => ApiMethod.Propfind;

            ///<summary>Gets the REST path.</summary>
            public override string RestPath => string.Empty;

            ///<summary>Gets the depth.</summary>
            public override string Depth => "1";

            ///<summary>Returns the body of the request.</summary>
            protected override object GetBody() => new PropFind();
        }
    }
}
