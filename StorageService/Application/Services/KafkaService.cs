using System.Text;
using Application.Configuration;
using Application.Handlers;
using Confluent.Kafka;
using Contracts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProtoBuf;

namespace Application.Services;

public class KafkaService : BackgroundService
{
    private readonly ISettings settings;
    private readonly Dictionary<string, IHandler> handlers;

    private readonly ILogger<KafkaService> logger;

    public KafkaService(ISettings settings,
        IEnumerable<IHandler> conditionMatchers,
        ILogger<KafkaService> logger)
    {
        this.settings = settings;
        this.handlers = conditionMatchers.ToDictionary(ch => ch.ContractType.ToString());
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = settings.Kafka.BootstrapServer,
            GroupId = settings.Kafka.ClientId,
            AutoOffsetReset = AutoOffsetReset.Latest,
            EnablePartitionEof = true,
        };

        using var consumer = new ConsumerBuilder<string, byte[]>(consumerConfig).Build();

        consumer.Subscribe(settings.Kafka.Topics);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = consumer.Consume();

                var messageHeader =
                    consumeResult.Message.Headers.FirstOrDefault(h => h.Key.Equals(MessageHeaders.MESSAGE_TYPE));

                if (messageHeader is null)
                {
                    continue;
                }

                var messageType = Encoding.UTF8.GetString(messageHeader.GetValueBytes());

                if (!handlers.TryGetValue(messageType, out var handler))
                {
                    logger.LogError($"{DateTime.UtcNow.ToString("O")} | {"Unknow message type"}");
                    continue;
                }

                IMessage? message;

                using (MemoryStream memoryStream = new MemoryStream(consumeResult.Message.Value))
                {
                    message = Serializer.Deserialize(handler.ContractType, memoryStream) as IMessage;
                }

                await handler.Handle(message!);
            }
            catch (Exception e)
            {
            }
        }

        consumer.Close();
    }
}