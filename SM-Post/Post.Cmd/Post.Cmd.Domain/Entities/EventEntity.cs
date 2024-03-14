using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using CQRS.Core.Events;

namespace Post.Cmd.Domain.Entities;

public class EventEntity
{
    public Guid Id { get; set; }
    public Guid AggregateId { get; set; }
    public string Author { get; set; }
    public DateTime DatePosted { get; set; }
    public int Version { get; set; }
    public string Content { get; set; }
    [NotMapped]
    public BaseEvent CastedContent
    {
        get
        {
            var options = new JsonSerializerOptions { Converters = { new Common.Converters.EventJsonConverter() } };
            var @event = System.Text.Json.JsonSerializer.Deserialize<BaseEvent>(Content, options);
            return @event;
        }
        set
        {
            var serializedString = System.Text.Json.JsonSerializer.Serialize(value, value.GetType());
            Content = serializedString;
        }
    }
}

