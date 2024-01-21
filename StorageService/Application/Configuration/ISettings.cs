namespace Application.Configuration;

public interface ISettings
{
    public KafkaConfiguration Kafka { get; }

    public DataConfiguration Data { get; }
}