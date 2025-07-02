namespace Whispers.Chat.Core.BoundedContexts.Posts.Events;

/// <summary>
/// Event raised when a new post is created
/// </summary>
public class PostCreatedEvent : DomainEventBase
{
  public Guid PostId { get; }
  public string Title { get; }
  public Guid AuthorId { get; }
  public List<string> Tags { get; }

  public PostCreatedEvent(Guid postId, string title, Guid authorId, List<string> tags)
  {
    PostId = postId;
    Title = title;
    AuthorId = authorId;
    Tags = tags;
  }
} 
