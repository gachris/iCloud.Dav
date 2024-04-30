using System;
using System.IO;
using iCloud.Dav.People.Serialization.DataTypes;
using vCard.Net;
using vCard.Net.DataTypes;

namespace iCloud.Dav.People.DataTypes;

/// <summary>
/// Represents a label associated with a contact's property.
/// </summary>
public class Label : EncodableDataType
{
    #region Properties

    /// <summary>
    /// Gets or sets the value of the label.
    /// </summary>
    public virtual string Value { get; set; }

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="Label"/> class.
    /// </summary>
    public Label()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Label"/> class with a string value.
    /// </summary>
    /// <param name="value">A string representation of the label value.</param>
    public Label(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return;

        var serializer = new LabelSerializer();
        CopyFrom(serializer.Deserialize(new StringReader(value)) as ICopyable);
    }

    #region Methods

    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
        return !(obj is null) && (ReferenceEquals(this, obj) || obj.GetType() == GetType() && Equals((Label)obj));
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns><see langword = "true" /> if the specified object is equal to the current object; otherwise, <see langword = "false" />.</returns>
    protected bool Equals(Label obj)
    {
        return string.Equals(Value, obj.Value, StringComparison.OrdinalIgnoreCase);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        unchecked
        {
            return Value.GetHashCode();
        }
    }

    #endregion
}