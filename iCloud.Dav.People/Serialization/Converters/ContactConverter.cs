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
            return !CanConvertFrom(context, value.GetType()) ? throw GetConvertFromException(value) : (object)((string)value).DeserializeContact();
        }
    }
}