using iCloud.Dav.People.Serialization.Converters;
using System;
using System.ComponentModel;

namespace iCloud.Dav.People.Types;

/// <summary>A related person defined in a <see cref="RelatedPeople"/>.</summary>
[Serializable]
[TypeConverter(typeof(RelatedPeopleConverter))]
public class RelatedPeople : ICloneable
{
    #region Properties

    public virtual string? Name { get; set; }

    public virtual string? Label { get; set; }

    public virtual bool IsPreferred { get; set; }

    /// <summary>The related person type.</summary>
    public virtual RelatedPeopleType RelatedPersonType { get; set; }

    #endregion

    public object Clone() => MemberwiseClone();
}
