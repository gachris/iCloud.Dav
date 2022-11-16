using iCloud.Dav.Calendar.CalDav.Types;
using iCloud.Dav.Calendar.Serialization.Converters;
using iCloud.Dav.Core.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace iCloud.Dav.Calendar.Types;

[XmlDeserializeType(typeof(MultiStatus))]
[TypeConverter(typeof(CalendarListConverter))]
public class CalendarList : List<Calendar>, IList<Calendar>, IEnumerable<Calendar>, IEnumerable
{
    /// <inheritdoc/>
    public CalendarList()
    {
    }

    /// <inheritdoc/>
    public CalendarList(int capacity) : base(capacity)
    {
    }

    /// <inheritdoc/>
    public CalendarList(IEnumerable<Calendar> collection) : base(collection)
    {
    }
}
