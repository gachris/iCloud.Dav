using iCloud.Dav.People.DataTypes;
using iCloud.Dav.People.Utils;
using System;
using System.ComponentModel;
using System.Globalization;

namespace iCloud.Dav.People.Serialization.Converters
{
    internal sealed class ContactConverter : TypeConverter
    {
        /// <inheritdoc/>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => sourceType == typeof(string);

        /// <inheritdoc/>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (!CanConvertFrom(context, value.GetType())) throw GetConvertFromException(value);
            return ((string)value).Deserialize<Contact>();
        }
    }
}