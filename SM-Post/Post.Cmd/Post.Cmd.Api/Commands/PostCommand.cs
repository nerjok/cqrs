using CQRS.Core.Commands;

namespace Post.Cmd.Api.Commands;

public class PostCommand : BaseCommand
{
    public string Author { get; set; }
    public string Message { get; set; }
}
