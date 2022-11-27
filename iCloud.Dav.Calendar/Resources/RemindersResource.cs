﻿using iCloud.Dav.Calendar.CalDav.Types;
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
    /// The reminders collection of methods.
    /// </summary>
    public class RemindersResource
    {
        /// <summary>
        /// The service which this resource belongs to.
        /// </summary>
        private readonly IClientService _service;

        /// <summary>
        /// Constructs a new resource.
        /// </summary>
        public RemindersResource(IClientService service) => _service = service;

        /// <summary>
        /// Returns changes on the specified calendar.
        /// </summary>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the <see cref="CalendarListResource.List"/> method.</param>
        public virtual SyncCollectionRequest SyncCollection(string calendarId) => new SyncCollectionRequest(_service, calendarId);

        /// <summary>
        /// Returns reminders on the specified calendar.
        /// </summary>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the <see cref="CalendarListResource.List"/> method.</param>
        public virtual ListRequest List(string calendarId) => new ListRequest(_service, calendarId);

        /// <summary>
        /// Returns a reminder.
        /// </summary>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the <see cref="CalendarListResource.List"/> method.</param>
        /// <param name="reminderId">Reminder identifier. To retrieve reminder IDs call the <see cref="List"/> method.</param>
        public virtual GetRequest Get(string calendarId, string reminderId) => new GetRequest(_service, calendarId, reminderId);

        /// <summary>
        /// Returns events on the specified calendar.
        /// </summary>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the <see cref="CalendarListResource.List"/> method.</param>
        /// <param name="reminderIds">Reminder identifierss. To retrieve reminder IDs call the <see cref="List"/> method.</param>
        public virtual MultiGetRequest MultiGet(string calendarId, params string[] reminderIds) => new MultiGetRequest(_service, calendarId, reminderIds);

        /// <summary>
        /// Creates a reminder.
        /// </summary>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the <see cref="CalendarListResource.List"/> method.</param>
        /// <param name="body">The body of the request.</param>
        public virtual InsertRequest Insert(Reminder body, string calendarId) => new InsertRequest(_service, body, calendarId);

        /// <summary>
        /// Updates a reminder.
        /// </summary>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the <see cref="CalendarListResource.List"/> method.</param>
        /// <param name="body">The body of the request.</param>
        public virtual UpdateRequest Update(Reminder body, string calendarId) => new UpdateRequest(_service, body, calendarId);

        /// <summary>
        /// Deletes a reminder.
        /// </summary>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the <see cref="CalendarListResource.List"/> method.</param>
        /// <param name="reminderId">Reminder identifier. To retrieve reminder IDs call the <see cref="List"/> method.</param>
        public virtual DeleteRequest Delete(string calendarId, string reminderId) => new DeleteRequest(_service, calendarId, reminderId);

        /// <summary>
        /// Returns changes on the specified calendar.
        /// </summary>
        public class SyncCollectionRequest : CalendarBaseServiceRequest<Reminders>
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
        /// Returns reminders on the specified calendar.
        /// </summary>
        public class ListRequest : CalendarBaseServiceRequest<Reminders>
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
            /// Upper bound (exclusive) for an reminder's start time to filter by. Optional. The default is not to
            /// filter by start time. Milliseconds may be provided but are ignored. If timeMin is set, timeMax must be greater than timeMin.
            /// </summary>
            public virtual DateTime? TimeMax { get; set; }

            /// <summary>
            /// Lower bound (exclusive) for an reminder's end time to filter by. Optional. The default is not to
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
                    _body = new CalendarQuery() { CompFilter = new CompFilter("VCALENDAR") };
                    _body.CompFilter.Child = new CompFilter("VTODO");
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
        /// Returns a reminder.
        /// </summary>
        public class GetRequest : CalendarBaseServiceRequest<Reminder>
        {
            /// <summary>
            /// Constructs a new Get request.
            /// </summary>
            public GetRequest(IClientService service, string calendarId, string reminderId) : base(service)
            {
                CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(calendarId));
                ReminderId = reminderId.ThrowIfNullOrEmpty(nameof(reminderId));
            }

            /// <summary>
            /// Calendar identifier. To retrieve calendar IDs call the <see cref="CalendarListResource.List"/> method.
            /// </summary>
            [RequestParameter("calendarId", RequestParameterType.Path)]
            public virtual string CalendarId { get; }

            /// <summary>
            /// Reminder identifier. To retrieve reminder IDs call the <see cref="List"/> method.
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
        /// Returns reminders on the specified calendar.
        /// </summary>
        public class MultiGetRequest : CalendarBaseServiceRequest<Reminders>
        {
            private CalendarMultiget _body;

            /// <summary>
            /// Constructs a new Multi Get request.
            /// </summary>
            public MultiGetRequest(IClientService service, string calendarId, params string[] reminderIds) : base(service)
            {
                CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(calendarId));
                ReminderIds = reminderIds;
            }

            /// <summary>
            /// Calendar identifier. To retrieve calendar IDs call the <see cref="CalendarListResource.List"/> method.
            /// </summary>
            [RequestParameter("calendarId", RequestParameterType.Path)]
            public virtual string CalendarId { get; }

            /// <summary>
            /// Reminder identifiers. To retrieve reminder IDs call the <see cref="List"/> method.
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
                if (_body == null)
                {
                    _body = new CalendarMultiget();
                }

                _body.Href.Clear();
                _body.Href.AddRange(ReminderIds);

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
        /// Creates a reminder.
        /// </summary>
        public class InsertRequest : CalendarBaseServiceRequest<VoidResponse>
        {
            /// <summary>
            /// Constructs a new Insert request.
            /// </summary>
            public InsertRequest(IClientService service, Reminder body, string calendarId) : base(service)
            {
                Body = body.ThrowIfNull(nameof(body));
                ReminderId = body.Uid.ThrowIfNull(nameof(body));
                CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(calendarId));
            }

            /// <summary>
            /// Reminder identifier.
            /// </summary>
            [RequestParameter("reminderId", RequestParameterType.Path)]
            public virtual string ReminderId { get; }

            /// <summary>
            /// Calendar identifier. To retrieve calendar IDs call the <see cref="CalendarListResource.List"/> method.
            /// </summary>
            [RequestParameter("calendarId", RequestParameterType.Path)]
            public virtual string CalendarId { get; }

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
            protected override object GetBody() => Body.SerializeToString();

            /// <inheritdoc/>
            protected override void InitParameters()
            {
                base.InitParameters();

                RequestParameters.Add("calendarId", new Parameter("calendarId", "path", true));
                RequestParameters.Add("reminderId", new Parameter("reminderId", "path", true));
            }
        }

        /// <summary>
        /// Updates a reminder.
        /// </summary>
        public class UpdateRequest : CalendarBaseServiceRequest<VoidResponse>
        {
            /// <summary>
            /// Constructs a new Update request.
            /// </summary>
            public UpdateRequest(IClientService service, Reminder body, string calendarId) : base(service)
            {
                Body = body.ThrowIfNull(nameof(body));
                ReminderId = Body.Uid.ThrowIfNullOrEmpty(nameof(Reminder.Uid));
                CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(calendarId));
            }

            /// <summary>
            /// Calendar identifier. To retrieve calendar IDs call the <see cref="CalendarListResource.List"/> method.
            /// </summary>
            [RequestParameter("calendarId", RequestParameterType.Path)]
            public virtual string CalendarId { get; }

            /// <summary>
            /// Reminder identifier. To retrieve reminder IDs call the <see cref="List"/> method.
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
            protected override object GetBody() => Body.SerializeToString();

            /// <inheritdoc/>
            protected override void InitParameters()
            {
                base.InitParameters();

                RequestParameters.Add("calendarId", new Parameter("calendarId", "path", true));
                RequestParameters.Add("reminderId", new Parameter("reminderId", "path", true));
            }
        }

        /// <summary>
        /// Deletes a reminder.
        /// </summary>
        public class DeleteRequest : CalendarBaseServiceRequest<VoidResponse>
        {
            /// <summary>
            /// Constructs a new Delete request.
            /// </summary>
            public DeleteRequest(IClientService service, string calendarId, string reminderId) : base(service)
            {
                CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(calendarId));
                ReminderId = reminderId.ThrowIfNullOrEmpty(nameof(reminderId));
            }

            /// <summary>
            /// Calendar identifier. To retrieve calendar IDs call the <see cref="CalendarListResource.List"/> method.
            /// </summary>
            [RequestParameter("calendarId", RequestParameterType.Path)]
            public virtual string CalendarId { get; }

            /// <summary>
            /// Reminder identifier. To retrieve reminder IDs call the <see cref="List"/> method.
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
}