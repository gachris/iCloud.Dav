using iCloud.Dav.People.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace iCloud.Dav.People.Utils
{
    internal static class ConverterHelper
    {
        public static PersonList ToPersonList(this List<Response<Prop>> responses)
        {
            if (responses != null && responses.Any())
            {
                var personList = new PersonList();
                var contactGroupList = new ContactGroupsList();

                foreach (var multistatusItem in responses)
                {
                    if (multistatusItem.Propstat.Prop.Addressdata.Value.Contains("X-ADDRESSBOOKSERVER-KIND:group"))
                    {
                        var contactGroup = new ContactGroup(multistatusItem.Propstat.Prop.Addressdata.Value)
                        {
                            ETag = multistatusItem.Propstat.Prop.Getetag.Value
                        };
                        contactGroupList.Add(contactGroup);
                    }
                    else
                    {
                        var person = new Person(multistatusItem.Propstat.Prop.Addressdata.Value)
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
                    person.Memberships = new ReadOnlyMembershipCollection(memberships);
                }

                return personList;
            }
            return default;
        }

        public static IdentityCardList ToIdentityCardList(this List<Response<Prop>> responses)
        {
            var listItems = new IdentityCardList();
            foreach (var multistatusItem in responses)
            {
                var cardUrl = multistatusItem.Url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                if (cardUrl.Length == 3)
                {
                    var card = new IdentityCard()
                    {
                        ResourceName = cardUrl.Last(),
                        UniqueId = cardUrl.Last(),
                        Url = multistatusItem.Url,
                    };
                    listItems.Add(card);
                }
            }
            return listItems;
        }

        public static ContactGroupsList ToContactGroupsList(this List<Response<Prop>> responses)
        {
            var listItems = new ContactGroupsList();
            foreach (var response in responses)
            {
                var card = new ContactGroup(response.Propstat.Prop.Addressdata.Value)
                {
                    Url = response.Url,
                    ETag = response.Propstat.Prop.Getetag.Value,
                };
                listItems.Add(card);
            }
            return listItems;
        }
    }
}
