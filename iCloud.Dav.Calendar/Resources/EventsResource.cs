using Ical.Net.Serialization;
using iCloud.Dav.Calendar.CalDav.Types;
using iCloud.Dav.Calendar.DataTypes;
using iCloud.Dav.Calendar.Request;
using iCloud.Dav.Calendar.Utils;
using iCloud.Dav.Core;
using iCloud.Dav.Core.Response;
using iCloud.Dav.Core.Utils;
using System;

namespace iCloud.Dav.Calendar.Resources
{
    /// <summary>
    /// The events collection of methods.
    /// </summary>
    public class EventsResource
    {
        /// <summary>
        /// The service which this resource belongs to.
        /// </summary>
        private readonly IClientService _service;

        /// <summary>
        /// Constructs a new resource.
        /// </summary>
        public EventsResource(IClientService service) => _service = service;

        /// <summary>
        /// Returns events on the specified calendar.
        /// </summary>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the <see cref="CalendarsResource.List"/> method.</param>
        public virtual ListRequest List(string calendarId) => new ListRequest(_service, calendarId);

        /// <summary>
        /// Returns an event.
        /// </summary>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the <see cref="CalendarsResource.List"/> method.</param>
        /// <param name="eventId">Event identifier. To retrieve event IDs call the <see cref="List"/> method.</param>
        public virtual GetRequest Get(string calendarId, string eventId) => new GetRequest(_service, calendarId, eventId);

        /// <summary>
        /// Creates an event.
        /// </summary>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the <see cref="CalendarsResource.List"/> method.</param>
        /// <param name="body">The body of the request.</param>
        public virtual InsertRequest Insert(Event body, string calendarId) => new InsertRequest(_service, body, calendarId);

        /// <summary>
        /// Updates an event.
        /// </summary>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the <see cref="CalendarsResource.List"/> method.</param>
        /// <param name="body">The body of the request.</param>
        public virtual UpdateRequest Update(Event body, string calendarId) => new UpdateRequest(_service, body, calendarId);

        /// <summary>
        /// Deletes an event.
        /// </summary>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the <see cref="CalendarsResource.List"/> method.</param>
        /// <param name="eventId">Event identifier.</param>
        public virtual DeleteRequest Delete(string calendarId, string eventId) => new DeleteRequest(_service, calendarId, eventId);

        /// <summary>
        /// Returns events on the specified calendar.
        /// </summary>
        public class ListRequest : CalendarBaseServiceRequest<Events>
        {
            private object _body;

            /// <summary>
            /// Constructs a new List request.
            /// </summary>
            public ListRequest(IClientService service, string calendarId) : base(service) => CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(calendarId));

            /// <summary>
            /// Calendar identifier. To retrieve calendar IDs call the <see cref="CalendarsResource.List"/> method.
            /// </summary>
            [RequestParameter("calendarId", RequestParameterType.Path)]
            public virtual string CalendarId { get; }

            /// <summary>
            /// Upper bound (exclusive) for an event's start time to filter by. Optional. The default is not to
            /// filter by start time. Milliseconds may be provided but are ignored. If timeMin is set, timeMax must be greater than timeMin.
            /// </summary>
            public virtual DateTime? TimeMax { get; set; }

            /// <summary>
            /// Lower bound (exclusive) for an event's end time to filter by. Optional. The default is not to
            /// filter by end time. Milliseconds may be provided but are ignored. If timeMax is set, timeMin must be smaller than timeMax.
            /// </summary>
            public virtual DateTime? TimeMin { get; set; }

            /// <inheritdoc/>
            public override string MethodName => "list";

            /// <inheritdoc/>
            public override string HttpMethod => Constants.Report;

            /// <inheritdoc/>
            public override string RestPath => "{calendarId}";

            /// <inheritdoc/>
            public override string Depth => "1";

