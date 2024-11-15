using System.ComponentModel;
using System.Globalization;
using iCloud.Dav.Calendar.Extensions;
using iCloud.Dav.Calendar.WebDav.DataTypes;

namespace iCloud.Dav.Calendar.Serialization.Converters;

internal sealed class CalendarConverter : TypeConverter
{
    /// <inheritdoc/>
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => sourceType == typeof(MultiStatus);

    /// <inheritdoc/>
    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        if (!CanConvertFrom(context, value.GetType()))
            throw GetConvertFromException(value);

        var multiStatus = (MultiStatus)value;
        return multiStatus.Responses.First().ToCalendar();
    }
}