using Ical.Net.CalendarComponents;
using iCloud.Dav.Calendar.Serialization.Converters;
using iCloud.Dav.Core;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace iCloud.Dav.Calendar.DataTypes
{
    /// <inheritdoc/>
    [TypeConverter(typeof(EventConverter))]
    public class Event : CalendarEvent, IDirectResponseSchema, IUrlPath
    {
        public virtual string Id { get; set; }

        /// <inheritdoc/>
        public virtual string ETag { get; set; }

        public Event() : base() 
        {
            EnsureProperties();
        }

        protected override void OnDeserialized(StreamingContext context)
        {
            base.OnDeserialized(context);

            EnsureProperties();
        }

        private void EnsureProperties()
        {
            if (string.IsNullOrEmpty(Uid))
            {
                // Create a new UID for the component
                Id = Uid = Guid.NewGuid().ToString();
            }
        }
    }
}