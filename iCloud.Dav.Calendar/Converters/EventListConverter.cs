using iCloud.Dav.Calendar.Cal.Types;
using iCloud.Dav.Calendar.Utils;
using System;
using System.ComponentModel;
using System.Globalization;

namespace iCloud.Dav.Calendar.Converters
{
    internal class EventListConverter : TypeConverter
    {
        /// <summary>
        /// TypeConverter method override.
        /// </summary>
        /// <param name="context">ITypeDescriptorContext</param>
        /// <param name="sourceType">Type to convert from</param>
        /// <returns>true if conversion is possible</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(MultiStatus))
                return true;
            return false;
        }

        /// <summary>
        /// TypeConverter method implementation.
        /// </summary>
        /// <param name="context">ITypeDescriptorContext</param>
        /// <param name="culture">current culture (see CLR specs)</param>
        /// <param name="value">value to convert from</param>
        /// <returns>value that is result of conversion</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value != null)
            {
                var responses = ((MultiStatus)value).Responses;
                return responses.ToEventList();
            }
            throw GetConvertFromException(value);
        }
    }
}
