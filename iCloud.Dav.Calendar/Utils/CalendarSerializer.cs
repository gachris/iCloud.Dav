using Ical.Net.Interfaces;
using Ical.Net.Interfaces.Serialization;
using Ical.Net.Serialization.iCalendar;
using Ical.Net.Utility;
using System.IO;

namespace iCloud.Dav.Calendar.Utils
{
    public class CalendarSerializer : Ical.Net.Serialization.iCalendar.Serializers.CalendarSerializer
    {
        private readonly ICalendar _calendar;

        public CalendarSerializer() : this(new SerializationContext())
        {
        }

        public CalendarSerializer(ICalendar cal)
        {
            this._calendar = cal;
        }

        public CalendarSerializer(ISerializationContext ctx) : base(ctx)
        {
        }

        public override string SerializeToString()
        {
            return this.SerializeToString(_calendar);
        }

        public override string SerializeToString(object obj)
        {
            ICalendar calendar = obj as ICalendar;
            if (string.IsNullOrWhiteSpace(calendar.Version))
                calendar.Version = "2.0";
            if (string.IsNullOrWhiteSpace(calendar.ProductId))
                calendar.ProductId = "-//github.com/rianjs/ical.net//NONSGML ical.net 2.2//EN";
            return base.SerializeToString(calendar);
        }

        public override object Deserialize(TextReader tr)
        {
            if (tr == null)
                return null;
            using (tr)
            {
                ISerializationContext serializationContext = this.SerializationContext;
                using (TextReader r = TextUtil.Normalize(tr.ReadToEnd(), serializationContext))
                {
                    var iCalLexer = new iCalLexer(r);
                    var iCalParser = new ICalParser(iCalLexer);
                    return iCalParser.Icalendar(this.SerializationContext);
                }
            }
        }
    }
}
