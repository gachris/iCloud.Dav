using System.Text;
using iCloud.Dav.People.Serialization;

namespace iCloud.Dav.People.Tests.Contacts;

[TestFixture]
public class ContactSerializerTests
{
    #region Fields/Consts

    private readonly ContactFixture _fixture;

    #endregion

    public ContactSerializerTests()
    {
        _fixture = new ContactFixture();
    }

    [SetUp]
    public void Setup()
    {
        // Any setup code goes here if needed.
    }

    [Test]
    public void IMPPTest_Success()
    {
        // Prepare the path to the vCard file
        var dataFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Assets\IMPPTest.vcf");

        // Assert that the vCard file exists
        Assert.That(File.Exists(dataFilePath), Is.True, $"File not found: {dataFilePath}");

        // Read the vCard data from file
        var vCardData = File.ReadAllText(dataFilePath);

        // Ensure that the vCard data is not empty or null
        Assert.That(string.IsNullOrWhiteSpace(vCardData), Is.False, "The vCard data is empty.");

        // Deserialize vCard data
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(vCardData));
        using var reader = new StreamReader(stream, Encoding.UTF8);
        var deserializedCard = ContactDeserializer.Default.Deserialize(reader).First();

        // Serialize vCard object
        var serializer = new ContactSerializer();
        var vCardAsString = serializer.SerializeToString(deserializedCard);
    }

    [Test]
    public void SerializeToString_Success()
    {
        var dataFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Assets\John Smith.vcf");

        // Assert that the vCard file exists
        Assert.That(File.Exists(dataFilePath), Is.True, $"File not found: {dataFilePath}");

        // Read the vCard data from file
        var vCardData = File.ReadAllText(dataFilePath);

        // Ensure that the vCard data is not empty or null
        Assert.That(string.IsNullOrWhiteSpace(vCardData), Is.False, "The vCard data is empty.");

        // Serialize vCard object
        var serializer = new ContactSerializer();
        var vCardAsString = serializer.SerializeToString(_fixture.TestContact);

        // Check if serialized string matches expected file content
        Assert.That(vCardAsString.Trim(), Is.EqualTo(vCardData.Trim()));
    }

    [Test]
    public void Deserialize_Success()
    {
        // Prepare the path to the vCard file
        var dataFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Assets\John Smith.vcf");

        // Assert that the vCard file exists
        Assert.That(File.Exists(dataFilePath), Is.True, $"File not found: {dataFilePath}");

        // Read the vCard data from file
        var vCardData = File.ReadAllText(dataFilePath);

        // Ensure that the vCard data is not empty or null
        Assert.That(string.IsNullOrWhiteSpace(vCardData), Is.False, "The vCard data is empty.");

        // Deserialize vCard data
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(vCardData));
        using var reader = new StreamReader(stream, Encoding.UTF8);
        var deserializedCard = ContactDeserializer.Default.Deserialize(reader).First();

        var serializer = new ContactSerializer();
        var vCardAsString = serializer.SerializeToString(deserializedCard);

        // Check individual properties
        Assert.That(deserializedCard.Id, Is.EqualTo(_fixture.TestContact.Id));
        Assert.That(deserializedCard.Uid, Is.EqualTo(_fixture.TestContact.Uid));
        Assert.That(deserializedCard.FormattedName, Is.EqualTo(_fixture.TestContact.FormattedName));
        Assert.That(deserializedCard.N, Is.EqualTo(_fixture.TestContact.N));

        // Assert full object equality last (after checking important fields)
        Assert.That(deserializedCard, Is.EqualTo(_fixture.TestContact));
    }

    [Test]
    public void Deserialize_Multiple_Success()
    {
        // Prepare the path to the vCard file
        var dataFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Assets\MultipleContacts.vcf");

        // Assert that the vCard file exists
        Assert.That(File.Exists(dataFilePath), Is.True, $"File not found: {dataFilePath}");

        // Read the vCard data from file
        var vCardData = File.ReadAllText(dataFilePath);

        // Ensure that the vCard data is not empty or null
        Assert.That(string.IsNullOrWhiteSpace(vCardData), Is.False, "The vCard data is empty.");

        // Deserialize vCard data
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(vCardData));
        using var reader = new StreamReader(stream, Encoding.UTF8);
        var deserializedCards = ContactGroupDeserializer.Default.Deserialize(reader);

        // Assert that deserialization returned a collection
        Assert.That(deserializedCards, Is.Not.Null);

        // Assert that two vCards were deserialized
        Assert.That(deserializedCards.Count(), Is.EqualTo(2));
    }
}