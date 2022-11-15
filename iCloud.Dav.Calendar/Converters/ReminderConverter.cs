using iCloud.Dav.Calendar.Utils;
using System;
using System.ComponentModel;
using System.Globalization;

namespace iCloud.Dav.Calendar.Converters;

internal sealed class ReminderConverter : TypeConverter
{
    /// <inheritdoc/>
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) => sourceType == typeof(string);

    /// <inheritdoc/>
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is not string text || !string.IsNullOrEmpty(text)) throw GetConvertFromException(value);
        return text.ToReminder();
    }
}
