using iCloud.Dav.Calendar.Extensions;
using System;
using System.ComponentModel;
using System.Globalization;

namespace iCloud.Dav.Calendar.Serialization.Converters;

internal sealed class ReminderConverter : TypeConverter
{
    /// <inheritdoc/>
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => sourceType == typeof(string);

    /// <inheritdoc/>
    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        return !CanConvertFrom(context, value.GetType()) ? throw GetConvertFromException(value) : (object)((string)value).ToReminder();
    }
}