using iCloud.Dav.Core;
using iCloud.Dav.Core.Response;
using iCloud.Dav.Core.Utils;
using iCloud.Dav.People.CardDav.Types;
using iCloud.Dav.People.DataTypes;
using iCloud.Dav.People.Requests;
using iCloud.Dav.People.Utils;
using System;
using System.Linq;

namespace iCloud.Dav.People.Resources
{
    /// <summary>
    /// The "People" collection of methods.
    /// </summary>
    public class PeopleResource
    {
        /// <summary>
        /// The service which this resource belongs to.
        /// </summary>
        private readonly IClientService _service;

        /// <summary>
        /// Constructs a new resource.
        /// </summary>
        public PeopleResource(IClientService service) => _service = service;

        /// <summary>
        /// Returns the peoples on the user's people list.
        /// </summary>
        /// <param name="resourceName">Resource Name. To retrieve resource names call the <see cref="IdentityCardResource.List"/> method.</param>
        public virtual ListRequest List(string resourceName) => new ListRequest(_service, resourceName);

        /// <summary>
        /// Returns a contact from the user's people list.
        /// </summary>
        /// <param name="contactId">Contact identifier. To retrieve contact IDs call the <see cref="List(string)"/> method.</param>
        /// <param name="resourceName">Resource Name. To retrieve resource names call the <see cref="IdentityCardResource.List"/> method.</param>
        public virtual GetRequest Get(string contactId, string resourceName) => new GetRequest(_service, contactId, resourceName);

        /// <summary>
        /// Returns events on the specified calendar.
        /// </summary>
        /// <param name="resourceName">Resource Name. To retrieve resource names call the <see cref="IdentityCardResource.List"/> method.</param>
        /// <param name="contactIds">Contact identifiers. To retrieve contact IDs call the <see cref="List(string)"/> method.</param>
        public virtual MultiGetRequest MultiGet(string resourceName, string[] contactIds) => new MultiGetRequest(_service, resourceName, contactIds);

        /// <summary>Inserts an existing contact into the user's people list.</summary>
        /// <param name="body">The body of the request.</param>
        /// <param name="resourceName">Resource Name. To retrieve resource names call the <see cref="IdentityCardResource.List"/> method.</param>
        public virtual InsertRequest Insert(Contact body, string resourceName) => new InsertRequest(_service, body, resourceName);

        /// <summary>
        /// Updates an existing contact on the user's people list.
        /// </summary>
        /// <param name="body">The body of the request.</param>
        /// <param name="resourceName">Resource Name. To retrieve resource names call the <see cref="IdentityCardResource.List"/> method.</param>
        public virtual UpdateRequest Update(Contact body, string resourceName) => new UpdateRequest(_service, body, resourceName);

        /// <summary>
        /// Removes a contact from the user's people list.
        /// </summary>
        /// <param name="contactId">Contact identifier. To retrieve contact IDs call the <see cref="List(string)"/> method.</param>
        /// <param name="resourceName">Resource Name. To retrieve resource names call the <see cref="IdentityCardResource.List"/> method.</param>
        public virtual DeleteRequest Delete(string contactId, string resourceName) => new DeleteRequest(_service, contactId, resourceName);

