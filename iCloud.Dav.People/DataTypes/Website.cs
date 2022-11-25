using System;
using iCloud.Dav.People.PeopleComponents;

using vCard.Net.DataTypes;
namespace iCloud.Dav.People.DataTypes;

/// <summary>A web site defined in a <see cref="Contact"/>.</summary>
/// <seealso cref="WebsiteType" />
[Serializable]
public class Website : EncodableDataType
{
    #region Properties

    /// <summary>The URL of the web site.</summary>
    /// <remarks>The format of the URL is not validated.</remarks>
    public virtual string? Url { get; set; }

    /// <summary>The type of web site (e.g. home, work, etc).</summary>
    public virtual WebsiteType WebsiteType { get; set; }

    public virtual string? Label { get; set; }

    public virtual bool IsPreferred { get; set; }

    #endregion
}
