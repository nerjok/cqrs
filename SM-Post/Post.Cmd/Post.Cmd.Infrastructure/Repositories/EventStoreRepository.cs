using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Domain;
using CQRS.Core.Events;

namespace Post.Cmd.Infrastructure.Repositories
{
    public class EventStoreRepository : IEventStoreRepository
    {
        public Task<List<EventModel>> FindByAggregateId(Guid aggregateId)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(EventModel @event)
        {
            throw new NotImplementedException();
        }
    }
}