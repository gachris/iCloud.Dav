using iCloud.Dav.Core;
using iCloud.Dav.Core.Response;
using iCloud.Dav.Core.Utils;
using iCloud.Dav.People.CardDav.Types;
using iCloud.Dav.People.Requests;
using iCloud.Dav.People.Serialization.Write;
using iCloud.Dav.People.Types;
using System.IO;
using System.Text;

namespace iCloud.Dav.People.Resources;

/// <summary>The "people" collection of methods.</summary>
public class PeopleResource
{
    /// <summary>The service which this resource belongs to.</summary>
    private readonly IClientService _service;

    /// <summary>Constructs a new resource.</summary>
    public PeopleResource(IClientService service) => _service = service;

    /// <summary>Returns the peoples on the user's people list.</summary>
    /// <param name="resourceName">Resource Name. To retrieve resource names call the identityCard.list method.</param>
    public virtual ListRequest List(string resourceName) => new(_service, resourceName);

    /// <summary>Returns a people from the user's people list.</summary>
    /// <param name="uniqueId">People identifier. To retrieve people IDs call the people.list method.</param>
    /// <param name="resourceName">Resource Name. To retrieve resource names call the identityCard.list method.</param>
    public virtual GetRequest Get(string uniqueId, string resourceName) => new(_service, uniqueId, resourceName);

    /// <summary>Inserts an existing people into the user's people list.</summary>
    /// <param name="body">The body of the request.</param>
    /// <param name="resourceName">Resource Name. To retrieve resource names call the identityCard.list method.</param>
    public virtual InsertRequest Insert(Contact body, string resourceName) => new(_service, body, resourceName);

    /// <summary>Updates an existing people on the user's people list.</summary>
    /// <param name="body">The body of the request.</param>
    /// <param name="resourceName">Resource Name. To retrieve resource names call the identityCard.list method.</param>
    public virtual UpdateRequest Update(Contact body, string resourceName) => new(_service, body, resourceName);

    /// <summary>Removes a people from the user's people list.</summary>
    /// <param name="uniqueId">People identifier. To retrieve people IDs call the people.list method.</param>
    /// <param name="resourceName">Resource Name. To retrieve resource names call the identityCard.list method.</param>
    public virtual DeleteRequest Delete(string uniqueId, string resourceName) => new(_service, uniqueId, resourceName);

    /// <summary>Returns the peoples on the user's people list.</summary>
    public class ListRequest : PeopleBaseServiceRequest<ContactList>
    {
        private object? _body;

        /// <summary>Constructs a new List request.</summary>
        public ListRequest(IClientService service, string resourceName) : base(service) => ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(resourceName));

        /// <summary>Resource Name. To retrieve resource names call the identityCard.list method.</summary>
        [RequestParameter("resourceName", RequestParameterType.Path)]
        public virtual string ResourceName { get; }

        ///<summary>Gets the method name.</summary>
        public override string MethodName => "list";

        ///<summary>Gets the HTTP method.</summary>
        public override string HttpMethod => Constants.Report;

        ///<summary>Gets the REST path.</summary>
        public override string RestPath => "{resourceName}";

        ///<summary>Gets the depth.</summary>
        public override string Depth => "1";

        ///<summary>Returns the body of the request.</summary>
        protected override object GetBody() => _body ??= new AddressBookQuery();

        /// <summary>Initializes List parameter list.</summary>
        protected override void InitParameters()
        {
            base.InitParameters();

            RequestParameters.Add("resourceName", new Parameter("resourceName", "path", true));
        }
    }

    /// <summary>Returns a people from the user's people list.</summary>
    public class GetRequest : PeopleBaseServiceRequest<Contact>
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

        /// <summary>People identifier. To retrieve people IDs call the people.list method.</summary>
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

    /// <summary>Inserts an existing people into the user's people list.</summary>
    public class InsertRequest : PeopleBaseServiceRequest<VoidResponse>
    {
        private object? _body;

        /// <summary>Constructs a new Insert request.</summary>
        public InsertRequest(IClientService service, Contact body, string resourceName) : base(service)
        {
            Body = body.ThrowIfNull(nameof(body));
            UniqueId = body.UniqueId.ThrowIfNull(nameof(body.UniqueId));
            ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(resourceName));
            ETagAction = ETagAction.IfNoneMatch;
        }

        /// <summary>Resource Name. To retrieve resource names call the identityCard.list method.</summary>
        [RequestParameter("resourceName", RequestParameterType.Path)]
        public virtual string ResourceName { get; }

        /// <summary>People identifier. To retrieve people IDs call the people.list method.</summary>
        [RequestParameter("uniqueId", RequestParameterType.Path)]
        public virtual string UniqueId { get; }

        /// <summary>Gets the body of this request.</summary>
        private Contact Body { get; }

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
                var writer = new ContactWriter();
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

    /// <summary>Updates an existing people on the user's people list.</summary>
    public class UpdateRequest : PeopleBaseServiceRequest<VoidResponse>
    {
        private object? _body;

        /// <summary>Constructs a new Update request.</summary>
        public UpdateRequest(IClientService service, Contact body, string resourceName) : base(service)
        {
            ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(resourceName));
            Body = body.ThrowIfNull(nameof(body));
            UniqueId = body.UniqueId.ThrowIfNullOrEmpty(nameof(body.UniqueId));
            ETagAction = ETagAction.IfMatch;
        }

        /// <summary>Resource Name. To retrieve resource names call the identityCard.list method.</summary>
        [RequestParameter("resourceName", RequestParameterType.Path)]
        public virtual string ResourceName { get; }

        /// <summary>People identifier. To retrieve people IDs call the people.list method.</summary>
        [RequestParameter("uniqueId", RequestParameterType.Path)]
        public virtual string UniqueId { get; }

        /// <summary>Gets the body of this request.</summary>
        private Contact Body { get; }

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
                var writer = new ContactWriter();
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

    /// <summary>Removes a people from the user's people list.</summary>
    public class DeleteRequest : PeopleBaseServiceRequest<VoidResponse>
    {
        /// <summary>Constructs a new Delete request.</summary>
        public DeleteRequest(IClientService service, string uniqueId, string resourceName) : base(service)
        {
            UniqueId = uniqueId.ThrowIfNullOrEmpty(nameof(uniqueId));
            ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(resourceName));
        }

        /// <summary>Resource Name. To retrieve resource names call the identityCard.list method.</summary>
        [RequestParameter("resourceName", RequestParameterType.Path)]
        public virtual string ResourceName { get; }

        /// <summary>People identifier. To retrieve people IDs call the people.list method.</summary>
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
