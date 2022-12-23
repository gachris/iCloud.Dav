using iCloud.Dav.People.DataTypes;
using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace iCloud.Dav.People.Serialization.Converters
{
    internal sealed class ContactGroupConverter : TypeConverter
    {
        /// <inheritdoc/>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => sourceType == typeof(string);

        /// <inheritdoc/>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return !CanConvertFrom(context, value.GetType())
                ? throw GetConvertFromException(value)
                : DeserializeContactGroup((string)value);
        }

        private static ContactGroup DeserializeContactGroup(string data)
        {
            if (!data.Contains("X-ADDRESSBOOKSERVER-KIND:group")) return null;

            var bytes = Encoding.UTF8.GetBytes(data);
            using (var stream = new MemoryStream(bytes))
            {
                return ContactGroupDeserializer.Default.Deserialize(new StreamReader(stream, Encoding.UTF8)).First();
            }
        }
    }
}