using Ical.Net.Interfaces;
using Ical.Net.Interfaces.DataTypes;
using Ical.Net.Interfaces.Serialization;
using iCloud.Dav.Calendar.Converters;
using iCloud.Dav.Calendar.Utils;
using iCloud.Dav.Core.Services;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;

namespace iCloud.Dav.Calendar
{
    [TypeConverter(typeof(EventConverter))]
    public class Event : Ical.Net.Event, Ical.Net.Interfaces.Components.IEvent, IDirectResponseSchema, ICloneable
    {
        [Required]
        public override string Uid { get => base.Uid; set => base.Uid = value; }

        [Required]
        public override string Summary { get => base.Summary; set => base.Summary = value; }

        [Required]
        public override string Description { get => base.Description; set => base.Description = value; }

        [Required]
        public override IDateTime Start { get => base.Start; set => base.Start = value; }

        [Required]
        public override IDateTime End { get => base.End; set => base.End = value; }

        public virtual string ETag { get; set; }

        public static new IICalendarCollection LoadFromStream(Stream s)
        {
            return Event.LoadFromStream(s, Encoding.UTF8, new CalendarSerializer());
        }

        public static new IICalendarCollection LoadFromStream(Stream s, Encoding e, ISerializer serializer)
        {
            return serializer.Deserialize(s, e) as IICalendarCollection;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
