using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace Post.Query.Domain.Entities;

[Table("Post")]
public class PostEntity
{
    [Key]
    public Guid PostId { get; set; }
    public string Author { get; set; }
    public DateTime DatePosted { get; set; }

    public string Message { get; set; }
    public int Likes { get; set; }
    // public virtual ICollection<CommentEntity> Comments { get; set; }

    public int Version { get; set; }
}
