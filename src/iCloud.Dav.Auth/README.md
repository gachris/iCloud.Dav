## iCloud.Dav.Auth
iCloud.Dav.Auth is a .NET library that facilitates authentication and interaction with iCloud services.

## Installation
You can install iCloud.Dav.Auth via NuGet package manager or by downloading the source code and building it manually.

## NuGet Installation
To install iCloud.Dav.Auth via NuGet, run the following command in the Package Manager Console:
```
Install-Package iCloud.Dav.Auth
```

## Manual Installation
To manually install iCloud.Dav.Auth, follow these steps:

1. Download the source code from this repository.
2. Open the solution file (.sln) in Visual Studio.
3. Build the solution.

## Usage
To use iCloud.Dav.Auth in your .NET project, you need to add a reference to the iCloud.Dav.Auth assembly. You can do this either by adding a reference to the respective NuGet package or by referencing the assembly directly.

Here are examples of how to use iCloud.Dav.Auth:

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

### Revoke Token
```cs
using System.Threading;

bool result = await userCredential.RevokeTokenAsync(CancellationToken.None);
```

## Contributing
Contributions to iCloud.Dav are welcome! If you want to contribute to the project, please fork the repository and create a pull request with your changes.

## License
iCloud.Dav is licensed under the MIT license. See the [License](../License) file for more information.
