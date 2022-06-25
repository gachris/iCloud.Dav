using iCloud.Dav.Core.Services;
using iCloud.Dav.People.Converters;
using iCloud.Dav.People.Utils;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace iCloud.Dav.People
{
    [TypeConverter(typeof(ContactGroupConverter))]
    public class ContactGroup : Person, IDirectResponseSchema
    {
        #region Properties

        /// <summary>
        /// The group type of the ContactGroup.
        /// </summary>
        public virtual string GroupType { get; internal set; }

        /// <summary>
        /// The member resource names of the ContactGroup.
        /// </summary>
        public virtual IList<string> MemberResourceNames { get; }

        /// <summary>
        /// The resource name of ContactGroup.
        /// </summary>
        public virtual string ResourceName { get; set; }

        /// <summary>
        /// The Url of the ContactGroup.
        /// </summary>
        public virtual string Url { get; internal set; }

        /// <summary>
        /// The count of members.
        /// </summary>
        public virtual int MemberCount => MemberResourceNames.Count;

        #endregion

        /// <summary>
        ///     Initializes a new instance of the <see cref="ContactGroup" /> class.
        /// </summary>
        public ContactGroup() : base()
        {
            GroupType = "group";
            MemberResourceNames = new List<string>();
        }

        /// <summary>
        ///     Loads a new instance of the <see cref="ContactGroup" /> class
        ///     from a byte array.
        /// </summary>
        /// <param name="bytes">An initialized byte array.</param>
        public ContactGroup(byte[] bytes) : this()
        {
            var standardReader = new CardStandardReader();
            standardReader.ReadInto(this, new StringReader(Encoding.UTF8.GetString(bytes)));
        }

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uniqueId"></param>
        /// <returns></returns>
        public virtual bool AddMemberResource(string uniqueId)
        {
            if (!MemberResourceNames.Contains(uniqueId))
            {
                MemberResourceNames.Add(uniqueId);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uniqueId"></param>
        /// <returns></returns>
        public virtual bool RemoveMemberResource(string uniqueId)
        {
            if (MemberResourceNames.Contains(uniqueId))
            {
                MemberResourceNames.Remove(uniqueId);
                return true;
            }
            return false;
        }

        #endregion
    }
}
