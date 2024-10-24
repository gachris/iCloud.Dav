
# iCloud.Dav

## Description
iCloud.Dav is a .NET library that allows you to interact with various iCloud services, including contacts, calendars, events, reminder lists, and reminders. It is designed to be easy to integrate with .NET projects and offers seamless authentication through iCloud.Dav.Auth.

## Installation

You can install iCloud.Dav packages via the NuGet package manager or by downloading and building the source code manually.

### NuGet Installation

- To install **iCloud.Dav.Core**, run:
  ```
  Install-Package iCloud.Dav.Core
  ```
- To install **iCloud.Dav.Auth**, run:
  ```
  Install-Package iCloud.Dav.Auth
  ```
- To install **iCloud.Dav.Calendar**, run:
  ```
  Install-Package iCloud.Dav.Calendar
  ```
- To install **iCloud.Dav.People**, run:
  ```
  Install-Package iCloud.Dav.People
  ```

### Manual Installation

1. Download the source code from this repository.
2. Open the solution file (.sln) in Visual Studio.
3. Build the solution.

## Usage

To use the iCloud.Dav libraries in your .NET project, add a reference to the respective assembly (either via NuGet or by referencing the assembly directly). Below are examples for using different libraries.

### iCloud.Dav.Auth

- **Authentication**:
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

- **Revoke Token**:
  ```cs
  using System.Threading;
  
  bool result = await userCredential.RevokeTokenAsync(CancellationToken.None);
  ```

### iCloud.Dav.Calendar

- **Authentication**: Refer to the [authentication example](#iclouddavauth).
  
- **Get Calendars**:
  ```cs
  using iCloud.Dav.Calendar.DataTypes;
  using iCloud.Dav.Calendar.Services;
  using System.Linq;
  
  CalendarService calendarService = new CalendarService(initializer);
  CalendarList calendarList = await calendarService.Calendars.List().Execute();
  var calendarListEntries = calendarList.Items.Where(x => x.SupportedCalendarComponents.Contains("VEVENT"));
  ```

- **Get Events**:
  ```cs
  using iCloud.Dav.Calendar.DataTypes;
  using iCloud.Dav.Calendar.Services;
  
  CalendarService calendarService = a new CalendarService(initializer);
  Events events = calendarService.Events.List(calendarListEntry.Id).Execute();
  ```

- **Get Reminders**:
  ```cs
  using iCloud.Dav.Calendar.DataTypes;
  using iCloud.Dav.Calendar.Services;
  
  CalendarService calendarService = new CalendarService(initializer);
  Reminders reminders = calendarService.Reminders.List(calendarListEntry.Id).Execute();
  ```

### iCloud.Dav.People

- **Authentication**: Refer to the [authentication example](#iclouddavauth).
  
- **Get Resource Name**:
  ```cs
  using iCloud.Dav.People.DataTypes;
  using iCloud.Dav.People.Services;
  using System.Linq;
  
  PeopleService peopleService = new PeopleService(initializer);
  IdentityCardList identityCardList = peopleService.IdentityCard.List().Execute();
  IdentityCard identityCard = identityCardList.Items.First();
  string resourceName = identityCard.ResourceName;
  ```
  
- **Get Contact Groups**:
  ```cs
  using iCloud.Dav.People.DataTypes;
  using iCloud.Dav.People.Services;
  
  PeopleService peopleService = new PeopleService(initializer);
  ContactGroupList contactGroupList = peopleService.ContactGroups.List(resourceName).Execute();
  ```

- **Get Contacts**:
  ```cs
  using iCloud.Dav.People.DataTypes;
  using iCloud.Dav.People.Services;
  
  PeopleService peopleService = new PeopleService(initializer);
  ContactList contactList = peopleService.People.List(resourceName).Execute();
  ```

## Contributing

Contributions to iCloud.Dav are welcome! If you want to contribute to the project, please fork the repository and create a pull request with your changes.

## License

iCloud.Dav is licensed under the MIT license. See the [License](../License) file for more information.
