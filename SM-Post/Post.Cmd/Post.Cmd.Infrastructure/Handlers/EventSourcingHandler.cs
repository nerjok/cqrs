
using CQRS.Core.Domain;
using CQRS.Core.Handlers;
using Post.Cmd.Domain.Aggregates;
using Post.Cmd.Infrastructure.Dispatchers;

namespace Post.Common.Handlers;

public class EventSourcingHandler : IEventSourcingHandler<PostAggregate>
{
    private IEventStore _eventStore;
    public EventSourcingHandler(IEventStore eventStore)
    {
        _eventStore = eventStore;
    }
    public async Task<PostAggregate> GetByAsync(Guid aggregateId)
    {
        var aggregate = new PostAggregate();
        var events = await _eventStore.GetEventsAsync(aggregateId);

        if (events == null || !events.Any())
        {
            return aggregate;
        }
        aggregate.ReplayEvents(events);
        var latestVersion = events.Select(x => x.Version).Max();
        aggregate.Version = latestVersion;
        return aggregate;
    }

    public async Task SaveAsync(AggregateRoot aggregate)
    {
        await _eventStore.SaveEventsAsync(aggregate.Id, aggregate.GetUncommittedChanges(), aggregate.Version);
        aggregate.MarkChangesAsCommitted();
    }
}
