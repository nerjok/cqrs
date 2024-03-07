using CQRS.Core.Events;

namespace Post.Common.Events;

public class PostRemovedEvent : BaseEvent
{
    protected PostRemovedEvent() : base(nameof(PostRemovedEvent))
    {
    }
}
