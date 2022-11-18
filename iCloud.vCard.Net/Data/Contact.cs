using System;
using System.Collections.Generic;

namespace iCloud.vCard.Net.Data;

/// <summary>
///     A contact object for exchanging personal contact information.
/// </summary>
/// <remarks>
///     <para>
///         A contact contains personal information, such as postal
///         addresses, public security certificates, email addresses, and
///         web sites.  The contact specification makes it possible for
///         different computer programs to exchange personal contact
///         information; for example, a contact can be attached to an email or
///         sent over a wireless connection.
///     </para>
///     <para>
///         The standard contact format is a text file with properties in
///         name:value format.  However, there are multiple versions of
///         this format as well as compatible alternatives in XML and
///         HTML formats.  This class library aims to accomodate these
///         variations but be aware some some formats do not support
///         all possible properties.
///     </para>
/// </remarks>
[Serializable]
public class Contact : Card
{
    #region Properties

    /// <summary>
    /// The name of the contact.
    /// </summary>
    public virtual Name? Name
    {
        get => Properties.Get<Name>(Constants.Contact.N);
        set => Properties.Set(Constants.Contact.N, value);
    }

    /// <summary>
    /// The phonetic last name of the contact.
    /// </summary>
    public virtual string? PhoneticLastName
    {
        get => Properties.Get<string>(Constants.Contact.X_PHONETIC_LAST_NAME);
        set => Properties.Set(Constants.Contact.X_PHONETIC_LAST_NAME, value);
    }

    /// <summary>
    /// The phonetic first name of the contact.
    /// </summary>
    public virtual string? PhoneticFirstName
    {
        get => Properties.Get<string>(Constants.Contact.X_PHONETIC_FIRST_NAME);
        set => Properties.Set(Constants.Contact.X_PHONETIC_FIRST_NAME, value);
    }

    /// <summary>
    /// The nickname of the contact.
    /// </summary>
    public virtual string? Nickname
    {
        get => Properties.Get<string>(Constants.Contact.NICKNAME);
        set => Properties.Set(Constants.Contact.NICKNAME, value);
    }

    /// <summary>
    /// The job title of the contact.
    /// </summary>
    public virtual string? Title
    {
        get => Properties.Get<string>(Constants.Contact.TITLE);
        set => Properties.Set(Constants.Contact.TITLE, value);
    }

    /// <summary>
    /// The phonetic organization of the contact.
    /// </summary>
    public virtual string? PhoneticOrganization
    {
        get => Properties.Get<string>(Constants.Contact.X_PHONETIC_ORG);
        set => Properties.Set(Constants.Contact.X_PHONETIC_ORG, value);
    }

    /// <summary>
    /// The org of the contact.
    /// </summary>
    public virtual Organization? Organization
    {
        get => Properties.Get<Organization>(Constants.Contact.ORG);
        set => Properties.Set(Constants.Contact.ORG, value);
    }

    /// <summary>
    /// The birthdate of the contact.
    /// </summary>
    public virtual DateTime? Birthdate
    {
        get => Properties.Get<DateTime?>(Constants.Contact.BDAY);
        set => Properties.Set(Constants.Contact.BDAY, value);
    }

    /// <summary>
    /// Notes or comments of the contact.
    /// </summary>
    public virtual string? Notes
    {
        get => Properties.Get<string>(Constants.Contact.NOTE);
        set => Properties.Set(Constants.Contact.NOTE, value);
    }

    /// <summary>
    /// The organization or company of the contact.
    /// </summary>
    public virtual string? ShowAs
    {
        get => Properties.Get<string>(Constants.Contact.X_ABShowAs);
        set => Properties.Set(Constants.Contact.X_ABShowAs, value);
    }

    /// <summary>
    /// The photo of the contact.
    /// </summary>
    public virtual Photo? Photo
    {
        get => Properties.Get<Photo>(Constants.Contact.Photo.Property.PHOTO);
        set => Properties.Set(Constants.Contact.Photo.Property.PHOTO, value);
    }

    /// <summary>
    /// A collection of <see cref="Website" /> objects for the contact.
    /// </summary>
    /// <seealso cref="Website" />
    public virtual List<Website> Websites
    {
        get => Properties.GetMany<Website>(Constants.Contact.Website.Property.URL);
        set => Properties.Set(Constants.Contact.Website.Property.URL, value);
    }

    /// <summary>
    /// A collection of <see cref="Phone" /> objects for the contact.
    /// </summary>
    public virtual List<Phone> Phones
    {
        get => Properties.GetMany<Phone>(Constants.Contact.Phone.Property.TEL);
        set => Properties.Set(Constants.Contact.Phone.Property.TEL, value);
    }

    /// <summary>
    /// A collection of <see cref="Profile" /> objects for the contact.
    /// </summary>
    public virtual List<Profile> Profiles
    {
        get => Properties.GetMany<Profile>(Constants.Contact.Profile.Property.X_SOCIALPROFILE);
        set => Properties.Set(Constants.Contact.Profile.Property.X_SOCIALPROFILE, value);
    }

    /// <summary>
    /// A collection of <see cref="Address" /> objects for the contact.
    /// </summary>
    public virtual List<Address> Addresses
    {
        get => Properties.GetMany<Address>(Constants.Contact.Address.Property.ADR);
        set => Properties.Set(Constants.Contact.Address.Property.ADR, value);
    }

    /// <summary>
    /// A collection of <see cref="Data.RelatedPeople" /> objects for the contact.
    /// </summary>
    public virtual List<RelatedPeople> RelatedPeople
    {
        get => Properties.GetMany<RelatedPeople>(Constants.Contact.RelatedPerson.Property.X_ABRELATEDNAMES);
        set => Properties.Set(Constants.Contact.RelatedPerson.Property.X_ABRELATEDNAMES, value);
    }

    /// <summary>
    /// A collection of <see cref="Date" /> objects for the contact.
    /// </summary>
    public virtual List<Date> Dates
    {
        get => Properties.GetMany<Date>(Constants.Contact.Date.Property.X_ABDATE);
        set => Properties.Set(Constants.Contact.Date.Property.X_ABDATE, value);
    }

    /// <summary>
    /// A collection of <see cref="Email" /> objects for the contact.
    /// </summary>
    public virtual List<Email> EmailAddresses
    {
        get => Properties.GetMany<Email>(Constants.Contact.EmailAddress.Property.EMAIL);
        set => Properties.Set(Constants.Contact.EmailAddress.Property.EMAIL, value);
    }

    #endregion
}