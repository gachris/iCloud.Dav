using iCloud.Dav.Calendar.DataTypes;
using iCloud.Dav.Calendar.Extensions;
using iCloud.Dav.Calendar.Request;
using iCloud.Dav.Calendar.WebDav.DataTypes;
using iCloud.Dav.Core;
using iCloud.Dav.Core.Extensions;
using iCloud.Dav.Core.Response;
using System;
using System.Collections.Generic;
using System.Linq;

namespace iCloud.Dav.Calendar.Resources;

/// <summary>
/// Represents a resource on iCloud for accessing iCloud reminders for the authenticated user.
/// </summary>
public class RemindersResource
{
    private readonly IClientService _service;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemindersResource"/> class.
    /// </summary>
    /// <param name="service">The client service used for making requests.</param>
    public RemindersResource(IClientService service) => _service = service;

    /// <summary>
    /// Creates a new <see cref="ListRequest"/> instance for retrieving the list of reminders.
    /// </summary>
    /// <param name="calendarId">The ID of the calendar to retrieve the list from. To retrieve calendar IDs, call the <see cref="CalendarListResource.List"/> method.</param>
    /// <returns>A new <see cref="ListRequest"/> instance for retrieving the list of reminders.</returns>
    public virtual ListRequest List(string calendarId) => new ListRequest(_service, calendarId);

    /// <summary>
    /// Creates a new <see cref="GetRequest"/> instance for retrieving a specific reminder by ID.
    /// </summary>
    /// <param name="calendarId">The ID of the calendar where the reminder is located. To retrieve calendar IDs, call the <see cref="CalendarListResource.List"/> method.</param>
    /// <param name="reminderId">The ID of the reminder to retrieve. To retrieve reminder IDs, call the <see cref="List"/> method.</param>
    /// <returns>A new <see cref="GetRequest"/> instance for retrieving a specific reminder by ID.</returns>
    public virtual GetRequest Get(string calendarId, string reminderId) => new GetRequest(_service, calendarId, reminderId);

    /// <summary>
    /// Creates a new <see cref="MultiGetRequest"/> instance for retrieving multiple reminders by ID.
    /// </summary>
    /// <param name="calendarId">The ID of the calendar where the reminders are located. To retrieve calendar IDs, call the <see cref="CalendarListResource.List"/> method.</param>
    /// <param name="reminderIds">The IDs of the reminders to retrieve. To retrieve reminder IDs, call the <see cref="List"/> method.</param>
    /// <returns>A new <see cref="MultiGetRequest"/> instance for retrieving multiple reminders by ID.</returns>
    public virtual MultiGetRequest MultiGet(string calendarId, params string[] reminderIds) => new MultiGetRequest(_service, calendarId, reminderIds);

    /// <summary>
    /// Creates a new <see cref="InsertRequest"/> instance that can insert a new reminder.
    /// </summary>            
    /// <param name="body">The reminder to insert.</param>
    /// <param name="calendarId">The ID of the calendar where the reminder will be stored. To retrieve calendar IDs, call the <see cref="CalendarListResource.List"/> method.</param>
    /// <returns>A new <see cref="InsertRequest"/> instance that can insert a new reminder.</returns>
    public virtual InsertRequest Insert(Reminder body, string calendarId) => new InsertRequest(_service, body, calendarId);

    /// <summary>
    /// Creates a new <see cref="UpdateRequest"/> instance that can update an existing reminder.
    /// </summary>
    /// <param name="body">The body of the request containing the updated reminder information.</param>
    /// <param name="calendarId">The ID of the calendar where the reminder is located. To retrieve calendar IDs, call the <see cref="CalendarListResource.List"/> method.</param>
    /// <returns>A new <see cref="UpdateRequest"/> instance that can update an existing reminder.</returns>
    public virtual UpdateRequest Update(Reminder body, string calendarId) => new UpdateRequest(_service, body, calendarId);

    /// <summary>
    /// Creates a new <see cref="DeleteRequest"/> instance that can delete an existing reminder by ID.
    /// </summary>
    /// <param name="calendarId">The ID of the calendar where the reminder is located. To retrieve calendar IDs, call the <see cref="CalendarListResource.List"/> method.</param>
    /// <param name="reminderId">The ID of the reminder to delete. To retrieve reminder IDs, call the <see cref="List"/> method.</param>
    /// <returns>A new <see cref="DeleteRequest"/> instance that can delete an existing reminder by ID.</returns>
    public virtual DeleteRequest Delete(string calendarId, string reminderId) => new DeleteRequest(_service, calendarId, reminderId);

    /// <summary>
    /// Represents a request to retrieve a list of reminders from iCloud.
    /// </summary>
    public class ListRequest : CalendarBaseServiceRequest<Reminders>
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
        /// Gets or sets the maximum time of the reminders to retrieve.
        /// </summary>
        public virtual DateTime? TimeMax { get; set; }

