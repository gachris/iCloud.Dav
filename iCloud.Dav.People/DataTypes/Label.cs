using iCloud.Dav.People.Serialization.DataTypes;
using System;
using System.IO;
using vCard.Net;
using vCard.Net.DataTypes;

namespace iCloud.Dav.People.DataTypes
{
    public class Label : EncodableDataType
    {
        public virtual string Value { get; set; }

        public Label()
        {
        }

        public Label(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            var serializer = new LabelSerializer();
            CopyFrom(serializer.Deserialize(new StringReader(value)) as ICopyable);
        }

        protected bool Equals(Label other)
        {
            return string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            return !(obj is null) && (ReferenceEquals(this, obj) || obj.GetType() == GetType() && Equals((Label)obj));
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Value.GetHashCode();
            }
        }
    }
}