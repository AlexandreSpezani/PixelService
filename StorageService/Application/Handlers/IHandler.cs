using Contracts;

namespace Application.Handlers;

public interface IHandler
{
    public Type ContractType { get; }
    public Task Handle(IMessage message);
}