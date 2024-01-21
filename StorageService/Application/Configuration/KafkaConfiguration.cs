namespace Application.Configuration;

public class KafkaConfiguration : IKafkaConfiguration
{
    public string BootstrapServer { get; set; } = null!;

    public string ClientId { get; set; } = null!;

    public IEnumerable<string> Topics { get; set; } = Array.Empty<string>();
}