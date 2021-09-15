﻿using iCloud.Dav.Calendar.Request;
using iCloud.Dav.Calendar.Types;
using iCloud.Dav.Calendar.Utils;
using iCloud.Dav.Core.Attributes;
using iCloud.Dav.Core.Enums;
using iCloud.Dav.Core.Response;
using iCloud.Dav.Core.Services;
using iCloud.Dav.Core.Utils;
using System;

namespace iCloud.Dav.Calendar.Resources
{
    /// <summary>The "reminders" collection of methods.</summary>
    public class RemindersResource
    {
        private const string Resource = "reminders";

        /// <summary>The service which this resource belongs to.</summary>
        private readonly IClientService _service;

        /// <summary>Constructs a new resource.</summary>
        public RemindersResource(IClientService service)
        {
            this._service = service;
        }

        /// <summary>Returns reminders on the specified calendar.</summary>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the calendarList.list method.</param>
        public virtual ListRequest List(string calendarId)
        {
            return new ListRequest(this._service, calendarId);
        }

        /// <summary>Returns an reminder.</summary>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the calendarList.list method.</param>
        /// <param name="reminderId">Reminder identifier.</param>
        public virtual GetRequest Get(string calendarId, string reminderId)
        {
            return new GetRequest(this._service, calendarId, reminderId);
        }

        /// <summary>Creates an reminder.</summary>
        /// <param name="body">The body of the request.</param>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the calendarList.list method.</param>
        public virtual InsertRequest Insert(Reminder body, string calendarId)
        {
            return new InsertRequest(this._service, body, calendarId);
        }

        /// <summary>Updates an reminder.</summary>
        /// <param name="body">The body of the request.</param>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the calendarList.list method.</param>
        public virtual UpdateRequest Update(Reminder body, string calendarId)
        {
            return new UpdateRequest(this._service, body, calendarId);
        }

        /// <summary>Deletes an reminder.</summary>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the calendarList.list method.</param>
        /// <param name="reminderId">Reminder identifier.</param>
        public virtual DeleteRequest Delete(string calendarId, string reminderId)
        {
            return new DeleteRequest(this._service, calendarId, reminderId);
        }

        /// <summary>Returns reminders on the specified calendar.</summary>
        public class ListRequest : CalendarBaseServiceRequest<ReminderList>
        {
            /// <summary>Constructs a new List request.</summary>
            public ListRequest(IClientService service, string calendarId) : base(service)
            {
                this.CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(calendarId));
                this.InitParameters();
            }

            /// <summary>Calendar identifier. To retrieve calendar IDs call the calendarList.list method.</summary>
            [RequestParameter("calendarId", RequestParameterType.Path)]
            public virtual string CalendarId { get; }

            /// <summary>Upper bound (exclusive) for an reminder's start time to filter by. Optional. The default is not to
            /// filter by start time. Must be an RFC3339 timestamp with mandatory time zone offset, for example,
            /// 2011-06-03T10:00:00-07:00, 2011-06-03T10:00:00Z. Milliseconds may be provided but are ignored. If
            /// timeMin is set, timeMax must be greater than timeMin.</summary>
            public virtual DateTime? TimeMax { get; set; }

            /// <summary>Lower bound (exclusive) for an reminder's end time to filter by. Optional. The default is not to
            /// filter by end time. Must be an RFC3339 timestamp with mandatory time zone offset, for example,
            /// 2011-06-03T10:00:00-07:00, 2011-06-03T10:00:00Z. Milliseconds may be provided but are ignored. If
            /// timeMax is set, timeMin must be smaller than timeMax.</summary>
            public virtual DateTime? TimeMin { get; set; }

            /// <summary>Gets the method name.</summary>
            public override string MethodName => "list";

            /// <summary>Gets the HTTP method.</summary>
            public override string HttpMethod => ApiMethod.REPORT;

            /// <summary>Gets the REST path.</summary>
            public override string RestPath => "{calendarId}";

            ///<summary>Gets the depth.</summary>
            public override string Depth => "1";

