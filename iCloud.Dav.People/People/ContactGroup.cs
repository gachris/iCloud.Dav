using iCloud.Dav.Core.Services;
using iCloud.Dav.People.Converters;
using iCloud.Dav.People.Serializer;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using vCards;

namespace iCloud.Dav.People
{
    [TypeConverter(typeof(ContactGroupConverter))]
    public class ContactGroup : vCard, IDirectResponseSchema
    {
        public virtual string ETag { get; set; }

        public virtual string GroupType { get; internal set; }

        public virtual int? MemberCount => MemberResourceNames.Count;

        public virtual IList<string> MemberResourceNames { get; }

        public virtual string ResourceName { get; set; }

        public virtual string Url { get; internal set; }

        public ContactGroup() : base()
        {
            GroupType = "group";
            MemberResourceNames = new List<string>();
        }

        public ContactGroup(TextReader input) : this()
        {
            var standardReader = new CardStandardReader();
            standardReader.ReadInto(this, input);
        }

        public ContactGroup(byte[] bytes) : this()
        {
            var standardReader = new CardStandardReader();
            standardReader.ReadInto(this, new StringReader(Encoding.UTF8.GetString(bytes)));
        }

        public ContactGroup(string content) : this()
        {
            var standardReader = new CardStandardReader();
            standardReader.ReadInto(this, new StringReader(content));
        }

        public virtual bool AddMemberResource(string uniqueId)
        {
            if (!MemberResourceNames.Contains(uniqueId))
            {
                MemberResourceNames.Add(uniqueId);
                return true;
            }
            return false;
        }

        public virtual bool RemoveMemberResource(string uniqueId)
        {
            if (MemberResourceNames.Contains(uniqueId))
            {
                MemberResourceNames.Remove(uniqueId);
                return true;
            }
            return false;
        }
    }
}
