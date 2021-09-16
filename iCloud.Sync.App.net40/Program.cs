using Ical.Net.DataTypes;
using iCloud.Dav.Calendar;
using iCloud.Dav.Calendar.Services;
using iCloud.Dav.People;
using iCloud.Dav.People.Services;
using System;
using System.Linq;

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
            CalendarService service = Services.GetCalendarService();
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
            CalendarService service = Services.GetCalendarService();
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
            PeopleService service = Services.GetPeopleService();
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
    }
}
