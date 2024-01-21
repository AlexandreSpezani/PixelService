using Application.Handlers;
using AutoFixture;
using Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTests.Application.Handlers;

[TestClass]
public class AddTrackInfoHandlerTests
{
    private readonly IFixture fixture = new Fixture();

    private readonly Mock<ILogger<AddTrackInfo>> loggerMock = new();

    private AddTrackInfoHandler handler;

    public AddTrackInfoHandlerTests()
    {
        this.handler = new AddTrackInfoHandler(this.loggerMock.Object);
    }

    [TestMethod]
    public async Task AddTrackInfoHandler_Handle_ShouldLog()
    {
        // Arrange
        var addTrackInfo = this.fixture.Create<AddTrackInfo>();
        IMessage message = addTrackInfo;

        // Act
        await this.handler.Handle(message);

        this.loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)!)
            , Times.Once);
    }
}