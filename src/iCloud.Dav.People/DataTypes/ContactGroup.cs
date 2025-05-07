using System.ComponentModel;
using System.Runtime.Serialization;
using iCloud.Dav.Core;
using iCloud.Dav.People.Extensions;
using iCloud.Dav.People.Serialization.Converters;
using vCard.Net;
using vCard.Net.CardComponents;
using vCard.Net.DataTypes;

namespace iCloud.Dav.People.DataTypes;

/// <summary>
/// A class that represents a vCard contact group object.
/// </summary>
[TypeConverter(typeof(ContactGroupConverter))]
public class ContactGroup : UniqueComponent, IDirectResponseSchema, IResource
{
    /// <summary>
    /// Gets or sets the vCard version. Only vCard 3.0 is supported by iCloud.
    /// </summary>
    /// <remarks>
    /// iCloud exclusively supports vCard 3.0, so setting this property
    /// to any other version will result in an InvalidOperationException.
    /// </remarks>
    public override VCardVersion Version
    {
        get => base.Version;
        set
        {
            if (value != VCardVersion.vCard3_0)
            {
                throw new InvalidOperationException("Invalid version specified. Please set 'Version' to 'v3', as this is the only version supported by iCloud.");
            }

            base.Version = value; // Optionally keep this line if needed for any base class functionality
        }
    }

    /// <summary>
    /// Gets or sets the e-tag associated with this contact group.
    /// </summary>
    /// <remarks>
    /// Will be set by the service deserialization method,
    /// or by the XML response parser if implemented on the service.
    /// </remarks>
    public virtual string ETag { get; set; }

    /// <summary>
    /// Gets or sets the href (resource URL) of this contact group.
    /// </summary>
    /// <remarks>
    /// The href is assigned by the response from the service.
    /// </remarks>
    public virtual string Href { get; set; }

    /// <summary>
    /// Gets the unique identifier of this contact group.
    /// </summary>
    /// <remarks>
    /// If the href is not set, the Uid is used as the identifier.
    /// </remarks>
    public virtual string Id => string.IsNullOrEmpty(Href) ? Uid : Href.ExtractId();

    /// <summary>
    /// Gets or sets the product identifier associated with this contact group.
    /// </summary>
    public virtual string ProductId
    {
        get => Properties.Get<string>("PRODID");
        set => Properties.Set("PRODID", value);
    }

    /// <summary>
    /// Gets or sets the revision date associated with this contact group.
    /// </summary>
    /// <remarks>
    ///     The revision date is not automatically updated by the
    ///     card when modifying properties. It is up to the
    ///     developer to change the revision date as needed.
    /// </remarks>
    public virtual IDateTime RevisionDate
    {
        get => Properties.Get<IDateTime>("REV");
        set => Properties.Set("REV", value);
    }

    /// <summary>
    /// Gets or sets formatted name associated with this contact group.
    /// </summary>
    /// <remarks>
    ///     This property allows the name of the vCard to be
    ///     written in a manner specific to his or her culture.
    ///     The formatted name is not required to strictly
    ///     correspond with the family name, given name, etc.
    /// </remarks>
    public virtual string FormattedName
    {
        get => Properties.Get<string>("FN");
        set => Properties.Set("FN", value);
    }

    /// <summary>
    /// Gets or sets the name associated with this contact group.
    /// </summary>
    public virtual string N
    {
        get => Properties.Get<string>("N");
        set => Properties.Set("N", value);
    }

    /// <summary>
    /// Gets or sets the kind associated with this contact group.
    /// </summary>
    public virtual Kind Kind
    {
        get => Properties.Get<Kind>("X-ADDRESSBOOKSERVER-KIND");
        set => Properties.Set("X-ADDRESSBOOKSERVER-KIND", value);
    }

    /// <summary>
    /// Gets or sets the list of members associated with this contact group.
    /// </summary>
    public virtual IList<Member> Members
    {
        get => Properties.GetMany<Member>("X-ADDRESSBOOKSERVER-MEMBER");
        set => Properties.Set("X-ADDRESSBOOKSERVER-MEMBER", value);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ContactGroup" /> class.
    /// </summary>
    public ContactGroup()
    {
        Name = Components.VCARD;
        EnsureProperties();
    }

    /// <summary>
    /// Method that is called after the object is deserialized. It ensures that the object has all required properties.
    /// </summary>
    /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
    protected override void OnDeserialized(StreamingContext context)
    {
        base.OnDeserialized(context);

        EnsureProperties();
    }

    /// <summary>
    /// Ensures contact group properties.
    /// </summary>
    private void EnsureProperties()
    {
        if (string.IsNullOrEmpty(Uid))
        {
            Uid = Guid.NewGuid().ToString();
        }

        Version = VCardVersion.vCard3_0;
        Kind ??= new Kind()
        {
            CardKind = KindType.Group
        };
    }

    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
        return obj is not null && (ReferenceEquals(this, obj) || obj.GetType() == GetType() && Equals((ContactGroup)obj));
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns><see langword = "true" /> if the specified object is equal to the current object; otherwise, <see langword = "false" />.</returns>
    protected bool Equals(ContactGroup obj)
    {
        return object.Equals(Version, obj.Version)
               && string.Equals(FormattedName, obj.FormattedName, StringComparison.OrdinalIgnoreCase)
               && string.Equals(N, obj.N, StringComparison.OrdinalIgnoreCase)
               && string.Equals(ProductId, obj.ProductId)
               && object.Equals(RevisionDate, obj.RevisionDate)
               && string.Equals(Uid, obj.Uid, StringComparison.OrdinalIgnoreCase)
               && object.Equals(Kind, obj.Kind)
               && CollectionHelpers.Equals(Members, obj.Members);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = Uid != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Uid) : 0;
            hashCode = hashCode * 23 + StringComparer.OrdinalIgnoreCase.GetHashCode(Version.ToString());
            hashCode = hashCode * 23 + FormattedName != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(FormattedName) : 0;
            hashCode = hashCode * 23 + N != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(N) : 0;
            hashCode = hashCode * 23 + ProductId != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(ProductId) : 0;
            hashCode = hashCode * 23 + (RevisionDate?.GetHashCode() ?? 0);
            hashCode = hashCode * 23 + (Kind?.GetHashCode() ?? 0);
            hashCode = hashCode * 23 + (Members != null ? CollectionHelpers.GetHashCode(Members) : 0);
            return hashCode;
        }
    }
}