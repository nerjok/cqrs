using CQRS.Core.Domain;
using CQRS.Core.Events;

namespace Post.Cmd.Infrastructure.Repositories;

public class EventStoreRepository : IEventStoreRepository
{
    public Task<List<EventModel>> FindByAggregateId(Guid aggregateId)
    {
        // TODO persist to sqlLite
        throw new NotImplementedException();
    }

    public Task SaveAsync(EventModel @event)
    {
        // TODO persist to sqlLite
        throw new NotImplementedException();
    }
}
