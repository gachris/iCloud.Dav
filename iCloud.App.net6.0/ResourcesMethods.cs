using iCloud.Dav.ICalendar;
using iCloud.Dav.ICalendar.Resources;
using iCloud.Dav.Core.Response;
using iCloud.Dav.People;
using iCloud.Dav.People.Resources;

namespace iCloud.Sync.App
{
    public class CalendarMethods
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

    public class EventMethods
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

    public class ReminderMethods
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

    public class IdentityCardMethods
    {
        public static IdentityCardList GetList(IdentityCardResource identityCardResource)
        {
            IdentityCardResource.ListRequest listRequest = identityCardResource.List();
            return listRequest.Execute();
        }
    }

    public class ContactGroupMethods
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

    public class PeopleMethods
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
}
