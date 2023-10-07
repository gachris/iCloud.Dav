using Ical.Net.Serialization;
using iCloud.Dav.Calendar.DataTypes;
using iCloud.Dav.Calendar.Extensions;
using iCloud.Dav.Calendar.Request;
using iCloud.Dav.Calendar.WebDav.DataTypes;
using iCloud.Dav.Core;
using iCloud.Dav.Core.Extensions;
using iCloud.Dav.Core.Response;
using System.Linq;

namespace iCloud.Dav.Calendar.Resources;

/// <summary>
/// Represents a resource on iCloud for accessing iCloud calendars for the authenticated user.
/// </summary>
public class CalendarListResource
{
    private readonly IClientService _service;

    /// <summary>
    /// Initializes a new instance of the <see cref="CalendarListResource"/> class.
    /// </summary>
    /// <param name="service">The client service used for making requests.</param>
    public CalendarListResource(IClientService service) => _service = service;

    /// <summary>
    /// Creates a new <see cref="GetSyncTokenRequest"/> instance for retrieving the next sync token for calendars resource on iCloud.
    /// </summary>
    /// <returns>A new <see cref="GetSyncTokenRequest"/> instance for retrieving the next sync token for calendars resource on iCloud.</returns>
    public virtual GetSyncTokenRequest GetSyncToken() => new GetSyncTokenRequest(_service);

    /// <summary>
    /// Creates a new <see cref="GetSyncTokenRequest"/> instance for retrieving the next sync token for specific calendar on iCloud.
    /// </summary>
    /// <param name="calendarId">The ID of the calendar. To retrieve calendar IDs, call the <see cref="List"/> method.</param>
    /// <returns>A new <see cref="GetSyncTokenRequest"/> instance for retrieving the next sync token for specific calendar on iCloud.</returns>
    public virtual GetSyncTokenRequest GetSyncToken(string calendarId) => new GetSyncTokenRequest(_service, calendarId);

    /// <summary>
    /// Creates a new <see cref="SyncCollectionRequest"/> instance for retrieving changes from the last sync token associated with the calendars resource.
    /// </summary>
    /// <returns>A new <see cref="SyncCollectionRequest"/> instance for retrieving changes from the last sync token associated with the calendars resource.</returns>
    public virtual SyncCollectionRequest SyncCollection() => new SyncCollectionRequest(_service);

    /// <summary>
    /// Creates a new <see cref="SyncCollectionRequest"/> instance for retrieving changes from the last sync token associated with the specified calendar.
    /// </summary>
    /// <param name="calendarId">The ID of the calendar. To retrieve calendar IDs, call the <see cref="List"/> method.</param>
    /// <returns>A new <see cref="SyncCollectionRequest"/> instance for retrieving changes from the last sync token associated with the specified calendar.</returns>
    public virtual SyncCollectionRequest SyncCollection(string calendarId) => new SyncCollectionRequest(_service, calendarId);

    /// <summary>
    /// Creates a new <see cref="ListRequest"/> instance for retrieving the list of calendars.
    /// </summary>
    /// <returns>A new <see cref="ListRequest"/> instance for retrieving the list of calendars.</returns>
    public virtual ListRequest List() => new ListRequest(_service);

    /// <summary>
    /// Creates a new <see cref="GetRequest"/> instance for retrieving a specific calendar by ID.
    /// </summary>
    /// <param name="calendarId">The ID of the calendar to retrieve. To retrieve calendar IDs, call the <see cref="List"/> method.</param>
    /// <returns>A new <see cref="GetRequest"/> instance for retrieving a specific calendar by ID.</returns>
    public virtual GetRequest Get(string calendarId) => new GetRequest(_service, calendarId);

    /// <summary>
    /// Creates a new <see cref="InsertRequest"/> instance that can insert a new calendar.
    /// </summary>
    /// <param name="body">The calendar to insert.</param>
    /// <returns>A new <see cref="InsertRequest"/> instance that can insert a new calendar.</returns>
    public virtual InsertRequest Insert(CalendarListEntry body) => new InsertRequest(_service, body);

