using iCloud.Dav.Calendar.CalDav.Types;
using iCloud.Dav.Calendar.Utils;
using System;
using System.ComponentModel;
using System.Globalization;

namespace iCloud.Dav.Calendar.Serialization.Converters
{
    internal sealed class EventListConverter : TypeConverter
    {
        /// <inheritdoc/>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => sourceType == typeof(MultiStatus);

        /// <inheritdoc/>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (!CanConvertFrom(context, value.GetType())) throw GetConvertFromException(value);
            return ((MultiStatus)value).Responses.ToEventList();
        }
    }
}