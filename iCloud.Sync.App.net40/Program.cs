using Ical.Net.DataTypes;
using iCloud.Dav.Auth;
using iCloud.Dav.Auth.Utils.Store;
using iCloud.Dav.Calendar;
using iCloud.Dav.Calendar.Resources;
using iCloud.Dav.Calendar.Services;
using iCloud.Dav.Core.Response;
using iCloud.Dav.Core.Services;
using iCloud.Dav.People;
using iCloud.Dav.People.Resources;
using iCloud.Dav.People.Services;
using System;
using System.Linq;
using System.Net;
using System.Threading;

namespace iCloud.Sync.App
{
    internal class Program
    {
        private static void Main()
        {
            People();
            EventsCalendar();
            RemindersCalendar();
        }

        public static void EventsCalendar()
        {
            CalendarService service = GetCalendarService();
            string eventId = Guid.NewGuid().ToString().ToUpper();
            string calendarId = Guid.NewGuid().ToString().ToUpper();

            // create new calendar 
            CalendarEntry calendarToInsert = new CalendarEntry();
            calendarToInsert.Id = calendarId;
            calendarToInsert.Summary = "New calendar from calendar test method";
            calendarToInsert.Color = "#FFFFFFFF";
            calendarToInsert.SupportedCalendarComponents.Add("VEVENT");

            // insert calendar
            var insertResponseObject = CalendarMethods.Insert(service.Calendars, calendarToInsert);

            // get calendars
            CalendarEntryList calendarEntries = CalendarMethods.GetList(service.Calendars);
            var insertedCalendar = calendarEntries.Where(x => x.Id == calendarToInsert.Id).SingleOrDefault();

            // update calendar fields
            CalendarEntry calendarToUpdate = (CalendarEntry)insertedCalendar.Clone();
            calendarToUpdate.Summary = "Updated calendar from calendar test method";
            calendarToUpdate.Color = "#FF000000";

            // update calendar
            var updateResponseObject = CalendarMethods.Update(service.Calendars, calendarToUpdate);

            // get calendar
            var updatedCalendar = CalendarMethods.Get(service.Calendars, calendarId);

            // create new event 
            Event eventToInserted = new Event();
            eventToInserted.Uid = eventId;
            eventToInserted.Summary = "New event from event test method";
            eventToInserted.Description = "New event from event test method";
            eventToInserted.Start = new CalDateTime(DateTime.Now);
            eventToInserted.End = new CalDateTime(DateTime.Now.AddHours(1));

            // insert event
            var eventInsertResponseObject = EventMethods.Insert(service.Events, eventToInserted, calendarId);

            // get events
            EventList eventList = EventMethods.GetList(service.Events, calendarId);
            var insertedEvent = eventList.Where(x => x.Uid == eventToInserted.Uid).SingleOrDefault();

            // update event fields
            Event eventToUpdate = (Event)insertedEvent.Clone();
            eventToUpdate.Summary = "Updated event from event test method";
            eventToUpdate.Description = "Updated event from event test method";
            eventToUpdate.Start = new CalDateTime(DateTime.Now.AddHours(1));
            eventToUpdate.End = new CalDateTime(DateTime.Now.AddHours(2));

            // update event
            var eventUpdateResponseObject = EventMethods.Update(service.Events, eventToUpdate, calendarId);

            // get event
            var updatedEvent = EventMethods.Get(service.Events, calendarId, eventId);

            // delete event
            var eventDeleteResponseObject = EventMethods.Delete(service.Events, calendarId, eventId);

            // delete calendar
            var calendarDeleteResponseObject = CalendarMethods.Delete(service.Calendars, calendarId);
        }

