using iCloud.Dav.People.DataTypes;
using System;
using vCard.Net.Serialization;

namespace iCloud.Dav.People.Serialization
{
    public class ContactGroupSerializer : ComponentSerializer
    {
        public ContactGroupSerializer()
        {
            SetService(new ExtendedDataTypeMapper());
            SetService(new ExtendedSerializerFactory());
        }

        public ContactGroupSerializer(SerializationContext ctx) : base(ctx)
        {
            SetService(new ExtendedDataTypeMapper());
            SetService(new ExtendedSerializerFactory());
        }

        public override Type TargetType => typeof(ContactGroup);
    }
}