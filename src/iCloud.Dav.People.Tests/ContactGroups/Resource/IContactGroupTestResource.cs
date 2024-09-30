using iCloud.Dav.Core.Response;
using iCloud.Dav.People.DataTypes;

namespace iCloud.Dav.People.Tests.ContactGroups.Resource;

public interface IContactGroupTestResource
{
    ContactGroupList List(string resourceName);

    ContactGroup Get(string contactGroupId, string resourceName);

    ContactGroupList MultiGet(string resourceName, string[] contactGroupIds);

    VoidResponse Insert(ContactGroup body, string resourceName);

    VoidResponse Update(ContactGroup body, string resourceName);

    VoidResponse Delete(string contactGroupId, string resourceName);
}