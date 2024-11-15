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
/// A class that represents a vCard contact object.
/// </summary>
[TypeConverter(typeof(ContactConverter))]
public class Contact : UniqueComponent, IDirectResponseSchema, IResource
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
    /// Gets or sets the e-tag associated with this contact.
    /// </summary>
    /// <remarks>
    /// Will be set by the service deserialization method,
    /// or by the XML response parser if implemented on the service.
    /// </remarks>
    public virtual string ETag { get; set; }

    /// <summary>
    /// Gets or sets the href (resource URL) of this contact.
    /// </summary>
    /// <remarks>
    /// The href is assigned by the response from the service.
    /// </remarks>
    public virtual string Href { get; set; }

    /// <summary>
    /// Gets the unique identifier of this contact.
    /// </summary>
    /// <remarks>
    /// If the href is not set, the Uid is used as the identifier.
    /// </remarks>
    public virtual string Id => string.IsNullOrEmpty(Href) ? Uid : Href.ExtractId();

    /// <summary>
    /// Gets or sets the product identifier associated with this contact.
    /// </summary>
    public virtual string ProductId
    {
        get => Properties.Get<string>("PRODID");
        set => Properties.Set("PRODID", value);
    }

    /// <summary>
    /// Gets or sets the revision date associated with this contact.
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
    /// Gets or sets formatted name associated with this contact.
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
    /// Gets or sets the name associated with this contact.
    /// </summary>
    public virtual StructuredName N
    {
        get => Properties.Get<StructuredName>("N");
        set => Properties.Set("N", value);
    }

    /// <summary>
    /// Gets or sets the kind associated with this contact.
    /// </summary>
    public Kind Kind
    {
        get => Properties.Get<Kind>("X-ADDRESSBOOKSERVER-KIND");
        set => Properties.Set("X-ADDRESSBOOKSERVER-KIND", value);
    }

    /// <summary>
    /// Gets or sets the phonetic last name associated with this contact.
    /// </summary>
    public virtual string PhoneticLastName
    {
        get => Properties.Get<string>("X-PHONETIC-LAST-NAME");
        set => Properties.Set("X-PHONETIC-LAST-NAME", value);
    }

    /// <summary>
    /// Gets or sets the phonetic first name associated with this contact.
    /// </summary>
    public virtual string PhoneticFirstName
    {
        get => Properties.Get<string>("X-PHONETIC-FIRST-NAME");
        set => Properties.Set("X-PHONETIC-FIRST-NAME", value);
    }

    /// <summary>
    /// Gets or sets the nickname associated with this contact.
    /// </summary>
    public virtual string Nickname
    {
        get => Properties.Get<string>("NICKNAME");
        set => Properties.Set("NICKNAME", value);
    }

    /// <summary>
    /// Gets or sets the title associated with this contact.
    /// </summary>
    public virtual string Title
    {
        get => Properties.Get<string>("TITLE");
        set => Properties.Set("TITLE", value);
    }

    /// <summary>
    /// Gets or sets the phonetic representation organization name associated with this contact.
    /// </summary>
    public virtual string PhoneticOrganization
    {
        get => Properties.Get<string>("X-PHONETIC-ORG");
        set => Properties.Set("X-PHONETIC-ORG", value);
    }

    /// <summary>
    /// Gets or sets the organization associated with this contact.
    /// </summary>
    public virtual Organization Organization
    {
        get => Properties.Get<Organization>("ORG");
        set => Properties.Set("ORG", value);
    }

    /// <summary>
    /// Gets or sets the birthdate associated with this contact.
    /// </summary>
    public virtual IDateTime Birthdate
    {
        get => Properties.Get<IDateTime>("BDAY");
        set => Properties.Set("BDAY", value);
    }

    /// <summary>
    /// Gets or sets the notes associated with the contact.
    /// </summary>
    public virtual string Notes
    {
        get => Properties.Get<string>("NOTE");
        set => Properties.Set("NOTE", value);
    }

    /// <summary>
    /// Gets or sets the show as associated with this contact.
    /// </summary>
    public virtual string ShowAs
    {
        get => Properties.Get<string>("X-ABSHOWAS");
        set => Properties.Set("X-ABSHOWAS", value);
    }

    /// <summary>
    /// Gets or sets the photo associated with this contact.
    /// </summary>
    public virtual Photo Photo
    {
        get => Properties.Get<Photo>("PHOTO");
        set => Properties.Set("PHOTO", value);
    }

    /// <summary>
    /// Gets or sets the list of websites associated with this contact.
    /// </summary>
    public virtual IList<Website> Websites
    {
        get => Properties.GetMany<Website>("URL");
        set => Properties.Set("URL", value);
    }

    /// <summary>
    /// Gets or sets the list of phone numbers associated with this contact.
    /// </summary>
    public virtual IList<Phone> Telephones
    {
        get => Properties.GetMany<Phone>("TEL");
        set => Properties.Set("TEL", value);
    }

    /// <summary>
    /// Gets or sets the list of social profiles associated with this contact.
    /// </summary>
    public virtual IList<SocialProfile> SocialProfiles
    {
        get => Properties.GetMany<SocialProfile>("X-SOCIALPROFILE");
        set => Properties.Set("X-SOCIALPROFILE", value);
    }

    /// <summary>
    /// Gets or sets the list of addresses associated with this contact.
    /// </summary>
    public virtual IList<Address> Addresses
    {
        get => Properties.GetMany<Address>("ADR");
        set => Properties.Set("ADR", value);
    }

    /// <summary>
    /// Gets or sets the list of related names associated with this contact.
    /// </summary>
    public virtual IList<RelatedNames> RelatedNames
    {
        get => Properties.GetMany<RelatedNames>("X-ABRELATEDNAMES");
        set => Properties.Set("X-ABRELATEDNAMES", value);
    }

    /// <summary>
    /// Gets or sets the list of dates associated with this contact.
    /// </summary>
    public virtual IList<Date> Dates
    {
        get => Properties.GetMany<Date>("X-ABDATE");
        set => Properties.Set("X-ABDATE", value);
    }

    /// <summary>
    /// Gets or sets the list of email addresses associated with this contact.
    /// </summary>
    public virtual IList<Email> Emails
    {
        get => Properties.GetMany<Email>("EMAIL");
        set => Properties.Set("EMAIL", value);
    }

    /// <summary>
    /// Gets or sets the list of instant messaging accounts associated with this contact.
    /// </summary>
    public virtual IList<InstantMessage> InstantMessages
    {
        get => Properties.GetMany<InstantMessage>("IMPP");
        set => Properties.Set("IMPP", value);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Contact"/> class.
    /// </summary>
    public Contact()
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
    /// Ensures contact properties.
    /// </summary>
    private void EnsureProperties()
    {
        if (string.IsNullOrEmpty(Uid))
        {
            Uid = Guid.NewGuid().ToString();
        }

        Version = VCardVersion.vCard3_0;
    }

    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
        return obj is not null && (ReferenceEquals(this, obj) || obj.GetType() == GetType() && Equals((Contact)obj));
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns><see langword = "true" /> if the specified object is equal to the current object; otherwise, <see langword = "false" />.</returns>
    protected bool Equals(Contact obj)
    {
        return string.Equals(Id, obj.Id, StringComparison.OrdinalIgnoreCase)
               && object.Equals(Version, obj.Version)
               && string.Equals(FormattedName, obj.FormattedName, StringComparison.OrdinalIgnoreCase)
               && object.Equals(N, obj.N)
               && string.Equals(ProductId, obj.ProductId)
               && object.Equals(RevisionDate, obj.RevisionDate)
               && string.Equals(Uid, obj.Uid, StringComparison.OrdinalIgnoreCase)
               && object.Equals(Kind, obj.Kind)
               && string.Equals(PhoneticLastName, obj.PhoneticLastName, StringComparison.OrdinalIgnoreCase)
               && string.Equals(PhoneticFirstName, obj.PhoneticFirstName, StringComparison.OrdinalIgnoreCase)
               && string.Equals(Nickname, obj.Nickname, StringComparison.OrdinalIgnoreCase)
               && string.Equals(Title, obj.Title, StringComparison.OrdinalIgnoreCase)
               && string.Equals(PhoneticOrganization, obj.PhoneticOrganization, StringComparison.OrdinalIgnoreCase)
               && object.Equals(Organization, obj.Organization)
               && object.Equals(Birthdate, obj.Birthdate)
               && string.Equals(Notes, obj.Notes, StringComparison.OrdinalIgnoreCase)
               && string.Equals(ShowAs, obj.ShowAs, StringComparison.OrdinalIgnoreCase)
               && object.Equals(Photo, obj.Photo)
               && CollectionHelpers.Equals(Websites, obj.Websites)
               && CollectionHelpers.Equals(Telephones, obj.Telephones)
               && CollectionHelpers.Equals(SocialProfiles, obj.SocialProfiles)
               && CollectionHelpers.Equals(Addresses, obj.Addresses)
               && CollectionHelpers.Equals(RelatedNames, obj.RelatedNames)
               && CollectionHelpers.Equals(Dates, obj.Dates)
               && CollectionHelpers.Equals(Emails, obj.Emails)
               && CollectionHelpers.Equals(InstantMessages, obj.InstantMessages);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (Uid != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Uid) : 0);
            hashCode = hashCode * 23 + StringComparer.OrdinalIgnoreCase.GetHashCode(Version.ToString());
            hashCode = hashCode * 23 + (FormattedName != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(FormattedName) : 0);
            hashCode = hashCode * 23 + (N?.GetHashCode() ?? 0);
            hashCode = hashCode * 23 + (ProductId != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(ProductId) : 0);
            hashCode = hashCode * 23 + (RevisionDate?.GetHashCode() ?? 0);
            hashCode = hashCode * 23 + (Kind?.GetHashCode() ?? 0);
            hashCode = hashCode * 23 + (PhoneticLastName != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(PhoneticLastName) : 0);
            hashCode = hashCode * 23 + (PhoneticFirstName != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(PhoneticFirstName) : 0);
            hashCode = hashCode * 23 + (Nickname != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Nickname) : 0);
            hashCode = hashCode * 23 + (Title != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Title) : 0);
            hashCode = hashCode * 23 + (PhoneticOrganization != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(PhoneticOrganization) : 0);
            hashCode = hashCode * 23 + (Organization?.GetHashCode() ?? 0);
            hashCode = hashCode * 23 + (Birthdate?.GetHashCode() ?? 0);
            hashCode = hashCode * 23 + (Notes != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Notes) : 0);
            hashCode = hashCode * 23 + (ShowAs != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(ShowAs) : 0);
            hashCode = hashCode * 23 + (Photo?.GetHashCode() ?? 0);
            hashCode = hashCode * 23 + (Websites != null ? CollectionHelpers.GetHashCode(Websites) : 0);
            hashCode = hashCode * 23 + (Telephones != null ? CollectionHelpers.GetHashCode(Telephones) : 0);
            hashCode = hashCode * 23 + (SocialProfiles != null ? CollectionHelpers.GetHashCode(SocialProfiles) : 0);
            hashCode = hashCode * 23 + (Addresses != null ? CollectionHelpers.GetHashCode(Addresses) : 0);
            hashCode = hashCode * 23 + (RelatedNames != null ? CollectionHelpers.GetHashCode(RelatedNames) : 0);
            hashCode = hashCode * 23 + (Dates != null ? CollectionHelpers.GetHashCode(Dates) : 0);
            hashCode = hashCode * 23 + (Emails != null ? CollectionHelpers.GetHashCode(Emails) : 0);
            hashCode = hashCode * 23 + (InstantMessages != null ? CollectionHelpers.GetHashCode(InstantMessages) : 0);
            return hashCode;
        }
    }
}