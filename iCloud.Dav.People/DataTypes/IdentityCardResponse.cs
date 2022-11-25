using iCloud.Dav.Core.Serialization;
using iCloud.Dav.People.CardDav.Types;
using iCloud.Dav.People.Serialization.Converters;
using System.Collections.Generic;
using System.ComponentModel;

namespace iCloud.Dav.People.DataTypes;

/// <summary>
/// Represents a strongly typed list of objects that can be accessed by index. Provides
/// methods to search, sort, and manipulate lists.
/// </summary>   
[TypeConverter(typeof(IdentityCardListConverter))]
[XmlDeserializeType(typeof(MultiStatus))]
public class IdentityCardResponse
{
    internal IdentityCardResponse(IEnumerable<IdentityCard> cards) => Cards = cards;

    public IEnumerable<IdentityCard> Cards { get; set; }
}
