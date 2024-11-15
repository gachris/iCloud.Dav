using System.Drawing;
using iCloud.Dav.People.Serialization.DataTypes;
using vCard.Net;
using vCard.Net.DataTypes;

namespace iCloud.Dav.People.DataTypes;

/// <summary>
/// Represents a photo value that can be associated with a contact.
/// </summary>
public class Photo : EncodableDataType
{
    #region Fields/Consts

    private Uri _url;
    private byte[] _data;

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the URL of the photo.
    /// Setting this property will nullify the value of the <see cref="Data"/> property.
    /// </summary>
    public Uri Url
    {
        get => _url;
        set
        {
            _url = value;
            _data = null;
        }
    }

    /// <summary>
    /// Gets or sets the binary value of the photo.
    /// Setting this property will nullify the value of the <see cref="Url"/> property.
    /// </summary>
    public byte[] Data
    {
        get => _data;
        set
        {
            _data = value;
            _url = null;
        }
    }

    /// <summary>
    /// Gets or sets the rectangular region of the photo.
    /// </summary>
    public Rectangle Rectangle { get; set; }

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="Photo"/> class.
    /// </summary>
    public Photo()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Photo"/> class with a string value.
    /// </summary>
    /// <param name="value">The value of the photo.</param>
    public Photo(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return;

        var serializer = new PhotoSerializer();
        CopyFrom(serializer.Deserialize(new StringReader(value)) as ICopyable);
    }

    #region Methods

    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
        return obj is not null && (ReferenceEquals(this, obj) || obj.GetType() == GetType() && Equals((Photo)obj));
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns><see langword = "true" /> if the specified object is equal to the current object; otherwise, <see langword = "false" />.</returns>
    protected bool Equals(Photo obj)
    {
        var flag = Equals(Url, obj.Url);
        return Data != null ? flag ? Data.SequenceEqual(obj.Data) : false : flag;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = Url != null ? Url.GetHashCode() : 0;
            hashCode = (hashCode * 397) ^ (Data != null ? Data.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ Rectangle.GetHashCode();
            return hashCode;
        }
    }

    #endregion
}