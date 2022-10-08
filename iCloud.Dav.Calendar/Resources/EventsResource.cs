using Ical.Net.Serialization;
using iCloud.Dav.Calendar.CalDav.Types;
using iCloud.Dav.Calendar.Request;
using iCloud.Dav.Calendar.Types;
using iCloud.Dav.Calendar.Utils;
using iCloud.Dav.Core;
using iCloud.Dav.Core.Response;
using iCloud.Dav.Core.Utils;
using System;

namespace iCloud.Dav.Calendar.Resources;

/// <summary>The "events" collection of methods.</summary>
public class EventsResource
{
    /// <summary>The service which this resource belongs to.</summary>
    private readonly IClientService _service;

    /// <summary>Constructs a new resource.</summary>
    public EventsResource(IClientService service)
    {
        _service = service;
    }

    /// <summary>Returns events on the specified calendar.</summary>
    /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the calendarList.list method.</param>
    public virtual ListRequest List(string calendarId)
    {
        return new ListRequest(_service, calendarId);
    }

    /// <summary>Returns an event.</summary>
    /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the calendarList.list method.</param>
    /// <param name="eventId">Event identifier.</param>
    public virtual GetRequest Get(string calendarId, string eventId)
    {
        return new GetRequest(_service, calendarId, eventId);
    }

    /// <summary>Creates an event.</summary>
    /// <param name="body">The body of the request.</param>
    /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the calendarList.list method.</param>
    public virtual InsertRequest Insert(Event body, string calendarId)
    {
        return new InsertRequest(_service, body, calendarId);
    }

    /// <summary>Updates an event.</summary>
    /// <param name="body">The body of the request.</param>
    /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the calendarList.list method.</param>
    public virtual UpdateRequest Update(Event body, string calendarId)
    {
        return new UpdateRequest(_service, body, calendarId);
    }

    /// <summary>Deletes an event.</summary>
    /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the calendarList.list method.</param>
    /// <param name="eventId">Event identifier.</param>
    public virtual DeleteRequest Delete(string calendarId, string eventId)
    {
        return new DeleteRequest(_service, calendarId, eventId);
    }

    /// <summary>Returns events on the specified calendar.</summary>
    public class ListRequest : CalendarBaseServiceRequest<EventList>
    {
        /// <summary>Constructs a new List request.</summary>
        public ListRequest(IClientService service, string calendarId) : base(service)
        {
            CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(calendarId));
            InitParameters();
        }

        /// <summary>Calendar identifier. To retrieve calendar IDs call the calendarList.list method.</summary>
        [RequestParameter("calendarId", RequestParameterType.Path)]
        public virtual string CalendarId { get; }

        /// <summary>Upper bound (exclusive) for an event's start time to filter by. Optional. The default is not to
        /// filter by start time. Must be an RFC3339 timestamp with mandatory time zone offset, for example,
        /// 2011-06-03T10:00:00-07:00, 2011-06-03T10:00:00Z. Milliseconds may be provided but are ignored. If
        /// timeMin is set, timeMax must be greater than timeMin.</summary>
        public virtual DateTime? TimeMax { get; set; }

        /// <summary>Lower bound (exclusive) for an event's end time to filter by. Optional. The default is not to
        /// filter by end time. Must be an RFC3339 timestamp with mandatory time zone offset, for example,
        /// 2011-06-03T10:00:00-07:00, 2011-06-03T10:00:00Z. Milliseconds may be provided but are ignored. If
        /// timeMax is set, timeMin must be smaller than timeMax.</summary>
        public virtual DateTime? TimeMin { get; set; }

        /// <summary>Gets the method name.</summary>
        public override string MethodName => "list";

        /// <summary>Gets the HTTP method.</summary>
        public override string HttpMethod => ApiMethod.Report;

        /// <summary>Gets the REST path.</summary>
        public override string RestPath => "{calendarId}";

        ///<summary>Gets the depth.</summary>
        public override string Depth => "1";

