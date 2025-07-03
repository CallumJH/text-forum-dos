namespace Whispers.Chat.Core.BoundedContexts.SiteModeration.Events;

public record PostFlaggedEvent : BaseDomainEvent
{
  public Guid PostId { get; }
  public Guid ModeratorId { get; }
  public string Reason { get; }

  public PostFlaggedEvent(Guid postId, Guid moderatorId, string reason)
  {
    PostId = postId;
    ModeratorId = moderatorId;
    Reason = reason;
  }
} 
