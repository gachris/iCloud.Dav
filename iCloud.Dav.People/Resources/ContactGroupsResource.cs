using iCloud.Dav.Core;
using iCloud.Dav.Core.Response;
using iCloud.Dav.Core.Utils;
using iCloud.Dav.People.CardDav.Types;
using iCloud.Dav.People.DataTypes;
using iCloud.Dav.People.Requests;
using iCloud.Dav.People.Utils;

namespace iCloud.Dav.People.Resources
{
    /// <summary>
    /// The "Contact Groups" collection of methods.
    /// </summary>
    public class ContactGroupsResource
    {
        /// <summary>
        /// The service which this resource belongs to.
        /// </summary>
        private readonly IClientService _service;

        /// <summary>
        /// Constructs a new resource.
        /// </summary>
        public ContactGroupsResource(IClientService service) => _service = service;

        /// <summary>
        /// Returns the contact groups on the user's contact group list.
        /// </summary>
        /// <param name="resourceName">Resource Name. To retrieve resource names call the <see cref="IdentityCardResource.List"/> method.</param>
        public virtual ListRequest List(string resourceName) => new ListRequest(_service, resourceName);

        /// <summary>
        /// Returns a contact group from the user's contact group list.
        /// </summary>
        /// <param name="contactGroupId">Contact Group identifier. To retrieve contact group IDs call the <see cref="List(string)"/> method.</param>
        /// <param name="resourceName">Resource Name. To retrieve resource names call the <see cref="IdentityCardResource.List"/> method.</param>
        public virtual GetRequest Get(string contactGroupId, string resourceName) => new GetRequest(_service, contactGroupId, resourceName);

        /// <summary>
        /// Inserts Contact Group into the user's contact group list.
        /// </summary>
        /// <param name="body">The body of the request.</param>
        /// <param name="resourceName">Resource Name. To retrieve resource names call the <see cref="IdentityCardResource.List"/> method.</param>
        public virtual InsertRequest Insert(ContactGroup body, string resourceName) => new InsertRequest(_service, body, resourceName);

        /// <summary>
        /// Updates an existing contact group on the user's contact group list.
        /// </summary>
        /// <param name="body">The body of the request.</param>
        /// <param name="resourceName">Resource Name. To retrieve resource names call the <see cref="IdentityCardResource.List"/> method.</param>
        public virtual UpdateRequest Update(ContactGroup body, string resourceName) => new UpdateRequest(_service, body, resourceName);

        /// <summary>
        /// Removes a contact group from the user's contact group list.
        /// </summary>
        /// <param name="contactGroupId">Contact Group identifier. To retrieve contact group IDs call the <see cref="List(string)"/> method.</param>
        /// <param name="resourceName">Resource Name. To retrieve resource names call the <see cref="IdentityCardResource.List"/> method.</param>
        public virtual DeleteRequest Delete(string contactGroupId, string resourceName) => new DeleteRequest(_service, contactGroupId, resourceName);

        /// <summary>
        /// Returns the contact groups on the user's contact group list.
        /// </summary>
        public class ListRequest : PeopleBaseServiceRequest<ContactGroupList>
        {
            private object _body;

            /// <summary>
            /// Constructs a new List request.
            /// </summary>
            public ListRequest(IClientService service, string resourceName) : base(service) => ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(IdentityCard.ResourceName));

            /// <summary>
            /// Resource Name. To retrieve resource names call the <see cref="IdentityCardResource.List"/> method.
            /// </summary>
            [RequestParameter("resourceName", RequestParameterType.Path)]
            public virtual string ResourceName { get; }

            /// <inheritdoc/>
            public override string MethodName => Constants.Report;

            /// <inheritdoc/>
            public override string HttpMethod => Constants.Report;

            /// <inheritdoc/>
            public override string RestPath => "{resourceName}";

            /// <inheritdoc/>
            public override string Depth => "1";

