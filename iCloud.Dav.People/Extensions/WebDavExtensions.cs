using iCloud.Dav.People.WebDav.DataTypes;
using System;
using System.IO;
using System.Linq;
using System.Net;

namespace iCloud.Dav.People.Extensions;

internal static class WebDavExtensions
{
    public const string GroupKind = "X-ADDRESSBOOKSERVER-KIND:group";
    public const string CollectionResourceType = "collection";
    public const string AddressbookResourceType = "addressbook";

    public static bool IsAddressbook(this Response response)
    {
        if (!response.IsOK()) return false;

        var propStat = response.GetSuccessPropStat();

        var isAddressbookResourceType = propStat.Prop.ResourceType?.Names?.Any(resourceType => string.Equals(resourceType, AddressbookResourceType, StringComparison.InvariantCultureIgnoreCase)) ?? false;
        return isAddressbookResourceType || !response.HasExtension();
    }

    public static bool IsCollection(this Response response)
    {
        if (!response.IsOK()) return false;

        var propStat = response.GetSuccessPropStat();

        var isCollectionResourceType = propStat.Prop.ResourceType?.Names?.Length == 1 && string.Equals(propStat.Prop.ResourceType?.Names.First(), CollectionResourceType, StringComparison.InvariantCultureIgnoreCase);
        return isCollectionResourceType || !response.HasExtension();
    }

    public static bool IsGroup(this Response response)
    {
        return response.PropStat[0].Prop.AddressData.Value.Contains(GroupKind);
    }

    public static bool IsOK(this Response response)
    {
        return response.HasError()
            ? false
            : response.StatusCode is System.Net.HttpStatusCode.OK
            || response.PropStat.Length > 0 && response.PropStat[0].StatusCode is System.Net.HttpStatusCode.OK;
    }

    public static bool HasExtension(this Response response)
    {
        var href = response.Href.Value.TrimEnd('/');
        return Path.HasExtension(href);
    }

    public static bool HasError(this Response response)
    {
        return response.Error?.Errors?.Any() ?? false;
    }

    public static PropStat GetSuccessPropStat(this Response response)
    {
        return response.IsOK() && response.PropStat.Length > 0 ? response.PropStat[0] : null;
    }

    public static string ExtractId(this Href href)
    {
        var value = href.Value.TrimEnd('/');
        return Path.GetFileNameWithoutExtension(value);
    }

    public static HttpStatusCode? ToHttpStatusCode(this string status)
    {
        if (string.IsNullOrWhiteSpace(status)) return null;

        var statusParts = status.Split(' ');
        var statusCodeString = statusParts.Length > 0 ? (int?)Convert.ToInt16(statusParts[1]) : null;
        return (HttpStatusCode?)statusCodeString;
    }
}