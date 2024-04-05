## iCloud.Dav.Calendar
iCloud.Dav.Calendar is a .NET library that allows you to interact with iCloud calendars, events, reminder lists, and reminders.

## Installation
You can install iCloud.Dav.Calendar via NuGet package manager or by downloading the source code and building it manually.

## NuGet Installation
To install iCloud.Dav.Calendar via NuGet, run the following command in the Package Manager Console:
```
Install-Package iCloud.Dav.Calendar
```

## Manual Installation
To manually install iCloud.Dav.Calendar, follow these steps:

1. Download the source code from this repository.
2. Open the solution file (.sln) in Visual Studio.
3. Build the solution.

## Usage
To use iCloud.Dav.Calendar in your .NET project, you need to add a reference to the iCloud.Dav.Calendar assembly. You can do this either by adding a reference to the respective NuGet package or by referencing the assembly directly.

Here are examples of how to use iCloud.Dav.Calendar:

### Authentication
For seamless authentication with iCloud.Dav.Calendar, refer to the [iCloud.Dav.Auth](../iCloud.Dav.Auth) module.

### Get Calendars
```cs
using iCloud.Dav.Calendar.DataTypes;
using iCloud.Dav.Calendar.Services;
using System.Collections.Generic;
using System.Linq;

CalendarService calendarService = new CalendarService(initializer);
CalendarList calendarList = await calendarService.Calendars.List().Execute();
IEnumerable<CalendarListEntry> calendarListEntries = calendarList.Items.Where(x => x.SupportedCalendarComponents.Contains("VEVENT"));
```

### Get Events
```cs
using iCloud.Dav.Calendar.DataTypes;
using iCloud.Dav.Calendar.Services;

CalendarService calendarService = new CalendarService(initializer);
Events events = calendarService.Events.List(calendarListEntry.Id).Execute();
```

### Get Reminder Lists
```cs
using iCloud.Dav.Calendar.DataTypes;
using iCloud.Dav.Calendar.Services;
using System.Collections.Generic;
using System.Linq;

CalendarService calendarService = new CalendarService(initializer);
CalendarList calendarList = await calendarService.Calendars.List().Execute();
IEnumerable<CalendarListEntry> calendarListEntries = calendarList.Items.Where(x => x.SupportedCalendarComponents.Contains("VTODO"));
```

### Get Reminders
```cs
using iCloud.Dav.Calendar.DataTypes;
using iCloud.Dav.Calendar.Services;

CalendarService calendarService = new CalendarService(initializer);
Reminders reminders = calendarService.Reminders.List(calendarListEntry.Id).Execute();
```

## Contributing
Contributions to iCloud.Dav are welcome! If you want to contribute to the project, please fork the repository and create a pull request with your changes.

## License
iCloud.Dav is licensed under the MIT license. See the [License](../License) file for more information.
