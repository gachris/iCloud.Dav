using iCloud.Dav.People.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace iCloud.Dav.People.Utils;

internal static class CardPropertyExtensions
{
    public static CardProperty? FindByName(this IEnumerable<CardProperty> properties, string name)
    {
        return properties?.FirstOrDefault(x => x.Name == name);
    }
}
