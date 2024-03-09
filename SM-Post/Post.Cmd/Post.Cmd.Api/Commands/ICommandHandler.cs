namespace Post.Cmd.Api.Commands;

public interface ICommandHandler
{
    Task HandleAsync(PostCommand command);
    Task HandleAsync(EditMessageCommand command);
    Task HandleAsync(LikesPostCommand command);
    Task HandleAsync(AddCommentCommand command);
    Task HandleAsync(EditCommentCommand command);
    Task HandleAsync(RemoveCommentCommand command);
    Task HandleAsync(DeletePostCommand command);
}