        /// <summary>
        /// Returns the peoples on the user's people list.
        /// </summary>
        public class ListRequest : PeopleBaseServiceRequest<ContactList>
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
            public override string MethodName => "list";

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
                            Name = "X-ADDRESSBOOKSERVER-KIND",
                            IsNotDefined = true,
                        }
                    };

                    addressBookQuery.Filter.TextMatches.Add(new TextMatch
                    {
                        Collation = "i;unicode-casemap",
                        NegateCondition = "yes",
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
        /// Returns a contact from the user's people list.
        /// </summary>
        public class GetRequest : PeopleBaseServiceRequest<Contact>
        {
            /// <summary>
            /// Constructs a new Get request.
            /// </summary>
            public GetRequest(IClientService service, string contactId, string resourceName) : base(service)
            {
                ContactId = contactId.ThrowIfNullOrEmpty(nameof(Contact.Id));
                ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(IdentityCard.ResourceName));
            }

            /// <summary>
            /// Resource Name. To retrieve resource names call the <see cref="IdentityCardResource.List"/> method.
            /// </summary>
            [RequestParameter("resourceName", RequestParameterType.Path)]
            public virtual string ResourceName { get; }

            /// <summary>
            /// Contact identifier. To retrieve contact IDs call the <see cref="List(string)"/> method.
            /// </summary>
            [RequestParameter("contactId", RequestParameterType.Path)]
            public virtual string ContactId { get; }

            /// <inheritdoc/>
            public override string MethodName => "get";

            /// <inheritdoc/>
            public override string HttpMethod => Constants.Get;

            /// <inheritdoc/>
            public override string RestPath => "{resourceName}/{contactId}.vcf";

            /// <inheritdoc/>
            protected override void InitParameters()
            {
                base.InitParameters();

                RequestParameters.Add("resourceName", new Parameter("resourceName", "path", true));
                RequestParameters.Add("contactId", new Parameter("contactId", "path", true));
            }
        }

        /// <summary>
        /// Returns contacts from the user's people list.
        /// </summary>
        public class MultiGetRequest : PeopleBaseServiceRequest<ContactList>
        {
            private AddressBookMultiget _body;

            /// <summary>
            /// Constructs a new MultiGet request.
            /// </summary>
            public MultiGetRequest(IClientService service, string resourceName, string[] contactIds) : base(service)
            {
                ContactIds = contactIds.ThrowIfNull(nameof(contactIds));
                ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(IdentityCard.ResourceName));
            }

            /// <summary>
            /// Resource Name. To retrieve resource names call the <see cref="IdentityCardResource.List"/> method.
            /// </summary>
            [RequestParameter("resourceName", RequestParameterType.Path)]
            public virtual string ResourceName { get; }

            /// <summary>
            /// Contact identifiers. To retrieve contact IDs call the <see cref="List(string)"/> method.
            /// </summary>
            public virtual string[] ContactIds { get; }

            /// <inheritdoc/>
            public override string MethodName => "get";

            /// <inheritdoc/>
            public override string HttpMethod => Constants.Report;

            /// <inheritdoc/>
            public override string RestPath => "{resourceName}";

            /// <inheritdoc/>
            public override string Depth => "1";

            /// <inheritdoc/>
            protected override object GetBody()
            {
                if (_body == null)
                {
                    _body = new AddressBookMultiget();
                }

                _body.Href.Clear();
                _body.Href.AddRange(ContactIds.Select(contactId =>
                new Uri(Service.HttpClientInitializer.GetUri(PrincipalHomeSet.AddressBook), string.Concat(ResourceName, "/", contactId, ".vcf")).AbsolutePath));

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
        /// Inserts an existing contact into the user's people list.
        /// </summary>
        public class InsertRequest : PeopleBaseServiceRequest<VoidResponse>
        {
            private object _body;

            /// <summary>
            /// Constructs a new Insert request.
            /// </summary>
            public InsertRequest(IClientService service, Contact body, string resourceName) : base(service)
            {
                Body = body.ThrowIfNull(nameof(Contact));
                ContactId = body.Id.ThrowIfNull(nameof(Contact.Id));
                ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(IdentityCard.ResourceName));
                ETagAction = ETagAction.IfNoneMatch;
            }

            /// <summary>
            /// Resource Name. To retrieve resource names call the <see cref="IdentityCardResource.List"/> method.
            /// </summary>
            [RequestParameter("resourceName", RequestParameterType.Path)]
            public virtual string ResourceName { get; }

            /// <summary>
            /// Contact identifier. To retrieve contact IDs call the <see cref="List(string)"/> method.
            /// </summary>
            [RequestParameter("contactId", RequestParameterType.Path)]
            public virtual string ContactId { get; }

            /// <summary>
            /// Gets the body of this request.
            /// </summary>
            private Contact Body { get; }

            /// <inheritdoc/>
            public override string MethodName => "insert";

            /// <inheritdoc/>
            public override string HttpMethod => Constants.Put;

            /// <inheritdoc/>
            public override string RestPath => "{resourceName}/{contactId}.vcf";

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
                RequestParameters.Add("contactId", new Parameter("contactId", "path", true));
            }
        }

        /// <summary>
        /// Updates an existing contact on the user's people list.
        /// </summary>
        public class UpdateRequest : PeopleBaseServiceRequest<VoidResponse>
        {
            private object _body;

            /// <summary>
            /// Constructs a new Update request.
            /// </summary>
            public UpdateRequest(IClientService service, Contact body, string resourceName) : base(service)
            {
                ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(IdentityCard.ResourceName));
                Body = body.ThrowIfNull(nameof(Contact));
                ContactId = body.Id.ThrowIfNullOrEmpty(nameof(Contact.Id));
                ETagAction = ETagAction.IfMatch;
            }

            /// <summary>
            /// Resource Name. To retrieve resource names call the <see cref="IdentityCardResource.List"/> method.
            /// </summary>
            [RequestParameter("resourceName", RequestParameterType.Path)]
            public virtual string ResourceName { get; }

            /// <summary>
            /// Contact identifier. To retrieve contact IDs call the <see cref="List(string)"/> method.
            /// </summary>
            [RequestParameter("contactId", RequestParameterType.Path)]
            public virtual string ContactId { get; }

            /// <summary>
            /// Gets the body of this request.
            /// </summary>
            private Contact Body { get; }

            /// <inheritdoc/>
            public override string MethodName => "update";

            /// <inheritdoc/>
            public override string HttpMethod => Constants.Put;

            /// <inheritdoc/>
            public override string RestPath => "{resourceName}/{contactId}.vcf";

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

            /// <summary>Initializes Update parameter list.</summary>
            protected override void InitParameters()
            {
                base.InitParameters();

                RequestParameters.Add("resourceName", new Parameter("resourceName", "path", true));
                RequestParameters.Add("contactId", new Parameter("contactId", "path", true));
            }
        }

        /// <summary>
        /// Removes a contact from the user's people list.
        /// </summary>
        public class DeleteRequest : PeopleBaseServiceRequest<VoidResponse>
        {
            /// <summary>
            /// Constructs a new Delete request.
            /// </summary>
            public DeleteRequest(IClientService service, string contactId, string resourceName) : base(service)
            {
                ContactId = contactId.ThrowIfNullOrEmpty(nameof(Contact.Id));
                ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(IdentityCard.ResourceName));
            }

            /// <summary>
            /// Resource Name. To retrieve resource names call the <see cref="IdentityCardResource.List"/> method.
            /// </summary>
            [RequestParameter("resourceName", RequestParameterType.Path)]
            public virtual string ResourceName { get; }

            /// <summary>
            /// Contact identifier. To retrieve contact IDs call the <see cref="List(string)"/> method.
            /// </summary>
            [RequestParameter("contactId", RequestParameterType.Path)]
            public virtual string ContactId { get; }

            /// <inheritdoc/>
            public override string MethodName => "delete";

            /// <inheritdoc/>
            public override string HttpMethod => Constants.Delete;

            /// <inheritdoc/>
            public override string RestPath => "{resourceName}/{contactId}.vcf";

            /// <summary>Initializes Delete parameter list.</summary>
            protected override void InitParameters()
            {
                base.InitParameters();

                RequestParameters.Add("resourceName", new Parameter("resourceName", "path", true));
                RequestParameters.Add("contactId", new Parameter("contactId", "path", true));
            }
        }
    }
}