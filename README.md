## iCloud.Dav
iCloud.Dav is a .NET library that allows you to interact with various iCloud data, including contacts, calendars, events, reminders lists, and reminders.

## Installation
You can install iCloud.Dav via NuGet package manager or by downloading the source code and building it manually.

## NuGet Installation
To install iCloud.Dav.Calendar via NuGet, run the following command in the Package Manager Console:
```
Install-Package iCloud.Dav.Calendar
```

## NuGet Installation
To install iCloud.Dav.People via NuGet, run the following command in the Package Manager Console:
```
Install-Package iCloud.Dav.People
```

## Manual Installation
To manually install iCloud.Dav, follow these steps:

1. Download the source code from this repository.
2. Open the solution file (.sln) in Visual Studio.
3. Build the solution.

## Usage
To use iCloud.Dav in your .NET project, you need to add a reference to either the iCloud.Dav.Calendar or iCloud.Dav.People assembly. You can do this either by adding a reference to the respective NuGet package or by referencing the assembly directly.

Here are examples of how to use iCloud.Dav:

### Authentication
```cs
using iCloud.Dav.Auth;
using iCloud.Dav.Auth.Store;
using iCloud.Dav.Core;
using System.Net;

var networkCredential = new NetworkCredential("icloud-email", "app-specific-password");

var dataStore = new FileDataStore("folder-to-store-data");

var userCredential = await AuthorizationBroker.AuthorizeAsync("folder-to-store-user-credentials", networkCredential, dataStore, CancellationToken.None);

var initializer = new BaseClientService.Initializer()
{
    HttpClientInitializer = userCredential,
};
```

### Calendars
```cs
using iCloud.Dav.Calendar;

var calendarService = new CalendarService(initializer);
```

### Contacts
```cs
using iCloud.Dav.People;

var peopleService = new PeopleService(initializer);
```

### Reminders
```cs
using iCloud.Dav.Calendar;

var calendarService = new CalendarService(initializer);
```

### Gateway
```cs
using iCloud.Dav.People;

var cloudGatewayService = new CloudGatewayService(initializer);
```
## Contributing
Contributions to iCloud.Dav are welcome! If you want to contribute to the project, please fork the repository and create a pull request with your changes.

## License
iCloud.Dav is licensed under the MIT license. See the [License](https://github.com/gachris/iCloud.Dav/blob/master/License) file for more information.
