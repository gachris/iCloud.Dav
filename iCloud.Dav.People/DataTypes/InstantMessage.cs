using iCloud.Dav.People.Serialization.DataTypes;
using System.IO;
using vCard.Net;
using vCard.Net.DataTypes;

namespace iCloud.Dav.People.DataTypes;

public class InstantMessage : EncodableDataType
{
    #region Properties

    /// <summary>
    /// The type of instant message (e.g. home, work, etc).
    /// </summary>
    public virtual InstantMessageType InstantMessageType { get; set; }

    public virtual string? Label { get; set; }

    public virtual bool IsPreferred { get; set; }

    #endregion

    public InstantMessage()
    {
    }

    public InstantMessage(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        var serializer = new InstantMessageSerializer();
        CopyFrom(serializer.Deserialize(new StringReader(value)) as ICopyable);
    }
}
