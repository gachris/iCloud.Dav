using iCloud.Dav.People.Serialization.DataTypes;
using vCard.Net;
using vCard.Net.DataTypes;

namespace iCloud.Dav.People.DataTypes;

/// <summary>
/// Represents a birthdate in a vCard format.
/// </summary>
public class Birthdate : VCardDataType
{
    #region Properties

    /// <summary>
    /// Gets or sets the birthdate value.
    /// </summary>
    public virtual IDateTime Date { get; set; }

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="Birthdate"/> class.
    /// </summary>
    public Birthdate()
    {
        ValueType = "date";
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Birthdate"/> class with a string value.
    /// </summary>
    /// <param name="value">The serialized vCard string representing the birthdate.</param>
    public Birthdate(string value)
    {
        ValueType = "date";

        if (string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        var serializer = new BirthdateSerializer();
        CopyFrom(serializer.Deserialize(new StringReader(value)) as ICopyable);
    }

    #region Methods

    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
        return obj is Birthdate other && Equals(other);
    }

    /// <summary>
    /// Determines whether the specified <see cref="Birthdate"/> instance is equal to the current instance.
    /// </summary>
    /// <param name="other">The <see cref="Birthdate"/> instance to compare with the current instance.</param>
    /// <returns><see langword="true"/> if the specified instance is equal to the current instance; otherwise, <see langword="false"/>.</returns>
    protected bool Equals(Birthdate other)
    {
        return Date?.Equals(other.Date) ?? other.Date == null;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Date?.GetHashCode() ?? 0;
    }

    /// <summary>
    /// Returns a string representation of the birthdate.
    /// </summary>
    /// <returns>A string representing the birthdate.</returns>
    public override string ToString()
    {
        return Date?.ToString() ?? string.Empty;
    }

    #endregion
}