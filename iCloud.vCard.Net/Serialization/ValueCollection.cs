using System.Collections.Specialized;

namespace iCloud.vCard.Net.Serialization;

/// <summary>A collection of string values.</summary>
public class ValueCollection : StringCollection
{
    #region Properties

    /// <summary>
    ///     The suggested separator when writing values to a string.
    /// </summary>
    public virtual char Separator { get; }

    #endregion

    /// <summary>
    ///     Initializes an empty <see cref="ValueCollection" />.
    /// </summary>
    public ValueCollection() => Separator = ',';

    /// <summary>
    ///     Initializes the value collection with the specified separator.
    /// </summary>
    /// <param name="separator">
    ///     The suggested character to use as a separator when
    ///     writing the collection as a string.
    /// </param>
    public ValueCollection(char separator) => Separator = separator;
}
