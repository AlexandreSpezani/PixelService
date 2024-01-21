using System.Text;
using Application.Configuration;
using Confluent.Kafka;
using Contracts;
using Contracts;
using Microsoft.Extensions.Logging;
using ProtoBuf;

namespace Application.Services;

public class KafkaService : IKafkaService
{
    private readonly IKafkaConfiguration kafkaConfiguration;
    private readonly IProducer<string, byte[]> producer;

    public KafkaService(
        ISettings settings,
        IProducer<string, byte[]> producer)
    {
        this.kafkaConfiguration = settings.Kafka;
        this.producer = producer;
    }

    public async Task Produce(IMessage message)
    {
        var topics = this.kafkaConfiguration.Producer.TopicConfigurations
            .ToDictionary(config => config.Contract, config => config.Name);

        var messageType = message.GetType().ToString().Split('.').Last();

        byte[] serializedMessage;
        using (MemoryStream stream = new MemoryStream())
        {
            Serializer.Serialize(stream, message);
            serializedMessage = stream.ToArray();
        }

        var headers = new Headers
        {
            new Header(MessageHeaders.MESSAGE_TYPE, Encoding.UTF8.GetBytes(message.GetType().ToString()))
        };

        var protoMessage = new Message<string, byte[]>()
        {
            Key = message.RoutingKey,
            Value = serializedMessage,
            Timestamp = new Timestamp(DateTime.UtcNow),
            Headers = headers
        };

        await this.producer.ProduceAsync(topics[messageType], protoMessage);
    }
}