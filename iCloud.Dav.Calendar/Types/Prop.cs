using System.Xml.Serialization;

namespace iCloud.Dav.ICalendar.Types
{
    [XmlRoot(ElementName = "prop", Namespace = "DAV:")]
    public class Prop
    {
        [XmlElement(ElementName = "displayname", Namespace = "DAV:")]
        public Displayname Displayname { get; set; }

        [XmlElement(ElementName = "getctag", Namespace = "http://calendarserver.org/ns/")]
        public Getctag Getctag { get; set; }

        [XmlElement(ElementName = "current-user-privilege-set", Namespace = "DAV:")]
        public Currentuserprivilegeset Currentuserprivilegeset { get; set; }

        [XmlElement(ElementName = "calendar-home-set", Namespace = "urn:ietf:params:xml:ns:caldav")]
        public Calendarhomeset Calendarhomeset { get; set; }

        [XmlElement(ElementName = "calendar-user-address-set", Namespace = "urn:ietf:params:xml:ns:caldav")]
        public Calendaruseraddressset Calendaruseraddressset { get; set; }

        [XmlElement(ElementName = "current-user-principal", Namespace = "DAV:")]
        public Currentuserprincipal Currentuserprincipal { get; set; }

        [XmlElement(ElementName = "email-address-set", Namespace = "http://calendarserver.org/ns/")]
        public Emailaddressset Emailaddressset { get; set; }

        [XmlElement(ElementName = "notification-URL", Namespace = "http://calendarserver.org/ns/")]
        public NotificationURL NotificationURL { get; set; }

        [XmlElement(ElementName = "principal-collection-set", Namespace = "DAV:")]
        public Principalcollectionset Principalcollectionset { get; set; }

        [XmlElement(ElementName = "principal-URL", Namespace = "DAV:")]
        public PrincipalURL PrincipalURL { get; set; }

        [XmlElement(ElementName = "schedule-inbox-URL", Namespace = "urn:ietf:params:xml:ns:caldav")]
        public ScheduleinboxURL ScheduleinboxURL { get; set; }

        [XmlElement(ElementName = "schedule-outbox-URL", Namespace = "urn:ietf:params:xml:ns:caldav")]
        public ScheduleoutboxURL ScheduleoutboxURL { get; set; }

        [XmlElement(ElementName = "supported-report-set", Namespace = "DAV:")]
        public Supportedreportset Supportedreportset { get; set; }

        [XmlElement(ElementName = "dropbox-home-URL", Namespace = "http://calendarserver.org/ns/")]
        public DropboxhomeURL DropboxhomeURL { get; set; }

        [XmlElement(ElementName = "resource-id", Namespace = "DAV:")]
        public ResourceId ResourceId { get; set; }

        [XmlElement(ElementName = "calendar-data", Namespace = "urn:ietf:params:xml:ns:caldav")]
        public Calendardata Calendardata { get; set; }

        [XmlElement(ElementName = "getetag", Namespace = "DAV:")]
        public Getetag Getetag { get; set; }

        [XmlElement(ElementName = "calendar-color", Namespace = "http://apple.com/ns/ical/")]
        public CalendarColor CalendarColor { get; set; }

        [XmlElement(ElementName = "supported-calendar-component-set", Namespace = "urn:ietf:params:xml:ns:caldav")]
        public SupportedCalendarComponentSet SupportedCalendarComponentSet { get; set; }

        [XmlElement(ElementName = "calendar-timezone", Namespace = "urn:ietf:params:xml:ns:caldav")]
        public CalendarTimeZone CalendarTimeZone { get; set; }

        [XmlText]
        public string Value { get; set; }
    }
}
