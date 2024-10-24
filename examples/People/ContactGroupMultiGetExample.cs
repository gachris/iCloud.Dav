using iCloud.Dav.Auth.Store;
using iCloud.Dav.Auth;
using iCloud.Dav.Core;
using iCloud.Dav.People.Services;
using System.Net;
using System.Threading;
using System;
using iCloud.Dav.People.DataTypes;
using System.Linq;

NetworkCredential networkCredential = new NetworkCredential("icloud-email", "app-specific-password");
FileDataStore dataStore = new FileDataStore("iCloudApp");

UserCredential userCredential = await AuthorizationBroker.AuthorizeAsync(Environment.UserName, networkCredential, dataStore, CancellationToken.None);

BaseClientService.Initializer initializer = new BaseClientService.Initializer()
{
    ApplicationName = "iCloudApp",
    HttpClientInitializer = userCredential
};

PeopleService peopleService = new PeopleService(initializer);

IdentityCardList identityCardList = peopleService.IdentityCard.List().Execute();
IdentityCard identityCard = identityCardList.Items.First();

// #0 Alternatively, you can use the string "card" instead of identityCard.ResourceName
// #1 Use the contactGroup's 'Id' property, not the 'Uid' property, when interacting with requests.
ContactGroupList contactGroupList = peopleService.ContactGroups.MultiGet(identityCard.ResourceName, new string[] { "contact.Id-1", "contact.Id-2" }).Execute();