            /// <summary>Returns the body of the request.</summary>
            protected override object GetBody()
            {
                Calendarquery calendarquery = new Calendarquery()
                {
                    Prop = new Prop() { Calendardata = Calendardata.Default, Getetag = Getetag.Default },
                    Filter = new Filter() { Compfilter = new Compfilter() { Name = "VCALENDAR", Child = new Compfilter() { Name = "VTODO" } } }
                };

                var timeMin = this.TimeMin.ToFilterTime();
                var timeMax = this.TimeMax.ToFilterTime();

                if (!String.IsNullOrEmpty(timeMin) || !String.IsNullOrEmpty(timeMax))
                    calendarquery.Filter.Compfilter.Child.Timerange = new Timerange { Start = timeMin, End = timeMax };

                return calendarquery;
            }

            /// <summary>Initializes List parameter list.</summary>
            protected override void InitParameters()
            {
                base.InitParameters();

                this.RequestParameters.Add("calendarId", new Parameter()
                {
                    Name = "calendarId",
                    IsRequired = true,
                    ParameterType = "path"
                });
            }
        }

        /// <summary>Returns an reminder.</summary>
        public class GetRequest : CalendarBaseServiceRequest<Reminder>
        {
            /// <summary>Constructs a new Get request.</summary>
            public GetRequest(IClientService service, string calendarId, string reminderId) : base(service)
            {
                this.CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(calendarId));
                this.ReminderId = reminderId.ThrowIfNullOrEmpty(nameof(reminderId));
                this.InitParameters();
            }

            /// <summary>Calendar identifier. To retrieve calendar IDs call the calendarList.list method.</summary>
            [RequestParameter("calendarId", RequestParameterType.Path)]
            public virtual string CalendarId { get; }

            /// <summary>Reminder identifier.</summary>
            [RequestParameter("reminderId", RequestParameterType.Path)]
            public virtual string ReminderId { get; }

            /// <summary>Gets the method name.</summary>
            public override string MethodName => "get";

            /// <summary>Gets the HTTP method.</summary>
            public override string HttpMethod => ApiMethod.GET;

            /// <summary>Gets the REST path.</summary>
            public override string RestPath => "{calendarId}/{reminderId}.ics";

            ///<summary>Gets the depth.</summary>
            public override string Depth => "1";

