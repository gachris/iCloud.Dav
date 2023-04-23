using iCloud.Dav.Core;
using iCloud.Dav.People.Serialization.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using vCard.Net;
using vCard.Net.CardComponents;
using vCard.Net.DataTypes;

namespace iCloud.Dav.People.DataTypes
{
    /// <inheritdoc/>
    [TypeConverter(typeof(ContactConverter))]
    public class Contact : UniqueComponent, IDirectResponseSchema, IUrlPath
    {
        /// <inheritdoc/>
        public virtual string ETag { get; set; }

        /// <summary>
        /// A value that uniquely identifies the vCard. It is used for requests and in most cases has the same value as the <seealso cref="UniqueComponent.Uid"/>.
        /// </summary>
        /// <remarks>The initial value of Id is same as the <seealso cref="UniqueComponent.Uid"/></remarks>
        public virtual string Id { get; set; }

        /// <summary>
        /// The name of the product that generated the vCard.
        /// </summary>
        public virtual string ProductId
        {
            get => Properties.Get<string>("PRODID");
            set => Properties.Set("PRODID", value);
        }

        /// <summary>
        /// The revision date of the vCard.
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
        /// The formatted name of the vCard.
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
        /// The name of the vCard.
        /// </summary>
        public virtual Name N
        {
            get => Properties.Get<Name>("N");
            set => Properties.Set("N", value);
        }

        /// <summary>
        /// The kind of the vCard.
        /// </summary>
        public Kind Kind
        {
            get => Properties.Get<Kind>("X-ADDRESSBOOKSERVER-KIND");
            set => Properties.Set("X-ADDRESSBOOKSERVER-KIND", value);
        }

        /// <summary>
        /// The phonetic last name of the vCard.
        /// </summary>
        public virtual string PhoneticLastName
        {
            get => Properties.Get<string>("X-PHONETIC-LAST-NAME");
            set => Properties.Set("X-PHONETIC-LAST-NAME", value);
        }

        /// <summary>
        /// The phonetic first name of the vCard.
        /// </summary>
        public virtual string PhoneticFirstName
        {
            get => Properties.Get<string>("X-PHONETIC-FIRST-NAME");
            set => Properties.Set("X-PHONETIC-FIRST-NAME", value);
        }

        /// <summary>
        /// The nickname of the vCard.
        /// </summary>
        public virtual string Nickname
        {
            get => Properties.Get<string>("NICKNAME");
            set => Properties.Set("NICKNAME", value);
        }

        /// <summary>
        /// The job title of the vCard.
        /// </summary>
        public virtual string Title
        {
            get => Properties.Get<string>("TITLE");
            set => Properties.Set("TITLE", value);
        }

        /// <summary>
        /// The phonetic ORG of the vCard.
        /// </summary>
        public virtual string PhoneticOrganization
        {
            get => Properties.Get<string>("X-PHONETIC-ORG");
            set => Properties.Set("X-PHONETIC-ORG", value);
        }

        /// <summary>
        /// The ORG of the vCard.
        /// </summary>
        public virtual Organization Organization
        {
            get => Properties.Get<Organization>("ORG");
            set => Properties.Set("ORG", value);
        }

        /// <summary>
        /// The birthdate of the vCard.
        /// </summary>
        public virtual IDateTime Birthdate
        {
            get => Properties.Get<IDateTime>("BDAY");
            set => Properties.Set("BDAY", value);
        }

        /// <summary>
        /// Notes or comments of the vCard.
        /// </summary>
        public virtual string Notes
        {
            get => Properties.Get<string>("NOTE");
            set => Properties.Set("NOTE", value);
        }

        /// <summary>
        /// The organization or company of the vCard.
        /// </summary>
        public virtual string ShowAs
        {
            get => Properties.Get<string>("X-ABSHOWAS");
            set => Properties.Set("X-ABSHOWAS", value);
        }

        /// <summary>
        /// The photo of the vCard.
        /// </summary>
        public virtual Photo Photo
        {
            get => Properties.Get<Photo>("PHOTO");
            set => Properties.Set("PHOTO", value);
        }

        /// <summary>
        /// A collection of <see cref="Website" /> objects for the vCard.
        /// </summary>
        /// <seealso cref="Website" />
        public virtual IList<Website> Websites
        {
            get => Properties.GetMany<Website>("URL");
            set => Properties.Set("URL", value);
        }

        /// <summary>
        /// A collection of <see cref="Phone" /> objects for the vCard.
        /// </summary>
        public virtual IList<Phone> Telephones
        {
            get => Properties.GetMany<Phone>("TEL");
            set => Properties.Set("TEL", value);
        }

        /// <summary>
        /// A collection of <see cref="SocialProfile" /> objects for the vCard.
        /// </summary>
        public virtual IList<SocialProfile> SocialProfiles
        {
            get => Properties.GetMany<SocialProfile>("X-SOCIALPROFILE");
            set => Properties.Set("X-SOCIALPROFILE", value);
        }

        /// <summary>
        /// A collection of <see cref="Address" /> objects for the vCard.
        /// </summary>
        public virtual IList<Address> Addresses
        {
            get => Properties.GetMany<Address>("ADR");
            set => Properties.Set("ADR", value);
        }

        /// <summary>
        /// A collection of <see cref="DataTypes.RelatedNames" /> objects for the vCard.
        /// </summary>
        public virtual IList<RelatedNames> RelatedNames
        {
            get => Properties.GetMany<RelatedNames>("X-ABRELATEDNAMES");
            set => Properties.Set("X-ABRELATEDNAMES", value);
        }

        /// <summary>
        /// A collection of <see cref="Date" /> objects for the vCard.
        /// </summary>
        public virtual IList<Date> Dates
        {
            get => Properties.GetMany<Date>("X-ABDATE");
            set => Properties.Set("X-ABDATE", value);
        }

        /// <summary>
        /// A collection of <see cref="Email" /> objects for the vCard.
        /// </summary>
        public virtual IList<Email> Emails
        {
            get => Properties.GetMany<Email>("EMAIL");
            set => Properties.Set("EMAIL", value);
        }

        /// <summary>
        /// A collection of <see cref="InstantMessage" /> objects for the vCard.
        /// </summary>
        public virtual IList<InstantMessage> InstantMessages
        {
            get => Properties.GetMany<InstantMessage>("IMPP");
            set => Properties.Set("IMPP", value);
        }

        public Contact()
        {
            Name = Components.VCARD;
            EnsureProperties();
        }

        protected override void OnDeserialized(StreamingContext context)
        {
            base.OnDeserialized(context);

            EnsureProperties();
        }

        private void EnsureProperties()
        {
            var id = Guid.NewGuid().ToString();
            if (string.IsNullOrEmpty(Id))
            {
                // Create a new UID for the component
                Id = id;
            }
            if (string.IsNullOrEmpty(Uid))
            {
                // Create a new UID for the component
                Uid = id;
            }
        }
    }
}