using System;
using System.Collections.Generic;
using System.Linq;
using iCloud.Dav.Calendar.DataTypes;
using iCloud.Dav.Calendar.Extensions;
using iCloud.Dav.Calendar.Request;
using iCloud.Dav.Calendar.WebDav.DataTypes;
using iCloud.Dav.Core;
using iCloud.Dav.Core.Extensions;
using iCloud.Dav.Core.Response;

namespace iCloud.Dav.Calendar.Resources;

/// <summary>
/// Represents a resource on iCloud for accessing iCloud events for the authenticated user.
/// </summary>
public class EventsResource
{
    private readonly IClientService _service;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventsResource"/> class.
    /// </summary>
    /// <param name="service">The client service used for making requests.</param>
    public EventsResource(IClientService service) => _service = service;

    /// <summary>
    /// Creates a new <see cref="ListRequest"/> instance for retrieving the list of events.
    /// </summary>
    /// <param name="calendarId">The ID of the calendar to retrieve the list from. To retrieve calendar IDs, call the <see cref="CalendarListResource.List"/> method.</param>
    /// <returns>A new <see cref="ListRequest"/> instance for retrieving the list of events.</returns>
    public virtual ListRequest List(string calendarId) => new ListRequest(_service, calendarId);

    /// <summary>
    /// Creates a new <see cref="GetRequest"/> instance for retrieving a specific event by ID.
    /// </summary>
    /// <param name="calendarId">The ID of the calendar where the event is located. To retrieve calendar IDs, call the <see cref="CalendarListResource.List"/> method.</param>
    /// <param name="eventId">The ID of the event to retrieve. To retrieve event IDs, call the <see cref="List"/> method.</param>
    /// <returns>A new <see cref="GetRequest"/> instance for retrieving a specific event by ID.</returns>
    public virtual GetRequest Get(string calendarId, string eventId) => new GetRequest(_service, calendarId, eventId);

    /// <summary>
    /// Creates a new <see cref="MultiGetRequest"/> instance for retrieving multiple events by ID.
    /// </summary>
    /// <param name="calendarId">The ID of the calendar where the events are located. To retrieve calendar IDs, call the <see cref="CalendarListResource.List"/> method.</param>
    /// <param name="eventIds">The IDs of the events to retrieve. To retrieve event IDs, call the <see cref="List"/> method.</param>
    /// <returns>A new <see cref="MultiGetRequest"/> instance for retrieving multiple events by ID.</returns>
    public virtual MultiGetRequest MultiGet(string calendarId, params string[] eventIds) => new MultiGetRequest(_service, calendarId, eventIds);

    /// <summary>
    /// Creates a new <see cref="InsertRequest"/> instance that can insert a new event.
    /// </summary>            
    /// <param name="body">The event to insert.</param>
    /// <param name="calendarId">The ID of the calendar where the event will be stored. To retrieve calendar IDs, call the <see cref="CalendarListResource.List"/> method.</param>
    /// <returns>A new <see cref="InsertRequest"/> instance that can insert a new event.</returns>
    public virtual InsertRequest Insert(Event body, string calendarId) => new InsertRequest(_service, body, calendarId);

    /// <summary>
    /// Creates a new <see cref="UpdateRequest"/> instance that can update an existing event.
    /// </summary>
    /// <param name="body">The body of the request containing the updated event information.</param>
    /// <param name="calendarId">The ID of the calendar where the event is located. To retrieve calendar IDs, call the <see cref="CalendarListResource.List"/> method.</param>
    /// <returns>A new <see cref="UpdateRequest"/> instance that can update an existing event.</returns>
    public virtual UpdateRequest Update(Event body, string calendarId) => new UpdateRequest(_service, body, calendarId);

    /// <summary>
    /// Creates a new <see cref="DeleteRequest"/> instance that can delete an existing event by ID.
    /// </summary>
    /// <param name="calendarId">The ID of the calendar where the event is located. To retrieve calendar IDs, call the <see cref="CalendarListResource.List"/> method.</param>
    /// <param name="eventId">The ID of the event to delete. To retrieve event IDs, call the <see cref="List"/> method.</param>
    /// <returns>A new <see cref="DeleteRequest"/> instance that can delete an existing event by ID.</returns>
    public virtual DeleteRequest Delete(string calendarId, string eventId) => new DeleteRequest(_service, calendarId, eventId);

    /// <summary>
    /// Represents a request to retrieve a list of events from iCloud.
    /// </summary>
    public class ListRequest : CalendarBaseServiceRequest<Events>
    {
        private object _body;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListRequest"/> class.
        /// </summary>
        /// <param name="service">The client service used for making requests.</param>
        /// <param name="calendarId">The ID of the calendar to retrieve the list from. To retrieve calendar IDs, call the <see cref="CalendarListResource.List"/> method.</param>
        public ListRequest(IClientService service, string calendarId) : base(service)
        {
            CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(CalendarListEntry.Id));
        }

