using iCloud.Dav.Core.Attributes;
using iCloud.Dav.Core.Enums;
using iCloud.Dav.Core.Response;
using iCloud.Dav.Core.Services;
using iCloud.Dav.Core.Utils;
using iCloud.Dav.People.CardDav.Types;
using iCloud.Dav.People.Request;
using iCloud.Dav.People.Utils;
using System;
using System.IO;
using System.Text;

namespace iCloud.Dav.People.Resources
{
    /// <summary>The "people" collection of methods.</summary>
    public class PeopleResource
    {
        /// <summary>The service which this resource belongs to.</summary>
        private readonly IClientService _service;

        /// <summary>Constructs a new resource.</summary>
        public PeopleResource(IClientService service)
        {
            _service = service;
        }

        /// <summary>Returns the peoples on the user's people list.</summary>
        /// <param name="resourceName">Resource Name. To retrieve resource names call the identityCard.list method.</param>
        public virtual ListRequest List(string resourceName)
        {
            return new ListRequest(_service, resourceName);
        }

        /// <summary>Returns a people from the user's people list.</summary>
        /// <param name="uniqueId">People identifier. To retrieve people IDs call the people.list method.</param>
        /// <param name="resourceName">Resource Name. To retrieve resource names call the identityCard.list method.</param>
        public virtual GetRequest Get(string uniqueId, string resourceName)
        {
            return new GetRequest(_service, uniqueId, resourceName);
        }

        /// <summary>Inserts an existing people into the user's people list.</summary>
        /// <param name="body">The body of the request.</param>
        /// <param name="resourceName">Resource Name. To retrieve resource names call the identityCard.list method.</param>
        public virtual InsertRequest Insert(Person body, string resourceName)
        {
            return new InsertRequest(_service, body, resourceName);
        }

        /// <summary>Updates an existing people on the user's people list.</summary>
        /// <param name="body">The body of the request.</param>
        /// <param name="resourceName">Resource Name. To retrieve resource names call the identityCard.list method.</param>
        public virtual UpdateRequest Update(Person body, string resourceName)
        {
            return new UpdateRequest(_service, body, resourceName);
        }

        /// <summary>Removes a people from the user's people list.</summary>
        /// <param name="uniqueId">People identifier. To retrieve people IDs call the people.list method.</param>
        /// <param name="resourceName">Resource Name. To retrieve resource names call the identityCard.list method.</param>
        public virtual DeleteRequest Delete(string uniqueId, string resourceName)
        {
            return new DeleteRequest(_service, uniqueId, resourceName);
        }

        /// <summary>Returns the peoples on the user's people list.</summary>
        public class ListRequest : PeopleBaseServiceRequest<PersonList>
        {
            /// <summary>Constructs a new List request.</summary>
            public ListRequest(IClientService service, string resourceName) : base(service)
            {
                ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(resourceName));
                InitParameters();
            }

            /// <summary>Resource Name. To retrieve resource names call the identityCard.list method.</summary>
            [RequestParameter("resourceName", RequestParameterType.Path)]
            public virtual string ResourceName { get; }

            ///<summary>Gets the method name.</summary>
            public override string MethodName => "list";

            ///<summary>Gets the HTTP method.</summary>
            public override string HttpMethod => ApiMethod.REPORT;

            ///<summary>Gets the REST path.</summary>
            public override string RestPath => "{resourceName}";

            ///<summary>Gets the depth.</summary>
            public override string Depth => "1";

            ///<summary>Returns the body of the request.</summary>
            protected override object GetBody() => new AddressBookQuery();

            /// <summary>Initializes List parameter list.</summary>
            protected override void InitParameters()
            {
                base.InitParameters();

                RequestParameters.Add(
                    "resourceName", new Parameter
                    {
                        Name = "resourceName",
                        IsRequired = true,
                        ParameterType = "path",
                        DefaultValue = null,
                        Pattern = null,
                    });
            }
        }

        /// <summary>Returns a people from the user's people list.</summary>
        public class GetRequest : PeopleBaseServiceRequest<Person>
        {
            /// <summary>Constructs a new Get request.</summary>
            public GetRequest(IClientService service, string uniqueId, string resourceName) : base(service)
            {
                UniqueId = uniqueId.ThrowIfNullOrEmpty(nameof(uniqueId));
                ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(resourceName));
                InitParameters();
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
            public override string HttpMethod => ApiMethod.GET;

            ///<summary>Gets the REST path.</summary>
            public override string RestPath => "{resourceName}/{uniqueId}.vcf";

            /// <summary>Initializes Get parameter list.</summary>
            protected override void InitParameters()
            {
                base.InitParameters();

                RequestParameters.Add("resourceName", new Parameter
                {
                    Name = "resourceName",
                    IsRequired = true,
                    ParameterType = "path",
                });
                RequestParameters.Add("uniqueId", new Parameter
                {
                    Name = "uniqueId",
                    IsRequired = true,
                    ParameterType = "path",
                });
            }
        }

        /// <summary>Inserts an existing people into the user's people list.</summary>
        public class InsertRequest : PeopleBaseServiceRequest<InsertResponseObject>
        {
            /// <summary>Constructs a new Insert request.</summary>
            public InsertRequest(IClientService service, Person body, string resourceName) : base(service)
            {
                Body = body.ThrowIfNull(nameof(body));
                if (string.IsNullOrEmpty(Body.UniqueId))
                    Body.UniqueId = Guid.NewGuid().ToString().ToUpper();
                UniqueId = Body.UniqueId;
                ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(resourceName));
                ETagAction = ETagAction.IfNoneMatch;
                InitParameters();
            }

