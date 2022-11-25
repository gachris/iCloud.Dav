using iCloud.Dav.People.Serialization.DataTypes;
using System;
using System.Collections.Generic;
using System.IO;
using vCard.Net;
using vCard.Net.DataTypes;

namespace iCloud.Dav.People.DataTypes;

/// <summary>A date defined in a <see cref="X_ABDate"/>.</summary>
public class X_ABDate : EncodableDataType
{
    public virtual DateTime? DateTime { get; set; }

    /// <summary>The url types.</summary>
    public virtual IList<string> Types
    {
        get => Parameters.GetMany("TYPE");
        set => Parameters.Set("TYPE", value);
    }

    #region Properties

    public virtual string? Label { get; set; }

    public virtual DateType DateType { get; set; }

    public virtual bool IsPreferred { get; set; }

    #endregion

    public X_ABDate()
    {
    }

    public X_ABDate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        var serializer = new X_ABDateSerializer();
        CopyFrom(serializer.Deserialize(new StringReader(value)) as ICopyable);
    }
}
