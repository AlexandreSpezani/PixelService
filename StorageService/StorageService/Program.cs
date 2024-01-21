using Application.Configuration;
using Application.Handlers;
using Application.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

ISettings settings = builder.Configuration.Get<Settings>()!;

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddSingleton(settings);
builder.Services.AddHostedService<KafkaService>();

builder.Services.Scan(scan => scan
    .FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
    .AddClasses(classes => classes.AssignableTo(typeof(IHandler)))
    .AsMatchingInterface()
    .AsImplementedInterfaces()
    .WithSingletonLifetime());

Log.Logger = new LoggerConfiguration()
    .WriteTo.File(settings.Data.FileLocation,
        fileSizeLimitBytes: 5242880,
        outputTemplate: "{Message}{NewLine}",
        rollOnFileSizeLimit: true)
    .CreateLogger();

builder.Services.AddLogging(loggingBuilder => { loggingBuilder.AddSerilog(); });

var app = builder.Build();

app.Run();