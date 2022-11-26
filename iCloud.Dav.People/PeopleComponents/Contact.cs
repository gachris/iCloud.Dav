using iCloud.Dav.Core;
using iCloud.Dav.People.DataTypes;
using iCloud.Dav.People.Serialization.Converters;
using System.Collections.Generic;
using System.ComponentModel;
using vCard.Net;
using vCard.Net.CardComponents;

namespace iCloud.Dav.People.PeopleComponents
{
    /// <inheritdoc/>
    [TypeConverter(typeof(ContactConverter))]
    public class Contact : CardComponent, IDirectResponseSchema
    {
        // TODO: Add AddRelationship() public method.
        // This method will add the UID of a related component
        // to the Related_To property, along with any "RELTYPE"
        // parameter ("PARENT", "CHILD", "SIBLING", or other)
        // TODO: Add RemoveRelationship() public method.        

        /// <inheritdoc/>
        public string ETag { get; set; }

        /// <summary>
        /// A value that uniquely identifies the card.
        /// </summary>
        /// <remarks>
        ///     This value is optional.  The string must be any string
        ///     that can be used to uniquely identify the Person.  The
        ///     usage of the field is determined by the software.  Typical
        ///     possibilities for a unique string include a URL, a GUID,
        ///     or an LDAP directory path.  However, there is no particular
        ///     standard dictated by the Person specification.
        /// </remarks>
        public virtual string Uid
        {
            get => Properties.Get<string>("UID");
            set => Properties.Set("UID", value);
        }

        /// <summary>
        /// The name of the product that generated the card.
        /// </summary>
        public virtual string ProductId
        {
            get => Properties.Get<string>("PRODID");
            set => Properties.Set("PRODID", value);
        }

        /// <summary>
        /// The revision date of the card.
        /// </summary>
        /// <remarks>
        ///     The revision date is not automatically updated by the
        ///     card when modifying properties. It is up to the
        ///     developer to change the revision date as needed.
        /// </remarks>
        public virtual vCard.Net.DataTypes.IDateTime RevisionDate
        {
            get => Properties.Get<vCard.Net.DataTypes.IDateTime>("REV");
            set => Properties.Set("REV", value);
        }

        /// <summary>
        /// The formatted name of the card.
        /// </summary>
        /// <remarks>
        ///     This property allows the name of the card to be
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
        /// The name of the card.
        /// </summary>
        public virtual vCard.Net.DataTypes.Name N
        {
            get => Properties.Get<vCard.Net.DataTypes.Name>("N");
            set => Properties.Set("N", value);
        }

        /// <summary>
        /// The kind of the card.
        /// </summary>
        public string Kind
        {
            get => Properties.Get<string>("X-ADDRESSBOOKSERVER-KIND");
            set => Properties.Set("X-ADDRESSBOOKSERVER-KIND", value);
        }

        /// <summary>
        /// The phonetic last name of the card.
        /// </summary>
        public virtual string PhoneticLastName
        {
            get => Properties.Get<string>("X-PHONETIC-LAST-NAME");
            set => Properties.Set("X-PHONETIC-LAST-NAME", value);
        }

        /// <summary>
        /// The phonetic first name of the card.
        /// </summary>
        public virtual string PhoneticFirstName
        {
            get => Properties.Get<string>("X-PHONETIC-FIRST-NAME");
            set => Properties.Set("X-PHONETIC-FIRST-NAME", value);
        }

        /// <summary>
        /// The nickname of the card.
        /// </summary>
        public virtual string Nickname
        {
            get => Properties.Get<string>("NICKNAME");
            set => Properties.Set("NICKNAME", value);
        }

        /// <summary>
        /// The job title of the card.
        /// </summary>
        public virtual string Title
        {
            get => Properties.Get<string>("TITLE");
            set => Properties.Set("TITLE", value);
        }

        /// <summary>
        /// The phonetic ORG of the card.
        /// </summary>
        public virtual string PhoneticORG
        {
            get => Properties.Get<string>("X-PHONETIC-ORG");
            set => Properties.Set("X-PHONETIC-ORG", value);
        }

        /// <summary>
        /// The ORG of the card.
        /// </summary>
        public virtual string ORG
        {
            get => Properties.Get<string>("ORG");
            set => Properties.Set("ORG", value);
        }

        /// <summary>
        /// The birthdate of the card.
        /// </summary>
        public virtual string Birthdate
        {
            get => Properties.Get<string>("BDAY");
            set => Properties.Set("BDAY", value);
        }

        /// <summary>
        /// Notes or comments of the card.
        /// </summary>
        public virtual string Notes
        {
            get => Properties.Get<string>("NOTE");
            set => Properties.Set("NOTE", value);
        }

        /// <summary>
        /// The organization or company of the card.
        /// </summary>
        public virtual string ShowAs
        {
            get => Properties.Get<string>("X-ABShowAs");
            set => Properties.Set("X-ABShowAs", value);
        }

        /// <summary>
        /// The photo of the card.
        /// </summary>
        public virtual Photo Photo
        {
            get => Properties.Get<Photo>("PHOTO");
            set => Properties.Set("PHOTO", value);
        }

        /// <summary>
        /// A collection of <see cref="Website" /> objects for the card.
        /// </summary>
        /// <seealso cref="Website" />
        public virtual IList<Website> Websites
        {
            get => Properties.GetMany<Website>("URL");
            set => Properties.Set("URL", value);
        }

        /// <summary>
        /// A collection of <see cref="Phone" /> objects for the card.
        /// </summary>
        public virtual IList<Phone> Phones
        {
            get => Properties.GetMany<Phone>("TEL");
            set => Properties.Set("TEL", value);
        }

        /// <summary>
        /// A collection of <see cref="X_SocialProfile" /> objects for the card.
        /// </summary>
        public virtual IList<X_SocialProfile> Profiles
        {
            get => Properties.GetMany<X_SocialProfile>("X-SOCIALPROFILE");
            set => Properties.Set("X-SOCIALPROFILE", value);
        }

        /// <summary>
        /// A collection of <see cref="Address" /> objects for the card.
        /// </summary>
        public virtual IList<Address> Addresses
        {
            get => Properties.GetMany<Address>("ADR");
            set => Properties.Set("ADR", value);
        }

        /// <summary>
        /// A collection of <see cref="X_ABRelatedNames" /> objects for the card.
        /// </summary>
        public virtual IList<X_ABRelatedNames> RelatedPeople
        {
            get => Properties.GetMany<X_ABRelatedNames>("X-ABRELATEDNAMES");
            set => Properties.Set("X-ABRELATEDNAMES", value);
        }

        /// <summary>
        /// A collection of <see cref="X_ABDate" /> objects for the card.
        /// </summary>
        public virtual IList<X_ABDate> Dates
        {
            get => Properties.GetMany<X_ABDate>("X-ABDATE");
            set => Properties.Set("X-ABDATE", value);
        }

        /// <summary>
        /// A collection of <see cref="Email" /> objects for the card.
        /// </summary>
        public virtual IList<Email> EmailAddresses
        {
            get => Properties.GetMany<Email>("EMAIL");
            set => Properties.Set("EMAIL", value);
        }

        /// <summary>
        /// A collection of <see cref="InstantMessage" /> objects for the card.
        /// </summary>
        public virtual IList<InstantMessage> InstantMessageCollection
        {
            get => Properties.GetMany<InstantMessage>("IMPP");
            set => Properties.Set("IMPP", value);
        }

        public Contact() => Name = Components.VCARD;
    }
}