        /// <summary>
        /// Gets or sets the minimum time of the reminders to retrieve.
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
                        Name = "VTODO",
                        Children = (!string.IsNullOrEmpty(timeMin) || !string.IsNullOrEmpty(timeMax)) ? new List<IFilter>()
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
    /// Represents a request to get a single reminder by ID from iCloud.
    /// </summary>
    public class GetRequest : CalendarBaseServiceRequest<Reminder>
    {
        /// <summary>
        /// Constructs a new <see cref="GetRequest"/> instance.
        /// </summary>
        /// <param name="service">The client service used for making requests.</param>
        /// <param name="calendarId">The ID of the calendar where the reminder is located. To retrieve calendar IDs, call the <see cref="CalendarListResource.List"/> method.</param>
        /// <param name="reminderId">The ID of the reminder to retrieve. To retrieve reminder IDs, call the <see cref="List"/> method.</param>
        public GetRequest(IClientService service, string calendarId, string reminderId) : base(service)
        {
            CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(CalendarListEntry.Id));
            ReminderId = reminderId.ThrowIfNullOrEmpty(nameof(Reminder.Id));
        }

        /// <summary>
        /// Gets the calendar ID.
        /// </summary>
        [RequestParameter("calendarId", RequestParameterType.Path)]
        public virtual string CalendarId { get; }

        /// <summary>
        /// Gets the reminder ID.
        /// </summary>
        [RequestParameter("reminderId", RequestParameterType.Path)]
        public virtual string ReminderId { get; }

        /// <inheritdoc/>
        public override string MethodName => "get";

        /// <inheritdoc/>
        public override string HttpMethod => Constants.Get;

        /// <inheritdoc/>
        public override string RestPath => "{calendarId}/{reminderId}.ics";

        /// <inheritdoc/>
        public override string Depth => "1";

