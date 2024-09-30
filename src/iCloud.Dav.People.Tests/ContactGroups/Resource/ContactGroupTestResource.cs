using iCloud.Dav.Core.Response;
using iCloud.Dav.People.DataTypes;
using iCloud.Dav.People.Services;

namespace iCloud.Dav.People.Tests.ContactGroups.Resource;

public class ContactGroupTestResource : IContactGroupTestResource
{
    private readonly PeopleService _service;

    public ContactGroupTestResource(PeopleService service)
    {
        _service = service;
    }

    public ContactGroupList List(string resourceName)
    {
        return _service.ContactGroups.List(resourceName).Execute();
    }

    public ContactGroup Get(string contactGroupId, string resourceName)
    {
        return _service.ContactGroups.Get(contactGroupId, resourceName).Execute();
    }

    public ContactGroupList MultiGet(string resourceName, string[] contactGroupIds)
    {
        return _service.ContactGroups.MultiGet(resourceName, contactGroupIds).Execute();
    }

    public VoidResponse Insert(ContactGroup body, string resourceName)
    {
        return _service.ContactGroups.Insert(body, resourceName).Execute();
    }

    public VoidResponse Update(ContactGroup body, string resourceName)
    {
        return _service.ContactGroups.Update(body, resourceName).Execute();
    }

    public VoidResponse Delete(string contactGroupId, string resourceName)
    {
        return _service.ContactGroups.Delete(contactGroupId, resourceName).Execute();
    }
}