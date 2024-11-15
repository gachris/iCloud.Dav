using iCloud.Dav.Auth;
using iCloud.Dav.Auth.Store;
using iCloud.Dav.Core;
using iCloud.Dav.People.DataTypes;
using iCloud.Dav.People.Services;
using System;
using System.Linq;
using System.Net;
using System.Threading;

NetworkCredential networkCredential = new NetworkCredential("icloud-email", "app-specific-password");
FileDataStore dataStore = new FileDataStore("iCloudApp");

UserCredential userCredential = await AuthorizationBroker.AuthorizeAsync(Environment.UserName, networkCredential, dataStore, CancellationToken.None);

BaseClientService.Initializer initializer = new BaseClientService.Initializer()
{
    ApplicationName = "iCloudApp",
    HttpClientInitializer = userCredential
};

CloudGatewayService cloudGatewayService = new CloudGatewayService(initializer);

PeopleService peopleService = new PeopleService(initializer);

IdentityCardList identityCardList = peopleService.IdentityCard.List().Execute();
IdentityCard identityCard = identityCardList.Items.First();

// Alternative you can use "card" instead of identityCard.ResourceName
ContactList contactList = peopleService.People.List(identityCard.ResourceName).Execute();

Contact contact = contactList.Items.First();

byte[] photo = cloudGatewayService.CloudGateway.Get(contact.Photo.Url).Execute();