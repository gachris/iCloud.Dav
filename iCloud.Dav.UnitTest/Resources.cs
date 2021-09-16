using Ical.Net.DataTypes;
using iCloud.Dav.Calendar;
using iCloud.Dav.Calendar.Services;
using iCloud.Dav.People;
using iCloud.Dav.People.Services;
using iCloud.Sync.App;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace iCloud.Dav.UnitTest
{
    [TestClass]
    public class Resources
    {
        [TestMethod]
        public void EventsCalendar()
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

            Assert.IsNotNull(insertResponseObject);

            // get calendars
            CalendarEntryList calendarEntries = CalendarMethods.GetList(service.Calendars);
            var insertedCalendar = calendarEntries.Where(x => x.Id == calendarToInsert.Id).SingleOrDefault();

            Assert.IsNotNull(insertedCalendar);
            Assert.AreEqual(insertedCalendar.Summary, calendarToInsert.Summary);
            Assert.AreEqual(insertedCalendar.Color, calendarToInsert.Color);
            Assert.AreNotEqual(insertedCalendar.CTag, calendarToInsert.CTag);
            Assert.AreNotEqual(insertedCalendar.ETag, calendarToInsert.ETag);

            // update calendar fields
            CalendarEntry calendarToUpdate = (CalendarEntry)insertedCalendar.Clone();
            calendarToUpdate.Summary = "Updated calendar from calendar test method";
            calendarToUpdate.Color = "#FF000000";

            // update calendar
            var updateResponseObject = CalendarMethods.Update(service.Calendars, calendarToUpdate);

            Assert.IsNotNull(updateResponseObject);

            // get calendar
            var updatedCalendar = CalendarMethods.Get(service.Calendars, calendarId);

            Assert.IsNotNull(updatedCalendar);
            Assert.AreEqual(updatedCalendar.Summary, calendarToUpdate.Summary);
            Assert.AreEqual(updatedCalendar.Color, calendarToUpdate.Color);
            Assert.AreNotEqual(updatedCalendar.CTag, calendarToUpdate.CTag);
            Assert.AreNotEqual(updatedCalendar.ETag, calendarToUpdate.ETag);

            // create new event 
            Event eventToInserted = new Event();
            eventToInserted.Uid = eventId;
            eventToInserted.Summary = "New event from event test method";
            eventToInserted.Description = "New event from event test method";
            eventToInserted.Start = new CalDateTime(DateTime.Now);
            eventToInserted.End = new CalDateTime(DateTime.Now.AddHours(1));

            // insert event
            var eventInsertResponseObject = EventMethods.Insert(service.Events, eventToInserted, calendarId);

            Assert.IsNotNull(eventInsertResponseObject);

            // get events
            EventList eventList = EventMethods.GetList(service.Events, calendarId);
            var insertedEvent = eventList.Where(x => x.Uid == eventToInserted.Uid).SingleOrDefault();

            Assert.IsNotNull(insertedEvent);
            Assert.AreEqual(insertedEvent.Summary, eventToInserted.Summary);
            Assert.AreEqual(insertedEvent.Description, eventToInserted.Description);
            Assert.AreEqual(insertedEvent.Start, eventToInserted.Start);
            Assert.AreEqual(insertedEvent.End, eventToInserted.End);
            Assert.AreNotEqual(insertedEvent.ETag, eventToInserted.ETag);

            // update event fields
            Event eventToUpdate = (Event)insertedEvent.Clone();
            eventToUpdate.Summary = "Updated event from event test method";
            eventToUpdate.Description = "Updated event from event test method";
            eventToUpdate.Start = new CalDateTime(DateTime.Now.AddHours(1));
            eventToUpdate.End = new CalDateTime(DateTime.Now.AddHours(2));

            // update event
            var eventUpdateResponseObject = EventMethods.Update(service.Events, eventToUpdate, calendarId);

            Assert.IsNotNull(eventUpdateResponseObject);

            // get event
            var updatedEvent = EventMethods.Get(service.Events, calendarId, eventId);

            Assert.IsNotNull(updatedEvent);
            Assert.AreEqual(updatedEvent.Summary, eventToUpdate.Summary);
            Assert.AreEqual(updatedEvent.Description, eventToUpdate.Description);
            Assert.AreEqual(updatedEvent.Start, eventToUpdate.Start);
            Assert.AreEqual(updatedEvent.End, eventToUpdate.End);
            Assert.AreNotEqual(updatedEvent.ETag, eventToUpdate.ETag);

            // delete event
            var eventDeleteResponseObject = EventMethods.Delete(service.Events, calendarId, eventId);

            Assert.IsNotNull(eventDeleteResponseObject);

            // delete calendar
            var calendarDeleteResponseObject = CalendarMethods.Delete(service.Calendars, calendarId);

            Assert.IsNotNull(calendarDeleteResponseObject);
        }

        [TestMethod]
        public void RemindersCalendar()
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

            Assert.IsNotNull(insertResponseObject);

            // get calendars
            CalendarEntryList calendarEntries = CalendarMethods.GetList(service.Calendars);
            var insertedCalendar = calendarEntries.Where(x => x.Id == calendarToInsert.Id).SingleOrDefault();

            Assert.IsNotNull(insertedCalendar);
            Assert.AreEqual(insertedCalendar.Summary, calendarToInsert.Summary);
            Assert.AreEqual(insertedCalendar.Color, calendarToInsert.Color);
            Assert.AreNotEqual(insertedCalendar.CTag, calendarToInsert.CTag);
            Assert.AreNotEqual(insertedCalendar.ETag, calendarToInsert.ETag);

            // update calendar fields
            CalendarEntry calendarToUpdate = (CalendarEntry)insertedCalendar.Clone();
            calendarToUpdate.Summary = "Updated reminder list from reminders test method";
            calendarToUpdate.Color = "#FF000000";

            // update calendar
            var updateResponseObject = CalendarMethods.Update(service.Calendars, calendarToUpdate);

            Assert.IsNotNull(updateResponseObject);

            // get calendar
            var updatedCalendar = CalendarMethods.Get(service.Calendars, calendarId);

            Assert.IsNotNull(updatedCalendar);
            Assert.AreNotEqual(updatedCalendar.Summary, insertedCalendar.Summary);
            Assert.AreNotEqual(updatedCalendar.Color, insertedCalendar.Color);
            Assert.AreNotEqual(updatedCalendar.CTag, insertedCalendar.CTag);
            Assert.AreNotEqual(updatedCalendar.ETag, insertedCalendar.ETag);

            // create new reminder 
            Reminder reminderToInserted = new Reminder();
            reminderToInserted.Uid = reminderId;
            reminderToInserted.Summary = "New reminder from reminder test method";
            reminderToInserted.Description = "New reminder from reminder test method";
            reminderToInserted.Start = new CalDateTime(DateTime.Now);

            // insert reminder
            var reminderInsertResponseObject = ReminderMethods.Insert(service.Reminders, reminderToInserted, calendarId);

            Assert.IsNotNull(reminderInsertResponseObject);

            // get reminders
            ReminderList reminderList = ReminderMethods.GetList(service.Reminders, calendarId);
            var insertedReminder = reminderList.Where(x => x.Uid == reminderToInserted.Uid).SingleOrDefault();

            Assert.IsNotNull(insertedReminder);
            Assert.AreEqual(insertedReminder.Summary, reminderToInserted.Summary);
            Assert.AreEqual(insertedReminder.Description, reminderToInserted.Description);
            Assert.AreEqual(insertedReminder.Start, reminderToInserted.Start);
            Assert.AreNotEqual(insertedReminder.ETag, reminderToInserted.ETag);

            // update event fields
            Reminder reminderToUpdate = (Reminder)insertedReminder.Clone();
            reminderToUpdate.Summary = "Updated reminder from reminder test method";
            reminderToUpdate.Description = "Updated reminder from reminder test method";
            reminderToUpdate.Start = new CalDateTime(DateTime.Now.AddHours(1));

            // update event
            var eventUpdateResponseObject = ReminderMethods.Update(service.Reminders, reminderToUpdate, calendarId);

            Assert.IsNotNull(eventUpdateResponseObject);

            // get event
            var updatedReminnder = ReminderMethods.Get(service.Reminders, calendarId, reminderId);

            Assert.IsNotNull(updatedReminnder);
            Assert.AreEqual(updatedReminnder.Summary, reminderToUpdate.Summary);
            Assert.AreEqual(updatedReminnder.Description, reminderToUpdate.Description);
            Assert.AreEqual(updatedReminnder.Start, reminderToUpdate.Start);
            Assert.AreNotEqual(updatedReminnder.ETag, reminderToUpdate.ETag);

            // delete event
            var eventDeleteResponseObject = ReminderMethods.Delete(service.Reminders, calendarId, reminderId);

            Assert.IsNotNull(eventDeleteResponseObject);

            // delete calendar
            var calendarDeleteResponseObject = CalendarMethods.Delete(service.Calendars, calendarId);

            Assert.IsNotNull(calendarDeleteResponseObject);
        }


        [TestMethod]
        public void People()
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