        public static void RemindersCalendar()
        {
            CalendarService service = GetCalendarService();
            string calendarId = Guid.NewGuid().ToString().ToUpper();
            string reminderId = Guid.NewGuid().ToString().ToUpper();

            // create new calendar 
            CalendarEntry calendarToInsert = new CalendarEntry();
            calendarToInsert.Id = calendarId;
            calendarToInsert.Summary = "New reminder list from reminders test method";
            calendarToInsert.Color = "#FFFFFFFF";
            calendarToInsert.SupportedCalendarComponents.Add("VTODO");

            // insert calendar
            var insertResponseObject = CalendarMethods.Insert(service.Calendars, calendarToInsert);

            // get calendars
            CalendarEntryList calendarEntries = CalendarMethods.GetList(service.Calendars);
            var insertedCalendar = calendarEntries.Where(x => x.Id == calendarToInsert.Id).SingleOrDefault();

            // update calendar fields
            CalendarEntry calendarToUpdate = (CalendarEntry)insertedCalendar.Clone();
            calendarToUpdate.Summary = "Updated reminder list from reminders test method";
            calendarToUpdate.Color = "#FF000000";

            // update calendar
            var updateResponseObject = CalendarMethods.Update(service.Calendars, calendarToUpdate);

            // get calendar
            var updatedCalendar = CalendarMethods.Get(service.Calendars, calendarId);

            // create new reminder 
            Reminder reminderToInserted = new Reminder();
            reminderToInserted.Uid = reminderId;
            reminderToInserted.Summary = "New reminder from reminder test method";
            reminderToInserted.Description = "New reminder from reminder test method";
            reminderToInserted.Start = new CalDateTime(DateTime.Now);

            // insert reminder
            var reminderInsertResponseObject = ReminderMethods.Insert(service.Reminders, reminderToInserted, calendarId);

            // get reminders
            ReminderList reminderList = ReminderMethods.GetList(service.Reminders, calendarId);
            var insertedReminder = reminderList.Where(x => x.Uid == reminderToInserted.Uid).SingleOrDefault();

            // update event fields
            Reminder reminderToUpdate = (Reminder)insertedReminder.Clone();
            reminderToUpdate.Summary = "Updated reminder from reminder test method";
            reminderToUpdate.Description = "Updated reminder from reminder test method";
            reminderToUpdate.Start = new CalDateTime(DateTime.Now.AddHours(1));

            // update event
            var eventUpdateResponseObject = ReminderMethods.Update(service.Reminders, reminderToUpdate, calendarId);

            // get event
            var updatedReminnder = ReminderMethods.Get(service.Reminders, calendarId, reminderId);

            // delete event
            var eventDeleteResponseObject = ReminderMethods.Delete(service.Reminders, calendarId, reminderId);

            // delete calendar
            var calendarDeleteResponseObject = CalendarMethods.Delete(service.Calendars, calendarId);
        }

        public static void People()
        {
            PeopleService service = GetPeopleService();
            string personId = Guid.NewGuid().ToString().ToUpper();
            string contactGroupId = Guid.NewGuid().ToString().ToUpper();

            // get identity cards
            IdentityCardList identityCards = IdentityCardMethods.GetList(service.IdentityCard);

            // get resourceName
            string resourceName = identityCards.FirstOrDefault().ResourceName;

            // create new contactGroup 
            ContactGroup contactGroup = new ContactGroup();
            contactGroup.UniqueId = contactGroupId;
            contactGroup.FamilyName = "New Group Description";
            contactGroup.FormattedName = "New Group Description";

            // insert contact group
            var contactGroupInsertResponseObject = ContactGroupMethods.Insert(service.ContactGroups, contactGroup, resourceName);

            // create new person 
            Person person = new Person();
            person.UniqueId = personId;
            person.FamilyName = "New Contact Description";
            person.FormattedName = "New Contact Description";

            // insert person
            var insertResponseObject = PeopleMethods.Insert(service.People, person, resourceName);

            // get contact groups
            ContactGroupsList contactGroups = ContactGroupMethods.GetList(service.ContactGroups, resourceName);

            // get contact group
            contactGroup = ContactGroupMethods.Get(service.ContactGroups, contactGroupId, resourceName);

            // update contact group fields
            contactGroup.FormattedName = "Updated Group Description";
            contactGroup.FamilyName = "Updated Group Description";
            contactGroup.MemberResourceNames.Add(personId);

            // update contact group
            var contactGroupUpdateResponseObject1 = ContactGroupMethods.Update(service.ContactGroups, contactGroup, resourceName);

            // get persons 
            PersonList personList = PeopleMethods.GetList(service.People, resourceName);

            // get person
            person = PeopleMethods.Get(service.People, personId, resourceName);

            // update person fields
            person.FormattedName = "Updated Contact Description";
            person.FamilyName = "Updated Contact Description";

            // update person
            var updateResponseObject = PeopleMethods.Update(service.People, person, resourceName);

            // delete person
            var deleteResponseObject1 = PeopleMethods.Delete(service.People, personId, resourceName);

            // delete contact group
            var contactGroupDeleteResponseObject = ContactGroupMethods.Delete(service.ContactGroups, contactGroupId, resourceName);
        }

        class IdentityCardMethods
        {
            public static IdentityCardList GetList(IdentityCardResource identityCardResource)
            {
                IdentityCardResource.ListRequest listRequest = identityCardResource.List();
                return listRequest.Execute();
            }
        }

        class ContactGroupMethods
        {
            public static ContactGroupsList GetList(ContactGroupsResource contactGroupsResource, string resourceName)
            {
                ContactGroupsResource.ListRequest listRequest = contactGroupsResource.List(resourceName);
                return listRequest.Execute();
            }

