using iCloud.Dav.People.DataTypes;
using System;
using System.IO;
using vCard.Net.Serialization;

namespace iCloud.Dav.People.Serialization.DataTypes
{
    public class InstantMessageSerializer : vCard.Net.Serialization.DataTypes.EncodableDataTypeSerializer
    {
        public InstantMessageSerializer() : base()
        {
        }

        public InstantMessageSerializer(SerializationContext ctx) : base(ctx)
        {
        }

        public override Type TargetType => typeof(InstantMessage);

        public override object Deserialize(TextReader tr) => new InstantMessage();

        public override string SerializeToString(object obj) => string.Empty;
    }
}