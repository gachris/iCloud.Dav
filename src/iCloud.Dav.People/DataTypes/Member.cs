using iCloud.Dav.People.Serialization.DataTypes;
using vCard.Net;
using vCard.Net.DataTypes;

namespace iCloud.Dav.People.DataTypes;

/// <summary>
/// Represents a member of a contact group in a vCard format.
/// </summary>
public class Member : VCardDataType
{
    #region Properties

    /// <summary>
    /// Gets or sets the unique identifier (UID) of the member.
    /// </summary>
    public virtual string Uid { get; set; }

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="Member"/> class.
    /// </summary>
    public Member()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Member"/> class with a string value.
    /// </summary>
    /// <param name="value">The serialized vCard string representing the member.</param>
    public Member(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        // Deserialize the string value using MemberSerializer and copy properties from the deserialized object.
        var serializer = new MemberSerializer();
        CopyFrom(serializer.Deserialize(new StringReader(value)) as ICopyable);
    }

    #region Methods

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns><see langword="true"/> if the specified object is equal to the current object; otherwise, <see langword="false"/>.</returns>
    public override bool Equals(object obj)
    {
        return obj is not null && (ReferenceEquals(this, obj) || obj.GetType() == GetType() && Equals((Member)obj));
    }

    /// <summary>
    /// Determines whether the specified <see cref="Member"/> instance is equal to the current instance.
    /// </summary>
    /// <param name="obj">The <see cref="Member"/> instance to compare with the current instance.</param>
    /// <returns><see langword="true"/> if the specified instance is equal to the current instance; otherwise, <see langword="false"/>.</returns>
    protected bool Equals(Member obj)
    {
        return string.Equals(Uid, obj.Uid, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Returns a hash code for the current instance.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
    {
        unchecked
        {
            return 17 * 23 + (Uid != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Uid) : 0);
        }
    }

    #endregion
}