using iCloud.Dav.People.DataTypes;
using System;
using System.Linq;
using System.Text;
using vCard.Net.CardComponents;
using vCard.Net.Serialization;

namespace iCloud.Dav.People.Serialization
{
    public class ContactGroupSerializer : ComponentSerializer
    {
        public ContactGroupSerializer()
        {
            SetService(new ContactGroupDataTypeMapper());
            SetService(new ExtendedSerializerFactory());
        }

        public ContactGroupSerializer(SerializationContext ctx) : base(ctx)
        {
            SetService(new ContactGroupDataTypeMapper());
            SetService(new ExtendedSerializerFactory());
        }

        public override Type TargetType => typeof(ContactGroup);

        public override string SerializeToString(object obj)
        {
            if (!(obj is ICardComponent c))
            {
                return null;
            }

            var sb = new StringBuilder();
            var upperName = c.Name.ToUpperInvariant();
            sb.Append(TextUtil.FoldLines($"BEGIN:{upperName}"));
            sb.Append(TextUtil.FoldLines($"VERSION:3.0"));

            // Get a serializer factory
            var sf = GetService<ISerializerFactory>();

            // Sort the vCard properties in alphabetical order before serializing them!
            var properties = c.Properties.OrderBy(p => p.Name).ToList();

            // Serialize properties
            foreach (var p in properties)
            {
                // Get a serializer for each property.
                var serializer = sf.Build(p.GetType(), SerializationContext) as IStringSerializer;
                sb.Append(serializer.SerializeToString(p));
            }

            // Serialize child objects
            foreach (var child in c.Children)
            {
                // Get a serializer for each child object.
                var serializer = sf.Build(child.GetType(), SerializationContext) as IStringSerializer;
                sb.Append(serializer.SerializeToString(child));
            }

            sb.Append(TextUtil.FoldLines($"END:{upperName}"));
            return sb.ToString();
        }
    }
}