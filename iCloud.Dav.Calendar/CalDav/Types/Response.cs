using System.Collections.Generic;
using System.Net;

namespace iCloud.Dav.Calendar.CalDav.Types
{
    internal sealed class Response
    {
        public string Href { get; set; }

        public string DisplayName { get; set; }

        public string CalendarColor { get; set; }

        public string Etag { get; set; }

        public Attribute Ctag { get; set; }

        public Attribute CalendarData { get; set; }

        public List<Privilege> CurrentUserPrivilegeSet { get; }

        public List<SupportedReport> SupportedReportSet { get; }

        public List<CalendarComponent> SupportedCalendarComponentSet { get; }

        public List<ResourceType> ResourceType { get; }

        public string SyncToken { get; set; }

        public int CalendarOrder { get; set; }

        public HttpStatusCode Status { get; set; }

        public string CalendarTimeZone { get;  set; }

        public string CalendarDescription { get; set; } 

        public Response()
        {
            CurrentUserPrivilegeSet = new List<Privilege>();
            SupportedReportSet = new List<SupportedReport>();
            SupportedCalendarComponentSet = new List<CalendarComponent>();
            ResourceType = new List<ResourceType>();
        }
    }
}