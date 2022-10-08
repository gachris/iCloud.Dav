using System;

namespace iCloud.Dav.People.Types;

/// <summary>A instant message defined in a <see cref="Person"/>.</summary>
/// <seealso cref="InstantMessageType" />
[Serializable]
public class InstantMessage : ICloneable
{
    #region Properties

    /// <summary>The type of instant message (e.g. home, work, etc).</summary>
    public virtual InstantMessageType InstantMessageType { get; set; }

    public virtual string? Label { get; set; }

    public virtual bool IsPreferred { get; set; }

    #endregion

    public object Clone() => MemberwiseClone();
}
