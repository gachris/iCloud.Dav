using iCloud.Dav.People.DataTypes;
using vCard.Net.DataTypes;
using Address = iCloud.Dav.People.DataTypes.Address;
using Email = iCloud.Dav.People.DataTypes.Email;

namespace iCloud.Dav.People.Tests.Contacts;

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
            Uid = "D2D76F16-A8CA-4697-8309-C67741BD44F0",
            N = new StructuredName()
            {
                GivenName = "John",
                FamilyName = "Smith",
                NamePrefix = "Mr",
                NameSuffix = "E.",
                AdditionalNames = "Junior"
            },
            FormattedName = "Mr John Smith E.",
            PhoneticFirstName = "J",
            PhoneticLastName = "S",
            Organization = new Organization()
            {
                Units =
                {
                    "Microsoft",
                    "HR"
                }
            },
            PhoneticOrganization = "MS",
            Nickname = "Chief",
            Birthdate = new Birthdate()
            {
                Date = new VCardDateTime(1996, 08, 05)
                {
                    HasTime = false
                }
            },
            Title = "HR Manager",
            Notes = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque luct us fermentum arcu id molestie. Praesent condimentum ex ac sodales accumsan.",
            Addresses =
            [
                new Address
                {
                    IsPreferred = true,
                    StreetAddress = "2701-2899 Wrangler Dr",
                    Locality = "Irving",
                    PostalCode = "75060",
                    Country = "Ηνωμένες Πολιτείες",
                    Type = AddressType.Home,
                    CountryCode = new X_ABAddress()
                    {
                        Value = "us"
                    }
                }
            ],
            Telephones =
            [
                new Phone
                {
                    FullNumber = "+1-555-123-4567",
                    Type = PhoneType.Mobile,
                    IsPreferred = true
                }
            ],
            RelatedNames =
            [
                new RelatedNames()
                {
                    IsPreferred = true,
                    Type = RelatedNamesType.Father,
                    Name = "Patrik"
                }
            ],
            InstantMessages =
            [
                new InstantMessage()
                {
                    IsPreferred = true,
                    ServiceType = InstantMessageServiceType.Facebook,
                    Type = InstantMessageType.Facebook,
                    UserName = "johnsm"
                }
            ],
            Emails =
            [
                new Email
                {
                    Address = "john@outlook.com",
                    Type = EmailType.Home,
                    IsPreferred = true
                }
            ],
            Dates =
            [
                new Date()
                {
                    Type = DateType.Anniversary,
                    DateTime = new DateTime(2024, 04, 08),
                }
            ],
            SocialProfiles =
            [
                new SocialProfile()
                {
                    Type = SocialProfileType.LinkedIn,
                    UserName = "johnsm",
                    Url = "https://www.linkedin.com/in/johnsm"
                }    
            ],
            RevisionDate = new VCardDateTime(2024, 11, 16, 07, 38, 58)
            {
                TzId = "UTC"
            },
            ProductId = "-//Apple Inc.//iCloud Web Address Book 2428B27//EN"
        };

        var contact2 = new Contact
        {
            Version = VCardVersion.vCard3_0,
            Uid = "B1D7BAF3-6B3E-40DA-9919-FD4CFC575380",
            N = new StructuredName
            {
                GivenName = "Emily",
                FamilyName = "Johnson"
            },
            FormattedName = "Emily Johnson",
            Birthdate = new Birthdate()
            {
                Date = new VCardDateTime(1998, 08, 03)
                {
                    HasTime = false
                }
            },
            Emails =
            [
                new Email
                {
                    Address = "emily.johnson@example.com",
                    Type = EmailType.Home,
                    IsPreferred = true
                }
            ],
            Telephones =
            [
                new Phone
                {
                    FullNumber = "+1-555-123-4567",
                    Type = PhoneType.Mobile,
                    IsPreferred = true
                }
            ],
            Addresses =
            [
                new Address
                {
                    StreetAddress = "123 Elm Street",
                    Locality = "Springfield",
                    Region = "IL",
                    PostalCode = "62701",
                    Country = "USA",
                    Type = AddressType.Home
                }
            ],
            Notes = "Emily is a software engineer with a passion for open-source projects.",
            RevisionDate = new VCardDateTime(2024, 10, 10, 18, 45, 00)
            {
                TzId = "UTC"
            },
            ProductId = "-//Apple Inc.//iCloud Web Address Book 2302B34//EN"
        };

        return new ContactList()
        {
            Items = [contact, contact2],
            Kind = "contacts"
        };
    }
}