        /// <summary>
        /// Gets the calendar ID.
        /// </summary>
        [RequestParameter("calendarId", RequestParameterType.Path)]
        public virtual string CalendarId { get; }

        /// <summary>
        /// Gets or sets the maximum time of the events to retrieve.
        /// </summary>
        public virtual DateTime? TimeMax { get; set; }

        /// <summary>
        /// Gets or sets the minimum time of the events to retrieve.
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
            var timeMin = TimeMin.ToFilterTime();
            var timeMax = TimeMax.ToFilterTime();

            _body ??= new CalendarQuery()
            {
                Filter = new Filter(),
                Prop = new Prop()
                {
                    CalendarColor = new CalendarColor(),
                    CalendarData = new CalendarData(),
                    CalendarDescription = new CalendarDescription(),
                    CalendarHomeSet = new CalendarHomeSet(),
                    CalendarOrder = new CalendarOrder(),
                    CalendarTimezone = new CalendarTimezone(),
                    CurrentUserPrincipal = new CurrentUserPrincipal(),
                    CurrentUserPrivilegeSet = new CurrentUserPrivilegeSet(),
                    DisplayName = new DisplayName(),
                    GetCTag = new GetCTag(),
                    GetETag = new GetETag(),
                    ResourceType = new ResourceType(),
                    SupportedReportSet = new SupportedReportSet(),
                    SupportedCalendarComponentSet = new SupportedCalendarComponentSet(),
                    SupportedCalendarComponentSets = new SupportedCalendarComponentSets(),
                    SyncToken = new WebDav.DataTypes.SyncToken()
                }
            };

            ((CalendarQuery)_body).Filter.Root = new CompFilter()
            {
                Name = "VCALENDAR",
                Children = new List<IFilter>()
                {
                    new CompFilter()
                    {
                        Name = "VEVENT",
                        Children = (!string.IsNullOrEmpty(timeMin) || !string.IsNullOrEmpty(timeMax))
                        ? new List<IFilter>()
                        {
                            new TimeRange()
                            {
                                Start = timeMin,
                                End = timeMax
                            }
                        } : null
                    }
                }
            };

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
    /// Represents a request to get a single event by ID from iCloud.
    /// </summary>
    public class GetRequest : CalendarBaseServiceRequest<Event>
    {
        /// <summary>
        /// Constructs a new <see cref="GetRequest"/> instance.
        /// </summary>
        /// <param name="service">The client service used for making requests.</param>
        /// <param name="calendarId">The ID of the calendar where the event is located. To retrieve calendar IDs, call the <see cref="CalendarListResource.List"/> method.</param>
        /// <param name="eventId">The ID of the event to retrieve. To retrieve event IDs, call the <see cref="List"/> method.</param>
        public GetRequest(IClientService service, string calendarId, string eventId) : base(service)
        {
            CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(CalendarListEntry.Id));
            EventId = eventId.ThrowIfNullOrEmpty(nameof(Event.Id));
        }

        /// <summary>
        /// Gets the calendar ID.
        /// </summary>
        [RequestParameter("calendarId", RequestParameterType.Path)]
        public virtual string CalendarId { get; }

        /// <summary>
        /// Gets the event ID.
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
    /// Represents a request to retrieve multiple iCloud events by ID.
    /// </summary>
    public class MultiGetRequest : CalendarBaseServiceRequest<Events>
    {
        private object _body;

        /// <summary>
        /// Constructs a new <see cref="MultiGetRequest"/> instance.
        /// </summary>
        /// <param name="service">The client service used for making requests.</param>
        /// <param name="calendarId">The ID of the calendar where the events are located. To retrieve calendar IDs, call the <see cref="CalendarListResource.List"/> method.</param>
        /// <param name="eventIds">The IDs of the events to retrieve. To retrieve event IDs, call the <see cref="List"/> method.</param>
        public MultiGetRequest(IClientService service, string calendarId, params string[] eventIds) : base(service)
        {
            CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(CalendarListEntry.Id));
            EventIds = eventIds.ThrowIfNull(nameof(eventIds));
        }

        /// <summary>
        /// Gets the calendar ID.
        /// </summary>
        [RequestParameter("calendarId", RequestParameterType.Path)]
        public virtual string CalendarId { get; }

