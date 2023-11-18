## iCloud.Dav.People
iCloud.Dav.People is a .NET library that allows you to interact with iCloud contacts and contact groups.

## Installation
You can install iCloud.Dav.People via NuGet package manager or by downloading the source code and building it manually.

## NuGet Installation
To install iCloud.Dav.People via NuGet, run the following command in the Package Manager Console:
```
Install-Package iCloud.Dav.People
```

## Manual Installation
To manually install iCloud.Dav.People, follow these steps:

1. Download the source code from this repository.
2. Open the solution file (.sln) in Visual Studio.
3. Build the solution.

## Usage
To use iCloud.Dav.People in your .NET project, you need to add a reference to the iCloud.Dav.People assembly. You can do this either by adding a reference to the respective NuGet package or by referencing the assembly directly.

Here are examples of how to use iCloud.Dav.People:

### Authentication
```cs
using iCloud.Dav.Auth;
using iCloud.Dav.Auth.Store;
using iCloud.Dav.Core;
using System.Net;
using System.Threading;

NetworkCredential networkCredential = new NetworkCredential("icloud-email", "app-specific-password");

FileDataStore dataStore = new FileDataStore("folder-to-store-data");

UserCredential userCredential = await AuthorizationBroker.AuthorizeAsync("file-to-store-user-credentials", networkCredential, dataStore, CancellationToken.None);

BaseClientService.Initializer initializer = new BaseClientService.Initializer()
{
    HttpClientInitializer = userCredential,
};
```

### Get Resource Name
```cs
using iCloud.Dav.People.DataTypes;
using iCloud.Dav.People.Services;
using System.Linq;

PeopleService peopleService = new PeopleService(initializer);
IdentityCardList identityCardList = peopleService.IdentityCard.List().Execute();
IdentityCard identityCard = identityCardList.Items.First();
string resourceName = identityCard.ResourceName;
```

### Get Contacts
```cs
using iCloud.Dav.People.DataTypes;
using iCloud.Dav.People.Services;

PeopleService peopleService = new PeopleService(initializer);
ContactList contactList = peopleService.People.List(resourceName).Execute();
```

### Get Contact Groups
```cs
using iCloud.Dav.People.DataTypes;
using iCloud.Dav.People.Services;

PeopleService peopleService = new PeopleService(initializer);
ContactGroupList contactGroupList = peopleService.ContactGroups.List(resourceName).Execute();
```

### Get File
```cs
using iCloud.Dav.People.Services;
using System;

CloudGatewayService cloudGatewayService = new CloudGatewayService(initializer);
byte[] data = cloudGatewayService.CloudGateway.Get(new Uri("file_url")).Execute();
```

### Get Next SyncToken
```cs
using iCloud.Dav.People.DataTypes;
using iCloud.Dav.People.Services;

PeopleService peopleService = new PeopleService(initializer);
SyncToken syncToken = peopleService.IdentityCard.GetSyncToken(resourceName).Execute();
```

### Sync Collection
```cs
using iCloud.Dav.People.DataTypes;
using iCloud.Dav.People.Services;
using iCloud.Dav.People.Resources;

PeopleService peopleService = new PeopleService(initializer);
IdentityCardResource.SyncCollectionRequest syncCollectionRequest = peopleService.IdentityCard.SyncCollection(resourceName);
syncCollectionRequest.SyncToken = syncToken.NextSyncToken;
SyncCollectionList syncCollectionList = syncCollectionRequest.Execute();
```

## Contributing
Contributions to iCloud.Dav are welcome! If you want to contribute to the project, please fork the repository and create a pull request with your changes.

## License
iCloud.Dav is licensed under the MIT license. See the [License](https://github.com/gachris/iCloud.Dav/blob/master/License) file for more information.
