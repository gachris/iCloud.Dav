using iCloud.Dav.People.Serialization.Converters;
using System;
using System.ComponentModel;

namespace iCloud.Dav.People.Types;

/// <summary>
///     Phone information for a <see cref="Person" />.
/// </summary>
/// <seealso cref="Types.PhoneType" />
[Serializable]
[TypeConverter(typeof(PhoneConverter))]
public class Phone : ICloneable
{
    #region Properties

    /// <summary>The full telephone number.</summary>
    public virtual string? FullNumber { get; set; }

    /// <summary>The phone type.</summary>
    public virtual PhoneType PhoneType { get; set; }

    public virtual string? Label { get; set; }

    public virtual bool IsPreferred { get; set; }

    #endregion

    public object Clone() => MemberwiseClone();
}