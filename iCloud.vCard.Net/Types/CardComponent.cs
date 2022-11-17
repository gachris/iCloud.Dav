using System;

namespace iCloud.vCard.Net.Types;

public abstract class CardComponent
{
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
    public virtual string? Uid { get; set; }

    /// <summary>
    /// The name of the product that generated the vCard.
    /// </summary>
    public virtual string? ProductId { get; set; }

    /// <summary>
    /// The revision date of the vCard.
    /// </summary>
    /// <remarks>
    ///     The revision date is not automatically updated by the
    ///     vCard when modifying properties. It is up to the
    ///     developer to change the revision date as needed.
    /// </remarks>
    public virtual DateTime? RevisionDate { get; set; }
}
