using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using iCloud.Dav.Calendar.Converters;
using iCloud.Dav.Calendar.Utils;
using iCloud.Dav.Core.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using IcalCalendar = Ical.Net.Calendar;

namespace iCloud.Dav.Calendar
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

        public static IList<IcalCalendar> LoadFromStream(Stream stream)
        {
            return CalendarDeserializer.Default.Deserialize(new StreamReader(stream, Encoding.UTF8)).OfType<IcalCalendar>().ToList();
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
