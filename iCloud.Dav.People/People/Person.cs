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
    [TypeConverter(typeof(PersonConverter))]
    public class Person : vCard, IDirectResponseSchema
    {
        public Person() : base()
        {
            Addresses = new vCardAddressCollection();
            Memberships = new ReadOnlyMembershipCollection(new List<Membership>());
        }

        public Person(TextReader input) : this()
        {
            var standardReader = new CardStandardReader();
            standardReader.ReadInto(this, input);
        }

        public Person(byte[] bytes) : this()
        {
            var standardReader = new CardStandardReader();
            standardReader.ReadInto(this, new StringReader(Encoding.UTF8.GetString(bytes)));
        }

        public Person(string content) : this()
        {
            var standardReader = new CardStandardReader();
            standardReader.ReadInto(this, new StringReader(content));
        }

        public virtual string ETag { get; set; }

        public virtual ReadOnlyMembershipCollection Memberships { get; internal set; }

        public virtual vCardAddressCollection Addresses { get; }
    }
}