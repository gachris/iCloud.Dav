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
    internal sealed class ReminderListConverter : TypeConverter
    {
        /// <inheritdoc/>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => sourceType == typeof(MultiStatus);

        /// <inheritdoc/>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (!CanConvertFrom(context, value.GetType())) throw GetConvertFromException(value);

            var multiStatus = (MultiStatus)value;
            var calendarResponse = multiStatus.Responses.FirstOrDefault(x => x.ResourceType?.Any(resourceType => resourceType.Name == "calendar") == true || !Path.HasExtension(x.Href));

            return new Reminders()
            {
                Kind = "reminders",
                ETag = calendarResponse?.Etag,
                NextSyncToken = calendarResponse?.SyncToken ?? multiStatus.SyncToken,
                Items = multiStatus.Responses.Except(new HashSet<Response>() { calendarResponse }).Select(ToReminder).ToList()
            };
        }

        private static Reminder ToReminder(Response response)
        {
            Reminder calendarReminder;
            var id = Path.GetFileNameWithoutExtension(response.Href);

            if (response.Status == Status.NotFound)
            {
                calendarReminder = new Reminder
                {
                    Id = id,
                    Uid = null,
                    Deleted = true,
                    ETag = response.Etag
                };
                return calendarReminder;
            }
            else if (response.CalendarData is null)
            {
                calendarReminder = new Reminder
                {
                    Id = id,
                    Uid = null,
                    ETag = response.Etag
                };
                return calendarReminder;
            }

            calendarReminder = response.CalendarData.Value.ToReminder();
            calendarReminder.ETag = response.Etag;
            calendarReminder.Id = id;
            return calendarReminder;
        }
    }
}