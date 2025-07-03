namespace Whispers.Chat.Core.BoundedContexts.Posts.Events;

public record PostCreatedEvent : BaseDomainEvent
{
  public Guid PostId { get; }
  public Guid AuthorId { get; }
  public string Content { get; }

  public PostCreatedEvent(Guid postId, Guid authorId, string content)
  {
    PostId = postId;
    AuthorId = authorId;
    Content = content;
  }
} 
