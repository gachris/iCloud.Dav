using iCloud.Dav.Calendar.CalDav.Types;
using iCloud.Dav.Calendar.Serialization.Converters;
using iCloud.Dav.Core.Serialization;
using System.Collections.Generic;
using System.ComponentModel;

namespace iCloud.Dav.Calendar.DataTypes
{
    /// <inheritdoc/>
    [TypeConverter(typeof(EventListConverter))]
    [XmlDeserializeType(typeof(MultiStatus))]
    public class Events
    {
        internal Events(IEnumerable<Event> events) => Items = events;

        public IEnumerable<Event> Items { get; set; }
    }
}