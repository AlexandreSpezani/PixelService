using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Api;

[TestClass]
public class TrackTests
{
    private TestApplication.TestApplication testApplication = new();
    private HttpClient client;

    public TrackTests()
    {
        client = testApplication.CreateDefaultClient();
    }

    [TestMethod]
    public async Task TrackEndpoint_ShouldReturnImageFile()
    {
        // Act
        var response = await client.GetAsync("/track");

        var contentType = response.Content.Headers.ContentType?.ToString();

        // Assert
        Assert.IsNotNull(response);
        Assert.IsTrue(response.IsSuccessStatusCode);
        Assert.AreEqual("image/png", contentType);
    }
}