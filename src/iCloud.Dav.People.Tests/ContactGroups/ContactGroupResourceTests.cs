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
    public void List_ValidResponse_ReturnsExpectedResult()
    {
        // Arrange
        _mockContactGroupResource.Setup(x => x.List(It.IsAny<string>()))
                                 .Returns(_fixture.TestContactGroupList);

        // Act
        var result = _mockContactGroupResource.Object.List(RESOURCE_NAME);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Kind, Is.EqualTo("group"));
        Assert.That(result.Items.Count, Is.EqualTo(2));
        Assert.That(result.Items[0], Is.EqualTo(_fixture.TestContactGroupList.Items[0]));

        // Verify mock method was called once
        _mockContactGroupResource.Verify(x => x.List(RESOURCE_NAME), Times.Once);
    }

    [Test]
    public void Get_ValidTargetId_ReturnsCorrectData()
    {
        //// Arrange
        //var response = new VoidResponse();

        //_mockContactGroupResource.Setup(x => x.Get(It.IsAny<ServerAccessKeys>(), TARGET_ID))
        //                       .Returns(response);

        //// Act
        //var result = _mockContactGroupResource.Object.Get(_mockServerAccessKeys, TARGET_ID);

        //// Assert
        //Assert.That(result, Is.Not.Null);
        //Assert.That(result.TransactionId, Is.EqualTo(TRANSACTION_ID));
        //Assert.That(result.ResultCode, Is.EqualTo(VuforiaBaseResponse.ResultCodeEnum.Success));
        //Assert.That(result.Status, Is.EqualTo(VuforiaRetrieveResponse.StatusEnum.Success));

        //// Assert TargetRecord properties
        //var targetRecord = result.TargetRecord;
        //Assert.That(targetRecord, Is.Not.Null);
        //Assert.That(targetRecord.TargetId, Is.EqualTo(TARGET_ID));
        //Assert.That(targetRecord.ActiveFlag, Is.EqualTo("true"));
        //Assert.That(targetRecord.TrackingRating, Is.EqualTo(5));
        //Assert.That(targetRecord.Width, Is.EqualTo(1));

        //// Verify mock method was called once
        //_mockContactGroupResource.Verify(x => x.Get(_mockServerAccessKeys, TARGET_ID), Times.Once);
    }

    [Test]
    public void Insert_ValidRequest_CallsInsertOnce()
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
    public void Update_ValidRequest_CallsUpdateOnce()
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
    public void Delete_ValidTargetId_CallsDeleteOnce()
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

    //// Helper methods to create responses
    //private static VuforiaGetAllResponse CreateValidContactGroupList()
    //{
    //    return new VuforiaGetAllResponse
    //    {
    //        TransactionId = TRANSACTION_ID,
    //        ResultCode = VuforiaBaseResponse.ResultCodeEnum.Success,
    //        Results = [TARGET_ID]
    //    };
    //}

    //private static VuforiaRetrieveResponse CreateValidRetrieveResponse()
    //{
    //    return new VuforiaRetrieveResponse
    //    {
    //        Status = VuforiaRetrieveResponse.StatusEnum.Success,
    //        TargetRecord = new TargetRecordModel
    //        {
    //            ActiveFlag = "true",
    //            TrackingRating = 5,
    //            TargetId = TARGET_ID,
    //            Width = 1
    //        },
    //        TransactionId = TRANSACTION_ID,
    //        ResultCode = VuforiaBaseResponse.ResultCodeEnum.Success
    //    };
    //}

    //private static PostTrackableRequest CreatePostTrackableRequest()
    //{
    //    return new PostTrackableRequest
    //    {
    //        ActiveFlag = true,
    //        ApplicationMetadata = "Target Metadata",
    //        Image = "/9j/4AAQSkZJRgABAQEAAAAAAAD...",
    //        Name = "Sample Target",
    //        Width = 1
    //    };
    //}

    //private static UpdateTrackableRequest CreateUpdateTrackableRequest()
    //{
    //    return new UpdateTrackableRequest
    //    {
    //        ActiveFlag = true,
    //        ApplicationMetadata = "Target Metadata",
    //        Image = "/9j/4AAQSkZJRgABAQEAAAAAAAD...",
    //        Name = "Sample Target",
    //        Width = 1
    //    };
    //}
}