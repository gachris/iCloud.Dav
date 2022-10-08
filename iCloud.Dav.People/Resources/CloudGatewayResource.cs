using iCloud.Dav.Core;
using iCloud.Dav.Core.Utils;
using iCloud.Dav.People.Requests;
using System;

namespace iCloud.Dav.People.Resources;

/// <summary>The "people" collection of methods.</summary>
public class CloudGatewayResource
{
    /// <summary>The service which this resource belongs to.</summary>
    private readonly IClientService _service;

    /// <summary>Constructs a new resource.</summary>
    public CloudGatewayResource(IClientService service)
    {
        _service = service;
    }

    /// <summary>Returns a people from the user's people list.</summary>
    /// <param name="uri">Uri to retrieve contact photo.</param>
    public virtual GetContactPhotoRequest GetContactPhoto(Uri uri)
    {
        return new GetContactPhotoRequest(_service, uri);
    }

    /// <summary>Returns a people from the user's people list.</summary>
    public class GetContactPhotoRequest : PeopleBaseServiceRequest<byte[]>
    {
        /// <summary>Constructs a new Get request.</summary>
        public GetContactPhotoRequest(IClientService service, Uri uri) : base(service)
        {
            InitParameters();
            RestPath = uri.AbsolutePath;
        }

        ///<summary>Gets the method name.</summary>
        public override string MethodName => "get";

        ///<summary>Gets the HTTP method.</summary>
        public override string HttpMethod => ApiMethod.Get;

        ///<summary>Gets the REST path.</summary>
        public override string RestPath { get; }
    }
}
