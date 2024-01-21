namespace Contracts;

public interface IMessage
{
    string RoutingKey { get; }
}