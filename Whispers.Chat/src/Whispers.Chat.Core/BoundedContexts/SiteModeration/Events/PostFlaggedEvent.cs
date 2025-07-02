namespace Whispers.Chat.Core.BoundedContexts.SiteModeration.Events;

/// <summary>
/// Event raised when a post is flagged by a moderator
/// </summary>
public class PostFlaggedEvent : DomainEventBase
{
  public Guid PostId { get; }
  public Guid ModeratorId { get; }
  public bool IsMisleading { get; }
  public bool IsFalseInformation { get; }

  public PostFlaggedEvent(Guid postId, Guid moderatorId, bool isMisleading, bool isFalseInformation)
  {
    PostId = postId;
    ModeratorId = moderatorId;
    IsMisleading = isMisleading;
    IsFalseInformation = isFalseInformation;
  }
} 
