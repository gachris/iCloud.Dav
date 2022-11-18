using iCloud.vCard.Net.Serialization;
using System;

namespace iCloud.vCard.Net.Data;

/// <summary>A date defined in a <see cref="Date"/>.</summary>
public class Date : CardDataType
{
    #region Properties

    public virtual string? Label { get; set; }

    public virtual DateTime? DateTime { get; set; }

    public virtual DateType DateType { get; set; }

    public virtual bool IsPreferred { get; set; }

    #endregion

    public Date()
    {
    }

    public Date(CardPropertyList properties)
    {
        var dateSerializer = new DateSerializer();
        dateSerializer.Deserialize(properties, this);
    }
}
