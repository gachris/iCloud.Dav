using iCloud.Dav.Auth;
using iCloud.Dav.Auth.Store;
using iCloud.Dav.Calendar.DataTypes;
using iCloud.Dav.Calendar.Services;
using iCloud.Dav.Core;
using System;
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

// #0 Use the event's 'Id' property, not the 'Uid' property, when interacting with requests.
Event eventItem = calendarService.Events.Get("calendar-id", "event-id").Execute();
eventItem.Summary = "Updated event title";

// #1 Use the event's 'Id' property, not the 'Uid' property, when interacting with requests.
HeaderMetadataResponse response = calendarService.Events.Update(eventItem, "event-id", "calendar-id").Execute();