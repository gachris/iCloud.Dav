using Ical.Net.Serialization;
using iCloud.Dav.ICalendar.Request;
using iCloud.Dav.ICalendar.Types;
using iCloud.Dav.ICalendar.Utils;
using iCloud.Dav.Core.Attributes;
using iCloud.Dav.Core.Enums;
using iCloud.Dav.Core.Response;
using iCloud.Dav.Core.Services;
using iCloud.Dav.Core.Utils;
using System;

namespace iCloud.Dav.ICalendar.Resources
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
            _service = service;
        }

        /// <summary>Returns the calendars on the user's calendar list.</summary>
        public virtual ListRequest List()
        {
            return new ListRequest(_service);
        }

        /// <summary>Returns a calendar from the user's calendar list.</summary>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the calendars.list method.</param>
        public virtual GetRequest Get(string calendarId)
        {
            return new GetRequest(_service, calendarId);
        }

        /// <summary>Inserts an existing calendar into the user's calendar list.</summary>
        /// <param name="body">The body of the request.</param>
        public virtual InsertRequest Insert(CalendarEntry body)
        {
            return new InsertRequest(_service, body);
        }

        /// <summary>Updates an existing calendar on the user's calendar list.</summary>
        /// <param name="body">The body of the request.</param>
        public virtual UpdateRequest Update(CalendarEntry body)
        {
            return new UpdateRequest(_service, body);
        }

        /// <summary>Removes a calendar from the user's calendar list.</summary>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the calendars.list method.</param>
        public virtual DeleteRequest Delete(string calendarId)
        {
            return new DeleteRequest(_service, calendarId);
        }

        /// <summary>Returns the calendars on the user's calendar list.</summary>
        public class ListRequest : CalendarBaseServiceRequest<CalendarEntryList>
        {
            /// <summary>Constructs a new List request.</summary>
            public ListRequest(IClientService service) : base(service)
            {
                InitParameters();
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
                CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(calendarId));
                InitParameters();
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

                RequestParameters.Add("calendarId", new Parameter
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
                Body = body.ThrowIfNull(nameof(body));
                if (string.IsNullOrEmpty(Body.Id))
                    Body.Id = Guid.NewGuid().ToString().ToUpper();
                CalendarId = Body.Id;
                InitParameters();
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
                var mkCalendar = new MkCalendar();
                var set = new Set();
                var prop = new Prop
                {
                    Displayname = new Displayname() { Value = Body.Summary },
                    CalendarColor = new CalendarColor() { Value = Body.Color },
                    SupportedCalendarComponentSet = new SupportedCalendarComponentSet
                    {
                        Comps = new System.Collections.Generic.List<Comp>()
                    }
                };

                if (Body.TimeZone != null)
                {
                    var calendar = new Ical.Net.Calendar();
                    var calendarSerializer = new CalendarSerializer();
                    calendar.TimeZones.Add(Body.TimeZone);
                    prop.CalendarTimeZone = new CalendarTimeZone
                    {
                        Value = calendarSerializer.SerializeToString(calendar)
                    };
                }

                Body.SupportedCalendarComponents.ForEach(calendarComponent =>
                {
                    var comp = new Comp() { Name = calendarComponent };
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

                RequestParameters.Add("calendarId", new Parameter
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
                Body = body.ThrowIfNull(nameof(body));
                CalendarId = body.Id.ThrowIfNullOrEmpty(nameof(CalendarEntry.Id));
                InitParameters();
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

                RequestParameters.Add("calendarId", new Parameter
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
                CalendarId = calendarId.ThrowIfNullOrEmpty(nameof(calendarId));
                InitParameters();
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

                RequestParameters.Add("calendarId", new Parameter
                {
                    Name = "calendarId",
                    IsRequired = true,
                    ParameterType = "path",
                });
            }
        }
    }
}
