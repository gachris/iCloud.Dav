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
    /// The calendars collection of methods.
    /// </summary>
    public class CalendarsResource
    {
        /// <summary>
        /// The service which this resource belongs to.
        /// </summary>
        private readonly IClientService _service;

        /// <summary>
        /// Constructs a new resource.
        /// </summary>
        public CalendarsResource(IClientService service) => _service = service;

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
        public virtual InsertRequest Insert(DataTypes.Calendar body) => new InsertRequest(_service, body);

        /// <summary>
        /// Updates an existing calendar on the user's calendar list.
        /// </summary>
        /// <param name="body">The body of the request.</param>
        public virtual UpdateRequest Update(DataTypes.Calendar body) => new UpdateRequest(_service, body);

        /// <summary>
        /// Removes a calendar from the user's calendar list.
        /// </summary>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the <see cref="List"/> method.</param>
        public virtual DeleteRequest Delete(string calendarId) => new DeleteRequest(_service, calendarId);

        /// <summary>
        /// Returns the calendars on the user's calendar list.
        /// </summary>
        public class ListRequest : CalendarBaseServiceRequest<CalendarList>
        {
            private object _body;

            /// <summary>
            /// Constructs a new List request.
            /// </summary>
            public ListRequest(IClientService service) : base(service)
            {
            }

            /// <inheritdoc/>
            public override string MethodName => "list";

            /// <inheritdoc/>
            public override string HttpMethod => Constants.Propfind;

            /// <inheritdoc/>
            public override string RestPath => string.Empty;

            /// <inheritdoc/>
            public override string Depth => "1";

            /// <inheritdoc/>
            protected override object GetBody()
            {
                if (_body == null)
                {
                    _body = new PropFind();
                }
                return _body;
            }
        }

        /// <summary>
        /// Returns a calendar from the user's calendar list.
        /// </summary>
        public class GetRequest : CalendarBaseServiceRequest<DataTypes.Calendar>
        {
            private object _body;

            /// <summary>
            /// Constructs a new Get request.
            /// </summary>
            public GetRequest(IClientService service, string calendarId) : base(service) => CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(calendarId));

            /// <summary>
            /// Calendar identifier. To retrieve calendar IDs call the <see cref="List"/> method.
            /// </summary>
            [RequestParameter("calendarId", RequestParameterType.Path)]
            public virtual string CalendarId { get; }

            /// <inheritdoc/>
            public override string MethodName => Constants.Propfind;

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
            public InsertRequest(IClientService service, DataTypes.Calendar body) : base(service)
            {
                Body = body.ThrowIfNull(nameof(body));
                CalendarId = Body.Uid.ThrowIfNull(nameof(Body.Uid));
            }

            /// <summary>
            /// Calendar identifier.
            /// </summary>
            [RequestParameter("calendarId", RequestParameterType.Path)]
            public virtual string CalendarId { get; }

            /// <summary>
            /// Gets the body of this request.
            /// </summary>
            private DataTypes.Calendar Body { get; }

            /// <inheritdoc/>
            public override string MethodName => Constants.Mkcalendar;

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
                    var mkCalendar = new MkCalendar(Body.Summary.ThrowIfNull(nameof(Body.Summary)))
                    {
                        CalendarColor = Body.Color
                    };

                    if (Body.TimeZone != null)
                    {
                        var calendar = new Ical.Net.Calendar();
                        var calendarSerializer = new CalendarSerializer();
                        calendar.TimeZones.Add(Body.TimeZone);
                        mkCalendar.CalendarTimeZoneSerializedString = calendarSerializer.SerializeToString(calendar);
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
            public UpdateRequest(IClientService service, DataTypes.Calendar body) : base(service)
            {
                Body = body.ThrowIfNull(nameof(body));
                CalendarId = body.Uid.ThrowIfNullOrEmpty(nameof(DataTypes.Calendar.Uid));
            }

            /// <summary>
            /// Calendar identifier. To retrieve calendar IDs call the <see cref="List"/> method.
            /// </summary>
            [RequestParameter("calendarId", RequestParameterType.Path)]
            public virtual string CalendarId { get; }

            /// <summary>
            /// Gets the body of this request.
            /// </summary>
            private DataTypes.Calendar Body { get; }

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
                    _body = new PropertyUpdate(Body.Summary.ThrowIfNull(nameof(Body.Summary)), Body.Color);
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
            public DeleteRequest(IClientService service, string calendarId) : base(service) => CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(calendarId));

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