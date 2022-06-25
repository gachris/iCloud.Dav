using iCloud.Dav.Core.Utils;
using iCloud.Dav.People.Types;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace iCloud.Dav.People.Converters
{
    internal class IdentityCardListConverter : TypeConverter
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

                    return new IdentityCardList(responses.Where(response => Split(response.Url).Length == 3).Select(response =>
                    {
                        var card = new IdentityCard { Url = response.Url.ThrowIfNull(nameof(response.Url)) };
                        var strings = Split(response.Url);
                        card.UniqueId = card.ResourceName = strings.Last().ThrowIfNull(nameof(card.ResourceName));
                        return card;
                    }));
                default:
                    throw GetConvertFromException(value);
            }
        }

        private static string[] Split(string url)
        {
            return url?.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
        }
    }
}