            /// <summary>Resource Name. To retrieve resource names call the identityCard.list method.</summary>
            [RequestParameter("resourceName", RequestParameterType.Path)]
            public virtual string ResourceName { get; }

            /// <summary>People identifier. To retrieve people IDs call the people.list method.</summary>
            [RequestParameter("uniqueId", RequestParameterType.Path)]
            public virtual string UniqueId { get; }

            /// <summary>Gets the body of this request.</summary>
            private Person Body { get; }

            ///<summary>Gets the method name.</summary>
            public override string MethodName => "insert";

            ///<summary>Gets the HTTP method.</summary>
            public override string HttpMethod => ApiMethod.PUT;

            ///<summary>Gets the REST path.</summary>
            public override string RestPath => "{resourceName}/{uniqueId}.vcf";

            ///<summary>Gets the Content-Type.</summary>
            public override string ContentType => ApiContentType.TEXT_VCARD;

            ///<summary>Returns the body of the request.</summary>
            protected override object GetBody()
            {
                using var stream = new MemoryStream();
                using var textWriter = new StreamWriter(stream);
                var writer = new CardStandardWriter();
                writer.Write(Body, textWriter, Encoding.UTF8.WebName);
                textWriter.Flush();

                stream.Seek(0, SeekOrigin.Begin);
                using var streamReader = new StreamReader(stream);
                return streamReader.ReadToEnd();
            }

            /// <summary>Initializes Insert parameter list.</summary>
            protected override void InitParameters()
            {
                base.InitParameters();

                RequestParameters.Add("resourceName", new Parameter
                {
                    Name = "resourceName",
                    IsRequired = true,
                    ParameterType = "path",
                });
                RequestParameters.Add("uniqueId", new Parameter
                {
                    Name = "uniqueId",
                    IsRequired = true,
                    ParameterType = "path",
                });
            }
        }

        /// <summary>Updates an existing people on the user's people list.</summary>
        public class UpdateRequest : PeopleBaseServiceRequest<UpdateResponseObject>
        {
            /// <summary>Constructs a new Update request.</summary>
            public UpdateRequest(IClientService service, Person body, string resourceName) : base(service)
            {
                ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(resourceName));
                Body = body.ThrowIfNull(nameof(body));
                UniqueId = Body.UniqueId.ThrowIfNullOrEmpty(nameof(Person.UniqueId));
                ETagAction = ETagAction.IfMatch;
                InitParameters();
            }

            /// <summary>Resource Name. To retrieve resource names call the identityCard.list method.</summary>
            [RequestParameter("resourceName", RequestParameterType.Path)]
            public virtual string ResourceName { get; }

            /// <summary>People identifier. To retrieve people IDs call the people.list method.</summary>
            [RequestParameter("uniqueId", RequestParameterType.Path)]
            public virtual string UniqueId { get; }

            /// <summary>Gets the body of this request.</summary>
            private Person Body { get; }

            ///<summary>Gets the method name.</summary>
            public override string MethodName => "update";

            ///<summary>Gets the HTTP method.</summary>
            public override string HttpMethod => ApiMethod.PUT;

            ///<summary>Gets the REST path.</summary>
            public override string RestPath => "{resourceName}/{uniqueId}.vcf";

            ///<summary>Gets the Content-Type.</summary>
            public override string ContentType => ApiContentType.TEXT_VCARD;

            ///<summary>Returns the body of the request.</summary>
            protected override object GetBody()
            {
                using var stream = new MemoryStream();
                using var textWriter = new StreamWriter(stream);
                var writer = new CardStandardWriter();
                writer.Write(Body, textWriter, Encoding.UTF8.WebName);
                textWriter.Flush();

                stream.Seek(0, SeekOrigin.Begin);
                using var streamReader = new StreamReader(stream);
                return streamReader.ReadToEnd();
            }

            /// <summary>Initializes Update parameter list.</summary>
            protected override void InitParameters()
            {
                base.InitParameters();

                RequestParameters.Add("resourceName", new Parameter
                {
                    Name = "resourceName",
                    IsRequired = true,
                    ParameterType = "path",
                });
                RequestParameters.Add("uniqueId", new Parameter
                {
                    Name = "uniqueId",
                    IsRequired = true,
                    ParameterType = "path",
                });
            }
        }

        /// <summary>Removes a people from the user's people list.</summary>
        public class DeleteRequest : PeopleBaseServiceRequest<DeleteResponseObject>
        {
            /// <summary>Constructs a new Delete request.</summary>
            public DeleteRequest(IClientService service, string uniqueId, string resourceName) : base(service)
            {
                UniqueId = uniqueId.ThrowIfNullOrEmpty(nameof(uniqueId));
                ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(resourceName));
                InitParameters();
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
            public override string HttpMethod => ApiMethod.DELETE;

            ///<summary>Gets the REST path.</summary>
            public override string RestPath => "{resourceName}/{uniqueId}.vcf";

            /// <summary>Initializes Delete parameter list.</summary>
            protected override void InitParameters()
            {
                base.InitParameters();

                RequestParameters.Add("resourceName", new Parameter
                {
                    Name = "resourceName",
                    IsRequired = true,
                    ParameterType = "path",
                });
                RequestParameters.Add("uniqueId", new Parameter
                {
                    Name = "uniqueId",
                    IsRequired = true,
                    ParameterType = "path",
                });
            }
        }
    }
}