        /// <summary>Returns the body of the request.</summary>
        protected override object GetBody()
        {
            var calendarquery = new CalendarQuery() { CompFilter = new CompFilter() { Name = "VCALENDAR" } };
            calendarquery.CompFilter.Child = new CompFilter() { Name = "VEVENT" };

            var timeMin = TimeMin.ToFilterTime();
            var timeMax = TimeMax.ToFilterTime();

            if (!string.IsNullOrEmpty(timeMin) || !string.IsNullOrEmpty(timeMax))
                calendarquery.CompFilter.Child.TimeRange = new TimeRange { Start = timeMin, End = timeMax };

            return calendarquery;
        }

        /// <summary>Initializes List parameter list.</summary>
        protected override void InitParameters()
        {
            base.InitParameters();

            RequestParameters.Add("calendarId", new Parameter("calendarId", "path")
            {
                IsRequired = true,
            });
        }
    }

    /// <summary>Returns an event.</summary>
    public class GetRequest : CalendarBaseServiceRequest<Event>
    {
        /// <summary>Constructs a new Get request.</summary>
        public GetRequest(IClientService service, string calendarId, string eventId) : base(service)
        {
            CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(calendarId));
            EventId = eventId.ThrowIfNullOrEmpty(nameof(eventId));
            InitParameters();
        }

        /// <summary>Calendar identifier. To retrieve calendar IDs call the calendarList.list method.</summary>
        [RequestParameter("calendarId", RequestParameterType.Path)]
        public virtual string CalendarId { get; }

        /// <summary>Event identifier.</summary>
        [RequestParameter("eventId", RequestParameterType.Path)]
        public virtual string EventId { get; }

        /// <summary>Gets the method name.</summary>
        public override string MethodName => "get";

        /// <summary>Gets the HTTP method.</summary>
        public override string HttpMethod => ApiMethod.Get;

        /// <summary>Gets the REST path.</summary>
        public override string RestPath => "{calendarId}/{eventId}.ics";

        ///<summary>Gets the depth.</summary>
        public override string Depth => "1";

        /// <summary>Initializes Get parameter list.</summary>
        protected override void InitParameters()
        {
            base.InitParameters();

            RequestParameters.Add("calendarId", new Parameter("calendarId", "path")
            {
                IsRequired = true,
            });
            RequestParameters.Add("eventId", new Parameter("eventId", "path")
            {
                IsRequired = true,
            });
        }
    }

    /// <summary>Creates an event.</summary>
    public class InsertRequest : CalendarBaseServiceRequest<InsertResponseObject>
    {
        /// <summary>Constructs a new Insert request.</summary>
        public InsertRequest(IClientService service, Event body, string calendarId) : base(service)
        {
            CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(calendarId));
            Body = body.ThrowIfNull(nameof(body));
            if (string.IsNullOrEmpty(body.Uid))
                body.Uid = Guid.NewGuid().ToString().ToUpper();
            EventId = body.Uid;
            InitParameters();
        }

        /// <summary>Event identifier.</summary>
        [RequestParameter("eventId", RequestParameterType.Path)]
        public virtual string EventId { get; }

        /// <summary>Calendar identifier. To retrieve calendar IDs call the calendarList.list method.</summary>
        [RequestParameter("calendarId", RequestParameterType.Path)]
        public virtual string CalendarId { get; }

        /// <summary>Gets the body of this request.</summary>
        private Event Body { get; }

        /// <summary>Gets the method name.</summary>
        public override string MethodName => "insert";

        /// <summary>Gets the HTTP method.</summary>
        public override string HttpMethod => ApiMethod.Put;

        /// <summary>Gets the REST path.</summary>
        public override string RestPath => "{calendarId}/{eventId}.ics";

        /// <summary>Gets the Content-Type header of this request.</summary>
        public override string ContentType => ApiContentType.TEXT_CALENDAR;

        /// <summary>Returns the body of the request.</summary>
        protected override object GetBody()
        {
            var calendar = new Ical.Net.Calendar();
            var calendarSerializer = new CalendarSerializer();
            calendar.Events.Add(Body);
            return calendarSerializer.SerializeToString(calendar);
        }

        /// <summary>Initializes Insert parameter list.</summary>
        protected override void InitParameters()
        {
            base.InitParameters();

            RequestParameters.Add("calendarId", new Parameter("calendarId", "path")
            {
                IsRequired = true,
            });
            RequestParameters.Add("eventId", new Parameter("eventId", "path")
            {
                IsRequired = true,
            });
        }
    }

    /// <summary>Updates an event.</summary>
    public class UpdateRequest : CalendarBaseServiceRequest<UpdateResponseObject>
    {
        /// <summary>Constructs a new Update request.</summary>
        public UpdateRequest(IClientService service, Event body, string calendarId) : base(service)
        {
            CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(calendarId));
            Body = body.ThrowIfNull(nameof(body));
            EventId = Body.Uid.ThrowIfNullOrEmpty(nameof(Event.Uid));
            InitParameters();
        }

        /// <summary>Calendar identifier. To retrieve calendar IDs call the calendarList.list method.</summary>
        [RequestParameter("calendarId", RequestParameterType.Path)]
        public virtual string CalendarId { get; }

        /// <summary>Event identifier.</summary>
        [RequestParameter("eventId", RequestParameterType.Path)]
        public virtual string EventId { get; }

        /// <summary>Gets the body of this request.</summary>
        private Event Body { get; }

        /// <summary>Gets the method name.</summary>
        public override string MethodName => "update";

        /// <summary>Gets the HTTP method.</summary>
        public override string HttpMethod => ApiMethod.Put;

        /// <summary>Gets the REST path.</summary>
        public override string RestPath => "{calendarId}/{eventId}.ics";

        /// <summary>Gets the Content-Type header of this request.</summary>
        public override string ContentType => ApiContentType.TEXT_CALENDAR;

        /// <summary>Returns the body of the request.</summary>
        protected override object GetBody()
        {
            var calendar = new Ical.Net.Calendar();
            var calendarSerializer = new CalendarSerializer();
            calendar.Events.Add(Body);
            return calendarSerializer.SerializeToString(calendar);
        }

        /// <summary>Initializes Update parameter list.</summary>
        protected override void InitParameters()
        {
            base.InitParameters();

            RequestParameters.Add("calendarId", new Parameter("calendarId", "path")
            {
                IsRequired = true,
            });
            RequestParameters.Add("eventId", new Parameter("eventId", "path")
            {
                IsRequired = true,
            });
        }
    }

    /// <summary>Deletes an event.</summary>
    public class DeleteRequest : CalendarBaseServiceRequest<DeleteResponseObject>
    {
        /// <summary>Constructs a new Delete request.</summary>
        public DeleteRequest(IClientService service, string calendarId, string eventId) : base(service)
        {
            CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(calendarId));
            EventId = eventId.ThrowIfNullOrEmpty(nameof(eventId));
            InitParameters();
        }

        /// <summary>Calendar identifier. To retrieve calendar IDs call the calendarList.list method.</summary>
        [RequestParameter("calendarId", RequestParameterType.Path)]
        public virtual string CalendarId { get; }

        /// <summary>Event identifier.</summary>
        [RequestParameter("eventId", RequestParameterType.Path)]
        public virtual string EventId { get; }

        /// <summary>Gets the method name.</summary>
        public override string MethodName => "delete";

        /// <summary>Gets the HTTP method.</summary>
        public override string HttpMethod => ApiMethod.Delete;

        /// <summary>Gets the REST path.</summary>
        public override string RestPath => "{calendarId}/{eventId}.ics";

        ///<summary>Gets the depth.</summary>
        public override string Depth => "1";

        /// <summary>Initializes Delete parameter list.</summary>
        protected override void InitParameters()
        {
            base.InitParameters();

            RequestParameters.Add("calendarId", new Parameter("calendarId", "path")
            {
                IsRequired = true,
            });
            RequestParameters.Add("eventId", new Parameter("eventId", "path")
            {
                IsRequired = true,
            });
        }
    }
}
