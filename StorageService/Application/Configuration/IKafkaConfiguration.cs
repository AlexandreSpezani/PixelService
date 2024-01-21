namespace Application.Configuration;

public interface IKafkaConfiguration
{
    public string BootstrapServer { get; }

    public string ClientId { get; }

    public IEnumerable<string> Topics { get; }
}