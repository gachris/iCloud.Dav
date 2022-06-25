using iCloud.Dav.Core.Utils;
using iCloud.Dav.People.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace iCloud.Dav.People.Converters
{
    internal class PersonListConverter : TypeConverter
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

                    if (!responses.Any()) return default;

                    var personList = new PersonList();
                    var contactGroupList = new ContactGroupsList();

                    foreach (var multistatusItem in responses)
                    {
                        if (multistatusItem.Propstat.Prop.Addressdata.Value.Contains("X-ADDRESSBOOKSERVER-KIND:group"))
                        {
                            var bytes = Encoding.UTF8.GetBytes(multistatusItem.Propstat.Prop.Addressdata.Value);
                            var contactGroup = new ContactGroup(bytes)
                            {
                                ETag = multistatusItem.Propstat.Prop.Getetag.Value
                            };
                            contactGroupList.Add(contactGroup);
                        }
                        else
                        {
                            var bytes = Encoding.UTF8.GetBytes(multistatusItem.Propstat.Prop.Addressdata.Value);
                            var person = new Person(bytes)
                            {
                                ETag = multistatusItem.Propstat.Prop.Getetag.Value
                            };
                            personList.Add(person);
                        }
                    }

                    foreach (var person in personList)
                    {
                        var memberships = new List<Membership>();
                        var contactGroups = contactGroupList.Where(contactGoup => contactGoup.MemberResourceNames.Contains(person.UniqueId));
                        foreach (var contactGroup in contactGroups)
                        {
                            var membership = new Membership()
                            {
                                ETag = contactGroup.ETag,
                                ContactGroupId = contactGroup.UniqueId
                            };
                            memberships.Add(membership);
                        }
                        person.Memberships = new ReadOnlyCollection<Membership>(memberships);
                    }

                    return personList;
                default:
                    throw GetConvertFromException(value);
            }
        }
    }
}
