using iCloud.Dav.Calendar.CalDav.Types;
using iCloud.Dav.Calendar.Serialization.Converters;
using iCloud.Dav.Core.Serialization;
using System.Collections.Generic;
using System.ComponentModel;

namespace iCloud.Dav.Calendar.DataTypes
{
    /// <inheritdoc/>
    [XmlDeserializeType(typeof(MultiStatus))]
    [TypeConverter(typeof(CalendarListConverter))]
    public class CalendarList
    {
        internal CalendarList(IEnumerable<Calendar> items) => Items = items;

        public IEnumerable<Calendar> Items { get; set; }
    }
}