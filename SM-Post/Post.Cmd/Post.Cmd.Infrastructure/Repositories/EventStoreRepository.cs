using CQRS.Core.Domain;
using CQRS.Core.Events;
using Microsoft.EntityFrameworkCore;
using Post.Cmd.Domain.Entities;
using Post.Cmd.Infrastructure.DataAccess;

namespace Post.Cmd.Infrastructure.Repositories;

public class EventStoreRepository : IEventStoreRepository
{
    private readonly DataContext _dataContext;

    public EventStoreRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<List<EventModel>> FindByAggregateId(Guid aggregateId)
    {
        var events = _dataContext.events.Where(x => x.AggregateId == aggregateId).Select(x => new EventModel
        {
            AggregateIdentifier = x.AggregateId,
            Guid = x.Id,
            EventData = x.CastedContent,
            TimeStamp = x.DatePosted,
            Version = x.Version
        }).OrderBy(x => x.TimeStamp).ToListAsync();

        return await events;
    }

    public async Task SaveAsync(EventModel @event)
    {
        var eventData = new EventEntity
        {
            Id = Guid.NewGuid(),
            Version = @event.Version,
            // Author= @event.EventData
            AggregateId = @event.AggregateIdentifier,
            DatePosted = @event.TimeStamp,
            CastedContent = @event.EventData
        };

        _dataContext.events.Add(eventData);
        await _dataContext.SaveChangesAsync();
    }
}
