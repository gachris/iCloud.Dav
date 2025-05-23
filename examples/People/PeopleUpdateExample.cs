using iCloud.Dav.Auth;
using iCloud.Dav.Auth.Store;
using iCloud.Dav.Core;
using iCloud.Dav.People.DataTypes;
using iCloud.Dav.People.Services;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using vCard.Net.DataTypes;

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

// #0 Use the contact's 'Id' property, not the 'Uid' property, when interacting with requests.
Contact contact = peopleService.People.Get("contact-id", identityCard.ResourceName).Execute();

contact.N.GivenName = "Updated name";
contact.RevisionDate = new CardDateTime(DateTime.Now);

// #0 Use the contact's 'Id' property, not the 'Uid' property, when interacting with requests.
// #1 Alternatively, you can use the string "card" instead of identityCard.ResourceName
HeaderMetadataResponse response = peopleService.People.Update(contact, "contact-id", identityCard.ResourceName).Execute();