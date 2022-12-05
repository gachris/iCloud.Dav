using Ical.Net.CalendarComponents;
using iCloud.Dav.Calendar.Serialization.Converters;
using iCloud.Dav.Core;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace iCloud.Dav.Calendar.DataTypes
{
    /// <inheritdoc/>
    [TypeConverter(typeof(ReminderConverter))]
    public class Reminder : Todo, IDirectResponseSchema, IUrlPath
    {
        /// <summary>
        /// A value that uniquely identifies the Reminder. It is used for requests and in most cases has the same value as the <seealso cref="UniqueComponent.Uid"/>.
        /// </summary>
        /// <remarks>The initial value of Id is same as the <seealso cref="UniqueComponent.Uid"/></remarks>
        public virtual string Id { get; set; }

        /// <inheritdoc/>
        public virtual string ETag { get; set; }

        public Reminder() : base()
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