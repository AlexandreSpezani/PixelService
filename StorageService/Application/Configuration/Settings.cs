namespace Application.Configuration;

public class Settings : ISettings
{
    public KafkaConfiguration Kafka { get; init; } = null!;
    public DataConfiguration Data { get; init; } = null!;
}