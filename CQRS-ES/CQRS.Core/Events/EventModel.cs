using MongoDB.Bson.Serialization.Attributes;

namespace CQRS.Core.Events;

public class EventModel
{
    public Guid Guid { get; set; }
    // [BsonId]
    // [BsonRepresentation(BsonType.Objectid)]
    public string Id { get; set; }

    public DateTime TimeStamp { get; set; }

    public Guid AggregateIdentifier { get; set; }
    public string AggregateType { get; set; }
    public int Version { get; set; }
    public string EventType { get; set; }
    public BaseEvent EventData { get; set; }

}