            /// <inheritdoc/>
            protected override object GetBody()
            {
                if (_body is null)
                {
                    var calendarquery = new CalendarQuery() { CompFilter = new CompFilter("VCALENDAR") };
                    calendarquery.CompFilter.Child = new CompFilter("VEVENT");

                    var timeMin = TimeMin.ToFilterTime();
                    var timeMax = TimeMax.ToFilterTime();

                    if (!string.IsNullOrEmpty(timeMin) || !string.IsNullOrEmpty(timeMax))
                        calendarquery.CompFilter.Child.TimeRange = new TimeRange(timeMin, timeMax);

                    _body = calendarquery;
                }
                return _body;
            }

            /// <inheritdoc/>
            protected override void InitParameters()
            {
                base.InitParameters();

                RequestParameters.Add("calendarId", new Parameter("calendarId", "path", true));
            }
        }

        /// <summary>
        /// Returns an event.
        /// </summary>
        public class GetRequest : CalendarBaseServiceRequest<Event>
        {
            /// <summary>
            /// Constructs a new Get request.
            /// </summary>
            public GetRequest(IClientService service, string calendarId, string eventId) : base(service)
            {
                CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(calendarId));
                EventId = eventId.ThrowIfNullOrEmpty(nameof(eventId));
            }

            /// <summary>
            /// Calendar identifier. To retrieve calendar IDs call the <see cref="CalendarsResource.List"/> method.
            /// </summary>
            [RequestParameter("calendarId", RequestParameterType.Path)]
            public virtual string CalendarId { get; }

            /// <summary>
            /// Event identifier. To retrieve event IDs call the <see cref="List"/> method.
            /// </summary>
            [RequestParameter("eventId", RequestParameterType.Path)]
            public virtual string EventId { get; }

            /// <inheritdoc/>
            public override string MethodName => "get";

            /// <inheritdoc/>
            public override string HttpMethod => Constants.Get;

            /// <inheritdoc/>
            public override string RestPath => "{calendarId}/{eventId}.ics";

            /// <inheritdoc/>
            public override string Depth => "1";

