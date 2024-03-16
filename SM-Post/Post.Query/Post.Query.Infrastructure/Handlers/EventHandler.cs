using CQRS.Core.Hubs;
using Microsoft.AspNetCore.SignalR;
using Post.Common.Events;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.Hubs;

namespace Post.Query.Infrastructure.Handlers;

public class EventHandler : IEventHandler
{
    private readonly IPostRepository _postRepository;
    private readonly ICommentsRepository _commentRepository;
    private readonly IHubContext<ChatHub, IChatHub> _chatHub;


    public EventHandler(IPostRepository postRepository, ICommentsRepository commentRepository, IHubContext<ChatHub, IChatHub> chatHub)
    {
        _postRepository = postRepository;
        _commentRepository = commentRepository;
        _chatHub = chatHub;
    }

    public async Task On(PostCreatedEvent @event)
    {
        var post = new PostEntity
        {
            PostId = @event.Id,
            Author = @event.Author,
            DatePosted = @event.DatePosted,
            Message = @event.Message,
        };

        await _postRepository.CreateAsync(post);
        await _chatHub.Clients.All.SendMessage("postUpdated");
    }

    public async Task On(MessageUpdatedEvent @event)
    {
        var post = await _postRepository.GetByIdAsync(@event.Id);
        if (post == null) return;
        post.Message = @event.Message;
        post.Version = @event.Version;
        await _postRepository.EditAsync(post);
        await _chatHub.Clients.All.SendMessage("postUpdated");
    }

    public async Task On(PostLikedEvent @event)
    {
        var post = await _postRepository.GetByIdAsync(@event.Id);
        if (post == null) return;
        post.Likes++;
        await _postRepository.EditAsync(post);
    }

    public async Task On(CommentAddedEvent @event)
    {
        var comment = new CommentEntity
        {
            PostId = @event.Id,
            CommentId = @event.CommentId,
            CommentDate = @event.CommentDate,
            Comment = @event.Comment,
            Username = @event.Username,
            Edited = false
        };
        await _commentRepository.CreateAsync(comment);
    }

    public async Task On(CommentUpdatedEvent @event)
    {
        var comment = await _commentRepository.GetByIdAsync(@event.Id);
        if (comment == null) return;
        comment.Comment = @event.Comment;
        comment.Edited = true;
        comment.CommentDate = @event.CommentDate;
        await _commentRepository.UpdateAsync(comment);
    }

    public async Task On(CommentRemovedEvent @event)
    {
        await _commentRepository.DeleteAsync(@event.Id);
    }

    public async Task On(PostRemovedEvent @event)
    {
        await _postRepository.DeleteAsync(@event.Id);
        await _chatHub.Clients.All.SendMessage("postUpdated");
    }
}

