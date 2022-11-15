﻿using iCloud.Dav.Core;
using iCloud.Dav.People.CardDav.Types;
using iCloud.Dav.People.Requests;
using iCloud.Dav.People.Types;

namespace iCloud.Dav.People.Resources;

/// <summary>The "IdentityCard" collection of methods.</summary>
public class IdentityCardResource
{
    /// <summary>The service which this resource belongs to.</summary>
    private readonly IClientService _service;

    /// <summary>Constructs a new resource.</summary>
    public IdentityCardResource(IClientService service) => _service = service;

    /// <summary>Returns the identity cards on the user's identity card list.</summary>
    public virtual ListRequest List() => new(_service);

    /// <summary>Returns the identity cards on the user's identity card list.</summary>
    public class ListRequest : PeopleBaseServiceRequest<IdentityCardList>
    {
        private object? _body;

        /// <summary>Constructs a new Get request.</summary>
        public ListRequest(IClientService service) : base(service)
        {
        }

        ///<summary>Gets the method name.</summary>
        public override string MethodName => "list";

        ///<summary>Gets the HTTP method.</summary>
        public override string HttpMethod => Constants.Propfind;

        ///<summary>Gets the REST path.</summary>
        public override string RestPath => string.Empty;

        ///<summary>Gets the depth.</summary>
        public override string Depth => "1";

        ///<summary>Returns the body of the request.</summary>
        protected override object GetBody() => _body ??= new PropFind();
    }
}
