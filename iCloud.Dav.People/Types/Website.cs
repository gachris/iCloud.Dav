using iCloud.Dav.People.Serialization.Converters;
using System;
using System.ComponentModel;

namespace iCloud.Dav.People.Types;

/// <summary>A web site defined in a <see cref="Contact"/>.</summary>
/// <seealso cref="WebsiteType" />
[Serializable]
[TypeConverter(typeof(WebsiteConverter))]
public class Website
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