            public static ContactGroup Get(ContactGroupsResource contactGroupsResource, string uniqueId, string resourceName)
            {
                ContactGroupsResource.GetRequest getRequest = contactGroupsResource.Get(uniqueId, resourceName);
                return getRequest.Execute();
            }

            public static InsertResponseObject Insert(ContactGroupsResource contactGroupsResource, ContactGroup contactGroup, string resourceName)
            {
                ContactGroupsResource.InsertRequest insertRequest = contactGroupsResource.Insert(contactGroup, resourceName);
                return insertRequest.Execute();
            }

            public static UpdateResponseObject Update(ContactGroupsResource contactGroupsResource, ContactGroup contactGroup, string resourceName)
            {
                ContactGroupsResource.UpdateRequest updateRequest = contactGroupsResource.Update(contactGroup, resourceName);
                return updateRequest.Execute();
            }

            public static DeleteResponseObject Delete(ContactGroupsResource contactGroupsResource, string uniqueId, string resourceName)
            {
                ContactGroupsResource.DeleteRequest deleteRequest = contactGroupsResource.Delete(uniqueId, resourceName);
                return deleteRequest.Execute();
            }
        }

        class PeopleMethods
        {
            public static PersonList GetList(PeopleResource peopleResource, string resourceName)
            {
                PeopleResource.ListRequest listRequest = peopleResource.List(resourceName);
                return listRequest.Execute();
            }

            public static Person Get(PeopleResource peopleResource, string uniqueId, string resourceName)
            {
                PeopleResource.GetRequest getRequest = peopleResource.Get(uniqueId, resourceName);
                return getRequest.Execute();
            }

            public static InsertResponseObject Insert(PeopleResource peopleResource, Person person, string resourceName)
            {
                PeopleResource.InsertRequest insertRequest = peopleResource.Insert(person, resourceName);
                return insertRequest.Execute();
            }

            public static UpdateResponseObject Update(PeopleResource peopleResource, Person person, string resourceName)
            {
                PeopleResource.UpdateRequest updateRequest = peopleResource.Update(person, resourceName);
                return updateRequest.Execute();
            }

            public static DeleteResponseObject Delete(PeopleResource peopleResource, string uniqueId, string resourceName)
            {
                PeopleResource.DeleteRequest deleteRequest = peopleResource.Delete(uniqueId, resourceName);
                return deleteRequest.Execute();
            }
        }

        private static PeopleService _peopleService;
        private static PeopleService GetPeopleService()
        {
            if (_peopleService == null)
            {
                IDataStore dataStore = new FileDataStore("icloudStore");

                UserCredential credential = AuthorizationBroker.AuthorizeAsync("gachris",
                    NetworkCredential,
                    CancellationToken.None,
                    dataStore)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();

                _peopleService = new PeopleService(new BaseClientService.Initializer()
                {
                    ApplicationName = "iCloud.SyncApp",
                    HttpClientInitializer = credential
                });
            }
            return _peopleService;
        }

        public void AddTimezone()
        {
            //var timezone = Ical.Net.VTimeZone.FromSystemTimeZone(TimeZoneInfo.Local, DateTime.Now, true);

            //var calendarProperty = new CalendarProperty("LAST-MODIFIED", ToFilterTime(DateTime.Now));
            //timezone.AddProperty(calendarProperty);

            //var statndard = new Statndard();
            //timezone.Children.Add(statndard);
            //statndard.AddProperty("DTSTART", ToFilterTime(DateTime.Now));

            //var rrule = new Ical.Net.DataTypes.RecurrencePattern(Ical.Net.FrequencyType.Yearly);
            //rrule.ByDay.Add(new Ical.Net.DataTypes.WeekDay(DayOfWeek.Sunday, Ical.Net.FrequencyOccurrence.Last));
            //rrule.ByMonth.Add(10);

            //statndard.AddProperty("RRULE", rrule.ToString());

            //var daylight = new Daylight();
            //timezone.Children.Add(daylight);
            //rrule = new Ical.Net.DataTypes.RecurrencePattern(Ical.Net.FrequencyType.Yearly);
            //rrule.ByDay.Add(new Ical.Net.DataTypes.WeekDay(DayOfWeek.Sunday, Ical.Net.FrequencyOccurrence.First));
            //rrule.ByMonth.Add(4);

            //daylight.AddProperty("RRULE", rrule.ToString());

            //calendarEntry.TimeZone = timezone;
        }

