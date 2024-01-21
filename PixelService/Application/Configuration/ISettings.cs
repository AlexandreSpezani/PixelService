namespace Application.Configuration;

public interface ISettings
{
    public DataConfiguration Data { get; }
    public KafkaConfiguration Kafka { get; }
}