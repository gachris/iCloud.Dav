using iCloud.Dav.People.PeopleComponents;
using System;
using vCard.Net.Serialization;

namespace iCloud.Dav.People.Serialization
{
    public class ContactSerializer : ComponentSerializer
    {
        public ContactSerializer()
        {
            SetService(new ExtendedDataTypeMapper());
            SetService(new ExtendedSerializerFactory());
        }

        public ContactSerializer(SerializationContext ctx) : base(ctx)
        {
            SetService(new ExtendedDataTypeMapper());
            SetService(new ExtendedSerializerFactory());
        }

        public override Type TargetType => typeof(Contact);
    }
}