    /// <summary>
    /// Creates a new <see cref="UpdateRequest"/> instance that can update an existing calendar.
    /// </summary>
    /// <param name="body">The body of the request containing the updated calendar information.</param>
    /// <returns>A new <see cref="UpdateRequest"/> instance that can update an existing calendar.</returns>
    public virtual UpdateRequest Update(CalendarListEntry body) => new UpdateRequest(_service, body);

    /// <summary>
    /// Creates a new <see cref="DeleteRequest"/> instance that can delete an existing calendar by ID.
    /// </summary>
    /// <param name="calendarId">The ID of the calendar to delete. To retrieve calendar IDs, call the <see cref="List"/> method.</param>
    /// <returns>A new <see cref="DeleteRequest"/> instance that can delete an existing calendar by ID.</returns>
    public virtual DeleteRequest Delete(string calendarId) => new DeleteRequest(_service, calendarId);

    /// <summary>
    /// Represents a request to get next sync token for specific calendar on iCloud.
    /// </summary>
    public class GetSyncTokenRequest : CalendarBaseServiceRequest<DataTypes.SyncToken>
    {
        private object _body;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetSyncTokenRequest"/> class.
        /// </summary>
        /// <param name="service">The client service used for making requests.</param>
        /// <param name="calendarId">The ID of the calendar. To retrieve calendar IDs, call the <see cref="List"/> method.</param>
        public GetSyncTokenRequest(IClientService service, string calendarId = null) : base(service)
        {
            CalendarId = calendarId;
        }

        /// <summary>
        /// Gets the calendar ID.
        /// </summary>
        [RequestParameter("calendarId", RequestParameterType.Path)]
        public virtual string CalendarId { get; }

        /// <inheritdoc/>
        public override string MethodName => "get";

        /// <inheritdoc/>
        public override string HttpMethod => Constants.Propfind;

        /// <inheritdoc/>
        public override string RestPath => string.IsNullOrEmpty(CalendarId) ? null : "{calendarId}";

        /// <inheritdoc/>
        public override string Depth => "0";

        /// <inheritdoc/>
        protected override object GetBody()
        {
            _body ??= new PropFind()
            {
                Prop = new Prop()
                {
                    GetETag = new GetETag(),
                    SyncToken = new WebDav.DataTypes.SyncToken()
                }
            };

            return _body;
        }

        /// <inheritdoc/>
        protected override void InitParameters()
        {
            base.InitParameters();

            RequestParameters.Add("calendarId", new Parameter("calendarId", "path", false));
        }
    }

    /// <summary>
    /// Represents a request to synchronize a specified calendar on iCloud.
    /// </summary>
    public class SyncCollectionRequest : CalendarBaseServiceRequest<SyncCollectionList>
    {
        private object _body;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListRequest"/> class.
        /// </summary>
        /// <param name="service">The client service used for making requests.</param>
        /// <param name="calendarId">The ID of the calendar. To retrieve calendar IDs, call the <see cref="List"/> method.</param>
        public SyncCollectionRequest(IClientService service, string calendarId = null) : base(service)
        {
            CalendarId = calendarId;
        }

        /// <summary>
        /// Gets the calendar ID.
        /// </summary>
        [RequestParameter("calendarId", RequestParameterType.Path)]
        public virtual string CalendarId { get; }

        /// <summary>
        /// Gets or sets the next sync token for retrieving only the entries that have changed since the last list request. 
        /// It is optional and by default, all entries will be returned.
        /// </summary>
        public virtual string SyncToken { get; set; }

        /// <inheritdoc/>
        public override string MethodName => "list";

        /// <inheritdoc/>
        public override string HttpMethod => Constants.Report;

        /// <inheritdoc/>
        public override string RestPath => string.IsNullOrEmpty(CalendarId) ? null : "{calendarId}";

        /// <inheritdoc/>
        public override string Depth => "0";

        /// <inheritdoc/>
        protected override object GetBody()
        {
            _body ??= new SyncCollection()
            {
                SyncToken = new WebDav.DataTypes.SyncToken() { Value = SyncToken },
                SyncLevel = new SyncLevel() { Value = "1" },
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

            return _body;
        }

        /// <inheritdoc/>
        protected override void InitParameters()
        {
            base.InitParameters();

            RequestParameters.Add("calendarId", new Parameter("calendarId", "path", false));
        }
    }

