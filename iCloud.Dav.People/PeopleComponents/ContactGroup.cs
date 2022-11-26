using iCloud.Dav.Core;
using iCloud.Dav.People.Serialization.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using vCard.Net;
using vCard.Net.CardComponents;
using vCard.Net.DataTypes;

namespace iCloud.Dav.People.PeopleComponents
{
    /// <inheritdoc/>
    [Serializable]
    [TypeConverter(typeof(ContactGroupConverter))]
    public class ContactGroup : CardComponent, IDirectResponseSchema
    {
        #region Fields/Consts

        private readonly List<string> _members = new List<string>();

        #endregion

        #region Properties

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
        public virtual string ProdId
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
        public virtual string N
        {
            get => Properties.Get<string>("N");
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
        /// The members of the card.
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
        public ContactGroup() => Name = Components.VCARD;

        #region Methods

        /// <summary>
        /// Adds member to the group.
        /// </summary>
        /// <param name="uniqueId"></param>
        /// <returns></returns>
        public bool AddMember(string uniqueId)
        {
            if (!_members.Contains(uniqueId))
            {
                _members.Add(uniqueId);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes member from the group.
        /// </summary>
        /// <param name="uniqueId"></param>
        /// <returns></returns>
        public bool RemoveMember(string uniqueId)
        {
            if (_members.Contains(uniqueId))
            {
                _members.Remove(uniqueId);
                return true;
            }
            return false;
        }

        #endregion
    }
}