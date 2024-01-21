namespace Application.Configuration;

public interface IKafkaConfiguration
{
    public string BootstrapServer { get; }
    public string ClientId { get; }

    public Producer Producer { get; }
}