﻿using iCloud.Dav.Core;
using iCloud.Dav.Core.Response;
using iCloud.Dav.Core.Utils;
using iCloud.Dav.People.CardDav.Types;
using iCloud.Dav.People.Requests;
using iCloud.Dav.People.Serialization.Write;
using iCloud.Dav.People.Types;
using System.IO;
using System.Text;

namespace iCloud.Dav.People.Resources;

/// <summary>The "ContactGroups" collection of methods.</summary>
public class ContactGroupsResource
{
    /// <summary>The service which this resource belongs to.</summary>
    private readonly IClientService _service;

    /// <summary>Constructs a new resource.</summary>
    public ContactGroupsResource(IClientService service) => _service = service;

    /// <summary>Returns the Contact Groups on the user's Contact Group list.</summary>
    /// <param name="resourceName">Resource Name. To retrieve resource names call the identityCard.list method.</param>
    public virtual ListRequest List(string resourceName) => new(_service, resourceName);

    /// <summary>Returns a contact group from the user's contact group list.</summary>
    /// <param name="uniqueId">Contact Group identifier. To retrieve contact group IDs call the contactGroups.list method.</param>
    /// <param name="resourceName">Resource Name. To retrieve resource names call the identityCard.list method.</param>
    public virtual GetRequest Get(string uniqueId, string resourceName) => new(_service, uniqueId, resourceName);

    /// <summary>Inserts Contact Group into the user's contact group list.</summary>
    /// <param name="body">The body of the request.</param>
    /// <param name="resourceName">Resource Name. To retrieve resource names call the identityCard.list method.</param>
    public virtual InsertRequest Insert(ContactGroup body, string resourceName) => new(_service, body, resourceName);

    /// <summary>Updates an existing contact group on the user's contact group list.</summary>
    /// <param name="body">The body of the request.</param>
    /// <param name="resourceName">Resource Name. To retrieve resource names call the identityCard.list method.</param>
    public virtual UpdateRequest Update(ContactGroup body, string resourceName) => new(_service, body, resourceName);

    /// <summary>Removes a contact group from the user's contact group list.</summary>
    /// <param name="uniqueId">Contact Group identifier. To retrieve contact group IDs call the contactGroups.list method.</param>
    /// <param name="resourceName">Resource Name. To retrieve resource names call the identityCard.list method.</param>
    public virtual DeleteRequest Delete(string uniqueId, string resourceName) => new(_service, uniqueId, resourceName);

    /// <summary>Returns the contact groups on the user's contact group list.</summary>
    public class ListRequest : PeopleBaseServiceRequest<ContactGroupsList>
    {
        private object? _body;

        /// <summary>Constructs a new List request.</summary>
        public ListRequest(IClientService service, string resourceName) : base(service) => ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(resourceName));

        /// <summary>Resource Name. To retrieve resource names call the identityCard.list method.</summary>
        [RequestParameter("resourceName", RequestParameterType.Path)]
        public virtual string ResourceName { get; }

        ///<summary>Gets the method name.</summary>
        public override string MethodName => Constants.Report;

        ///<summary>Gets the HTTP method.</summary>
        public override string HttpMethod => Constants.Report;

        ///<summary>Gets the REST path.</summary>
        public override string RestPath => "{resourceName}";

        ///<summary>Gets the depth.</summary>
        public override string Depth => "1";