            /// <summary>Initializes Get parameter list.</summary>
            protected override void InitParameters()
            {
                base.InitParameters();

                this.RequestParameters.Add("calendarId", new Parameter()
                {
                    Name = "calendarId",
                    IsRequired = true,
                    ParameterType = "path"
                });
                this.RequestParameters.Add("reminderId", new Parameter()
                {
                    Name = "reminderId",
                    IsRequired = true,
                    ParameterType = "path"
                });
            }
        }

        /// <summary>Creates an reminder.</summary>
        public class InsertRequest : CalendarBaseServiceRequest<InsertResponseObject>
        {
            /// <summary>Constructs a new Insert request.</summary>
            public InsertRequest(IClientService service, Reminder body, string calendarId) : base(service)
            {
                this.CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(calendarId));
                this.Body = body.ThrowIfNull(nameof(body));
                if (String.IsNullOrEmpty(body.Uid))
                    body.Uid = Guid.NewGuid().ToString().ToUpper();
                this.ReminderId = body.Uid;
                this.InitParameters();
            }

            /// <summary>Reminder identifier.</summary>
            [RequestParameter("reminderId", RequestParameterType.Path)]
            public virtual string ReminderId { get; }

            /// <summary>Calendar identifier. To retrieve calendar IDs call the calendarList.list method.</summary>
            [RequestParameter("calendarId", RequestParameterType.Path)]
            public virtual string CalendarId { get; }

            /// <summary>Gets the body of this request.</summary>
            private Reminder Body { get; }

            /// <summary>Gets the method name.</summary>
            public override string MethodName => "insert";

            /// <summary>Gets the HTTP method.</summary>
            public override string HttpMethod => ApiMethod.PUT;

            /// <summary>Gets the REST path.</summary>
            public override string RestPath => "{calendarId}/{reminderId}.ics";

            /// <summary>Gets the Content-Type header of this request.</summary>
            public override string ContentType => ApiContentType.TEXT_CALENDAR;

            /// <summary>Returns the body of the request.</summary>
            protected override object GetBody()
            {
                Ical.Net.Calendar calendar = new Ical.Net.Calendar();
                Ical.Net.Serialization.iCalendar.Serializers.CalendarSerializer calendarSerializer = new Ical.Net.Serialization.iCalendar.Serializers.CalendarSerializer();
                calendar.Todos.Add(this.Body);
                return calendarSerializer.SerializeToString(calendar);
            }

            /// <summary>Initializes Insert parameter list.</summary>
            protected override void InitParameters()
            {
                base.InitParameters();

                this.RequestParameters.Add("calendarId", new Parameter()
                {
                    Name = "calendarId",
                    IsRequired = true,
                    ParameterType = "path",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("reminderId", new Parameter()
                {
                    Name = "reminderId",
                    IsRequired = true,
                    ParameterType = "path"
                });
            }
        }

        /// <summary>Updates an reminder.</summary>
        public class UpdateRequest : CalendarBaseServiceRequest<UpdateResponseObject>
        {
            /// <summary>Constructs a new Update request.</summary>
            public UpdateRequest(IClientService service, Reminder body, string calendarId) : base(service)
            {
                this.CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(calendarId));
                this.Body = body.ThrowIfNull(nameof(body));
                this.ReminderId = this.Body.Uid.ThrowIfNullOrEmpty(nameof(Reminder.Uid));
                this.InitParameters();
            }

            /// <summary>Calendar identifier. To retrieve calendar IDs call the calendarList.list method.</summary>
            [RequestParameter("calendarId", RequestParameterType.Path)]
            public virtual string CalendarId { get; }

            /// <summary>Reminder identifier.</summary>
            [RequestParameter("reminderId", RequestParameterType.Path)]
            public virtual string ReminderId { get; }

            /// <summary>Gets the body of this request.</summary>
            private Reminder Body { get; }

            /// <summary>Gets the method name.</summary>
            public override string MethodName => "update";

            /// <summary>Gets the HTTP method.</summary>
            public override string HttpMethod => ApiMethod.PUT;

            /// <summary>Gets the REST path.</summary>
            public override string RestPath => "{calendarId}/{reminderId}.ics";

            /// <summary>Gets the Content-Type header of this request.</summary>
            public override string ContentType => ApiContentType.TEXT_CALENDAR;

            /// <summary>Returns the body of the request.</summary>
            protected override object GetBody()
            {
                Ical.Net.Calendar calendar = new Ical.Net.Calendar();
                Ical.Net.Serialization.iCalendar.Serializers.CalendarSerializer calendarSerializer = new Ical.Net.Serialization.iCalendar.Serializers.CalendarSerializer();
                calendar.Todos.Add(this.Body);
                return calendarSerializer.SerializeToString(calendar);
            }

            /// <summary>Initializes Update parameter list.</summary>
            protected override void InitParameters()
            {
                base.InitParameters();

                this.RequestParameters.Add("calendarId", new Parameter()
                {
                    Name = "calendarId",
                    IsRequired = true,
                    ParameterType = "path",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("reminderId", new Parameter()
                {
                    Name = "reminderId",
                    IsRequired = true,
                    ParameterType = "path"
                });
            }
        }

        /// <summary>Deletes an reminder.</summary>
        public class DeleteRequest : CalendarBaseServiceRequest<DeleteResponseObject>
        {
            /// <summary>Constructs a new Delete request.</summary>
            public DeleteRequest(IClientService service, string calendarId, string reminderId) : base(service)
            {
                this.CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(calendarId));
                this.ReminderId = reminderId.ThrowIfNullOrEmpty(nameof(reminderId));
                this.InitParameters();
            }

            /// <summary>Calendar identifier. To retrieve calendar IDs call the calendarList.list method.</summary>
            [RequestParameter("calendarId", RequestParameterType.Path)]
            public virtual string CalendarId { get; }

            /// <summary>Reminder identifier.</summary>
            [RequestParameter("reminderId", RequestParameterType.Path)]
            public virtual string ReminderId { get; }

            /// <summary>Gets the method name.</summary>
            public override string MethodName => "delete";

            /// <summary>Gets the HTTP method.</summary>
            public override string HttpMethod => ApiMethod.DELETE;

            /// <summary>Gets the REST path.</summary>
            public override string RestPath => "{calendarId}/{reminderId}.ics";

            ///<summary>Gets the depth.</summary>
            public override string Depth => "1";

            /// <summary>Initializes Delete parameter list.</summary>
            protected override void InitParameters()
            {
                base.InitParameters();

                this.RequestParameters.Add("calendarId", new Parameter()
                {
                    Name = "calendarId",
                    IsRequired = true,
                    ParameterType = "path"
                });
                this.RequestParameters.Add("reminderId", new Parameter()
                {
                    Name = "reminderId",
                    IsRequired = true,
                    ParameterType = "path"
                });
            }
        }
    }
}
