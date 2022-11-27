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
        /// Returns changes on the specified calendar.
        /// </summary>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the <see cref="CalendarListResource.List"/> method.</param>
        public virtual SyncCollectionRequest SyncCollection(string calendarId) => new SyncCollectionRequest(_service, calendarId);

        /// <summary>
        /// Returns events on the specified calendar.
        /// </summary>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the <see cref="CalendarListResource.List"/> method.</param>
        public virtual ListRequest List(string calendarId) => new ListRequest(_service, calendarId);

        /// <summary>
        /// Returns an event.
        /// </summary>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the <see cref="CalendarListResource.List"/> method.</param>
        /// <param name="eventId">Event identifier. To retrieve event IDs call the <see cref="List"/> method.</param>
        public virtual GetRequest Get(string calendarId, string eventId) => new GetRequest(_service, calendarId, eventId);

        /// <summary>
        /// Returns events on the specified calendar.
        /// </summary>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the <see cref="CalendarListResource.List"/> method.</param>
        /// <param name="eventIds">Event identifiers. To retrieve event IDs call the <see cref="List"/> method.</param>
        public virtual MultiGetRequest MultiGet(string calendarId, params string[] eventIds) => new MultiGetRequest(_service, calendarId, eventIds);

        /// <summary>
        /// Creates an event.
        /// </summary>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the <see cref="CalendarListResource.List"/> method.</param>
        /// <param name="body">The body of the request.</param>
        public virtual InsertRequest Insert(Event body, string calendarId) => new InsertRequest(_service, body, calendarId);

        /// <summary>
        /// Updates an event.
        /// </summary>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the <see cref="CalendarListResource.List"/> method.</param>
        /// <param name="body">The body of the request.</param>
        public virtual UpdateRequest Update(Event body, string calendarId) => new UpdateRequest(_service, body, calendarId);

        /// <summary>
        /// Deletes an event.
        /// </summary>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the <see cref="CalendarListResource.List"/> method.</param>
        /// <param name="eventId">Event identifier.</param>
        public virtual DeleteRequest Delete(string calendarId, string eventId) => new DeleteRequest(_service, calendarId, eventId);

        /// <summary>
        /// Returns changes on the specified calendar.
        /// </summary>
        public class SyncCollectionRequest : CalendarBaseServiceRequest<Events>
        {
            private SyncCollection _body;

            /// <summary>
            /// Constructs a new Sync Collection request.
            /// </summary>
            public SyncCollectionRequest(IClientService service, string calendarId) : base(service)
            {
                SyncLevel = "1";
                CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(calendarId));
            }

            /// <summary>
            /// Calendar identifier. To retrieve calendar IDs call the <see cref="CalendarListResource.List"/> method.
            /// </summary>
            [RequestParameter("calendarId", RequestParameterType.Path)]
            public virtual string CalendarId { get; }

            /// <summary>
            /// Token obtained from the nextSyncToken field returned on the last page of results
            /// from the previous list request. It makes the result of this list request contain
            /// only entries that have changed since then. All events deleted since the previous
            /// list request will always be in the result.
            /// Optional. The default is to return all entries.
            /// </summary>
            public virtual string SyncToken { get; set; }

            public virtual string SyncLevel { get; set; }

            /// <inheritdoc/>
            public override string MethodName => "list";

            /// <inheritdoc/>
            public override string HttpMethod => Constants.Report;

            /// <inheritdoc/>
            public override string RestPath => "{calendarId}";

            /// <inheritdoc/>
            public override string Depth => "0";

            /// <inheritdoc/>
            protected override object GetBody()
            {
                if (_body == null)
                {
                    _body = new SyncCollection();
                }

                _body.SyncToken = SyncToken;
                _body.SyncLevel = SyncLevel;

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
        /// Returns events on the specified calendar.
        /// </summary>
        public class ListRequest : CalendarBaseServiceRequest<Events>
        {
            private CalendarQuery _body;

            /// <summary>
            /// Constructs a new List request.
            /// </summary>
            public ListRequest(IClientService service, string calendarId) : base(service) => CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(calendarId));

            /// <summary>
            /// Calendar identifier. To retrieve calendar IDs call the <see cref="CalendarListResource.List"/> method.
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
                if (_body == null)
                {
                    _body = new CalendarQuery() { CompFilter = new CompFilter("VCALENDAR") };
                    _body.CompFilter.Child = new CompFilter("VEVENT");
                }

                var timeMin = TimeMin.ToFilterTime();
                var timeMax = TimeMax.ToFilterTime();

                if (!string.IsNullOrEmpty(timeMin) || !string.IsNullOrEmpty(timeMax))
                    _body.CompFilter.Child.TimeRange = new TimeRange(timeMin, timeMax);

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
            /// Calendar identifier. To retrieve calendar IDs call the <see cref="CalendarListResource.List"/> method.
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
        /// Returns events on the specified calendar.
        /// </summary>
        public class MultiGetRequest : CalendarBaseServiceRequest<Events>
        {
            private CalendarMultiget _body;

            /// <summary>
            /// Constructs a new Multi Get request.
            /// </summary>
            public MultiGetRequest(IClientService service, string calendarId, params string[] eventIds) : base(service)
            {
                CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(calendarId));
                EventIds = eventIds;
            }

            /// <summary>
            /// Calendar identifier. To retrieve calendar IDs call the <see cref="CalendarListResource.List"/> method.
            /// </summary>
            [RequestParameter("calendarId", RequestParameterType.Path)]
            public virtual string CalendarId { get; }

            /// <summary>
            /// Event identifiers. To retrieve event IDs call the <see cref="List"/> method.
            /// </summary>
            public virtual string[] EventIds { get; }

            /// <inheritdoc/>
            public override string MethodName => "get";

            /// <inheritdoc/>
            public override string HttpMethod => Constants.Report;

            /// <inheritdoc/>
            public override string RestPath => "{calendarId}";

            /// <inheritdoc/>
            public override string Depth => "1";

            /// <inheritdoc/>
            protected override object GetBody()
            {
                if (_body == null)
                {
                    _body = new CalendarMultiget();
                }

                _body.Href.Clear();
                _body.Href.AddRange(EventIds);

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
        /// Creates an event.
        /// </summary>
        public class InsertRequest : CalendarBaseServiceRequest<VoidResponse>
        {
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
            /// Calendar identifier. To retrieve calendar IDs call the <see cref="CalendarListResource.List"/> method.
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
            protected override object GetBody() => Body.SerializeToString();

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
            /// Calendar identifier. To retrieve calendar IDs call the <see cref="CalendarListResource.List"/> method.
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
            protected override object GetBody() => Body.SerializeToString();

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
            /// Calendar identifier. To retrieve calendar IDs call the <see cref="CalendarListResource.List"/> method.
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