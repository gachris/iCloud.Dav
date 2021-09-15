using iCloud.Dav.Calendar.Request;
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
    /// <summary>The "calendars" collection of methods.</summary>
    public class CalendarsResource
    {
        private const string Resource = "calendars";

        /// <summary>The service which this resource belongs to.</summary>
        private readonly IClientService _service;

        /// <summary>Constructs a new resource.</summary>
        public CalendarsResource(IClientService service)
        {
            this._service = service;
        }

        /// <summary>Returns the calendars on the user's calendar list.</summary>
        public virtual ListRequest List()
        {
            return new ListRequest(this._service);
        }

        /// <summary>Returns a calendar from the user's calendar list.</summary>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the calendars.list method.</param>
        public virtual GetRequest Get(string calendarId)
        {
            return new GetRequest(this._service, calendarId);
        }

        /// <summary>Inserts an existing calendar into the user's calendar list.</summary>
        /// <param name="body">The body of the request.</param>
        public virtual InsertRequest Insert(CalendarEntry body)
        {
            return new InsertRequest(this._service, body);
        }

        /// <summary>Updates an existing calendar on the user's calendar list.</summary>
        /// <param name="body">The body of the request.</param>
        public virtual UpdateRequest Update(CalendarEntry body)
        {
            return new UpdateRequest(this._service, body);
        }

        /// <summary>Removes a calendar from the user's calendar list.</summary>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the calendars.list method.</param>
        public virtual DeleteRequest Delete(string calendarId)
        {
            return new DeleteRequest(this._service, calendarId);
        }

        /// <summary>Returns the calendars on the user's calendar list.</summary>
        public class ListRequest : CalendarBaseServiceRequest<CalendarEntryList>
        {
            /// <summary>Constructs a new List request.</summary>
            public ListRequest(IClientService service) : base(service)
            {
                this.InitParameters();
            }

            ///<summary>Gets the method name.</summary>
            public override string MethodName => "list";

            ///<summary>Gets the HTTP method.</summary>
            public override string HttpMethod => ApiMethod.PROPFIND;

            ///<summary>Gets the REST path.</summary>
            public override string RestPath { get; }

            ///<summary>Gets the depth.</summary>
            public override string Depth => "1";

            ///<summary>Returns the body of the request.</summary>
            protected override object GetBody()
            {
                var calendar = new Propfind<Prop>()
                {
                    Prop = new Prop
                    {
                        Displayname = Displayname.Default,
                        Getctag = Getctag.Default,
                        Getetag = Getetag.Default,
                        Supportedreportset = Supportedreportset.Default,
                        SupportedCalendarComponentSet = SupportedCalendarComponentSet.Default,
                        Currentuserprivilegeset = Currentuserprivilegeset.Default,
                        CalendarColor = CalendarColor.Default,
                        Value = PropValue.Calendardata
                    }
                };
                return calendar;
            }
        }

        /// <summary>Returns a calendar from the user's calendar list.</summary>
        public class GetRequest : CalendarBaseServiceRequest<CalendarEntry>
        {
            /// <summary>Constructs a new Get request.</summary>
            public GetRequest(IClientService service, string calendarId) : base(service)
            {
                this.CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(calendarId));
                this.InitParameters();
            }

            /// <summary>Calendar identifier. To retrieve calendar IDs call the calendars.list method.</summary>
            [RequestParameter("calendarId", RequestParameterType.Path)]
            public virtual string CalendarId { get; }

            ///<summary>Gets the method name.</summary>
            public override string MethodName => ApiMethod.PROPFIND;

            ///<summary>Gets the HTTP method.</summary>
            public override string HttpMethod => ApiMethod.PROPFIND;

            ///<summary>Gets the REST path.</summary>
            public override string RestPath => "{calendarId}";

            ///<summary>Returns the body of the request.</summary>
            protected override object GetBody()
            {
                return new Propfind<Prop>()
                {
                    Prop = new Prop
                    {
                        Displayname = Displayname.Default,
                        Getctag = Getctag.Default,
                        Getetag = Getetag.Default,
                        Supportedreportset = Supportedreportset.Default,
                        SupportedCalendarComponentSet = SupportedCalendarComponentSet.Default,
                        Currentuserprivilegeset = Currentuserprivilegeset.Default,
                        CalendarColor = CalendarColor.Default,
                        Value = PropValue.Calendardata
                    }
                };
            }

            ///<summary>Gets the depth.</summary>
            public override string Depth => "1";

            /// <summary>Initializes Get parameter list.</summary>
            protected override void InitParameters()
            {
                base.InitParameters();

                this.RequestParameters.Add("calendarId", new Parameter
                {
                    Name = "calendarId",
                    IsRequired = true,
                    ParameterType = "path",
                });
            }
        }

        /// <summary>Inserts an existing calendar into the user's calendar list.</summary>
        public class InsertRequest : CalendarBaseServiceRequest<InsertResponseObject>
        {
            /// <summary>Constructs a new Insert request.</summary>
            public InsertRequest(IClientService service, CalendarEntry body) : base(service)
            {
                this.Body = body.ThrowIfNull(nameof(body));
                if (string.IsNullOrEmpty(this.Body.Id))
                    this.Body.Id = Guid.NewGuid().ToString().ToUpper();
                this.CalendarId = this.Body.Id;
                this.InitParameters();
            }

            /// <summary>Calendar identifier. To retrieve calendar IDs call the calendars.list method.</summary>
            [RequestParameter("calendarId", RequestParameterType.Path)]
            public virtual string CalendarId { get; }

            /// <summary>Gets the body of this request.</summary>
            private CalendarEntry Body { get; }

            ///<summary>Gets the method name.</summary>
            public override string MethodName => ApiMethod.MKCALENDAR;

            ///<summary>Gets the HTTP method.</summary>
            public override string HttpMethod => ApiMethod.MKCALENDAR;

            ///<summary>Gets the REST path.</summary>
            public override string RestPath => "{calendarId}";

            ///<summary>Gets the depth.</summary>
            public override string Depth => "1";

            ///<summary>Returns the body of the request.</summary>
            protected override object GetBody()
            {
                MkCalendar mkCalendar = new MkCalendar();
                Set set = new Set();
                Prop prop = new Prop();
                prop.Displayname = new Displayname() { Value = this.Body.Summary };
                prop.CalendarColor = new CalendarColor() { Value = this.Body.Color };
                prop.SupportedCalendarComponentSet = new SupportedCalendarComponentSet();
                prop.SupportedCalendarComponentSet.Comps = new System.Collections.Generic.List<Comp>();

                if (this.Body.TimeZone != null)
                {
                    Ical.Net.Calendar calendar = new Ical.Net.Calendar();
                    CalendarSerializer calendarSerializer = new CalendarSerializer();
                    calendar.TimeZones.Add(this.Body.TimeZone);
                    prop.CalendarTimeZone = new CalendarTimeZone();
                    prop.CalendarTimeZone.Value = calendarSerializer.SerializeToString(calendar);
                }

                this.Body.SupportedCalendarComponents.ForEach(calendarComponent =>
                {
                    Comp comp = new Comp() { Name = calendarComponent };
                    prop.SupportedCalendarComponentSet.Comps.Add(comp);
                });
                set.Prop = prop;
                mkCalendar.Set = set;
                return mkCalendar;
            }

            /// <summary>Initializes Insert parameter list.</summary>
            protected override void InitParameters()
            {
                base.InitParameters();

                this.RequestParameters.Add("calendarId", new Parameter
                {
                    Name = "calendarId",
                    IsRequired = true,
                    ParameterType = "path",
                });
            }
        }

        /// <summary>Updates an existing calendar on the user's calendar list.</summary>
        public class UpdateRequest : CalendarBaseServiceRequest<UpdateResponseObject>
        {
            /// <summary>Constructs a new Update request.</summary>
            public UpdateRequest(IClientService service, CalendarEntry body) : base(service)
            {
                this.Body = body.ThrowIfNull(nameof(body));
                this.CalendarId = body.Id.ThrowIfNullOrEmpty(nameof(CalendarEntry.Id));
                this.InitParameters();
            }

            /// <summary>Calendar identifier. To retrieve calendar IDs call the calendars.list method.</summary>
            [RequestParameter("calendarId", RequestParameterType.Path)]
            public virtual string CalendarId { get; }

            /// <summary>Gets the body of this request.</summary>
            private CalendarEntry Body { get; }

            ///<summary>Gets the method name.</summary>
            public override string MethodName => "update";

            ///<summary>Gets the HTTP method.</summary>
            public override string HttpMethod => ApiMethod.PROPPATCH;

            ///<summary>Gets the REST path.</summary>
            public override string RestPath => "{calendarId}";

            ///<summary>Returns the body of the request.</summary>
            protected override object GetBody()
            {
                return new PropertyUpdate()
                {
                    Set = new Set()
                    {
                        Prop = new Prop()
                        {
                            Displayname = new Displayname() { Value = Body.Summary },
                            CalendarColor = new CalendarColor() { Value = Body.Color }
                        }
                    }
                };
            }

            /// <summary>Initializes Update parameter list.</summary>
            protected override void InitParameters()
            {
                base.InitParameters();

                this.RequestParameters.Add("calendarId", new Parameter
                {
                    Name = "calendarId",
                    IsRequired = true,
                    ParameterType = "path",
                });
            }
        }

        /// <summary>Removes a calendar from the user's calendar list.</summary>
        public class DeleteRequest : CalendarBaseServiceRequest<DeleteResponseObject>
        {
            /// <summary>Constructs a new Delete request.</summary>
            public DeleteRequest(IClientService service, string calendarId) : base(service)
            {
                this.CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(calendarId));
                this.InitParameters();
            }

            /// <summary>Calendar identifier. To retrieve calendar IDs call the calendars.list method.</summary>
            [RequestParameter("calendarId", RequestParameterType.Path)]
            public virtual string CalendarId { get; }

            ///<summary>Gets the method name.</summary>
            public override string MethodName => "delete";

            ///<summary>Gets the HTTP method.</summary>
            public override string HttpMethod => ApiMethod.DELETE;

            ///<summary>Gets the REST path.</summary>
            public override string RestPath => "{calendarId}";

            ///<summary>Gets the depth.</summary>
            public override string Depth => "1";

            /// <summary>Initializes Delete parameter list.</summary>
            protected override void InitParameters()
            {
                base.InitParameters();

                this.RequestParameters.Add("calendarId", new Parameter
                {
                    Name = "calendarId",
                    IsRequired = true,
                    ParameterType = "path",
                });
            }
        }
    }
}
