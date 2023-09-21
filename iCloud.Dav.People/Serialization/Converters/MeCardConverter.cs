using iCloud.Dav.Core.WebDav.Card;
using iCloud.Dav.People.Extensions;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace iCloud.Dav.People.Serialization.Converters
{
    internal sealed class MeCardConverter : TypeConverter
    {
        /// <inheritdoc/>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => sourceType == typeof(MultiStatus);

        /// <inheritdoc/>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (!CanConvertFrom(context, value.GetType()))
                throw GetConvertFromException(value);

            var multiStatus = (MultiStatus)value;
            var response = multiStatus.Responses.FirstOrDefault(x => x.IsCollection());

            if (response is null)
                throw new ArgumentNullException(nameof(response));
            if (!(response.GetSuccessPropStat() is PropStat propStat))
                throw new ArgumentNullException(nameof(propStat));

            return new People.DataTypes.MeCard()
            {
                Id = propStat.Prop.MeCard?.Href.ExtractId()
            };
        }
    }
}