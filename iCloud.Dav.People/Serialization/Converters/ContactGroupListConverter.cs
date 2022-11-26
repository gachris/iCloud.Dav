using iCloud.Dav.People.CardDav.Types;
using iCloud.Dav.People.DataTypes;
using iCloud.Dav.People.PeopleComponents;
using iCloud.Dav.People.Utils;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace iCloud.Dav.People.Serialization.Converters
{
    internal sealed class ContactGroupListConverter : TypeConverter
    {
        /// <inheritdoc/>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => sourceType == typeof(MultiStatus);

        /// <inheritdoc/>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (!CanConvertFrom(context, value.GetType())) throw GetConvertFromException(value);
            return new ContactGroupList(((MultiStatus)value).Responses.Select(MappingExtensions.Deserialize<ContactGroup>));
        }
    }
}