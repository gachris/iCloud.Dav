using iCloud.vCard.Net.Serialization;
using System;

namespace iCloud.vCard.Net.Data;

/// <summary>A web site defined in a <see cref="Contact"/>.</summary>
/// <seealso cref="WebsiteType" />
[Serializable]
public class Website : CardDataType
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

    public Website()
    {
    }

    public Website(CardPropertyList properties)
    {
        var websiteSerializer = new WebsiteSerializer();
        websiteSerializer.Deserialize(properties, this);
    }
}
