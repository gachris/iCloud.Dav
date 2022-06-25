using iCloud.Dav.Core.Utils;
using iCloud.Dav.People.Types;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace iCloud.Dav.People.Converters
{
    internal class ContactGroupsListConverter : TypeConverter
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
            switch (value)
            {
                case Multistatus<Prop> multistatusProp:
                    var responses = multistatusProp.Responses.ThrowIfNull(nameof(multistatusProp.Responses));

                    return new ContactGroupsList(responses.Select(response =>
                    {
                        var propstat = response.Propstat.ThrowIfNull(nameof(response.Propstat));
                        var prop = propstat.Prop.ThrowIfNull(nameof(propstat.Prop));
                        var addressdata = prop.Addressdata.ThrowIfNull(nameof(prop.Addressdata));
                        var addressdataValue = addressdata.Value.ThrowIfNull(nameof(prop.Addressdata));
                        var etag = prop.Getetag.ThrowIfNull(nameof(prop.Getetag));
                        var bytes = Encoding.UTF8.GetBytes(addressdataValue);
                        return new ContactGroup(bytes)
                        {
                            Url = response.Url.ThrowIfNull(nameof(response.Url)),
                            ETag = etag.Value.ThrowIfNull(nameof(etag.Value))
                        };
                    }));
                default:
                    throw GetConvertFromException(value);
            }
        }
    }
}
