using Contracts;
using Microsoft.Extensions.Logging;

namespace Application.Handlers;

public class AddTrackInfoHandler : IHandler
{
    public Type ContractType => typeof(AddTrackInfo);

    private readonly ILogger<AddTrackInfo> logger;

    public AddTrackInfoHandler(ILogger<AddTrackInfo> logger)
    {
        this.logger = logger;
    }

    public async Task Handle(IMessage message)
    {
        var trackInfo = message as AddTrackInfo;

        var log =
            $"{trackInfo!.Timestamp}|{trackInfo.Referrer ?? "null"}|{trackInfo.UserAgent ?? "null"}|{trackInfo.IpAddress}";

        logger.LogInformation(log);
    }
}