using System;

namespace iCloud.vCard.Net.Data;

/// <summary>
/// A instant message defined in a <see cref="Contact"/>.
/// </summary>
[Serializable]
public class InstantMessage : CardDataType
{
    #region Properties

    /// <summary>
    /// The type of instant message (e.g. home, work, etc).
    /// </summary>
    public virtual InstantMessageType InstantMessageType { get; set; }

    public virtual string? Label { get; set; }

    public virtual bool IsPreferred { get; set; }

    #endregion
}
