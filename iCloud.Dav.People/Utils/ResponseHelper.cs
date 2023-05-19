using iCloud.Dav.People.CardDav.Types;
using System;
using System.IO;
using System.Linq;

namespace iCloud.Dav.People.Utils
{
    internal static class ResponseHelper
    {
        private const string GroupKind = "X-ADDRESSBOOKSERVER-KIND:group";
        private const string CollectionResourceType = "collection";
        private const string AddressbookResourceType = "addressbook";

        public static bool IsAddressbook(this Response response)
        {
            var isAddressbookResourceType = response.ResourceType?.Any(resourceType => string.Equals(resourceType.Name, AddressbookResourceType, StringComparison.InvariantCultureIgnoreCase)) ?? false;
            return isAddressbookResourceType || !response.HasExtension();
        }

        public static bool IsCollection(this Response response)
        {
            var isCollectionResourceType = response.ResourceType?.Count == 1 && string.Equals(response.ResourceType.First().Name, CollectionResourceType, StringComparison.InvariantCultureIgnoreCase);
            return isCollectionResourceType || !response.HasExtension();
        }

        public static bool IsGroup(this Response response)
        {
            return response.AddressData.Value.Contains(GroupKind);
        }

        public static bool IsOK(this Response response)
        {
            return response.Status is System.Net.HttpStatusCode.OK;
        }

        public static bool HasExtension(this Response response)
        {
            var href = response.Href.TrimEnd('/');
            return Path.HasExtension(href);
        }
    }
}