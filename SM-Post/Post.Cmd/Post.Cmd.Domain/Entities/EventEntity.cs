
using System.ComponentModel.DataAnnotations.Schema;


namespace Post.Cmd.Domain.Entities;

public class EventEntity
{
    public Guid Id { get; set; }
    public Guid AggregateId { get; set; }
    public string Author { get; set; }
    public DateTime DatePosted { get; set; }

    [Column(TypeName = "longtext")]
    public string Data { get; set; }
    public int Version { get; set; }

}

