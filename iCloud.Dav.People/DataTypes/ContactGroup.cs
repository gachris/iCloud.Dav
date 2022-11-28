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
    [TypeConverter(typeof(ContactGroupConverter))]
    public class ContactGroup : UniqueComponent, IDirectResponseSchema, IUrlPath
    {
        /// <inheritdoc/>
        public virtual string ETag { get; set; }

        public virtual string Id { get; set; }

        #region Properties

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
        ///     vCard when modifying properties. It is up to the
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
        public virtual string N
        {
            get => Properties.Get<string>("N");
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
        /// The members of the vCard.
        /// </summary>
        public virtual IList<string> Members
        {
            get => Properties.GetMany<string>("X-ADDRESSBOOKSERVER-MEMBER");
            set => Properties.Set("X-ADDRESSBOOKSERVER-MEMBER", value);
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactGroup" /> class.
        /// </summary>
        public ContactGroup()
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
            if (string.IsNullOrEmpty(Uid))
            {
                // Create a new UID for the component
                Id = Uid = Guid.NewGuid().ToString();
            }

            if (Kind == null)
            {
                Kind = new Kind() { CardKind = CardKind.Group };
            }
        }
    }
}