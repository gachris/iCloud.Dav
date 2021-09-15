using iCloud.Dav.Core.Request;
using iCloud.Dav.Core.Services;
using iCloud.Dav.Core.Utils;
using iCloud.Dav.People.Request;
using iCloud.Dav.People.Types;

namespace iCloud.Dav.People.Resources
{
    /// <summary>The "IdentityCard" collection of methods.</summary>
    public class IdentityCardResource
    {
        private const string _resource = "identityCard";

        /// <summary>The service which this resource belongs to.</summary>
        private readonly IClientService _service;

        /// <summary>Constructs a new resource.</summary>
        public IdentityCardResource(IClientService service)
        {
            this._service = service;
        }

        /// <summary>Returns the identity cards on the user's identity card list.</summary>
        public virtual ListRequest List()
        {
            return new ListRequest(this._service);
        }

        /// <summary>Returns the identity cards on the user's identity card list.</summary>
        public class ListRequest : PeopleBaseServiceRequest<IdentityCardList>
        {
            /// <summary>Constructs a new Get request.</summary>
            public ListRequest(IClientService service) : base(service)
            {
                this.InitParameters();
            }

            ///<summary>Gets the method name.</summary>
            public override string MethodName => "list";

            ///<summary>Gets the HTTP method.</summary>
            public override string HttpMethod => ApiMethod.PROPFIND;

            ///<summary>Gets the REST path.</summary>
            public override string RestPath => string.Empty;

            ///<summary>Gets the depth.</summary>
            public override string Depth => "1";

            ///<summary>Returns the body of the request.</summary>
            protected override object GetBody()
            {
                var contactsListPropfind = new Propfind<Prop>()
                {
                    Prop = new Prop
                    {
                        Resourcetype = new Resourcetype()
                    }
                };
                return contactsListPropfind;
            }
        }
    }
}
