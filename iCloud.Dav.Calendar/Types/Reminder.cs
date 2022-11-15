using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using iCloud.Dav.Calendar.Converters;
using iCloud.Dav.Core;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace iCloud.Dav.Calendar.Types;

[TypeConverter(typeof(ReminderConverter))]
public class Reminder : Todo, IDirectResponseSchema, ICloneable
{
    [Required]
    public override string Uid { get => base.Uid; set => base.Uid = value; }

    [Required]
    public override string Summary { get => base.Summary; set => base.Summary = value; }

    [Required]
    public override string Description { get => base.Description; set => base.Description = value; }

    [Required]
    public override IDateTime Start { get => base.Start; set => base.Start = value; }

    /// <inheritdoc/>
    public virtual string? ETag { get; set; }

    /// <inheritdoc/>
    public object Clone() => MemberwiseClone();
}
