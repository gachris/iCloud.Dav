using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using iCloud.Dav.ICalendar.Converters;
using iCloud.Dav.Core.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using iCloud.Dav.ICalendar.Utils;
using Ical.Net;
using System.Linq;

namespace iCloud.Dav.ICalendar
{
    [TypeConverter(typeof(ReminderConverter))]
    public class Reminder : Todo, IDirectResponseSchema, ICloneable
    {
        [Required]
        public override string Uid { get => base.Uid; set => base.Uid = value; }

        [Required]
        public override string Summary { get => base.Summary; set => base.Summary = value; }

        [Required]
        public override string Description { get => base.Description; set => base.Description = value; }

        [Required]
        public override IDateTime Start { get => base.Start; set => base.Start = value; }

        public virtual string ETag { get; set; }

        public static IList<Calendar> LoadFromStream(Stream stream)
        {
            return CalendarDeserializer.Default.Deserialize(new StreamReader(stream, Encoding.UTF8)).OfType<Calendar>().ToList();
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
