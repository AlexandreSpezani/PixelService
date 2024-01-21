using Contracts;

namespace Application.Services;

public interface IKafkaService
{
    public Task Produce(IMessage message);
}