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
    /// The "ContactGroups" collection of methods.
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
        /// Returns the changes on the user's contact group list.
        /// </summary>
        /// <param name="resourceName">Resource Name. To retrieve resource names call the identityCard.list method.</param>
        public virtual SyncCollectionRequest SyncCollection(string resourceName) => new SyncCollectionRequest(_service, resourceName);

        /// <summary>
        /// Returns the contact groups on the user's contact group list.
        /// </summary>
        /// <param name="resourceName">Resource Name. To retrieve resource names call the identityCard.list method.</param>
        public virtual ListRequest List(string resourceName) => new ListRequest(_service, resourceName);

        /// <summary>
        /// Returns a contact group from the user's contact group list.
        /// </summary>
        /// <param name="uniqueId">Contact Group identifier. To retrieve contact group IDs call the contactGroups.list method.</param>
        /// <param name="resourceName">Resource Name. To retrieve resource names call the identityCard.list method.</param>
        public virtual GetRequest Get(string uniqueId, string resourceName) => new GetRequest(_service, uniqueId, resourceName);

        /// <summary>
        /// Inserts Contact Group into the user's contact group list.
        /// </summary>
        /// <param name="body">The body of the request.</param>
        /// <param name="resourceName">Resource Name. To retrieve resource names call the identityCard.list method.</param>
        public virtual InsertRequest Insert(ContactGroup body, string resourceName) => new InsertRequest(_service, body, resourceName);

        /// <summary>
        /// Updates an existing contact group on the user's contact group list.
        /// </summary>
        /// <param name="body">The body of the request.</param>
        /// <param name="resourceName">Resource Name. To retrieve resource names call the identityCard.list method.</param>
        public virtual UpdateRequest Update(ContactGroup body, string resourceName) => new UpdateRequest(_service, body, resourceName);

        /// <summary>
        /// Removes a contact group from the user's contact group list.
        /// </summary>
        /// <param name="uniqueId">Contact Group identifier. To retrieve contact group IDs call the contactGroups.list method.</param>
        /// <param name="resourceName">Resource Name. To retrieve resource names call the identityCard.list method.</param>
        public virtual DeleteRequest Delete(string uniqueId, string resourceName) => new DeleteRequest(_service, uniqueId, resourceName);

        /// <summary>
        /// Returns the changes on the user's contact group list.
        /// </summary>
        public class SyncCollectionRequest : PeopleBaseServiceRequest<SyncCollectionList>
        {
            private SyncCollection _body;

            /// <summary>
            /// Constructs a new Sync Collection request.
            /// </summary>
            public SyncCollectionRequest(IClientService service, string resourceName) : base(service)
            {
                SyncLevel = "1";
                ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(resourceName));
            }

            /// <summary>
            /// Resource Name. To retrieve resource names call the identityCard.list method.
            /// </summary>
            [RequestParameter("resourceName", RequestParameterType.Path)]
            public virtual string ResourceName { get; }

            /// <summary>
            /// Optional. A sync token, received from a previous response `next_sync_token` Provide
            /// this to retrieve only the resources changed since the last request. When syncing,
            /// all other parameters provided to `people.list` must match the first
            /// call that provided the sync token. More details about sync behavior at `people.list`.
            /// </summary>
            public virtual string SyncToken { get; set; }

            public virtual string SyncLevel { get; set; }

            /// <inheritdoc/>
            public override string MethodName => "list";

            /// <inheritdoc/>
            public override string HttpMethod => Constants.Report;

            /// <inheritdoc/>
            public override string RestPath => "{resourceName}";

            /// <inheritdoc/>
            public override string Depth => "0";

            /// <inheritdoc/>
            protected override object GetBody()
            {
                if (_body == null)
                {
                    _body = new SyncCollection();
                }

                _body.SyncToken = SyncToken;
                _body.SyncLevel = SyncLevel;

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
        /// Returns the contact groups on the user's contact group list.
        /// </summary>
        public class ListRequest : PeopleBaseServiceRequest<ContactGroupList>
        {
            private object _body;

            /// <summary>
            /// Constructs a new List request.
            /// </summary>
            public ListRequest(IClientService service, string resourceName) : base(service) => ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(resourceName));

            /// <summary>
            /// Resource Name. To retrieve resource names call the identityCard.list method.
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
            public GetRequest(IClientService service, string uniqueId, string resourceName) : base(service)
            {
                UniqueId = uniqueId.ThrowIfNullOrEmpty(nameof(uniqueId));
                ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(resourceName));
            }

            /// <summary>
            /// Resource Name. To retrieve resource names call the identityCard.list method.
            /// </summary>
            [RequestParameter("resourceName", RequestParameterType.Path)]
            public virtual string ResourceName { get; }

            /// <summary>
            /// Contact Group identifier. To retrieve contact group IDs call the contactGroups.list method.
            /// </summary>
            [RequestParameter("uniqueId", RequestParameterType.Path)]
            public virtual string UniqueId { get; }

            /// <inheritdoc/>
            public override string MethodName => "get";

            /// <inheritdoc/>
            public override string HttpMethod => Constants.Get;

            /// <inheritdoc/>
            public override string RestPath => "{resourceName}/{uniqueId}.vcf";

            /// <inheritdoc/>
            protected override void InitParameters()
            {
                base.InitParameters();

                RequestParameters.Add("resourceName", new Parameter("resourceName", "path", true));
                RequestParameters.Add("uniqueId", new Parameter("uniqueId", "path", true));
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
                ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(resourceName));
                Body = body.ThrowIfNull(nameof(body));
                UniqueId = body.Id.ThrowIfNull(nameof(body.Id));
                ETagAction = ETagAction.IfMatch;
            }

            /// <summary>
            /// Resource Name. To retrieve resource names call the identityCard.list method.
            /// </summary>
            [RequestParameter("resourceName", RequestParameterType.Path)]
            public virtual string ResourceName { get; }

            /// <summary>
            /// Contact Group identifier. To retrieve contact group IDs call the contactGroups.list method.
            /// </summary>
            [RequestParameter("uniqueId", RequestParameterType.Path)]
            public virtual string UniqueId { get; }

            /// <summary>
            /// Gets the body of this request.
            /// </summary>
            private ContactGroup Body { get; }

            /// <inheritdoc/>
            public override string MethodName => "insert";

            /// <inheritdoc/>
            public override string HttpMethod => Constants.Put;

            /// <inheritdoc/>
            public override string RestPath => "{resourceName}/{uniqueId}.vcf";

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
                RequestParameters.Add("uniqueId", new Parameter("uniqueId", "path", true));
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
                Body = body.ThrowIfNull(nameof(body));
                UniqueId = body.Id.ThrowIfNullOrEmpty(nameof(ContactGroup.Id));
                ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(resourceName));
                ETagAction = ETagAction.IfMatch;
            }

            /// <summary>
            /// Resource Name. To retrieve resource names call the identityCard.list method.
            /// </summary>
            [RequestParameter("resourceName", RequestParameterType.Path)]
            public virtual string ResourceName { get; }

            /// <summary>
            /// Contact Group identifier. To retrieve contact group IDs call the contactGroups.list method.
            /// </summary>
            [RequestParameter("uniqueId", RequestParameterType.Path)]
            public virtual string UniqueId { get; }

            /// <summary>
            /// Gets the body of this request.
            /// </summary>
            private ContactGroup Body { get; }

            /// <inheritdoc/>
            public override string MethodName => "update";

            /// <inheritdoc/>
            public override string HttpMethod => Constants.Put;

            /// <inheritdoc/>
            public override string RestPath => "{resourceName}/{uniqueId}.vcf";

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
                RequestParameters.Add("uniqueId", new Parameter("uniqueId", "path", true));
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
            public DeleteRequest(IClientService service, string uniqueId, string resourceName) : base(service)
            {
                UniqueId = uniqueId.ThrowIfNullOrEmpty(nameof(uniqueId));
                ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(uniqueId));
            }

            /// <summary>
            /// Resource Name. To retrieve resource names call the identityCard.list method.
            /// </summary>
            [RequestParameter("resourceName", RequestParameterType.Path)]
            public virtual string ResourceName { get; }

            /// <summary>
            /// Contact Group identifier. To retrieve contact group IDs call the contactGroups.list method.
            /// </summary>
            [RequestParameter("uniqueId", RequestParameterType.Path)]
            public virtual string UniqueId { get; }

            /// <inheritdoc/>
            public override string MethodName => "delete";

            /// <inheritdoc/>
            public override string HttpMethod => Constants.Delete;

            /// <inheritdoc/>
            public override string RestPath => "{resourceName}/{uniqueId}.vcf";

            /// <inheritdoc/>
            protected override void InitParameters()
            {
                base.InitParameters();

                RequestParameters.Add("resourceName", new Parameter("resourceName", "path", true));
                RequestParameters.Add("uniqueId", new Parameter("uniqueId", "path", true));
            }
        }
    }
}