        /// <inheritdoc/>
        protected override void InitParameters()
        {
            base.InitParameters();

            RequestParameters.Add("calendarId", new Parameter("calendarId", "path", true));
            RequestParameters.Add("reminderId", new Parameter("reminderId", "path", true));
        }
    }

    /// <summary>
    /// Represents a request to retrieve multiple iCloud reminders by ID.
    /// </summary>
    public class MultiGetRequest : CalendarBaseServiceRequest<Reminders>
    {
        private CalendarMultiget _body;

        /// <summary>
        /// Constructs a new <see cref="MultiGetRequest"/> instance.
        /// </summary>
        /// <param name="service">The client service used for making requests.</param>
        /// <param name="calendarId">The ID of the calendar where the reminders are located. To retrieve calendar IDs, call the <see cref="CalendarListResource.List"/> method.</param>
        /// <param name="reminderIds">The IDs of the reminders to retrieve. To retrieve reminder IDs, call the <see cref="List"/> method.</param>
        public MultiGetRequest(IClientService service, string calendarId, params string[] reminderIds) : base(service)
        {
            CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(CalendarListEntry.Id));
            ReminderIds = reminderIds;
        }

        /// <summary>
        /// Gets the calendar ID.
        /// </summary>
        [RequestParameter("calendarId", RequestParameterType.Path)]
        public virtual string CalendarId { get; }

        /// <summary>
        /// Gets the reminder IDs.
        /// </summary>
        public virtual string[] ReminderIds { get; }

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
                Hrefs = ReminderIds.Select(reminderId => new Href() { Value = Service.HttpClientInitializer.GetCalendarFullHref(CalendarId, reminderId) }).ToArray()
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
    /// Represents a request to insert a reminder into iCloud.
    /// </summary>
    public class InsertRequest : CalendarBaseServiceRequest<VoidResponse>
    {
        private object _body;

        /// <summary>
        /// Constructs a new <see cref="InsertRequest"/> instance.
        /// </summary>
        /// <param name="service">The client service used for making requests.</param>
        /// <param name="body">The reminder to insert.</param>
        /// <param name="calendarId">The ID of the calendar where the reminder will be stored. To retrieve calendar IDs, call the <see cref="CalendarListResource.List"/> method.</param>
        public InsertRequest(IClientService service, Reminder body, string calendarId) : base(service)
        {
            Body = body.ThrowIfNull(nameof(Reminder));
            ReminderId = body.Id.ThrowIfNull(nameof(Reminder.Id));
            CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(CalendarListEntry.Id));
        }

        /// <summary>
        /// Gets the calendar ID.
        /// </summary>
        [RequestParameter("calendarId", RequestParameterType.Path)]
        public virtual string CalendarId { get; }

        /// <summary>
        /// Gets the reminder ID.
        /// </summary>
        [RequestParameter("reminderId", RequestParameterType.Path)]
        public virtual string ReminderId { get; }

        /// <summary>
        /// Gets the body of this request.
        /// </summary>
        private Reminder Body { get; }

        /// <inheritdoc/>
        public override string MethodName => "insert";

        /// <inheritdoc/>
        public override string HttpMethod => Constants.Put;

        /// <inheritdoc/>
        public override string RestPath => "{calendarId}/{reminderId}.ics";

        /// <inheritdoc/>
        public override string ContentType => Constants.TEXT_CALENDAR;

        /// <inheritdoc/>
        protected override object GetBody() => _body ??= Body.SerializeToString();

        /// <inheritdoc/>
        protected override void InitParameters()
        {
            base.InitParameters();

            RequestParameters.Add("calendarId", new Parameter("calendarId", "path", true));
            RequestParameters.Add("reminderId", new Parameter("reminderId", "path", true));
        }
    }

    /// <summary>
    /// Represents a request to update an existing reminder in iCloud.
    /// </summary>
    public class UpdateRequest : CalendarBaseServiceRequest<VoidResponse>
    {
        private object _body;

        /// <summary>
        /// Constructs a new <see cref="UpdateRequest"/> instance.
        /// </summary>
        /// <param name="service">The client service used for making requests.</param>  
        /// <param name="body">The body of the request containing the updated reminder information.</param>
        /// <param name="calendarId">The ID of the calendar where the reminder is located. To retrieve calendar IDs, call the <see cref="CalendarListResource.List"/> method.</param>
        public UpdateRequest(IClientService service, Reminder body, string calendarId) : base(service)
        {
            Body = body.ThrowIfNull(nameof(Reminder));
            ReminderId = Body.Id.ThrowIfNullOrEmpty(nameof(Reminder.Id));
            CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(CalendarListEntry.Id));
        }

        /// <summary>
        /// Gets the calendar ID.
        /// </summary>
        [RequestParameter("calendarId", RequestParameterType.Path)]
        public virtual string CalendarId { get; }

        /// <summary>
        /// Gets the reminder ID.
        /// </summary>
        [RequestParameter("reminderId", RequestParameterType.Path)]
        public virtual string ReminderId { get; }

        /// <summary>
        /// Gets the body of this request.
        /// </summary>
        private Reminder Body { get; }

        /// <inheritdoc/>
        public override string MethodName => "update";

        /// <inheritdoc/>
        public override string HttpMethod => Constants.Put;

        /// <inheritdoc/>
        public override string RestPath => "{calendarId}/{reminderId}.ics";

        /// <inheritdoc/>
        public override string ContentType => Constants.TEXT_CALENDAR;

        /// <inheritdoc/>
        protected override object GetBody() => _body ??= Body.SerializeToString();

        /// <inheritdoc/>
        protected override void InitParameters()
        {
            base.InitParameters();

            RequestParameters.Add("calendarId", new Parameter("calendarId", "path", true));
            RequestParameters.Add("reminderId", new Parameter("reminderId", "path", true));
        }
    }

    /// <summary>
    /// Represents a request to delete a reminder from iCloud.
    /// </summary>
    public class DeleteRequest : CalendarBaseServiceRequest<VoidResponse>
    {
        /// <summary>
        /// Constructs a new <see cref="DeleteRequest"/> instance.
        /// </summary>
        /// <param name="service">The client service used for making requests.</param>
        /// <param name="calendarId">The ID of the calendar where the reminder is located. To retrieve calendar IDs, call the <see cref="CalendarListResource.List"/> method.</param>
        /// <param name="reminderId">The ID of the reminder to delete. To retrieve reminder IDs, call the <see cref="List"/> method.</param>
        public DeleteRequest(IClientService service, string calendarId, string reminderId) : base(service)
        {
            CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(CalendarListEntry.Id));
            ReminderId = reminderId.ThrowIfNullOrEmpty(nameof(Reminder.Id));
        }

        /// <summary>
        /// Gets the calendar ID.
        /// </summary>
        [RequestParameter("calendarId", RequestParameterType.Path)]
        public virtual string CalendarId { get; }

        /// <summary>
        /// Gets the reminder ID.
        /// </summary>
        [RequestParameter("reminderId", RequestParameterType.Path)]
        public virtual string ReminderId { get; }

        /// <inheritdoc/>
        public override string MethodName => "delete";

        /// <inheritdoc/>
        public override string HttpMethod => Constants.Delete;

        /// <inheritdoc/>
        public override string RestPath => "{calendarId}/{reminderId}.ics";

        /// <inheritdoc/>
        public override string Depth => "1";

        /// <inheritdoc/>
        protected override void InitParameters()
        {
            base.InitParameters();

            RequestParameters.Add("calendarId", new Parameter("calendarId", "path", true));
            RequestParameters.Add("reminderId", new Parameter("reminderId", "path", true));
        }
    }
}