        /// <summary>
        /// Gets the event IDs.
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
            return _body ??= new CalendarMultiget()
            {
                Prop = new Prop()
                {
                    CalendarColor = new CalendarColor(),
                    CalendarData = new CalendarData(),
                    CalendarDescription = new CalendarDescription(),
                    CalendarHomeSet = new CalendarHomeSet(),
                    CalendarOrder = new CalendarOrder(),
                    CalendarTimezone = new CalendarTimezone(),
                    CurrentUserPrincipal = new CurrentUserPrincipal(),
                    CurrentUserPrivilegeSet = new CurrentUserPrivilegeSet(),
                    DisplayName = new DisplayName(),
                    GetCTag = new GetCTag(),
                    GetETag = new GetETag(),
                    ResourceType = new ResourceType(),
                    SupportedReportSet = new SupportedReportSet(),
                    SupportedCalendarComponentSet = new SupportedCalendarComponentSet(),
                    SupportedCalendarComponentSets = new SupportedCalendarComponentSets(),
                    SyncToken = new WebDav.DataTypes.SyncToken()
                },
                Hrefs = EventIds.Select(eventId => new Href() { Value = Service.HttpClientInitializer.GetCalendarFullHref(CalendarId, eventId) }).ToArray()
            };
        }

        /// <inheritdoc/>
        protected override void InitParameters()
        {
            base.InitParameters();

            RequestParameters.Add("calendarId", new Parameter("calendarId", "path", true));
        }
    }

    /// <summary>
    /// Represents a request to insert a event into iCloud.
    /// </summary>
    public class InsertRequest : CalendarBaseServiceRequest<VoidResponse>
    {
        private object _body;

        /// <summary>
        /// Constructs a new <see cref="InsertRequest"/> instance.
        /// </summary>
        /// <param name="service">The client service used for making requests.</param>
        /// <param name="body">The event to insert.</param>
        /// <param name="calendarId">The ID of the calendar where the event will be stored. To retrieve calendar IDs, call the <see cref="CalendarListResource.List"/> method.</param>
        public InsertRequest(IClientService service, Event body, string calendarId) : base(service)
        {
            Body = body.ThrowIfNull(nameof(Event));
            EventId = body.Id.ThrowIfNull(nameof(Event.Id));
            CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(CalendarListEntry.Id));
        }

        /// <summary>
        /// Event identifier.
        /// </summary>
        [RequestParameter("eventId", RequestParameterType.Path)]
        public virtual string EventId { get; }

        /// <summary>
        /// Gets the event ID.
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
        protected override object GetBody() => _body ??= Body.SerializeToString();

        /// <inheritdoc/>
        protected override void InitParameters()
        {
            base.InitParameters();

            RequestParameters.Add("calendarId", new Parameter("calendarId", "path", true));
            RequestParameters.Add("eventId", new Parameter("eventId", "path", true));
        }
    }

    /// <summary>
    /// Represents a request to update an existing event in iCloud.
    /// </summary>
    public class UpdateRequest : CalendarBaseServiceRequest<VoidResponse>
    {
        private object _body;

        /// <summary>
        /// Constructs a new <see cref="UpdateRequest"/> instance.
        /// </summary>
        /// <param name="service">The client service used for making requests.</param>  
        /// <param name="body">The body of the request containing the updated event information.</param>
        /// <param name="calendarId">The ID of the calendar where the event is located. To retrieve calendar IDs, call the <see cref="CalendarListResource.List"/> method.</param>
        public UpdateRequest(IClientService service, Event body, string calendarId) : base(service)
        {
            Body = body.ThrowIfNull(nameof(Event));
            EventId = Body.Id.ThrowIfNullOrEmpty(nameof(Event.Id));
            CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(CalendarListEntry.Id));
        }

        /// <summary>
        /// Gets the calendar ID.
        /// </summary>
        [RequestParameter("calendarId", RequestParameterType.Path)]
        public virtual string CalendarId { get; }

        /// <summary>
        /// Gets the event ID.
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
        protected override object GetBody() => _body ??= Body.SerializeToString();

        /// <inheritdoc/>
        protected override void InitParameters()
        {
            base.InitParameters();

            RequestParameters.Add("calendarId", new Parameter("calendarId", "path", true));
            RequestParameters.Add("eventId", new Parameter("eventId", "path", true));
        }
    }

    /// <summary>
    /// Represents a request to delete a event from iCloud.
    /// </summary>
    public class DeleteRequest : CalendarBaseServiceRequest<VoidResponse>
    {
        /// <summary>
        /// Constructs a new <see cref="DeleteRequest"/> instance.
        /// </summary>
        /// <param name="service">The client service used for making requests.</param>
        /// <param name="calendarId">The ID of the calendar where the event is located. To retrieve calendar IDs, call the <see cref="CalendarListResource.List"/> method.</param>
        /// <param name="eventId">The ID of the event to delete. To retrieve event IDs, call the <see cref="List"/> method.</param>
        public DeleteRequest(IClientService service, string calendarId, string eventId) : base(service)
        {
            CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(CalendarListEntry.Id));
            EventId = eventId.ThrowIfNullOrEmpty(nameof(Event.Id));
        }

        /// <summary>
        /// Gets the calendar ID.
        /// </summary>
        [RequestParameter("calendarId", RequestParameterType.Path)]
        public virtual string CalendarId { get; }

        /// <summary>
        /// Gets the event ID.
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