        public static string ToFilterTime(DateTime? dateTime)
        {
            if (dateTime != null)
            {
                var universalTime = dateTime.Value.ToUniversalTime();
                var universalTimeString = universalTime.ToString("s");
                universalTimeString = universalTimeString.Replace("-", "");
                universalTimeString = universalTimeString.Replace(":", "");
                universalTimeString = string.Concat(universalTimeString, "Z");
                return universalTimeString;
            }
            return default;
        }

        class CalendarMethods
        {
            public static CalendarEntryList GetList(CalendarsResource calendarsResource)
            {
                CalendarsResource.ListRequest listRequest = calendarsResource.List();
                return listRequest.Execute();
            }

            public static CalendarEntry Get(CalendarsResource calendarsResource, string calendarId)
            {
                CalendarsResource.GetRequest getRequest = calendarsResource.Get(calendarId);
                return getRequest.Execute();
            }

            public static InsertResponseObject Insert(CalendarsResource calendarsResource, CalendarEntry calendarEntry)
            {
                CalendarsResource.InsertRequest insertRequest = calendarsResource.Insert(calendarEntry);
                return insertRequest.Execute();
            }

            public static UpdateResponseObject Update(CalendarsResource calendarsResource, CalendarEntry calendarEntry)
            {
                CalendarsResource.UpdateRequest updateRequest = calendarsResource.Update(calendarEntry);
                return updateRequest.Execute();
            }

            public static DeleteResponseObject Delete(CalendarsResource calendarsResource, string calendarId)
            {
                CalendarsResource.DeleteRequest deleteRequest = calendarsResource.Delete(calendarId);
                return deleteRequest.Execute();
            }
        }

        class EventMethods
        {
            public static EventList GetList(EventsResource eventsResource, string calendarId)
            {
                EventsResource.ListRequest request = eventsResource.List(calendarId);
                return request.Execute();
            }

            public static Event Get(EventsResource eventsResource, string calendarId, string eventId)
            {
                EventsResource.GetRequest request = eventsResource.Get(calendarId, eventId);
                return request.Execute();
            }

            public static InsertResponseObject Insert(EventsResource eventsResource, Event @event, string calendarId)
            {
                EventsResource.InsertRequest request = eventsResource.Insert(@event, calendarId);
                return request.Execute();
            }

            public static UpdateResponseObject Update(EventsResource eventsResource, Event @event, string calendarId)
            {
                EventsResource.UpdateRequest request = eventsResource.Update(@event, calendarId);
                return request.Execute();
            }

            public static DeleteResponseObject Delete(EventsResource eventsResource, string calendarId, string eventId)
            {
                EventsResource.DeleteRequest request = eventsResource.Delete(calendarId, eventId);
                return request.Execute();
            }
        }

        class ReminderMethods
        {
            public static ReminderList GetList(RemindersResource eventsResource, string calendarId)
            {
                RemindersResource.ListRequest request = eventsResource.List(calendarId);
                return request.Execute();
            }

            public static Reminder Get(RemindersResource eventsResource, string calendarId, string reminderId)
            {
                RemindersResource.GetRequest request = eventsResource.Get(calendarId, reminderId);
                return request.Execute();
            }

            public static InsertResponseObject Insert(RemindersResource eventsResource, Reminder reminder, string calendarId)
            {
                RemindersResource.InsertRequest request = eventsResource.Insert(reminder, calendarId);
                return request.Execute();
            }

            public static UpdateResponseObject Update(RemindersResource eventsResource, Reminder reminder, string calendarId)
            {
                RemindersResource.UpdateRequest request = eventsResource.Update(reminder, calendarId);
                return request.Execute();
            }

            public static DeleteResponseObject Delete(RemindersResource eventsResource, string calendarId, string reminderId)
            {
                RemindersResource.DeleteRequest request = eventsResource.Delete(calendarId, reminderId);
                return request.Execute();
            }
        }

        private static CalendarService _calendarService;
        private static CalendarService GetCalendarService()
        {
            if (_calendarService == null)
            {
                IDataStore dataStore = new FileDataStore("icloudStore");

                UserCredential credential = AuthorizationBroker.AuthorizeAsync("gachris",
                    NetworkCredential,
                    CancellationToken.None,
                    dataStore)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();

                _calendarService = new CalendarService(new BaseClientService.Initializer()
                {
                    ApplicationName = "iCloud.SyncApp",
                    HttpClientInitializer = credential
                });
            }
            return _calendarService;
        }

        private static NetworkCredential _networkCredential;
        public static NetworkCredential NetworkCredential
        {
            get
            {
                if (_networkCredential == null)
                {
                    _networkCredential = new NetworkCredential("icloud.email", "icloud.app-specific-password");
                }
                return _networkCredential;
            }
        }
    }
}
