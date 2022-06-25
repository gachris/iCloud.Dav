using Ical.Net.DataTypes;
using iCloud.Dav.Calendar;
using iCloud.Dav.People;
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
            var service = Services.GetCalendarService();
            var eventId = Guid.NewGuid().ToString().ToUpper();
            var calendarId = Guid.NewGuid().ToString().ToUpper();

            // create new calendar 
            var calendarToInsert = new CalendarEntry
            {
                Id = calendarId,
                Summary = "New calendar from calendar test method",
                Color = "#FFFFFFFF"
            };
            calendarToInsert.SupportedCalendarComponents.Add("VEVENT");

            // insert calendar
            _ = CalendarMethods.Insert(service.Calendars, calendarToInsert);

            // get calendars
            var calendarEntries = CalendarMethods.GetList(service.Calendars);
            var insertedCalendar = calendarEntries.Where(x => x.Id == calendarToInsert.Id).SingleOrDefault();

            // update calendar fields
            var calendarToUpdate = (CalendarEntry)insertedCalendar.Clone();
            calendarToUpdate.Summary = "Updated calendar from calendar test method";
            calendarToUpdate.Color = "#FF000000";

            // update calendar
            _ = CalendarMethods.Update(service.Calendars, calendarToUpdate);

            // get calendar
            _ = CalendarMethods.Get(service.Calendars, calendarId);

            // create new event 
            var eventToInserted = new Event
            {
                Uid = eventId,
                Summary = "New event from event test method",
                Description = "New event from event test method",
                Start = new CalDateTime(DateTime.Now),
                End = new CalDateTime(DateTime.Now.AddHours(1))
            };

            // insert event
            _ = EventMethods.Insert(service.Events, eventToInserted, calendarId);

            // get events
            var eventList = EventMethods.GetList(service.Events, calendarId);
            var insertedEvent = eventList.Where(x => x.Uid == eventToInserted.Uid).SingleOrDefault();

            // update event fields
            var eventToUpdate = (Event)insertedEvent.Clone();
            eventToUpdate.Summary = "Updated event from event test method";
            eventToUpdate.Description = "Updated event from event test method";
            eventToUpdate.Start = new CalDateTime(DateTime.Now.AddHours(1));
            eventToUpdate.End = new CalDateTime(DateTime.Now.AddHours(2));

            // update event
            _ = EventMethods.Update(service.Events, eventToUpdate, calendarId);

            // get event
            _ = EventMethods.Get(service.Events, calendarId, eventId);

            // delete event
            _ = EventMethods.Delete(service.Events, calendarId, eventId);

            // delete calendar
            _ = CalendarMethods.Delete(service.Calendars, calendarId);
        }

        public static void RemindersCalendar()
        {
            var service = Services.GetCalendarService();
            var calendarId = Guid.NewGuid().ToString().ToUpper();
            var reminderId = Guid.NewGuid().ToString().ToUpper();

            // create new calendar 
            var calendarToInsert = new CalendarEntry
            {
                Id = calendarId,
                Summary = "New reminder list from reminders test method",
                Color = "#FFFFFFFF"
            };
            calendarToInsert.SupportedCalendarComponents.Add("VTODO");

            // insert calendar
            _ = CalendarMethods.Insert(service.Calendars, calendarToInsert);

            // get calendars
            var calendarEntries = CalendarMethods.GetList(service.Calendars);
            var insertedCalendar = calendarEntries.Where(x => x.Id == calendarToInsert.Id).SingleOrDefault();

            // update calendar fields
            var calendarToUpdate = (CalendarEntry)insertedCalendar.Clone();
            calendarToUpdate.Summary = "Updated reminder list from reminders test method";
            calendarToUpdate.Color = "#FF000000";

            // update calendar
            _ = CalendarMethods.Update(service.Calendars, calendarToUpdate);

            // get calendar
            _ = CalendarMethods.Get(service.Calendars, calendarId);

            // create new reminder 
            var reminderToInserted = new Reminder
            {
                Uid = reminderId,
                Summary = "New reminder from reminder test method",
                Description = "New reminder from reminder test method",
                Start = new CalDateTime(DateTime.Now)
            };

            // insert reminder
            _ = ReminderMethods.Insert(service.Reminders, reminderToInserted, calendarId);

            // get reminders
            var reminderList = ReminderMethods.GetList(service.Reminders, calendarId);
            var insertedReminder = reminderList.Where(x => x.Uid == reminderToInserted.Uid).SingleOrDefault();

            // update event fields
            var reminderToUpdate = (Reminder)insertedReminder.Clone();
            reminderToUpdate.Summary = "Updated reminder from reminder test method";
            reminderToUpdate.Description = "Updated reminder from reminder test method";
            reminderToUpdate.Start = new CalDateTime(DateTime.Now.AddHours(1));

            // update event
            _ = ReminderMethods.Update(service.Reminders, reminderToUpdate, calendarId);

            // get event
            _ = ReminderMethods.Get(service.Reminders, calendarId, reminderId);

            // delete event
            _ = ReminderMethods.Delete(service.Reminders, calendarId, reminderId);

            // delete calendar
            _ = CalendarMethods.Delete(service.Calendars, calendarId);
        }

        public static void People()
        {
            var service = Services.GetPeopleService();
            string personId = Guid.NewGuid().ToString().ToUpper();
            string contactGroupId = Guid.NewGuid().ToString().ToUpper();

            // get identity cards
            var identityCards = IdentityCardMethods.GetList(service.IdentityCard);

            // get resourceName
            var resourceName = identityCards.FirstOrDefault().ResourceName;

            // create new contactGroup 
            var contactGroup = new ContactGroup
            {
                UniqueId = contactGroupId,
                FamilyName = "New Group Description",
                FormattedName = "New Group Description"
            };

            // insert contact group
            _ = ContactGroupMethods.Insert(service.ContactGroups, contactGroup, resourceName);

            // create new person 
            var person = new Person
            {
                UniqueId = personId,
                FamilyName = "New Contact Description",
                FormattedName = "New Contact Description"
            };

            // insert person
            _ = PeopleMethods.Insert(service.People, person, resourceName);

            // get contact groups
            _ = ContactGroupMethods.GetList(service.ContactGroups, resourceName);

            // get contact group
            contactGroup = ContactGroupMethods.Get(service.ContactGroups, contactGroupId, resourceName);

            // update contact group fields
            contactGroup.FormattedName = "Updated Group Description";
            contactGroup.FamilyName = "Updated Group Description";
            contactGroup.MemberResourceNames.Add(personId);

            // update contact group
            _ = ContactGroupMethods.Update(service.ContactGroups, contactGroup, resourceName);

            // get persons 
            var people = PeopleMethods.GetList(service.People, resourceName);

            // get person
            person = PeopleMethods.Get(service.People, personId, resourceName);

            // update person fields
            person.FormattedName = "Updated Contact Description";
            person.FamilyName = "Updated Contact Description";
            person.Notes.Add(new Note() { Text = "Alli mia wraia mera!" });
            person.Addresses.Add(new Address() { AddressType = AddressTypes.Home, City = "Athens", Country = "Greece", Street = "Arkesilaou 4", PostalCode = "11473" });
            person.Labels.Add(new Label() { Text = "new label here!" });
            person.Websites.Add(new Website() { Url = "www.index.com", WebsiteType = WebsiteTypes.Work });
            // update person
            _ = PeopleMethods.Update(service.People, person, resourceName);

            // get person
            person = PeopleMethods.Get(service.People, personId, resourceName);

            // delete person
            _ = PeopleMethods.Delete(service.People, personId, resourceName);

            // delete contact group
            _ = ContactGroupMethods.Delete(service.ContactGroups, contactGroupId, resourceName);
        }

        public static void AddTimezone()
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
