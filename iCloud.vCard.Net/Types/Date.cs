using iCloud.vCard.Net.Serialization.Converters;
using System;
using System.ComponentModel;

namespace iCloud.vCard.Net.Types;

/// <summary>A date defined in a <see cref="Date"/>.</summary>
[TypeConverter(typeof(DateConverter))]
public class Date
{
    #region Properties

    public virtual string? Label { get; set; }

    public virtual DateTime? DateTime { get; set; }

    public virtual DateType DateType { get; set; }

    public virtual bool IsPreferred { get; set; }

    #endregion
}