            /// <inheritdoc/>
            protected override object GetBody()
            {
                if (_body is null)
                {
                    var addressBookQuery = new AddressBookQuery
                    {
                        Filter = new Filters
                        {
                            Type = "anyof",
                            Name = "X-ADDRESSBOOKSERVER-KIND"
                        }
                    };

                    addressBookQuery.Filter.TextMatches.Add(new TextMatch
                    {
                        Collation = "i;unicode-casemap",
                        NegateCondition = "no",
                        MatchType = "equals",
                        SearchText = "group"
                    });

                    _body = addressBookQuery;
                }
                return _body;
            }

            /// <inheritdoc/>
            protected override void InitParameters()
            {
                base.InitParameters();

                RequestParameters.Add("resourceName", new Parameter("resourceName", "path", true));
            }
        }

        /// <summary>
        /// Returns a contact group from the user's contact group list.
        /// </summary>
        public class GetRequest : PeopleBaseServiceRequest<ContactGroup>
        {
            /// <summary>
            /// Constructs a new Get request.
            /// </summary>
            public GetRequest(IClientService service, string contactGroupId, string resourceName) : base(service)
            {
                ContactGroupId = contactGroupId.ThrowIfNullOrEmpty(nameof(ContactGroup.Id));
                ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(IdentityCard.ResourceName));
            }

            /// <summary>
            /// Resource Name. To retrieve resource names call the <see cref="IdentityCardResource.List"/> method.
            /// </summary>
            [RequestParameter("resourceName", RequestParameterType.Path)]
            public virtual string ResourceName { get; }

            /// <summary>
            /// Contact Group identifier. To retrieve contact group IDs call the <see cref="List(string)"/> method.
            /// </summary>
            [RequestParameter("contactGroupId", RequestParameterType.Path)]
            public virtual string ContactGroupId { get; }

            /// <inheritdoc/>
            public override string MethodName => "get";

            /// <inheritdoc/>
            public override string HttpMethod => Constants.Get;

            /// <inheritdoc/>
            public override string RestPath => "{resourceName}/{contactGroupId}.vcf";

