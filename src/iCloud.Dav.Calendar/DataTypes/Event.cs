using System.ComponentModel;
using System.Runtime.Serialization;
using Ical.Net.CalendarComponents;
using iCloud.Dav.Calendar.Extensions;
using iCloud.Dav.Calendar.Serialization.Converters;
using iCloud.Dav.Core;

namespace iCloud.Dav.Calendar.DataTypes;

/// <summary>
/// Represents a calendar event, a component that has a start time, end time, and an optional duration.
/// </summary>
[TypeConverter(typeof(EventConverter))]
public class Event : CalendarEvent, IDirectResponseSchema, IResource
{
    /// <summary>
    /// Gets or sets the e-tag associated with this event.
    /// </summary>
    /// <remarks>
    /// Will be set by the service deserialization method,
    /// or by the XML response parser if implemented on the service.
    /// </remarks>
    public virtual string ETag { get; set; }

    /// <summary>
    /// Gets or sets the href (resource URL) of this event.
    /// </summary>
    /// <remarks>
    /// The href is assigned by the response from the service.
    /// </remarks>
    public virtual string Href { get; set; }

    /// <summary>
    /// Gets the unique identifier of this event.
    /// </summary>
    /// <remarks>
    /// If the href is not set, the Uid is used as the identifier.
    /// </remarks>
    public virtual string Id => string.IsNullOrEmpty(Href) ? Uid : Href.ExtractId();

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

        if (Calendar is null)
        {
            var calendar = new Ical.Net.Calendar();
            calendar.Events.Add(this);
            Parent = calendar;
        }
    }
}