        ///<summary>Returns the body of the request.</summary>
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
                    MatchType = "contains",
                    SearchText = "group"
                });

                _body = addressBookQuery;
            }
            return _body;
        }

        /// <summary>Initializes List parameter list.</summary>
        protected override void InitParameters()
        {
            base.InitParameters();

            RequestParameters.Add("resourceName", new Parameter("resourceName", "path", true));
        }
    }

    /// <summary>Returns a contact group from the user's contact group list.</summary>
    public class GetRequest : PeopleBaseServiceRequest<ContactGroup>
    {
        /// <summary>Constructs a new Get request.</summary>
        public GetRequest(IClientService service, string uniqueId, string resourceName) : base(service)
        {
            UniqueId = uniqueId.ThrowIfNullOrEmpty(nameof(uniqueId));
            ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(resourceName));
        }

        /// <summary>Resource Name. To retrieve resource names call the identityCard.list method.</summary>
        [RequestParameter("resourceName", RequestParameterType.Path)]
        public virtual string ResourceName { get; }

        /// <summary>Contact Group identifier. To retrieve contact group IDs call the contactGroups.list method.</summary>
        [RequestParameter("uniqueId", RequestParameterType.Path)]
        public virtual string UniqueId { get; }

        ///<summary>Gets the method name.</summary>
        public override string MethodName => "get";

        ///<summary>Gets the HTTP method.</summary>
        public override string HttpMethod => Constants.Get;

        ///<summary>Gets the REST path.</summary>
        public override string RestPath => "{resourceName}/{uniqueId}.vcf";

        /// <summary>Initializes Get parameter list.</summary>
        protected override void InitParameters()
        {
            base.InitParameters();

            RequestParameters.Add("resourceName", new Parameter("resourceName", "path", true));
            RequestParameters.Add("uniqueId", new Parameter("uniqueId", "path", true));
        }
    }

    /// <summary>Inserts an existing contact group into the user's contact group list.</summary>
    public class InsertRequest : PeopleBaseServiceRequest<VoidResponse>
    {
        private object? _body;

        /// <summary>Constructs a new Insert request.</summary>
        public InsertRequest(IClientService service, ContactGroup body, string resourceName) : base(service)
        {
            ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(resourceName));
            Body = body.ThrowIfNull(nameof(body));
            UniqueId = body.UniqueId.ThrowIfNull(nameof(body.UniqueId));
            ETagAction = ETagAction.IfMatch;
        }

        /// <summary>Resource Name. To retrieve resource names call the identityCard.list method.</summary>
        [RequestParameter("resourceName", RequestParameterType.Path)]
        public virtual string ResourceName { get; }

        /// <summary>Contact Group identifier. To retrieve contact group IDs call the contactGroups.list method.</summary>
        [RequestParameter("uniqueId", RequestParameterType.Path)]
        public virtual string UniqueId { get; }

        /// <summary>Gets the body of this request.</summary>
        private ContactGroup Body { get; }

        ///<summary>Gets the method name.</summary>
        public override string MethodName => "insert";

        ///<summary>Gets the HTTP method.</summary>
        public override string HttpMethod => Constants.Put;

        ///<summary>Gets the REST path.</summary>
        public override string RestPath => "{resourceName}/{uniqueId}.vcf";

        ///<summary>Gets the Content-Type.</summary>
        public override string ContentType => Constants.TEXT_VCARD;

        ///<summary>Returns the body of the request.</summary>
        protected override object GetBody()
        {
            if (_body is null)
            {
                using var stream = new MemoryStream();
                using var textWriter = new StreamWriter(stream);
                var writer = new ContactGroupWriter();
                writer.Write(Body, textWriter, Encoding.UTF8.WebName);
                textWriter.Flush();

                stream.Seek(0, SeekOrigin.Begin);
                using var streamReader = new StreamReader(stream);
                _body = streamReader.ReadToEnd();
            }
            return _body;
        }

        /// <summary>Initializes Insert parameter list.</summary>
        protected override void InitParameters()
        {
            base.InitParameters();

            RequestParameters.Add("resourceName", new Parameter("resourceName", "path", true));
            RequestParameters.Add("uniqueId", new Parameter("uniqueId", "path", true));
        }
    }

    /// <summary>Updates an existing contact group on the user's contact group list.</summary>
    public class UpdateRequest : PeopleBaseServiceRequest<VoidResponse>
    {
        private object? _body;

        /// <summary>Constructs a new Update request.</summary>
        public UpdateRequest(IClientService service, ContactGroup body, string resourceName) : base(service)
        {
            Body = body.ThrowIfNull(nameof(body));
            UniqueId = body.UniqueId.ThrowIfNullOrEmpty(nameof(ContactGroup.UniqueId));
            ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(resourceName));
            ETagAction = ETagAction.IfMatch;
        }

        /// <summary>Resource Name. To retrieve resource names call the identityCard.list method.</summary>
        [RequestParameter("resourceName", RequestParameterType.Path)]
        public virtual string ResourceName { get; }

        /// <summary>Contact Group identifier. To retrieve contact group IDs call the contactGroups.list method.</summary>
        [RequestParameter("uniqueId", RequestParameterType.Path)]
        public virtual string UniqueId { get; }

        /// <summary>Gets the body of this request.</summary>
        private ContactGroup Body { get; }

        ///<summary>Gets the method name.</summary>
        public override string MethodName => "update";

        ///<summary>Gets the HTTP method.</summary>
        public override string HttpMethod => Constants.Put;

        ///<summary>Gets the REST path.</summary>
        public override string RestPath => "{resourceName}/{uniqueId}.vcf";

        ///<summary>Gets the Content-Type.</summary>
        public override string ContentType => Constants.TEXT_VCARD;

        ///<summary>Returns the body of the request.</summary>
        protected override object GetBody()
        {
            if (_body is null)
            {
                using var stream = new MemoryStream();
                using var textWriter = new StreamWriter(stream);
                var writer = new ContactGroupWriter();
                writer.Write(Body, textWriter, Encoding.UTF8.WebName);
                textWriter.Flush();

                stream.Seek(0, SeekOrigin.Begin);
                using var streamReader = new StreamReader(stream);
                _body = streamReader.ReadToEnd();
            }
            return _body;
        }

        /// <summary>Initializes Update parameter list.</summary>
        protected override void InitParameters()
        {
            base.InitParameters();

            RequestParameters.Add("resourceName", new Parameter("resourceName", "path", true));
            RequestParameters.Add("uniqueId", new Parameter("uniqueId", "path", true));
        }
    }

    /// <summary>Removes a contact group from the user's contact group list.</summary>
    public class DeleteRequest : PeopleBaseServiceRequest<VoidResponse>
    {
        /// <summary>Constructs a new Delete request.</summary>
        public DeleteRequest(IClientService service, string uniqueId, string resourceName) : base(service)
        {
            UniqueId = uniqueId.ThrowIfNullOrEmpty(nameof(uniqueId));
            ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(uniqueId));
        }

        /// <summary>Resource Name. To retrieve resource names call the identityCard.list method.</summary>
        [RequestParameter("resourceName", RequestParameterType.Path)]
        public virtual string ResourceName { get; }

        /// <summary>Contact Group identifier. To retrieve contact group IDs call the contactGroups.list method.</summary>
        [RequestParameter("uniqueId", RequestParameterType.Path)]
        public virtual string UniqueId { get; }

        ///<summary>Gets the method name.</summary>
        public override string MethodName => "delete";

        ///<summary>Gets the HTTP method.</summary>
        public override string HttpMethod => Constants.Delete;

        ///<summary>Gets the REST path.</summary>
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
