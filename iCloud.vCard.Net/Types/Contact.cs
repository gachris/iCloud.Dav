using System;
using System.Collections.Generic;

namespace iCloud.vCard.Net.Types;

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
public class Contact : CardComponent
{
    #region Properties

    /// <summary>
    /// The job title of the contact.
    /// </summary>
    public virtual string? Title { get; set; }

    /// <summary>
    /// Any additional (e.g. middle) names of the contact.
    /// </summary>
    public virtual string? MiddleName { get; set; }

    /// <summary>
    /// The birthdate of the contact.
    /// </summary>
    public virtual DateTime? Birthdate { get; set; }

    /// <summary>
    /// The department of the contact in the organization.
    /// </summary>
    public virtual string? Department { get; set; }

    /// <summary>
    /// The last name of the contact.
    /// </summary>
    public virtual string? LastName { get; set; }

    /// <summary>
    /// The formatted name of the contact.
    /// </summary>
    /// <remarks>
    ///     This property allows the name of the contact to be
    ///     written in a manner specific to his or her culture.
    ///     The formatted name is not required to strictly
    ///     correspond with the family name, given name, etc.
    /// </remarks>
    public virtual string? FormattedName { get; set; }

    /// <summary>
    /// The given (first) name of the contact.
    /// </summary>
    public virtual string? FirstName { get; set; }

    /// <summary>
    /// Notes or comments of the contact.
    /// </summary>
    public virtual string? Notes { get; set; }

    /// <summary>
    /// The phonetic first name of the contact.
    /// </summary>
    public string? PhoneticFirstName { get; set; }

    /// <summary>
    /// The phonetic last name of the contact.
    /// </summary>
    public string? PhoneticLastName { get; set; }

    /// <summary>
    /// The nickname of the contact.
    /// </summary>
    public virtual string? Nickname { get; set; }

    /// <summary>
    /// The prefix (e.g. "Mr.") of the contact.
    /// </summary>
    public virtual string? NamePrefix { get; set; }

    /// <summary>
    /// The suffix (e.g. "Jr.") of the contact.
    /// </summary>
    public virtual string? NameSuffix { get; set; }

    /// <summary>
    /// The organization or company of the contact.
    /// </summary>
    public virtual string? Organization { get; set; }

    /// <summary>
    /// The phonetic organization of the contact.
    /// </summary>
    public string? PhoneticOrganization { get; set; }

    /// <summary>
    /// The photo of the contact.
    /// </summary>
    public virtual Photo? Photo { get; set; }

    /// <summary>
    /// A collection of <see cref="Website" /> objects for the contact.
    /// </summary>
    /// <seealso cref="Website" />
    public virtual List<Website> Websites { get; }

    /// <summary>
    /// A collection of <see cref="Phone" /> objects for the contact.
    /// </summary>
    public virtual List<Phone> Phones { get; }

    /// <summary>
    /// A collection of <see cref="Profile" /> objects for the contact.
    /// </summary>
    public virtual List<Profile> Profiles { get; }

    /// <summary>
    /// A collection of <see cref="Address" /> objects for the contact.
    /// </summary>
    public virtual List<Address> Addresses { get; }

    /// <summary>
    /// A collection of <see cref="Types.RelatedPeople" /> objects for the contact.
    /// </summary>
    public virtual List<RelatedPeople> RelatedPeople { get; }

    /// <summary>
    /// A collection of <see cref="Date" /> objects for the contact.
    /// </summary>
    public virtual List<Date> Dates { get; }

    /// <summary>
    /// A collection of <see cref="Email" /> objects for the contact.
    /// </summary>
    public virtual List<Email> EmailAddresses { get; }

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="Contact" /> class.
    /// </summary>
    public Contact()
    {
        Addresses = new List<Address>();
        EmailAddresses = new List<Email>();
        Phones = new List<Phone>();
        Websites = new List<Website>();
        Profiles = new List<Profile>();
        RelatedPeople = new List<RelatedPeople>();
        Dates = new List<Date>();
    }
}