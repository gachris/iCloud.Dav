using iCloud.Dav.Auth;
using iCloud.Dav.Auth.Utils.Store;
using iCloud.Dav.Core.Response;
using iCloud.Dav.Core.Services;
using iCloud.Dav.People;
using iCloud.Dav.People.Resources;
using iCloud.Dav.People.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading;

namespace iCloud.Dav.UnitTest
{
    [TestClass]
    public class PeopleServiceTest
    {
        [TestMethod]
        public void PeopleTestMethod()
        {
            PeopleService service = GetService();
            string personId = Guid.NewGuid().ToString().ToUpper();
            string contactGroupId = Guid.NewGuid().ToString().ToUpper();

            // get identity cards
            IdentityCardList identityCards = IdentityCardMethods.GetList(service.IdentityCard);

            // get resourceName
            string resourceName = identityCards.FirstOrDefault().ResourceName;

            // create new contactGroup 
            ContactGroup contactGroup = new ContactGroup();
            contactGroup.UniqueId = contactGroupId;
            contactGroup.FamilyName = "New Group Description";
            contactGroup.FormattedName = "New Group Description";

            // insert contact group
            var contactGroupInsertResponseObject = ContactGroupMethods.Insert(service.ContactGroups, contactGroup, resourceName);

            // create new person 
            Person person = new Person();
            person.UniqueId = personId;
            person.FamilyName = "New Contact Description";
            person.FormattedName = "New Contact Description";

            // insert person
            var insertResponseObject = PeopleMethods.Insert(service.People, person, resourceName);

            // get contact groups
            ContactGroupsList contactGroups = ContactGroupMethods.GetList(service.ContactGroups, resourceName);

            // get contact group
            contactGroup = ContactGroupMethods.Get(service.ContactGroups, contactGroupId, resourceName);

            // update contact group fields
            contactGroup.FormattedName = "Updated Group Description";
            contactGroup.FamilyName = "Updated Group Description";
            contactGroup.MemberResourceNames.Add(personId);

            // update contact group
            var contactGroupUpdateResponseObject1 = ContactGroupMethods.Update(service.ContactGroups, contactGroup, resourceName);

            // get persons 
            PersonList personList = PeopleMethods.GetList(service.People, resourceName);

            // get person
            person = PeopleMethods.Get(service.People, personId, resourceName);

            // update person fields
            person.FormattedName = "Updated Contact Description";
            person.FamilyName = "Updated Contact Description";

            // update person
            var updateResponseObject = PeopleMethods.Update(service.People, person, resourceName);

            // delete person
            var deleteResponseObject1 = PeopleMethods.Delete(service.People, personId, resourceName);

            // delete contact group
            var contactGroupDeleteResponseObject = ContactGroupMethods.Delete(service.ContactGroups, contactGroupId, resourceName);
        }

        class IdentityCardMethods
        {
            public static IdentityCardList GetList(IdentityCardResource identityCardResource)
            {
                IdentityCardResource.ListRequest listRequest = identityCardResource.List();
                return listRequest.Execute();
            }
        }

        class ContactGroupMethods
        {
            public static ContactGroupsList GetList(ContactGroupsResource contactGroupsResource, string resourceName)
            {
                ContactGroupsResource.ListRequest listRequest = contactGroupsResource.List(resourceName);
                return listRequest.Execute();
            }

            public static ContactGroup Get(ContactGroupsResource contactGroupsResource, string uniqueId, string resourceName)
            {
                ContactGroupsResource.GetRequest getRequest = contactGroupsResource.Get(uniqueId, resourceName);
                return getRequest.Execute();
            }

            public static InsertResponseObject Insert(ContactGroupsResource contactGroupsResource, ContactGroup contactGroup, string resourceName)
            {
                ContactGroupsResource.InsertRequest insertRequest = contactGroupsResource.Insert(contactGroup, resourceName);
                return insertRequest.Execute();
            }

            public static UpdateResponseObject Update(ContactGroupsResource contactGroupsResource, ContactGroup contactGroup, string resourceName)
            {
                ContactGroupsResource.UpdateRequest updateRequest = contactGroupsResource.Update(contactGroup, resourceName);
                return updateRequest.Execute();
            }

            public static DeleteResponseObject Delete(ContactGroupsResource contactGroupsResource, string uniqueId, string resourceName)
            {
                ContactGroupsResource.DeleteRequest deleteRequest = contactGroupsResource.Delete(uniqueId, resourceName);
                return deleteRequest.Execute();
            }
        }

        class PeopleMethods
        {
            public static PersonList GetList(PeopleResource peopleResource, string resourceName)
            {
                PeopleResource.ListRequest listRequest = peopleResource.List(resourceName);
                return listRequest.Execute();
            }

            public static Person Get(PeopleResource peopleResource, string uniqueId, string resourceName)
            {
                PeopleResource.GetRequest getRequest = peopleResource.Get(uniqueId, resourceName);
                return getRequest.Execute();
            }

            public static InsertResponseObject Insert(PeopleResource peopleResource, Person person, string resourceName)
            {
                PeopleResource.InsertRequest insertRequest = peopleResource.Insert(person, resourceName);
                return insertRequest.Execute();
            }

            public static UpdateResponseObject Update(PeopleResource peopleResource, Person person, string resourceName)
            {
                PeopleResource.UpdateRequest updateRequest = peopleResource.Update(person, resourceName);
                return updateRequest.Execute();
            }

            public static DeleteResponseObject Delete(PeopleResource peopleResource, string uniqueId, string resourceName)
            {
                PeopleResource.DeleteRequest deleteRequest = peopleResource.Delete(uniqueId, resourceName);
                return deleteRequest.Execute();
            }
        }

        private static PeopleService _service;
        private static PeopleService GetService()
        {
            if (_service == null)
            {
                IDataStore dataStore = new FileDataStore("icloudStore");

                UserCredential credential = AuthorizationBroker.AuthorizeAsync("gachris",
                    Credentials.NetworkCredential,
                    CancellationToken.None,
                    dataStore)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();

                _service = new PeopleService(new BaseClientService.Initializer()
                {
                    ApplicationName = "iCloud.SyncApp",
                    HttpClientInitializer = credential
                });
            }
            return _service;
        }
    }
}
