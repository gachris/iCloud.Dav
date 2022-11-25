﻿using iCloud.Dav.Core;
using iCloud.Dav.Core.Response;
using iCloud.Dav.Core.Utils;
using iCloud.Dav.People.CardDav.Types;
using iCloud.Dav.People.DataTypes;
using iCloud.Dav.People.PeopleComponents;
using iCloud.Dav.People.Requests;
using iCloud.Dav.People.Utils;

namespace iCloud.Dav.People.Resources;

/// <summary>
/// The people collection of methods.
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
    /// <param name="resourceName">Resource Name. To retrieve resource names call the identityCard.list method.</param>
    public virtual ListRequest List(string resourceName) => new(_service, resourceName);

    /// <summary>
    /// Returns a people from the user's people list.
    /// </summary>
    /// <param name="uniqueId">People identifier. To retrieve people IDs call the people.list method.</param>
    /// <param name="resourceName">Resource Name. To retrieve resource names call the identityCard.list method.</param>
    public virtual GetRequest Get(string uniqueId, string resourceName) => new(_service, uniqueId, resourceName);

    /// <summary>Inserts an existing people into the user's people list.</summary>
    /// <param name="body">The body of the request.</param>
    /// <param name="resourceName">Resource Name. To retrieve resource names call the identityCard.list method.</param>
    public virtual InsertRequest Insert(Contact body, string resourceName) => new(_service, body, resourceName);

    /// <summary>
    /// Updates an existing people on the user's people list.
    /// </summary>
    /// <param name="body">The body of the request.</param>
    /// <param name="resourceName">Resource Name. To retrieve resource names call the identityCard.list method.</param>
    public virtual UpdateRequest Update(Contact body, string resourceName) => new(_service, body, resourceName);

    /// <summary>
    /// Removes a people from the user's people list.
    /// </summary>
    /// <param name="uniqueId">People identifier. To retrieve people IDs call the people.list method.</param>
    /// <param name="resourceName">Resource Name. To retrieve resource names call the identityCard.list method.</param>
    public virtual DeleteRequest Delete(string uniqueId, string resourceName) => new(_service, uniqueId, resourceName);

    /// <summary>
    /// Returns the peoples on the user's people list.
    /// </summary>
    public class ListRequest : PeopleBaseServiceRequest<ContactList>
    {
        private object? _body;

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
    /// Returns a people from the user's people list.
    /// </summary>
    public class GetRequest : PeopleBaseServiceRequest<Contact>
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
        /// People identifier. To retrieve people IDs call the people.list method.
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
    /// Inserts an existing people into the user's people list.
    /// </summary>
    public class InsertRequest : PeopleBaseServiceRequest<VoidResponse>
    {
        private object? _body;

        /// <summary>
        /// Constructs a new Insert request.
        /// </summary>
        public InsertRequest(IClientService service, Contact body, string resourceName) : base(service)
        {
            Body = body.ThrowIfNull(nameof(body));
            UniqueId = body.Uid.ThrowIfNull(nameof(body.Uid));
            ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(resourceName));
            ETagAction = ETagAction.IfNoneMatch;
        }

        /// <summary>
        /// Resource Name. To retrieve resource names call the identityCard.list method.
        /// </summary>
        [RequestParameter("resourceName", RequestParameterType.Path)]
        public virtual string ResourceName { get; }

        /// <summary>
        /// People identifier.
        /// </summary>
        [RequestParameter("uniqueId", RequestParameterType.Path)]
        public virtual string UniqueId { get; }

        /// <summary>
        /// Gets the body of this request.
        /// </summary>
        private Contact Body { get; }

        /// <inheritdoc/>
        public override string MethodName => "insert";

        /// <inheritdoc/>
        public override string HttpMethod => Constants.Put;

        /// <inheritdoc/>
        public override string RestPath => "{resourceName}/{uniqueId}.vcf";

        /// <inheritdoc/>
        public override string ContentType => Constants.TEXT_VCARD;

        /// <inheritdoc/>
        protected override object GetBody() => _body ??= Body.SerializeToString();

        /// <inheritdoc/>
        protected override void InitParameters()
        {
            base.InitParameters();

            RequestParameters.Add("resourceName", new Parameter("resourceName", "path", true));
            RequestParameters.Add("uniqueId", new Parameter("uniqueId", "path", true));
        }
    }

    /// <summary>
    /// Updates an existing people on the user's people list.
    /// </summary>
    public class UpdateRequest : PeopleBaseServiceRequest<VoidResponse>
    {
        private object? _body;

        /// <summary>
        /// Constructs a new Update request.
        /// </summary>
        public UpdateRequest(IClientService service, Contact body, string resourceName) : base(service)
        {
            ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(resourceName));
            Body = body.ThrowIfNull(nameof(body));
            UniqueId = body.Uid.ThrowIfNullOrEmpty(nameof(body.Uid));
            ETagAction = ETagAction.IfMatch;
        }

        /// <summary>
        /// Resource Name. To retrieve resource names call the identityCard.list method.
        /// </summary>
        [RequestParameter("resourceName", RequestParameterType.Path)]
        public virtual string ResourceName { get; }

        /// <summary>
        /// People identifier. To retrieve people IDs call the people.list method.
        /// </summary>
        [RequestParameter("uniqueId", RequestParameterType.Path)]
        public virtual string UniqueId { get; }

        /// <summary>
        /// Gets the body of this request.
        /// </summary>
        private Contact Body { get; }

        /// <inheritdoc/>
        public override string MethodName => "update";

        /// <inheritdoc/>
        public override string HttpMethod => Constants.Put;

        /// <inheritdoc/>
        public override string RestPath => "{resourceName}/{uniqueId}.vcf";

        /// <inheritdoc/>
        public override string ContentType => Constants.TEXT_VCARD;

        /// <inheritdoc/>
        protected override object GetBody() => _body ??= Body.SerializeToString();

        /// <summary>Initializes Update parameter list.</summary>
        protected override void InitParameters()
        {
            base.InitParameters();

            RequestParameters.Add("resourceName", new Parameter("resourceName", "path", true));
            RequestParameters.Add("uniqueId", new Parameter("uniqueId", "path", true));
        }
    }

    /// <summary>
    /// Removes a people from the user's people list.
    /// </summary>
    public class DeleteRequest : PeopleBaseServiceRequest<VoidResponse>
    {
        /// <summary>
        /// Constructs a new Delete request.
        /// </summary>
        public DeleteRequest(IClientService service, string uniqueId, string resourceName) : base(service)
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
        /// People identifier. To retrieve people IDs call the people.list method.
        /// </summary>
        [RequestParameter("uniqueId", RequestParameterType.Path)]
        public virtual string UniqueId { get; }

        /// <inheritdoc/>
        public override string MethodName => "delete";

        /// <inheritdoc/>
        public override string HttpMethod => Constants.Delete;

        /// <inheritdoc/>
        public override string RestPath => "{resourceName}/{uniqueId}.vcf";

        /// <summary>Initializes Delete parameter list.</summary>
        protected override void InitParameters()
        {
            base.InitParameters();

            RequestParameters.Add("resourceName", new Parameter("resourceName", "path", true));
            RequestParameters.Add("uniqueId", new Parameter("uniqueId", "path", true));
        }
    }
}
