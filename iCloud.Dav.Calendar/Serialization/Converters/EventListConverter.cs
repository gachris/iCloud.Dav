using iCloud.Dav.Calendar.CalDav.Types;
using iCloud.Dav.Calendar.DataTypes;
using iCloud.Dav.Calendar.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;

namespace iCloud.Dav.Calendar.Serialization.Converters
{
    internal sealed class EventListConverter : TypeConverter
    {
        /// <inheritdoc/>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => sourceType == typeof(MultiStatus);

        /// <inheritdoc/>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (!CanConvertFrom(context, value.GetType())) throw GetConvertFromException(value);

            var multiStatus = (MultiStatus)value;
            var calendarResponse = multiStatus.Responses.FirstOrDefault(x => x.ResourceType?.Any(resourceType => resourceType.Name == "calendar") == true || !Path.HasExtension(x.Href.TrimEnd('/')));

            return new Events()
            {
                Kind = "events",
                ETag = calendarResponse?.Etag,
                NextSyncToken = calendarResponse?.SyncToken ?? multiStatus.SyncToken,
                Items = multiStatus.Responses.Except(new HashSet<Response>() { calendarResponse }).Select(ToEvent).ToList()
            };
        }

        private static Event ToEvent(Response response)
        {
            var calendarEvent = response.CalendarData.Value.ToEvent();
            calendarEvent.ETag = response.Etag;
            calendarEvent.Id = Path.GetFileNameWithoutExtension(response.Href.TrimEnd('/'));
            return calendarEvent;
        }
    }
}