    /// <summary>
    /// Represents a request to retrieve a list of calendars from iCloud.
    /// </summary>
    public class ListRequest : CalendarBaseServiceRequest<CalendarList>
    {
        private object _body;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListRequest"/> class.
        /// </summary>
        /// <param name="service">The client service used for making requests.</param>
        public ListRequest(IClientService service) : base(service)
        {
        }

        /// <summary>
        /// Gets or sets the next sync token for retrieving only the entries that have changed since the last list request. 
        /// It is optional and by default, all entries will be returned.
        /// </summary>
        public virtual string SyncToken { get; set; }

        /// <inheritdoc/>
        public override string MethodName => "list";

        /// <inheritdoc/>
        public override string HttpMethod => string.IsNullOrEmpty(SyncToken) ? Constants.Propfind : Constants.Report;

        /// <inheritdoc/>
        public override string RestPath => string.Empty;

        /// <inheritdoc/>
        public override string Depth => "1";

        /// <inheritdoc/>
        protected override object GetBody()
        {
            var prop = new Prop()
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
            };

            if (string.IsNullOrEmpty(SyncToken))
            {
                _body = new PropFind() { Prop = prop };
            }
            else
            {
                _body = new SyncCollection()
                {
                    SyncToken = new WebDav.DataTypes.SyncToken() { Value = SyncToken },
                    SyncLevel = new SyncLevel() { Value = "1" },
                    Prop = prop
                };
            }

            return _body;
        }
    }

    /// <summary>
    /// Represents a request to get a single calendar by ID from iCloud.
    /// </summary>
    public class GetRequest : CalendarBaseServiceRequest<CalendarListEntry>
    {
        private object _body;

        /// <summary>
        /// Constructs a new <see cref="GetRequest"/> instance.
        /// </summary>
        /// <param name="service">The client service used for making requests.</param>
        /// <param name="calendarId">The ID of the calendar to retrieve. To retrieve calendar IDs, call the <see cref="List"/> method.</param>
        public GetRequest(IClientService service, string calendarId) : base(service)
        {
            CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(CalendarListEntry.Id));
        }

        /// <summary>
        /// Gets the calendar ID.
        /// </summary>
        [RequestParameter("calendarId", RequestParameterType.Path)]
        public virtual string CalendarId { get; }

        /// <inheritdoc/>
        public override string MethodName => "get";

        /// <inheritdoc/>
        public override string HttpMethod => Constants.Propfind;

        /// <inheritdoc/>
        public override string RestPath => "{calendarId}";

