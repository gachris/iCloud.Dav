using iCloud.Dav.People.DataTypes;
using vCard.Net.DataTypes;

namespace iCloud.Dav.People.Tests.ContactGroups;

public class ContactFixture
{
    #region Properties

    public Contact TestContact { get; }

    public ContactList TestContactList { get; }

    #endregion

    public ContactFixture()
    {
        TestContactList = CreateTestContactList();
        TestContact = TestContactList.Items[0];
    }

    private static ContactList CreateTestContactList()
    {
        var contact = new Contact
        {
            Version = VCardVersion.vCard3_0,
            Id = "D2D76F16-A8CA-4697-8309-C67741BD44F0",
            Uid = "D2D76F16-A8CA-4697-8309-C67741BD44F0",
            N = new StructuredName()
            {
                GivenName = "John",
                FamilyName = "Smith"
            },
            FormattedName = "John ",
            RevisionDate = new VCardDateTime(2024, 09, 30, 22, 10, 06),
            ProductId = "-//Apple Inc.//iCloud Web Address Book 2302B34//EN"
        };

        var contact2 = new Contact
        {
            Version = VCardVersion.vCard3_0,
            Id = "B1D7BAF3-6B3E-40DA-9919-FD4CFC575380",
            Uid = "B1D7BAF3-6B3E-40DA-9919-FD4CFC575380",
            N = new StructuredName()
            {
                GivenName = "",
                FamilyName = "",
            },
            FormattedName = "",
            RevisionDate = new VCardDateTime(2024, 09, 30, 22, 10, 06),
            ProductId = "-//Apple Inc.//iCloud Web Address Book 2302B34//EN"
        };

        return new ContactList()
        {
            Items = [contact, contact2],
            Kind = "contacts"
        };
    }
}