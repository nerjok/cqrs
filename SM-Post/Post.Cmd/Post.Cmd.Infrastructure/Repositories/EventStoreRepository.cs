using CQRS.Core.Domain;
using CQRS.Core.Events;
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
        return await Task.FromResult(dt);
    }

    public async Task SaveAsync(EventModel @event)
    {
        var eventData = new EventEntity
        {
            Id = Guid.NewGuid(),
            Version = @event.Version,
            AggregateId = @event.Guid,
            DatePosted = @event.TimeStamp,
            Data = JsonConvert.SerializeObject(@event.EventData)
        };

         _dataContext.events.Add(eventData);
         var dt = await _dataContext.SaveChangesAsync();

         if(dt != null) {

         }
        //  return dt;
        // TODO persist to sqlLite
        // throw new NotImplementedException("Please implemend databas estorage");
        //  return Task.CompletedTask;
    }
}
