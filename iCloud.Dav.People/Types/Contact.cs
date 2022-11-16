using iCloud.Dav.Core;
using iCloud.Dav.People.Serialization.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace iCloud.Dav.People.Types;

/// <summary>
///     A Person object for exchanging personal contact information.
/// </summary>
/// <remarks>
///     <para>
///         A Person contains personal information, such as postal
///         addresses, public security certificates, email addresses, and
///         web sites.  The Person specification makes it possible for
///         different computer programs to exchange personal contact
///         information; for example, a Person can be attached to an email or
///         sent over a wireless connection.
///     </para>
///     <para>
///         The standard Person format is a text file with properties in
///         name:value format.  However, there are multiple versions of
///         this format as well as compatible alternatives in XML and
///         HTML formats.  This class library aims to accomodate these
///         variations but be aware some some formats do not support
///         all possible properties.
///     </para>
/// </remarks>
[Serializable]
[TypeConverter(typeof(ContactConverter))]
public class Contact : IDirectResponseSchema
{
    #region Properties

    /// <summary>A value that uniquely identifies the Person.</summary>
    /// <remarks>
    ///     This value is optional.  The string must be any string
    ///     that can be used to uniquely identify the Person.  The
    ///     usage of the field is determined by the software.  Typical
    ///     possibilities for a unique string include a URL, a GUID,
    ///     or an LDAP directory path.  However, there is no particular
    ///     standard dictated by the Person specification.
    /// </remarks>
    public virtual string? UniqueId { get; set; }

    /// <summary>The job title of the person.</summary>
    /// <seealso cref="Organization" />4
    public virtual string? Title { get; set; }

    /// <summary>The e-tag of the person.</summary>
    /// <remarks>
    /// Will be set by the service deserialization method,
    /// or the by json response parser if implemented on service.
    /// </remarks>
    public virtual string? ETag { get; set; }

    /// <summary>Any additional (e.g. middle) names of the person.</summary>
    /// <seealso cref="LastName" />
    /// <seealso cref="FormattedName" />
    /// <seealso cref="FirstName" />
    /// <seealso cref="Nickname" />
    public virtual string? MiddleName { get; set; }

    /// <summary>The birthdate of the person.</summary>
    public virtual DateTime? BirthDate { get; set; }

    /// <summary>The department of the person in the organization.</summary>
    /// <seealso cref="Organization" />
    public virtual string? Department { get; set; }

    /// <summary>The family (last) name of the person.</summary>
    /// <seealso cref="MiddleName" />
    /// <seealso cref="FormattedName" />
    /// <seealso cref="FirstName" />
    /// <seealso cref="Nickname" />
    public virtual string? LastName { get; set; }

    /// <summary>The formatted name of the person.</summary>
    /// <remarks>
    ///     This property allows the name of the person to be
    ///     written in a manner specific to his or her culture.
    ///     The formatted name is not required to strictly
    ///     correspond with the family name, given name, etc.
    /// </remarks>
    /// <seealso cref="MiddleName" />
    /// <seealso cref="LastName" />
    /// <seealso cref="FirstName" />
    /// <seealso cref="Nickname" />
    public virtual string? FormattedName { get; set; }

    /// <summary>The given (first) name of the person.</summary>
    /// <seealso cref="MiddleName" />
    /// <seealso cref="LastName" />
    /// <seealso cref="FormattedName" />
    /// <seealso cref="Nickname" />
    public virtual string? FirstName { get; set; }

    /// <summary>Notes or comments.</summary>
    public virtual string? Notes { get; set; }

    public string? PhoneticFirstName { get; set; }

    public string? PhoneticLastName { get; set; }

    /// <summary>A collection of nicknames for the person.</summary>
    /// <seealso cref="MiddleName" />
    /// <seealso cref="LastName" />
    /// <seealso cref="FormattedName" />
    /// <seealso cref="FirstName" />
    public virtual string? Nickname { get; set; }

    /// <summary>The prefix (e.g. "Mr.") of the person.</summary>
    /// <seealso cref="NameSuffix" />
    public virtual string? NamePrefix { get; set; }

    /// <summary>The suffix (e.g. "Jr.") of the person.</summary>
    /// <seealso cref="NamePrefix" />
    public virtual string? NameSuffix { get; set; }

    /// <summary>The organization or company of the person.</summary>
    /// <seealso cref="Title" />
    public virtual string? Organization { get; set; }

    /// <summary>The name of the product that generated the Person.</summary>
    public virtual string? ProductId { get; set; }

    public string? PhoneticOrganization { get; set; }

    /// <summary>The revision date of the Person.</summary>
    /// <remarks>
    ///     The revision date is not automatically updated by the
    ///     Person when modifying properties.  It is up to the
    ///     developer to change the revision date as needed.
    /// </remarks>
    public virtual DateTime? RevisionDate { get; set; }

    /// <summary>
    ///     The photo of the Person.
    /// </summary>
    public virtual Photo? Photo { get; set; }

    /// <summary>Web sites associated with the person.</summary>
    /// <seealso cref="Website" />
    public virtual List<Website> Websites { get; }

    /// <summary>A collection of telephone numbers.</summary>
    public virtual List<Phone> Phones { get; }

    /// <summary>A collection of telephone numbers.</summary>
    public virtual List<Profile> Profiles { get; }

    /// <summary>Addresses associated with the person.</summary>
    public virtual List<Address> Addresses { get; }

    /// <summary>Related names.</summary>
    public virtual List<RelatedPeople> RelatedPeople { get; }

    /// <summary>The dates.</summary>
    public virtual List<Date> Dates { get; }

    /// <summary>
    ///     A collection of <see cref="Email" /> objects for the person.
    /// </summary>
    /// <seealso cref="Email" />
    public virtual List<Email> EmailAddresses { get; }

    #endregion

    /// <summary>
    ///     Initializes a new instance of the <see cref="Contact" /> class.
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