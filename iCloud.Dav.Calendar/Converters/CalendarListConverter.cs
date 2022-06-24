using iCloud.Dav.ICalendar.Types;
using iCloud.Dav.ICalendar.Utils;
using System;
using System.ComponentModel;
using System.Globalization;

namespace iCloud.Dav.ICalendar.Converters
{
    internal class CalendarListConverter : TypeConverter
    {
        /// <summary>
        /// TypeConverter method override.
        /// </summary>
        /// <param name="context">ITypeDescriptorContext</param>
        /// <param name="sourceType">Type to convert from</param>
        /// <returns>true if conversion is possible</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(Multistatus<Prop>))
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
                var responses = ((Multistatus<Prop>)value).Responses;
                return responses.ToCalendarList();
            }
            throw GetConvertFromException(value);
        }
    }
}
