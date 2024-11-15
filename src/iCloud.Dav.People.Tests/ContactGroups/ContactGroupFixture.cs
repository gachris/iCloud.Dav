using iCloud.Dav.People.DataTypes;
using vCard.Net.DataTypes;

namespace iCloud.Dav.People.Tests.ContactGroups;

public class ContactGroupFixture
{
    #region Properties

    public ContactGroup TestContactGroup { get; }

    public ContactGroupList TestContactGroupList { get; }

    #endregion

    public ContactGroupFixture()
    {
        TestContactGroupList = CreateTestContactGroupList();
        TestContactGroup = TestContactGroupList.Items[0];
    }

    private static ContactGroupList CreateTestContactGroupList()
    {
        var contactGroup = new ContactGroup
        {
            Version = VCardVersion.vCard3_0,
            Uid = "D2D76F16-A8CA-4697-8309-C67741BD44F0",
            N = "Untitled Group 1",
            FormattedName = "Untitled Group 1",
            Kind = new Kind()
            {
                CardKind = KindType.Group
            },
            RevisionDate = new VCardDateTime(2024, 09, 30, 22, 10, 06),
            Members = [new Member("18C30708-99D5-4737-9945-0FF75EAB2B9B"), new Member() { Uid = "CF580921-1CC1-4AD6-ACE2-99426B054831" }],
            ProductId = "-//Apple Inc.//iCloud Web Address Book 2302B34//EN"
        };

        var contactGroup2 = new ContactGroup
        {
            Version = VCardVersion.vCard3_0,
            Uid = "B1D7BAF3-6B3E-40DA-9919-FD4CFC575380",
            N = "Untitled Group 2",
            FormattedName = "Untitled Group 2",
            Kind = new Kind()
            {
                CardKind = KindType.Group
            },
            RevisionDate = new VCardDateTime(2024, 09, 30, 22, 10, 06),
            Members = [new Member("18C30708-99D5-4737-9945-0FF75EAB2B9B"), new Member() { Uid = "CF580921-1CC1-4AD6-ACE2-99426B054831" }],
            ProductId = "-//Apple Inc.//iCloud Web Address Book 2302B34//EN"
        };

        return new ContactGroupList()
        {
            Items = [contactGroup, contactGroup2],
            Kind = "group"
        };
    }
}