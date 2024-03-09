using CQRS.Core.Domain;

namespace CQRS.Core.Handlers;

public interface IEventSourcingHander<T>
{
    Task SaveAsync(AggregateRoot aggregate);
    Task<T> GetByAsync(Guid aggregateId);
}
