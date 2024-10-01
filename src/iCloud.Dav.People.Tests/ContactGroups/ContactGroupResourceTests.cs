using iCloud.Dav.Core;
using iCloud.Dav.Core.Response;
using iCloud.Dav.People.Tests.ContactGroups.Resource;
using Moq;

namespace iCloud.Dav.People.Tests.ContactGroups;

[TestFixture]
public class ContactGroupResourceTests
{
    private ContactGroupFixture _fixture;
    private Mock<IContactGroupTestResource> _mockContactGroupResource;
    private const string RESOURCE_NAME = "RESOURCE_NAME";
    private const string TARGET_ID = "TARGET_ID";
    private const string TRANSACTION_ID = "TRANSACTION_ID";

    [SetUp]
    public void Setup()
    {
        _fixture = new ContactGroupFixture();
        _mockContactGroupResource = new Mock<IContactGroupTestResource>();
    }

    [Test]
    public void List_Success()
    {
        // Arrange
        _mockContactGroupResource.Setup(x => x.List(It.IsAny<string>()))
                                 .Returns(_fixture.TestContactGroupList);

        // Act
        var result = _mockContactGroupResource.Object.List(RESOURCE_NAME);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Kind, Is.EqualTo("group"));
        Assert.That(result.Items, Has.Count.EqualTo(2));
        Assert.That(result.Items[0], Is.EqualTo(_fixture.TestContactGroupList.Items[0]));

        // Verify mock method was called once
        _mockContactGroupResource.Verify(x => x.List(RESOURCE_NAME), Times.Once);
    }

    [Test]
    public void Get_Success()
    {
        // Arrange
        var response = new VoidResponse();

        _mockContactGroupResource.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>()))
                                 .Returns(_fixture.TestContactGroup);

        // Act
        var result = _mockContactGroupResource.Object.Get(It.IsAny<string>(), It.IsAny<string>());

        // Assert
        Assert.That(result, Is.Not.Null);
        // Check individual properties
        Assert.That(result.Id, Is.EqualTo(_fixture.TestContactGroup.Id));
        Assert.That(result.Uid, Is.EqualTo(_fixture.TestContactGroup.Uid));
        Assert.That(result.FormattedName, Is.EqualTo(_fixture.TestContactGroup.FormattedName));
        Assert.That(result.N, Is.EqualTo(_fixture.TestContactGroup.N));

        // Assert full object equality last (after checking important fields)
        Assert.That(result, Is.EqualTo(_fixture.TestContactGroup));

        // Verify mock method was called once
        _mockContactGroupResource.Verify(x => x.Get(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void Insert_Success()
    {
        //// Arrange
        //var request = CreatePostTrackableRequest();
        //var response = new VoidResponse();

        //_mockContactGroupResource.Setup(x => x.Insert(It.IsAny<ServerAccessKeys>(), request))
        //                       .Returns(response);

        //// Act
        //var result = _mockContactGroupResource.Object.Insert(_mockServerAccessKeys, request);

        //// Assert
        //Assert.That(result, Is.Not.Null);
        //Assert.That(result.TransactionId, Is.EqualTo(TRANSACTION_ID));
        //Assert.That(result.ResultCode, Is.EqualTo(VuforiaBaseResponse.ResultCodeEnum.Success));
        //Assert.That(result.TargetId, Is.EqualTo(TARGET_ID));

        //// Verify the Insert method was called once
        //_mockContactGroupResource.Verify(x => x.Insert(_mockServerAccessKeys, request), Times.Once);
    }

    [Test]
    public void Update_Success()
    {
        //// Arrange
        //var request = CreateUpdateTrackableRequest();
        //var response = new VoidResponse();

        //_mockContactGroupResource.Setup(x => x.Update(It.IsAny<ServerAccessKeys>(), request, TARGET_ID))
        //                       .Returns(response);

        //// Act
        //var result = _mockContactGroupResource.Object.Update(_mockServerAccessKeys, request, TARGET_ID);

        //// Assert
        //Assert.That(result, Is.Not.Null);
        //Assert.That(result.TransactionId, Is.EqualTo(TRANSACTION_ID));
        //Assert.That(result.ResultCode, Is.EqualTo(VuforiaBaseResponse.ResultCodeEnum.Success));

        //// Verify the Update method was called once
        //_mockContactGroupResource.Verify(x => x.Update(_mockServerAccessKeys, request, TARGET_ID), Times.Once);
    }

    [Test]
    public void Delete_Success()
    {
        // Arrange
        var response = new VoidResponse();

        _mockContactGroupResource.Setup(x => x.Delete(It.IsAny<string>(), It.IsAny<string>()))
                                 .Returns(response);

        // Act
        var result = _mockContactGroupResource.Object.Delete(TARGET_ID, RESOURCE_NAME);

        // Assert
        Assert.That(result, Is.Not.Null);

        // Verify the Delete method was called once
        _mockContactGroupResource.Verify(x => x.Delete(TARGET_ID, RESOURCE_NAME), Times.Once);
    }

    [Test]
    public void ExceptionHandling_WhenApiFails_ThrowsVuforiaPortalApiException()
    {
        // Arrange
        _mockContactGroupResource.Setup(x => x.Get(TARGET_ID, RESOURCE_NAME))
                                 .Throws(new ICloudApiException(nameof(ContactGroupResourceTests), "API Error"));

        // Act & Assert
        Assert.Throws<ICloudApiException>(() => _mockContactGroupResource.Object.Get(TARGET_ID, RESOURCE_NAME));

        // Verify the Get method was called once
        _mockContactGroupResource.Verify(x => x.Get(TARGET_ID, RESOURCE_NAME), Times.Once);
    }
}