using System.ComponentModel;
using System.Globalization;
using iCloud.Dav.People.Extensions;
using iCloud.Dav.People.WebDav.DataTypes;

namespace iCloud.Dav.People.Serialization.Converters;

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

        return response is null
            ? throw new ArgumentNullException(nameof(response))
            : response.GetSuccessPropStat() is not PropStat propStat
            ? throw new ArgumentNullException(nameof(propStat))
            : (object)new People.DataTypes.MeCard()
            {
                Id = propStat.Prop.MeCard?.Href.ExtractId()
            };
    }
}