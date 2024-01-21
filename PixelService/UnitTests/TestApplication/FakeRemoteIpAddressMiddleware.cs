using System.Net;
using Microsoft.AspNetCore.Http;

namespace Tests.TestApplication;

public class FakeRemoteIpAddressMiddleware(RequestDelegate next)
{
    private readonly IPAddress fakeIpAddress = IPAddress.Parse("127.168.1.32");

    public async Task Invoke(HttpContext httpContext)
    {
        httpContext.Connection.RemoteIpAddress = fakeIpAddress;
        httpContext.Request.Headers["User-Agent"] = "User-Agent";
        httpContext.Request.Headers["Referrer"] = "Referrer";
        
         await next(httpContext);
    }
}