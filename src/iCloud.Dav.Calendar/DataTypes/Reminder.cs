using System.ComponentModel;
using System.Runtime.Serialization;
using Ical.Net.CalendarComponents;
using iCloud.Dav.Calendar.Serialization.Converters;
using iCloud.Dav.Core;

namespace iCloud.Dav.Calendar.DataTypes;

/// <summary>
/// Represents a Reminder, which is a type of Todo that has a due date and/or a location.
/// </summary>
[TypeConverter(typeof(ReminderConverter))]
public class Reminder : Todo, IDirectResponseSchema, IUrlPath
{
    /// <summary>
    /// A value that uniquely identifies the Reminder. It is used for requests and in most cases has the same value as the <seealso cref="UniqueComponent.Uid"/>.
    /// </summary>
    /// <remarks>The initial value of Id is the same as the <seealso cref="UniqueComponent.Uid"/>.</remarks>
    public virtual string Id { get; set; }

    /// <inheritdoc/>
    public virtual string ETag { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Reminder"/> class.
    /// </summary>
    public Reminder() : base()
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
            calendar.Todos.Add(this);
            Parent = calendar;
        }
    }
}