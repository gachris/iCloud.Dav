using iCloud.Dav.People.Types;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace iCloud.Dav.People.Serialization.Converters;

internal sealed class PersonConverter : TypeConverter
{
    /// <summary>
    /// TypeConverter method override.
    /// </summary>
    /// <param name="context">ITypeDescriptorContext</param>
    /// <param name="sourceType">Type to convert from</param>
    /// <returns>true if conversion is possible</returns>
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type? sourceType)
    {
        if (sourceType == typeof(string))
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
    public override object ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        var input = (string)value;
        if (!string.IsNullOrEmpty(input))
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            return new Person(bytes);
        }
        throw GetConvertFromException(value);
    }
}
