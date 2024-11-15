using System.ComponentModel;
using System.Runtime.Serialization;
using Ical.Net.CalendarComponents;
using iCloud.Dav.Calendar.Serialization.Converters;
using iCloud.Dav.Core;

namespace iCloud.Dav.Calendar.DataTypes;

/// <summary>
/// Represents a calendar event, a component that has a start time, end time, and an optional duration.
/// </summary>
[TypeConverter(typeof(EventConverter))]
public class Event : CalendarEvent, IDirectResponseSchema, IUrlPath
{
    /// <summary>
    /// A value that uniquely identifies the event. It is used for requests and in most cases has the same value as the <seealso cref="UniqueComponent.Uid"/>.
    /// </summary>
    /// <remarks>The initial value of the Id property is the same as the <seealso cref="UniqueComponent.Uid"/>.</remarks>
    public virtual string Id { get; set; }

    /// <inheritdoc/>
    public virtual string ETag { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Event"/> class.
    /// </summary>
    public Event() : base()
    {
        EnsureProperties();
    }

    /// <summary>
    /// This method is called during deserialization to initialize the object before any deserialization is done.
    /// </summary>
    /// <param name="context">The context for the serialization or deserialization operation.</param>
    protected override void OnDeserialized(StreamingContext context)
    {
        base.OnDeserialized(context);

        EnsureProperties();
    }

    /// <summary>
    /// Ensures that the properties of the calendar list entry are set.
    /// </summary>
    private void EnsureProperties()
    {
        if (string.IsNullOrEmpty(Uid))
        {
            Uid = Guid.NewGuid().ToString();
        }

        if (string.IsNullOrEmpty(Id))
        {
            Id = Uid;
        }

        if (Calendar is null)
        {
            var calendar = new Ical.Net.Calendar();
            calendar.Events.Add(this);
            Parent = calendar;
        }
    }
}