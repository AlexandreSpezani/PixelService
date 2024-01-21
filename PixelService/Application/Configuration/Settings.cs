namespace Application.Configuration;

public class Settings : ISettings
{
    public DataConfiguration Data { get; set; } = null!;
    public KafkaConfiguration Kafka { get; set; } = null!;
}