        /// <inheritdoc/>
        protected override object GetBody()
        {
            _body ??= new PropFind()
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
                }
            };
            return _body;
        }

        /// <inheritdoc/>
        public override string Depth => "1";

        /// <inheritdoc/>
        protected override void InitParameters()
        {
            base.InitParameters();

            RequestParameters.Add("calendarId", new Parameter("calendarId", "path", true));
        }
    }

    /// <summary>
    /// Represents a request to insert a calendar into iCloud.
    /// </summary>
    public class InsertRequest : CalendarBaseServiceRequest<VoidResponse>
    {
        private object _body;

        /// <summary>
        /// Constructs a new <see cref="InsertRequest"/> instance.
        /// </summary>
        /// <param name="service">The client service used for making requests.</param>
        /// <param name="body">The calendar to insert.</param>
        public InsertRequest(IClientService service, CalendarListEntry body) : base(service)
        {
            Body = body.ThrowIfNull(nameof(CalendarListEntry));
            CalendarId = Body.Id.ThrowIfNull(nameof(CalendarListEntry.Id));
        }

        /// <summary>
        /// Calendar identifier.
        /// </summary>
        [RequestParameter("calendarId", RequestParameterType.Path)]
        public virtual string CalendarId { get; }

        /// <summary>
        /// Gets the body of this request.
        /// </summary>
        private CalendarListEntry Body { get; }

        /// <inheritdoc/>
        public override string MethodName => "insert";

        /// <inheritdoc/>
        public override string HttpMethod => Constants.Mkcalendar;

        /// <inheritdoc/>
        public override string RestPath => "{calendarId}";

        /// <inheritdoc/>
        public override string Depth => "1";

        /// <inheritdoc/>
        protected override object GetBody()
        {
            if (_body is null)
            {
                var mkCalendar = new MkCalendar()
                {
                    Prop = new Prop()
                    {
                        DisplayName = new DisplayName() { Value = Body.Summary.ThrowIfNull(nameof(CalendarListEntry.Summary)) },
                        CalendarColor = new CalendarColor() { Value = Body.Color.FromRgb() },
                        CalendarOrder = new CalendarOrder() { Value = Body.Order.ToString() }
                    }
                };

                if (Body.TimeZone != null)
                {
                    var calendar = new Ical.Net.Calendar();
                    var calendarSerializer = new CalendarSerializer();
                    calendar.TimeZones.Add(Body.TimeZone);
                    mkCalendar.Prop.CalendarTimezone = new CalendarTimezone() { Value = calendarSerializer.SerializeToString(calendar) };
                }

                mkCalendar.Prop.SupportedCalendarComponentSet = new SupportedCalendarComponentSet()
                {
                    Comp = Body.SupportedCalendarComponents.Select(compName => new Comp() { Name = compName }).ToArray()
                };

                _body = mkCalendar;
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
    /// Represents a request to update an existing calendar in iCloud.
    /// </summary>
    public class UpdateRequest : CalendarBaseServiceRequest<VoidResponse>
    {
        private object _body;

        /// <summary>
        /// Constructs a new <see cref="UpdateRequest"/> instance.
        /// </summary>
        /// <param name="service">The client service used for making requests.</param>
        /// <param name="body">The body of the request containing the updated calendar information.</param>
        public UpdateRequest(IClientService service, CalendarListEntry body) : base(service)
        {
            Body = body.ThrowIfNull(nameof(CalendarListEntry));
            CalendarId = body.Id.ThrowIfNullOrEmpty(nameof(CalendarListEntry.Id));
        }

        /// <summary>
        /// Gets the calendar ID.
        /// </summary>
        [RequestParameter("calendarId", RequestParameterType.Path)]
        public virtual string CalendarId { get; }

        /// <summary>
        /// Gets the body of this request.
        /// </summary>
        private CalendarListEntry Body { get; }

        /// <inheritdoc/>
        public override string MethodName => "update";

        /// <inheritdoc/>
        public override string HttpMethod => Constants.Proppatch;

        /// <inheritdoc/>
        public override string RestPath => "{calendarId}";

        /// <inheritdoc/>
        protected override object GetBody()
        {
            _body ??= new PropertyUpdate()
            {
                Prop = new Prop()
                {
                    DisplayName = new DisplayName() { Value = Body.Summary.ThrowIfNull(nameof(CalendarListEntry.Summary)) },
                    CalendarColor = new CalendarColor() { Value = Body.Color.FromRgb() },
                    CalendarOrder = new CalendarOrder() { Value = Body.Order.ToString() }
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
    /// Represents a request to delete a calendar from iCloud.
    /// </summary>
    public class DeleteRequest : CalendarBaseServiceRequest<VoidResponse>
    {
        /// <summary>
        /// Constructs a new <see cref="DeleteRequest"/> instance.
        /// </summary>
        /// <param name="service">The client service used for making requests.</param>
        /// <param name="calendarId">The ID of the calendar to delete. To retrieve calendar IDs, call the <see cref="List"/> method.</param>
        public DeleteRequest(IClientService service, string calendarId) : base(service) => CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(CalendarListEntry.Id));

        /// <summary>
        /// Gets the calendar ID.
        /// </summary>
        [RequestParameter("calendarId", RequestParameterType.Path)]
        public virtual string CalendarId { get; }

        /// <inheritdoc/>
        public override string MethodName => "delete";

        /// <inheritdoc/>
        public override string HttpMethod => Constants.Delete;

        /// <inheritdoc/>
        public override string RestPath => "{calendarId}";

        /// <inheritdoc/>
        public override string Depth => "1";

        /// <inheritdoc/>
        protected override void InitParameters()
        {
            base.InitParameters();

            RequestParameters.Add("calendarId", new Parameter("calendarId", "path", true));
        }
    }
}