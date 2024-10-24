using iCloud.Dav.Auth;
using iCloud.Dav.Auth.Store;
using iCloud.Dav.Calendar.DataTypes;
using iCloud.Dav.Calendar.Services;
using iCloud.Dav.Core;
using System;
using System.Collections.Generic;
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

CalendarService calendarService = new CalendarService(initializer);

CalendarList calendarList = calendarService.Calendars.List().Execute();

IEnumerable<CalendarListEntry> eventCalendars = calendarList.Items.Where(x => x.SupportedCalendarComponents.Contains("VEVENT"));

IEnumerable<CalendarListEntry> reminderCalendars = calendarList.Items.Where(x => x.SupportedCalendarComponents.Contains("VTODO"));