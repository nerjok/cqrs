using CQRS.Core.Events;

namespace Post.Cmd.Infrastructure.Dispatchers;

public interface IEventStore
{
    Task SaveEventsAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion);
    Task<List<BaseEvent>> GetEventsAsync(Guid aggregate);
}