            /// <inheritdoc/>
            protected override void InitParameters()
            {
                base.InitParameters();

                RequestParameters.Add("calendarId", new Parameter("calendarId", "path", true));
                RequestParameters.Add("eventId", new Parameter("eventId", "path", true));
            }
        }

        /// <summary>
        /// Creates an event.
        /// </summary>
        public class InsertRequest : CalendarBaseServiceRequest<VoidResponse>
        {
            private object _body;

            /// <summary>
            /// Constructs a new Insert request.
            /// </summary>
            public InsertRequest(IClientService service, Event body, string calendarId) : base(service)
            {
                Body = body.ThrowIfNull(nameof(body));
                EventId = body.Uid.ThrowIfNull(nameof(body.Uid));
                CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(calendarId));
            }

            /// <summary>
            /// Event identifier.
            /// </summary>
            [RequestParameter("eventId", RequestParameterType.Path)]
            public virtual string EventId { get; }

            /// <summary>
            /// Calendar identifier. To retrieve calendar IDs call the <see cref="CalendarsResource.List"/> method.
            /// </summary>
            [RequestParameter("calendarId", RequestParameterType.Path)]
            public virtual string CalendarId { get; }

            /// <summary>
            /// Gets the body of this request.
            /// </summary>
            private Event Body { get; }

            /// <inheritdoc/>
            public override string MethodName => "insert";

            /// <inheritdoc/>
            public override string HttpMethod => Constants.Put;

            /// <inheritdoc/>
            public override string RestPath => "{calendarId}/{eventId}.ics";

            /// <inheritdoc/>
            public override string ContentType => Constants.TEXT_CALENDAR;

            /// <inheritdoc/>
            protected override object GetBody()
            {
                if (_body is null)
                {
                    var calendar = new Ical.Net.Calendar();
                    var calendarSerializer = new CalendarSerializer();
                    calendar.Events.Add(Body);
                    _body = calendarSerializer.SerializeToString(calendar);
                }
                return _body;
            }

            /// <inheritdoc/>
            protected override void InitParameters()
            {
                base.InitParameters();

                RequestParameters.Add("calendarId", new Parameter("calendarId", "path", true));
                RequestParameters.Add("eventId", new Parameter("eventId", "path", true));
            }
        }

        /// <summary>
        /// Updates an event.
        /// </summary>
        public class UpdateRequest : CalendarBaseServiceRequest<VoidResponse>
        {
            private object _body;

            /// <summary>
            /// Constructs a new Update request.
            /// </summary>
            public UpdateRequest(IClientService service, Event body, string calendarId) : base(service)
            {
                Body = body.ThrowIfNull(nameof(body));
                EventId = Body.Uid.ThrowIfNullOrEmpty(nameof(Event.Uid));
                CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(calendarId));
            }

            /// <summary>
            /// Calendar identifier. To retrieve calendar IDs call the <see cref="CalendarsResource.List"/> method.
            /// </summary>
            [RequestParameter("calendarId", RequestParameterType.Path)]
            public virtual string CalendarId { get; }

            /// <summary>
            /// Event identifier. To retrieve event IDs call the <see cref="List"/> method.
            /// </summary>
            [RequestParameter("eventId", RequestParameterType.Path)]
            public virtual string EventId { get; }

            /// <summary>
            /// Gets the body of this request.
            /// </summary>
            private Event Body { get; }

            /// <inheritdoc/>
            public override string MethodName => "update";

            /// <inheritdoc/>
            public override string HttpMethod => Constants.Put;

            /// <inheritdoc/>
            public override string RestPath => "{calendarId}/{eventId}.ics";

            /// <inheritdoc/>
            public override string ContentType => Constants.TEXT_CALENDAR;

            /// <inheritdoc/>
            protected override object GetBody()
            {
                if (_body is null)
                {
                    var calendar = new Ical.Net.Calendar();
                    var calendarSerializer = new CalendarSerializer();
                    calendar.Events.Add(Body);
                    _body = calendarSerializer.SerializeToString(calendar);
                }
                return _body;
            }

            /// <inheritdoc/>
            protected override void InitParameters()
            {
                base.InitParameters();

                RequestParameters.Add("calendarId", new Parameter("calendarId", "path", true));
                RequestParameters.Add("eventId", new Parameter("eventId", "path", true));
            }
        }

        /// <summary>
        /// Deletes an event.
        /// </summary>
        public class DeleteRequest : CalendarBaseServiceRequest<VoidResponse>
        {
            /// <summary>
            /// Constructs a new Delete request.
            /// </summary>
            public DeleteRequest(IClientService service, string calendarId, string eventId) : base(service)
            {
                CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(calendarId));
                EventId = eventId.ThrowIfNullOrEmpty(nameof(eventId));
            }

            /// <summary>
            /// Calendar identifier. To retrieve calendar IDs call the <see cref="CalendarsResource.List"/> method.
            /// </summary>
            [RequestParameter("calendarId", RequestParameterType.Path)]
            public virtual string CalendarId { get; }

            /// <summary>
            /// Event identifier. To retrieve event IDs call the <see cref="List"/> method.
            /// </summary>
            [RequestParameter("eventId", RequestParameterType.Path)]
            public virtual string EventId { get; }

            /// <inheritdoc/>
            public override string MethodName => "delete";

            /// <inheritdoc/>
            public override string HttpMethod => Constants.Delete;

            /// <inheritdoc/>
            public override string RestPath => "{calendarId}/{eventId}.ics";

            /// <inheritdoc/>
            public override string Depth => "1";

            /// <inheritdoc/>
            protected override void InitParameters()
            {
                base.InitParameters();

                RequestParameters.Add("calendarId", new Parameter("calendarId", "path", true));
                RequestParameters.Add("eventId", new Parameter("eventId", "path", true));
            }
        }
    }
}