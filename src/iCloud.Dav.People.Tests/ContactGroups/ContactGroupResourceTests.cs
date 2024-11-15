using iCloud.Dav.Auth;
using iCloud.Dav.Core;
using iCloud.Dav.Core.Response;
using iCloud.Dav.People.DataTypes;
using iCloud.Dav.People.Resources;
using iCloud.Dav.People.Services;
using Moq;

namespace iCloud.Dav.People.Tests.ContactGroups;

[TestFixture]
public class ContactGroupResourceTests
{
    private ContactGroupFixture _fixture;
    private Mock<PeopleService> _mockPeopleService;
    private const string RESOURCE_NAME = nameof(RESOURCE_NAME);
    private const string CONTACT_GROUP_ID = nameof(CONTACT_GROUP_ID);
    private static readonly string[] CONTACT_GROUP_IDS = [nameof(CONTACT_GROUP_ID)];

    [SetUp]
    public void Setup()
    {
        _fixture = new ContactGroupFixture();

        // Mocking dependencies for the Token class
        var mockCalendarServer = new Mock<DavServer>("calendarServerId", "https://calendar.example.com");
        var mockCalendarPrincipal = new Mock<CalendarPrincipal>("currentUserPrincipal", "https://calendarHomeSet", "DisplayName", new List<CalendarUserAddressSet>());
        var mockPeopleServer = new Mock<DavServer>("peopleServerId", "https://people.example.com");
        var mockPeoplePrincipal = new Mock<PeoplePrincipal>("currentUserPrincipal", "https://addressBookHomeSet", "DisplayName");

        // Mocking the token
        var mockToken = new Mock<Token>(mockCalendarServer.Object,
                                        mockCalendarPrincipal.Object,
                                        mockPeopleServer.Object,
                                        mockPeoplePrincipal.Object,
                                        "mockAccessToken",
                                        DateTime.Now);

        // Mocking other dependencies
        var mockAuthorizationCodeFlow = new Mock<IAuthorizationCodeFlow>();
        var mockUserCredential = new Mock<UserCredential>(mockAuthorizationCodeFlow.Object, It.IsAny<string>(), mockToken.Object);
        var mockInitializer = new Mock<BaseClientService.Initializer>();
        mockInitializer.SetupGet(x => x.HttpClientInitializer).Returns(mockUserCredential.Object);

        _mockPeopleService = new Mock<PeopleService>(mockInitializer.Object);

        var mockContactGroupResource = new Mock<ContactGroupsResource>(_mockPeopleService.Object);
        _mockPeopleService.SetupGet(x => x.ContactGroups).Returns(mockContactGroupResource.Object);

        var mockListRequest = new Mock<ContactGroupsResource.ListRequest>(_mockPeopleService.Object, RESOURCE_NAME);
        mockContactGroupResource.Setup(x => x.List(It.IsAny<string>()))
                                .Returns(mockListRequest.Object);

        var mockGetRequest = new Mock<ContactGroupsResource.GetRequest>(_mockPeopleService.Object, CONTACT_GROUP_ID, RESOURCE_NAME);
        mockContactGroupResource.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>()))
                                .Returns(mockGetRequest.Object);

        var mockMultiGetRequest = new Mock<ContactGroupsResource.MultiGetRequest>(_mockPeopleService.Object, RESOURCE_NAME, CONTACT_GROUP_IDS);
        mockContactGroupResource.Setup(x => x.MultiGet(It.IsAny<string>(), It.IsAny<string[]>()))
                                .Returns(mockMultiGetRequest.Object);

        var mockInsertRequest = new Mock<ContactGroupsResource.InsertRequest>(_mockPeopleService.Object, _fixture.TestContactGroup, RESOURCE_NAME);
        mockContactGroupResource.Setup(x => x.Insert(It.IsAny<ContactGroup>(), It.IsAny<string>()))
                                .Returns(mockInsertRequest.Object);

        var mockUpdateRequest = new Mock<ContactGroupsResource.UpdateRequest>(_mockPeopleService.Object, _fixture.TestContactGroup, RESOURCE_NAME);
        mockContactGroupResource.Setup(x => x.Update(It.IsAny<ContactGroup>(), It.IsAny<string>()))
                                .Returns(mockUpdateRequest.Object);

        var mockDeleteRequest = new Mock<ContactGroupsResource.DeleteRequest>(_mockPeopleService.Object, CONTACT_GROUP_ID, RESOURCE_NAME);
        mockContactGroupResource.Setup(x => x.Delete(It.IsAny<string>(), It.IsAny<string>()))
                                .Returns(mockDeleteRequest.Object);
    }

    [Test]
    public void List_Success()
    {
        // Arrange
        _mockPeopleService.Setup(x => x.ContactGroups.List(It.IsAny<string>()).Execute())
                          .Returns(_fixture.TestContactGroupList);

        // Act
        var result = _mockPeopleService.Object.ContactGroups.List(It.IsAny<string>())
                                                            .Execute();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Kind, Is.EqualTo("group"));
        Assert.That(result.Items, Has.Count.EqualTo(2));
        Assert.That(result.Items[0], Is.EqualTo(_fixture.TestContactGroupList.Items[0]));

        // Verify mock method was called once
        _mockPeopleService.Verify(x => x.ContactGroups.List(It.IsAny<string>()).Execute(), Times.Once);
    }

    [Test]
    public void Get_Success()
    {
        // Arrange
        _mockPeopleService.Setup(x => x.ContactGroups.Get(It.IsAny<string>(), It.IsAny<string>()).Execute())
                          .Returns(_fixture.TestContactGroup);

        // Act
        var result = _mockPeopleService.Object.ContactGroups.Get(It.IsAny<string>(), It.IsAny<string>())
                                                            .Execute();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(_fixture.TestContactGroup.Id));
        Assert.That(result.Uid, Is.EqualTo(_fixture.TestContactGroup.Uid));
        Assert.That(result.FormattedName, Is.EqualTo(_fixture.TestContactGroup.FormattedName));
        Assert.That(result.N, Is.EqualTo(_fixture.TestContactGroup.N));

        // Assert full object equality last (after checking important fields)
        Assert.That(result, Is.EqualTo(_fixture.TestContactGroup));

        // Verify mock method was called once
        _mockPeopleService.Verify(x => x.ContactGroups.Get(It.IsAny<string>(), It.IsAny<string>()).Execute(), Times.Once);
    }

    [Test]
    public void MultiGet_Success()
    {
        // Arrange
        _mockPeopleService.Setup(x => x.ContactGroups.MultiGet(It.IsAny<string>(), It.IsAny<string[]>()).Execute())
                          .Returns(_fixture.TestContactGroupList);

        // Act
        var result = _mockPeopleService.Object.ContactGroups.MultiGet(It.IsAny<string>(), It.IsAny<string[]>())
                                                            .Execute();

        // Assert
        Assert.That(result, Is.Not.Null);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Kind, Is.EqualTo("group"));
        Assert.That(result.Items, Has.Count.EqualTo(2));
        Assert.That(result.Items[0], Is.EqualTo(_fixture.TestContactGroupList.Items[0]));

        // Verify mock method was called once
        _mockPeopleService.Verify(x => x.ContactGroups.MultiGet(It.IsAny<string>(), It.IsAny<string[]>()).Execute(), Times.Once);
    }

    [Test]
    public void Insert_Success()
    {
        // Arrange
        _mockPeopleService.Setup(x => x.ContactGroups.Insert(It.IsAny<ContactGroup>(), It.IsAny<string>()).Execute())
                          .Returns(new HeaderMetadataResponse());

        // Act
        var result = _mockPeopleService.Object.ContactGroups.Insert(It.IsAny<ContactGroup>(), It.IsAny<string>()).Execute();

        // Assert
        Assert.That(result, Is.Not.Null);

        // Verify the Insert method was called once
        _mockPeopleService.Verify(x => x.ContactGroups.Insert(It.IsAny<ContactGroup>(), It.IsAny<string>()).Execute(), Times.Once);
    }

    [Test]
    public void Update_Success()
    {
        // Arrange
        _mockPeopleService.Setup(x => x.ContactGroups.Update(It.IsAny<ContactGroup>(), It.IsAny<string>()).Execute())
                          .Returns(new HeaderMetadataResponse());

        // Act
        var result = _mockPeopleService.Object.ContactGroups.Update(It.IsAny<ContactGroup>(), It.IsAny<string>()).Execute();

        // Assert
        Assert.That(result, Is.Not.Null);

        // Verify the Update method was called once
        _mockPeopleService.Verify(x => x.ContactGroups.Update(It.IsAny<ContactGroup>(), It.IsAny<string>()).Execute(), Times.Once);
    }

    [Test]
    public void Delete_Success()
    {
        // Arrange
        var response = new VoidResponse();

        _mockPeopleService.Setup(x => x.ContactGroups.Delete(It.IsAny<string>(), It.IsAny<string>()).Execute())
                          .Returns(response);

        // Act
        var result = _mockPeopleService.Object.ContactGroups.Delete(It.IsAny<string>(), It.IsAny<string>())
                                                            .Execute();

        // Assert
        Assert.That(result, Is.Not.Null);

        // Verify the Delete method was called once
        _mockPeopleService.Verify(x => x.ContactGroups.Delete(It.IsAny<string>(), It.IsAny<string>()).Execute(), Times.Once);
    }

    [Test]
    public void ExceptionHandling_WhenApiFails_ThrowsICloudApiException()
    {
        // Arrange
        _mockPeopleService.Setup(x => x.ContactGroups.Get(It.IsAny<string>(), It.IsAny<string>()).Execute())
                          .Throws(new ICloudApiException(nameof(ContactGroupResourceTests), "API Error"));

        // Act & Assert
        Assert.Throws<ICloudApiException>(() => _mockPeopleService.Object.ContactGroups.Get(It.IsAny<string>(), It.IsAny<string>()).Execute());

        // Verify the Get method was called once
        _mockPeopleService.Verify(x => x.ContactGroups.Get(It.IsAny<string>(), It.IsAny<string>()).Execute(), Times.Once);
    }
}