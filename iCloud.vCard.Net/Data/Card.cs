using iCloud.vCard.Net.Serialization;
using System;

namespace iCloud.vCard.Net.Data;

public abstract class Card : CardDataType
{
    public CardPropertyList Properties { get; }

    /// <summary>
    /// A value that uniquely identifies the vCard.
    /// </summary>
    /// <remarks>
    ///     This value is optional.  The string must be any string
    ///     that can be used to uniquely identify the Person.  The
    ///     usage of the field is determined by the software.  Typical
    ///     possibilities for a unique string include a URL, a GUID,
    ///     or an LDAP directory path.  However, there is no particular
    ///     standard dictated by the Person specification.
    /// </remarks>

    public virtual string? Uid
    {
        get => Properties.Get<string>(Constants.Card.UID);
        set => Properties.Set(Constants.Card.UID, value);
    }

    /// <summary>
    /// The name of the product that generated the vCard.
    /// </summary>
    public virtual string? ProductId
    {
        get => Properties.Get<string>(Constants.Card.PRODID);
        set => Properties.Set(Constants.Card.PRODID, value);
    }

    /// <summary>
    /// The revision date of the vCard.
    /// </summary>
    /// <remarks>
    ///     The revision date is not automatically updated by the
    ///     vCard when modifying properties. It is up to the
    ///     developer to change the revision date as needed.
    /// </remarks>
    public virtual DateTime? RevisionDate
    {
        get => Properties.Get<DateTime?>(Constants.Card.REV);
        set => Properties.Set(Constants.Card.REV, value);
    }

    /// <summary>
    /// The formatted name of the card.
    /// </summary>
    /// <remarks>
    ///     This property allows the name of the card to be
    ///     written in a manner specific to his or her culture.
    ///     The formatted name is not required to strictly
    ///     correspond with the family name, given name, etc.
    /// </remarks>
    public virtual string? FormattedName
    {
        get => Properties.Get<string>(Constants.Card.FN);
        set => Properties.Set(Constants.Card.FN, value);
    }

    public Card() => Properties = new CardPropertyList();
}
