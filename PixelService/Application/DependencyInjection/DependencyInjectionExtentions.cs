using Application.Configuration;
using Application.Services;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DependencyInjection;

public static class DependencyInjectionExtentions
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        ISettings settings)
    {
        return services
            .AddKafka(settings.Kafka);
    }

    private static IServiceCollection AddKafka(
        this IServiceCollection services,
        IKafkaConfiguration kafkaConfiguration)
    {
        services.AddSingleton<IAdminClient>(serviceProvider => new AdminClientBuilder(new AdminClientConfig
        {
            BootstrapServers = kafkaConfiguration.BootstrapServer
        }).Build());

        services.AddSingleton<IProducer<string, byte[]>>(serviceProvider =>
        {
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = kafkaConfiguration.BootstrapServer,
                ClientId = kafkaConfiguration.ClientId,
                Acks = Acks.All,
            };

            return new ProducerBuilder<string, byte[]>(producerConfig).Build();
        });

        services.AddSingleton<IKafkaService, KafkaService>();

        return services;
    }
}