            /// <inheritdoc/>
            protected override void InitParameters()
            {
                base.InitParameters();

                RequestParameters.Add("resourceName", new Parameter("resourceName", "path", true));
                RequestParameters.Add("contactGroupId", new Parameter("contactGroupId", "path", true));
            }
        }

        /// <summary>
        /// Inserts an existing contact group into the user's contact group list.
        /// </summary>
        public class InsertRequest : PeopleBaseServiceRequest<VoidResponse>
        {
            private object _body;

            /// <summary>
            /// Constructs a new Insert request.
            /// </summary>
            public InsertRequest(IClientService service, ContactGroup body, string resourceName) : base(service)
            {
                ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(IdentityCard.ResourceName));
                Body = body.ThrowIfNull(nameof(ContactGroup));
                ContactGroupId = body.Id.ThrowIfNull(nameof(ContactGroup.Id));
                ETagAction = ETagAction.IfMatch;
            }

            /// <summary>
            /// Resource Name. To retrieve resource names call the <see cref="IdentityCardResource.List"/> method.
            /// </summary>
            [RequestParameter("resourceName", RequestParameterType.Path)]
            public virtual string ResourceName { get; }

            /// <summary>
            /// Contact Group identifier. To retrieve contact group IDs call the <see cref="List(string)"/> method.
            /// </summary>
            [RequestParameter("contactGroupId", RequestParameterType.Path)]
            public virtual string ContactGroupId { get; }

            /// <summary>
            /// Gets the body of this request.
            /// </summary>
            private ContactGroup Body { get; }

            /// <inheritdoc/>
            public override string MethodName => "insert";

            /// <inheritdoc/>
            public override string HttpMethod => Constants.Put;

            /// <inheritdoc/>
            public override string RestPath => "{resourceName}/{contactGroupId}.vcf";

            /// <inheritdoc/>
            public override string ContentType => Constants.TEXT_VCARD;

            /// <inheritdoc/>
            protected override object GetBody()
            {
                if (_body == null)
                {
                    _body = Body.SerializeToString();
                }
                return _body;
            }

            /// <inheritdoc/>
            protected override void InitParameters()
            {
                base.InitParameters();

                RequestParameters.Add("resourceName", new Parameter("resourceName", "path", true));
                RequestParameters.Add("contactGroupId", new Parameter("contactGroupId", "path", true));
            }
        }

        /// <summary>
        /// Updates an existing contact group on the user's contact group list.
        /// </summary>
        public class UpdateRequest : PeopleBaseServiceRequest<VoidResponse>
        {
            private object _body;

            /// <summary>
            /// Constructs a new Update request.
            /// </summary>
            public UpdateRequest(IClientService service, ContactGroup body, string resourceName) : base(service)
            {
                Body = body.ThrowIfNull(nameof(ContactGroup));
                ContactGroupId = body.Id.ThrowIfNullOrEmpty(nameof(ContactGroup.Id));
                ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(IdentityCard.ResourceName));
                ETagAction = ETagAction.IfMatch;
            }

            /// <summary>
            /// Resource Name. To retrieve resource names call the <see cref="IdentityCardResource.List"/> method.
            /// </summary>
            [RequestParameter("resourceName", RequestParameterType.Path)]
            public virtual string ResourceName { get; }

            /// <summary>
            /// Contact Group identifier. To retrieve contact group IDs call the <see cref="List(string)"/> method.
            /// </summary>
            [RequestParameter("contactGroupId", RequestParameterType.Path)]
            public virtual string ContactGroupId { get; }

            /// <summary>
            /// Gets the body of this request.
            /// </summary>
            private ContactGroup Body { get; }

            /// <inheritdoc/>
            public override string MethodName => "update";

            /// <inheritdoc/>
            public override string HttpMethod => Constants.Put;

            /// <inheritdoc/>
            public override string RestPath => "{resourceName}/{contactGroupId}.vcf";

            /// <inheritdoc/>
            public override string ContentType => Constants.TEXT_VCARD;

            /// <inheritdoc/>
            protected override object GetBody()
            {
                if (_body == null)
                {
                    _body = Body.SerializeToString();
                }
                return _body;
            }

            /// <inheritdoc/>
            protected override void InitParameters()
            {
                base.InitParameters();

                RequestParameters.Add("resourceName", new Parameter("resourceName", "path", true));
                RequestParameters.Add("contactGroupId", new Parameter("contactGroupId", "path", true));
            }
        }

        /// <summary>
        /// Removes a contact group from the user's contact group list.
        /// </summary>
        public class DeleteRequest : PeopleBaseServiceRequest<VoidResponse>
        {
            /// <summary>
            /// Constructs a new Delete request.
            /// </summary>
            public DeleteRequest(IClientService service, string contactGroupId, string resourceName) : base(service)
            {
                ContactGroupId = contactGroupId.ThrowIfNullOrEmpty(nameof(ContactGroup.Id));
                ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(IdentityCard.ResourceName));
            }

            /// <summary>
            /// Resource Name. To retrieve resource names call the <see cref="IdentityCardResource.List"/> method.
            /// </summary>
            [RequestParameter("resourceName", RequestParameterType.Path)]
            public virtual string ResourceName { get; }

            /// <summary>
            /// Contact Group identifier. To retrieve contact group IDs call the <see cref="List(string)"/> method.
            /// </summary>
            [RequestParameter("contactGroupId", RequestParameterType.Path)]
            public virtual string ContactGroupId { get; }

            /// <inheritdoc/>
            public override string MethodName => "delete";

            /// <inheritdoc/>
            public override string HttpMethod => Constants.Delete;

            /// <inheritdoc/>
            public override string RestPath => "{resourceName}/{contactGroupId}.vcf";

            /// <inheritdoc/>
            protected override void InitParameters()
            {
                base.InitParameters();

                RequestParameters.Add("resourceName", new Parameter("resourceName", "path", true));
                RequestParameters.Add("contactGroupId", new Parameter("contactGroupId", "path", true));
            }
        }
    }
}