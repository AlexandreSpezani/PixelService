using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

namespace Tests.TestApplication;

public class TestApplication : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder
            .UseTestServer()
            .Configure(app => { app.UseMiddleware<FakeRemoteIpAddressMiddleware>(); });
    }
}