﻿using iCloud.Dav.Calendar.Converters;
using iCloud.Dav.Calendar.Types;
using iCloud.Dav.Core.Attributes;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace iCloud.Dav.Calendar
{
    [XmlDeserializeType(typeof(Multistatus<Prop>))]
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
}
