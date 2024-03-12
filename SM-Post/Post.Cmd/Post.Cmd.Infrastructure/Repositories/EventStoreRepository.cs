using CQRS.Core.Domain;
using CQRS.Core.Events;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
        // TODO persist to sqlLite
        // throw new NotImplementedException("FindByAggregateId not implemented");
        var dt = new List<EventModel>
        {
            new() { Guid = Guid.NewGuid(), Version = 1},

        };
        var events = _dataContext.events.Where(x => x.AggregateId == aggregateId).Select(x => new EventModel
        {
            AggregateIdentifier = x.AggregateId,
            Guid = x.Id,
            EventData =x.CastedContent
        }).ToListAsync();

        return await events;
        // await context.Post.AsNoTracking().Include(p => p.Comments).Where(x => x.Author == author).ToListAsync();
        return await Task.FromResult(dt);
    }

    public async Task SaveAsync(EventModel @event)
    {
        var eventData = new EventEntity
        {
            Id = Guid.NewGuid(),
            Version = @event.Version,
            AggregateId = @event.AggregateIdentifier,
            DatePosted = @event.TimeStamp,
            Data = JsonConvert.SerializeObject(@event.EventData),
            CastedContent=@event.EventData
        };

        _dataContext.events.Add(eventData);
        var dt = await _dataContext.SaveChangesAsync();

        if (dt != null)
        {

        }
        //  return dt;
        // TODO persist to sqlLite
        // throw new NotImplementedException("Please implemend databas estorage");
        //  return Task.CompletedTask;
    }
}
