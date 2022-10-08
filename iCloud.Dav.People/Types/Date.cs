using iCloud.Dav.People.Serialization.Converters;
using System;
using System.ComponentModel;

namespace iCloud.Dav.People.Types;

/// <summary>A date defined in a <see cref="Date"/>.</summary>
[TypeConverter(typeof(DateConverter))]
public class Date : ICloneable
{
    #region Properties

    public virtual string? Label { get; set; }

    public virtual DateTime? DateTime { get; set; }

    public virtual DateType DateType { get; set; }

    public virtual bool IsPreferred { get; set; }

    #endregion

    public object Clone() => MemberwiseClone();
}
