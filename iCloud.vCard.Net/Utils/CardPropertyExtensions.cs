using iCloud.vCard.Net.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace iCloud.vCard.Net.Utils;

internal static class CardPropertyExtensions
{
    public static CardProperty? FindByName(this IEnumerable<CardProperty> properties, string name) => properties?.FirstOrDefault(x => x.Name == name);
}
