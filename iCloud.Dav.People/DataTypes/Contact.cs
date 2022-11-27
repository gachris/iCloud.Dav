using iCloud.Dav.People.Serialization.Converters;
using System.Collections.Generic;
using System.ComponentModel;
using vCard.Net;
using vCard.Net.DataTypes;

namespace iCloud.Dav.People.DataTypes
{
    /// <inheritdoc/>
    [TypeConverter(typeof(ContactConverter))]
    public class Contact : CloudComponent
    {
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
        public virtual IDateTime RevisionDate
        {
            get => Properties.Get<IDateTime>("REV");
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
        public virtual Name N
        {
            get => Properties.Get<Name>("N");
            set => Properties.Set("N", value);
        }

        /// <summary>
        /// The kind of the card.
        /// </summary>
        public Kind Kind
        {
            get => Properties.Get<Kind>("X-ADDRESSBOOKSERVER-KIND");
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
        public virtual Organization Organization
        {
            get => Properties.Get<Organization>("ORG");
            set => Properties.Set("ORG", value);
        }

        /// <summary>
        /// The birthdate of the card.
        /// </summary>
        public virtual IDateTime Birthdate
        {
            get => Properties.Get<IDateTime>("BDAY");
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