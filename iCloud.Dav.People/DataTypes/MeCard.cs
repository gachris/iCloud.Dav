using iCloud.Dav.Core.Serialization;
using iCloud.Dav.People.Serialization.Converters;
using iCloud.Dav.People.WebDav.DataTypes;
using System.ComponentModel;

namespace iCloud.Dav.People.DataTypes;

/// <summary>
/// Represents me-card component
/// </summary>   
[TypeConverter(typeof(MeCardConverter))]
[XmlDeserializeType(typeof(MultiStatus))]
public class MeCard
{
    /// <summary>
    /// The id of me-card
    /// </summary>
    public virtual string Id { get; set; }
}