using iCloud.Dav.Core.Utils;
using iCloud.Dav.People.CardDav.Types;
using iCloud.Dav.People.Types;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace iCloud.Dav.People.Serialization.Converters;

internal sealed class ContactGroupsListConverter : TypeConverter
{
    /// <summary>
    /// TypeConverter method override.
    /// </summary>
    /// <param name="context">ITypeDescriptorContext</param>
    /// <param name="sourceType">Type to convert from</param>
    /// <returns>true if conversion is possible</returns>
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type? sourceType)
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
    public override object ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        switch (value)
        {
            case MultiStatus multistatus:
                var responses = multistatus.Responses.ThrowIfNull(nameof(multistatus.Responses));

                return new ContactGroupsList(responses.Select(response =>
                {
                    var addressData = response.AddressData.ThrowIfNull(nameof(response.AddressData));
                    var addressdataValue = addressData.Value.ThrowIfNull(nameof(addressData.Value));
                    var bytes = Encoding.UTF8.GetBytes(addressdataValue);
                    var d = new ContactGroup(bytes)
                    {
                        Url = response.Href.ThrowIfNull(nameof(response.Href)),
                        ETag = response.Etag.ThrowIfNull(nameof(response.Etag))
                    };
                    return d;
                }));
            default:
                throw GetConvertFromException(value);
        }
    }
}
