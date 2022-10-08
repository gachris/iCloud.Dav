using iCloud.Dav.Calendar.CalDav.Types;
using iCloud.Dav.Calendar.Converters;
using iCloud.Dav.Core;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace iCloud.Dav.Calendar.Types;

[XmlDeserializeType(typeof(MultiStatus))]
[TypeConverter(typeof(CalendarListConverter))]
public class CalendarEntryList : List<CalendarEntry>, IList<CalendarEntry>, IEnumerable<CalendarEntry>, IEnumerable
{
    public CalendarEntryList()
    {
    }

    public CalendarEntryList(int capacity) : base(capacity)
    {
    }

    public CalendarEntryList(IEnumerable<CalendarEntry> collection) : base(collection)
    {
    }
}
