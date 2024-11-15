using vCard.Net;
using vCard.Net.Collections;

namespace iCloud.Dav.People.DataTypes;

/// <summary>
/// Represents a list of properties associated with a card data type.
/// </summary>
public class CardDataTypePropertyList : GroupedValueList<string, ICardProperty, CardProperty, object>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CardDataTypePropertyList"/> class.
    /// </summary>
    public CardDataTypePropertyList()
    {
    }

    /// <summary>
    /// Gets the first property with the specified name, or null if the list does not contain a property with that name.
    /// </summary>
    /// <param name="name">The name of the property.</param>
    /// <returns>The first property with the specified name, or null if the list does not contain a property with that name.</returns>
    public ICardProperty this[string name] => ContainsKey(name) ? AllOf(name).FirstOrDefault() : null;
}