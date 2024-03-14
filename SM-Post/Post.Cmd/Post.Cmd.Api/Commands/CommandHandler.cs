using CQRS.Core.Handlers;

using Post.Cmd.Domain.Aggregates;

namespace Post.Cmd.Api.Commands;

public class CommandHandler : ICommandHandler
{
    private readonly IEventSourcingHandler<PostAggregate> _eventSourceHandler;
    public CommandHandler(IEventSourcingHandler<PostAggregate> eventSourceHandler)
    {
        _eventSourceHandler = eventSourceHandler;
    }
    public async Task HandleAsync(PostCommand command)
    {
        var aggregate = new PostAggregate(command.Id, command.Author, command.Message);
        await _eventSourceHandler.SaveAsync(aggregate);
    }

    public async Task HandleAsync(EditMessageCommand command)
    {
        var aggregate = await _eventSourceHandler.GetByAsync(command.Id);
        aggregate.EditMessage(command.Message);
        await _eventSourceHandler.SaveAsync(aggregate);
    }

    public async Task HandleAsync(LikesPostCommand command)
    {
        var aggregate = await _eventSourceHandler.GetByAsync(command.Id);
        aggregate.LikePost();
        await _eventSourceHandler.SaveAsync(aggregate);
    }

    public async Task HandleAsync(AddCommentCommand command)
    {
        var aggregate = await _eventSourceHandler.GetByAsync(command.Id);
        aggregate.AddComment(command.Comment, command.Username);
        await _eventSourceHandler.SaveAsync(aggregate);
    }

    public async Task HandleAsync(EditCommentCommand command)
    {
        var aggregate = await _eventSourceHandler.GetByAsync(command.Id);
        aggregate.EditComment(command.CommentId, command.Comment, command.Username);
        await _eventSourceHandler.SaveAsync(aggregate);
    }

    public async Task HandleAsync(RemoveCommentCommand command)
    {
        var aggregate = await _eventSourceHandler.GetByAsync(command.Id);
        aggregate.RemoveComment(command.CommentId, command.Username);
        await _eventSourceHandler.SaveAsync(aggregate);
    }

    public async Task HandleAsync(DeletePostCommand command)
    {
        var aggregate = await _eventSourceHandler.GetByAsync(command.Id);
        aggregate.DeletePost(command.Username);
        await _eventSourceHandler.SaveAsync(aggregate);
    }
}
