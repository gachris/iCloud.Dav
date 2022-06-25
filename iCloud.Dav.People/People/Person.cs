using iCloud.Dav.Core.Services;
using iCloud.Dav.People.Converters;
using iCloud.Dav.People.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace iCloud.Dav.People
{
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
    [TypeConverter(typeof(PersonConverter))]
    public class Person : IDirectResponseSchema
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
        public virtual string UniqueId { get; set; }

        /// <summary>The job title of the person.</summary>
        /// <seealso cref="Organization" />
        /// <seealso cref="Role" />
        public virtual string Title { get; set; }

        /// <summary>
        ///     A string identifying the time zone of the entity
        ///     represented by the Person.
        /// </summary>
        public virtual string TimeZone { get; set; }

        /// <summary>The e-tag of the person.</summary>
        /// <remarks>
        /// Will be set by the service deserialization method,
        /// or the by json response parser if implemented on service.
        /// </remarks>
        public virtual string ETag { get; set; }

        /// <summary>
        ///     The security access classification of the Person owner (e.g. private).
        /// </summary>
        public AccessClassification AccessClassification { get; set; }

        /// <summary>Any additional (e.g. middle) names of the person.</summary>
        /// <seealso cref="FamilyName" />
        /// <seealso cref="FormattedName" />
        /// <seealso cref="GivenName" />
        /// <seealso cref="Nicknames" />
        public virtual string AdditionalNames { get; set; }

        /// <summary>The birthdate of the person.</summary>
        public virtual DateTime? BirthDate { get; set; }

        /// <summary>The department of the person in the organization.</summary>
        /// <seealso cref="Office" />
        /// <seealso cref="Organization" />
        public virtual string Department { get; set; }

        /// <summary>The display name of the Person.</summary>
        /// <remarks>
        ///     This property is used by Person applications for titles,
        ///     headers, and other visual elements.
        /// </remarks>
        public virtual string DisplayName { get; set; }

        /// <summary>The family (last) name of the person.</summary>
        /// <seealso cref="AdditionalNames" />
        /// <seealso cref="FormattedName" />
        /// <seealso cref="GivenName" />
        /// <seealso cref="Nicknames" />
        public virtual string FamilyName { get; set; }

        /// <summary>The formatted name of the person.</summary>
        /// <remarks>
        ///     This property allows the name of the person to be
        ///     written in a manner specific to his or her culture.
        ///     The formatted name is not required to strictly
        ///     correspond with the family name, given name, etc.
        /// </remarks>
        /// <seealso cref="AdditionalNames" />
        /// <seealso cref="FamilyName" />
        /// <seealso cref="GivenName" />
        /// <seealso cref="Nicknames" />
        public virtual string FormattedName { get; set; }

        /// <summary>The gender of the person.</summary>
        /// <remarks>
        ///     The Person specification does not define a property
        ///     to indicate the gender of the contact.  Microsoft
        ///     Outlook implements it as a custom property named
        ///     X-WAB-GENDER.
        /// </remarks>
        /// <seealso cref="Gender" />
        public virtual Gender Gender { get; set; }

        /// <summary>The given (first) name of the person.</summary>
        /// <seealso cref="AdditionalNames" />
        /// <seealso cref="FamilyName" />
        /// <seealso cref="FormattedName" />
        /// <seealso cref="Nicknames" />
        public virtual string GivenName { get; set; }

        /// <summary>The latitude of the person in decimal degrees.</summary>
        /// <seealso cref="Longitude" />
        public virtual float? Latitude { get; set; }

        /// <summary>The longitude of the person in decimal degrees.</summary>
        /// <seealso cref="Latitude" />
        public virtual float? Longitude { get; set; }

        /// <summary>The mail software used by the person.</summary>
        public virtual string Mailer { get; set; }

        /// <summary>The prefix (e.g. "Mr.") of the person.</summary>
        /// <seealso cref="NameSuffix" />
        public virtual string NamePrefix { get; set; }

        /// <summary>The suffix (e.g. "Jr.") of the person.</summary>
        /// <seealso cref="NamePrefix" />
        public virtual string NameSuffix { get; set; }

        /// <summary>The office of the person at the organization.</summary>
        /// <seealso cref="Department" />
        /// <seealso cref="Organization" />
        public virtual string Office { get; set; }

        /// <summary>The organization or company of the person.</summary>
        /// <seealso cref="Office" />
        /// <seealso cref="Role" />
        /// <seealso cref="Title" />
        public virtual string Organization { get; set; }

        /// <summary>The name of the product that generated the Person.</summary>
        public virtual string ProductId { get; set; }

        /// <summary>The revision date of the Person.</summary>
        /// <remarks>
        ///     The revision date is not automatically updated by the
        ///     Person when modifying properties.  It is up to the
        ///     developer to change the revision date as needed.
        /// </remarks>
        public virtual DateTime? RevisionDate { get; set; }

        /// <summary>The role of the person (e.g. Executive).</summary>
        /// <remarks>
        ///     The role is shown as "Profession" in Microsoft Outlook.
        /// </remarks>
        /// <seealso cref="Department" />
        /// <seealso cref="Office" />
        /// <seealso cref="Organization" />
        /// <seealso cref="Title" />
        public virtual string Role { get; set; }

        /// <summary>Directory sources for the Person information.</summary>
        /// <remarks>
        ///     A Person may contain zero or more sources.  A source
        ///     identifies a directory that contains (or provided)
        ///     information found in the Person.  A program can
        ///     hypothetically connect to the source in order to
        ///     obtain updated information.
        /// </remarks>
        public virtual IList<Source> Sources { get; }

        /// <summary>Web sites associated with the person.</summary>
        /// <seealso cref="Website" />
        public virtual IList<Website> Websites { get; }

        /// <summary>A collection of telephone numbers.</summary>
        public virtual IList<Phone> Phones { get; }

        /// <summary>
        ///     A collection of photographic images embedded or
        ///     referenced by the Person.
        /// </summary>
        public virtual IList<Photo> Photos { get; }

        /// <summary>A collection of nicknames for the person.</summary>
        /// <seealso cref="AdditionalNames" />
        /// <seealso cref="FamilyName" />
        /// <seealso cref="FormattedName" />
        /// <seealso cref="GivenName" />
        public virtual StringCollection Nicknames { get; }

        /// <summary>Categories of the Person.</summary>
        /// <remarks>
        ///     This property is a collection of strings containing
        ///     keywords or category names.
        /// </remarks>
        public virtual StringCollection Categories { get; }

        /// <summary>Public key certificates attached to the Person.</summary>
        /// <seealso cref="Certificate" />
        public virtual IList<Certificate> Certificates { get; }

        /// <summary>Addresses associated with the person.</summary>
        public virtual IList<Address> Addresses { get; }

        /// <summary>Formatted delivery labels.</summary>
        public virtual IList<Label> Labels { get; }

        /// <summary>A collection of notes or comments.</summary>
        public virtual IList<Note> Notes { get; }

        /// <summary>
        ///     A collection of <see cref="EmailAddress" /> objects for the person.
        /// </summary>
        /// <seealso cref="EmailAddress" />
        public virtual IList<EmailAddress> EmailAddresses { get; }

        /// <summary>The Memberships of the person.</summary>
        public virtual IReadOnlyCollection<Membership> Memberships { get; internal set; }

        #endregion

        /// <summary>
        ///     Initializes a new instance of the <see cref="Person" /> class.
        /// </summary>
        public Person()
        {
            Categories = new StringCollection();
            Certificates = new List<Certificate>();
            Addresses = new List<Address>();
            Labels = new List<Label>();
            EmailAddresses = new List<EmailAddress>();
            Nicknames = new StringCollection();
            Notes = new List<Note>();
            Phones = new List<Phone>();
            Photos = new List<Photo>();
            Sources = new List<Source>();
            Websites = new List<Website>();
            Memberships = new ReadOnlyCollection<Membership>(new List<Membership>());
        }

        /// <summary>
        ///     Loads a new instance of the <see cref="Person" /> class
        ///     from a text reader.
        /// </summary>
        /// <param name="input">An initialized text reader.</param>
        public Person(TextReader input) : this()
        {
            new CardStandardReader().ReadInto(this, input);
        }

        /// <summary>
        ///     Loads a new instance of the <see cref="Person" /> class
        ///     from a byte array.
        /// </summary>
        /// <param name="bytes">An initialized byte array.</param>
        public Person(byte[] bytes) : this()
        {
            new CardStandardReader().ReadInto(this, new StringReader(Encoding.UTF8.GetString(bytes)));
        }

        /// <summary>
        ///     Loads a new instance of the <see cref="Person" /> class
        ///     from a text file.
        /// </summary>
        /// <param name="path">
        ///     The path to a text file containing Person data in
        ///     any recognized Person format.
        /// </param>
        public Person(string path) : this()
        {
            using var streamReader = new StreamReader(path);
            new CardStandardReader().ReadInto(this, streamReader);
        }
    }
}