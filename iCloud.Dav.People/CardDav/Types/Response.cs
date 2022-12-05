using System;
using System.Collections.Generic;

namespace iCloud.Dav.People.CardDav.Types
{
    internal sealed class Response
    {
        public string Href { get; set; }

        public Status Status { get; set; }

        public string Contentlength { get; set; }

        public string Etag { get; set; }

        public Attribute Ctag { get; set; }

        public Attribute MeCard { get; set; }

        public Attribute GuardianRestricted { get; set; }

        public Attribute AddressData { get; set; }

        public List<Attribute> ResourceType { get; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public string SyncToken { get; set; }

        public Response()
        {
            ResourceType = new List<Attribute>();
        }
    }
}