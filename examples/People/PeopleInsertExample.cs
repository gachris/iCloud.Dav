using iCloud.Dav.Auth.Store;
using iCloud.Dav.Auth;
using iCloud.Dav.Core;
using iCloud.Dav.People.Services;
using System.Net;
using System.Threading;
using System;
using iCloud.Dav.People.DataTypes;
using System.Linq;
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

var contact = new Contact()
{
    N = new Name()
    {
        GivenName = "John",
        FamilyName = "Doe"
    },
    RevisionDate = new CardDateTime(DateTime.Now)
};

// #1 Alternatively, you can use the string "card" instead of identityCard.ResourceName
HeaderMetadataResponse response = peopleService.People.Insert(contact, identityCard.ResourceName).Execute();