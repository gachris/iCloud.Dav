using Ical.Net.Serialization;
using iCloud.Dav.Calendar.CalDav.Types;
using iCloud.Dav.Calendar.DataTypes;
using iCloud.Dav.Calendar.Request;
using iCloud.Dav.Core;
using iCloud.Dav.Core.Response;
using iCloud.Dav.Core.Utils;

namespace iCloud.Dav.Calendar.Resources
{
    /// <summary>
    /// The calendar list collection of methods.
    /// </summary>
    public class CalendarListResource
    {
        /// <summary>
        /// The service which this resource belongs to.
        /// </summary>
        private readonly IClientService _service;

        /// <summary>
        /// Constructs a new resource.
        /// </summary>
        public CalendarListResource(IClientService service) => _service = service;

        /// <summary>
        /// Returns the next sync token the specified calendar.
        /// </summary>
        public virtual GetSyncTokenRequest GetSyncToken(string calendarId) => new GetSyncTokenRequest(_service, calendarId);

        /// <summary>
        /// Returns changes on the specified calendar.
        /// </summary>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the <see cref="CalendarListResource.List"/> method.</param>
        public virtual SyncCollectionRequest SyncCollection(string calendarId) => new SyncCollectionRequest(_service, calendarId);

        /// <summary>
        /// Returns the calendars on the user's calendar list.
        /// </summary>
        public virtual ListRequest List() => new ListRequest(_service);

        /// <summary>
        /// Returns a calendar from the user's calendar list.
        /// </summary>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the <see cref="List"/> method.</param>
        public virtual GetRequest Get(string calendarId) => new GetRequest(_service, calendarId);

        /// <summary>
        /// Inserts a calendar into the user's calendar list.
        /// </summary>
        /// <param name="body">The body of the request.</param>
        public virtual InsertRequest Insert(CalendarListEntry body) => new InsertRequest(_service, body);

        /// <summary>
        /// Updates an existing calendar on the user's calendar list.
        /// </summary>
        /// <param name="body">The body of the request.</param>
        public virtual UpdateRequest Update(CalendarListEntry body) => new UpdateRequest(_service, body);

        /// <summary>
        /// Removes a calendar from the user's calendar list.
        /// </summary>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the <see cref="List"/> method.</param>
        public virtual DeleteRequest Delete(string calendarId) => new DeleteRequest(_service, calendarId);

        /// <summary>
        /// Returns the next sync token on the user's identity card list.
        /// </summary>
        public class GetSyncTokenRequest : CalendarBaseServiceRequest<SyncToken>
        {
            private PropFind _body;

            /// <summary>
            /// Constructs a new Sync Collection request.
            /// </summary>
            public GetSyncTokenRequest(IClientService service, string calendarId) : base(service)
            {
                CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(calendarId));
            }

            /// <summary>
            /// Resource Name. To retrieve resource names call the <see cref="List"/> method.
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
            public override string Depth => "0";

            /// <inheritdoc/>
            protected override object GetBody()
            {
                if (_body == null)
                {
                    _body = new PropFind();
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
        /// Returns changes on the specified calendar.
        /// </summary>
        public class SyncCollectionRequest : CalendarBaseServiceRequest<SyncCollectionList>
        {
            private SyncCollection _body;

            /// <summary>
            /// Constructs a new Sync Collection request.
            /// </summary>
            public SyncCollectionRequest(IClientService service, string calendarId) : base(service)
            {
                CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(CalendarListEntry.Id));
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
            /// list request will always be in the result. Optional. The default is to return all entries.
            /// </summary>
            public virtual string SyncToken { get; set; }

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
                _body.SyncLevel = "1";

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
        /// Returns the calendars on the user's calendar list.
        /// </summary>
        public class ListRequest : CalendarBaseServiceRequest<CalendarList>
        {
            private SyncCollection _body;

            /// <summary>
            /// Constructs a new List request.
            /// </summary>
            public ListRequest(IClientService service) : base(service)
            {
            }

            /// <summary>
            /// Token obtained from the nextSyncToken field returned on the last page of results
            /// from the previous list request. It makes the result of this list request contain
            /// only entries that have changed since then. All entries deleted since the previous
            /// list request will always be in the result. Optional. The default is to return all entries.
            /// </summary>
            public virtual string SyncToken { get; set; }

            /// <inheritdoc/>
            public override string MethodName => "list";

            /// <inheritdoc/>
            public override string HttpMethod => Constants.Report;

            /// <inheritdoc/>
            public override string RestPath => string.Empty;

            /// <inheritdoc/>
            public override string Depth => "1";

            /// <inheritdoc/>
            protected override object GetBody()
            {
                if (_body == null)
                {
                    _body = new SyncCollection();
                }

                _body.SyncToken = SyncToken;
                _body.SyncLevel = "1";

                return _body;
            }
        }

        /// <summary>
        /// Returns a calendar from the user's calendar list.
        /// </summary>
        public class GetRequest : CalendarBaseServiceRequest<CalendarListEntry>
        {
            private object _body;

            /// <summary>
            /// Constructs a new Get request.
            /// </summary>
            public GetRequest(IClientService service, string calendarId) : base(service) => CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(CalendarListEntry.Id));

            /// <summary>
            /// Calendar identifier. To retrieve calendar IDs call the <see cref="List"/> method.
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
                if (_body == null)
                {
                    _body = new PropFind();
                }
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
        /// Inserts a calendar into the user's calendar list.
        /// </summary>
        public class InsertRequest : CalendarBaseServiceRequest<VoidResponse>
        {
            private object _body;

            /// <summary>
            /// Constructs a new Insert request.
            /// </summary>
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
                    var mkCalendar = new MkCalendar(Body.Summary.ThrowIfNull(nameof(CalendarListEntry.Summary)))
                    {
                        CalendarColor = Body.Color
                    };

                    if (Body.TimeZone != null)
                    {
                        var calendar = new Ical.Net.Calendar();
                        var calendarSerializer = new CalendarSerializer();
                        calendar.TimeZones.Add(Body.TimeZone);
                        mkCalendar.CalendarTimeZone = calendarSerializer.SerializeToString(calendar);
                    }

                    mkCalendar.SupportedCalendarComponents.AddRange(Body.SupportedCalendarComponents);

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
        /// Updates an existing calendar on the user's calendar list.
        /// </summary>
        public class UpdateRequest : CalendarBaseServiceRequest<VoidResponse>
        {
            private object _body;

            /// <summary>
            /// Constructs a new Update request.
            /// </summary>
            public UpdateRequest(IClientService service, CalendarListEntry body) : base(service)
            {
                Body = body.ThrowIfNull(nameof(CalendarListEntry));
                CalendarId = body.Id.ThrowIfNullOrEmpty(nameof(CalendarListEntry.Id));
            }

            /// <summary>
            /// Calendar identifier. To retrieve calendar IDs call the <see cref="List"/> method.
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
                if (_body == null)
                {
                    _body = new PropertyUpdate(Body.Summary.ThrowIfNull(nameof(CalendarListEntry.Summary)), Body.Color, Body.Order.ToString());
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
        /// Removes a calendar from the user's calendar list.
        /// </summary>
        public class DeleteRequest : CalendarBaseServiceRequest<VoidResponse>
        {
            /// <summary>
            /// Constructs a new Delete request.
            /// </summary>
            public DeleteRequest(IClientService service, string calendarId) : base(service) => CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(CalendarListEntry.Id));

            /// <summary>
            /// Calendar identifier. To retrieve calendar IDs call the <see cref="List"/> method.
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
}