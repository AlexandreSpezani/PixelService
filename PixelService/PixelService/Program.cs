using Application.Configuration;
using Application.DependencyInjection;
using Application.Services;
using Contracts;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

ISettings settings = builder.Configuration.Get<Settings>()!;

builder.Services
    .AddSingleton(settings)
    .AddApplication(settings);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/track", async (
        [FromServices] IKafkaService kafkaService,
        HttpContext httpContext) =>
    {
        var trackInfo = new AddTrackInfo()
        {
            IpAddress = httpContext.Connection.RemoteIpAddress!.ToString(),
            UserAgent = httpContext.Request.Headers["User-Agent"],
            Referrer = httpContext.Request.Headers["Referrer"],
            Timestamp = DateTime.UtcNow.ToString("O")
        };

        await kafkaService.Produce(trackInfo);

        var image = File.ReadAllBytes(settings.Data.ImageLocation);

        var imageName = settings.Data.ImageLocation.Split("/").Last();

        return Results.File(image, fileDownloadName: imageName);
    })
    .WithName("track")
    .WithOpenApi();

app.Run();

// To tests references
public partial class Program
{
}