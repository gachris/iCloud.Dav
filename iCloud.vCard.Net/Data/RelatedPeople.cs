using iCloud.vCard.Net.Serialization;
using System;

namespace iCloud.vCard.Net.Data;

/// <summary>A related person defined in a <see cref="RelatedPeople"/>.</summary>
[Serializable]
public class RelatedPeople : CardDataType
{
    #region Properties

    public virtual string? Name { get; set; }

    public virtual string? Label { get; set; }

    public virtual bool IsPreferred { get; set; }

    /// <summary>The related person type.</summary>
    public virtual RelatedPeopleType RelatedPersonType { get; set; }

    #endregion

    public RelatedPeople()
    {
    }

    public RelatedPeople(CardPropertyList properties)
    {
        var relatedPeopleSerializer = new RelatedPeopleSerializer();
        relatedPeopleSerializer.Deserialize(properties, this);
    }
}
