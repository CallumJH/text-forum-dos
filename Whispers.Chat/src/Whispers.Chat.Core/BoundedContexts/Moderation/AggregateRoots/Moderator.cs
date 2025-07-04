namespace Whispers.Chat.Core.BoundedContexts.SiteModeration.Aggregates;

public class Moderator : EntityBase<Guid>, IAggregateRoot, IAuditableEntity
{
  public Guid UserId { get; private set; }
  public bool IsActive { get; private set; }
  private readonly List<Guid> _moderatedPostIds = new();
  public IReadOnlyCollection<Guid> ModeratedPostIds => _moderatedPostIds.AsReadOnly();

  public DateTime DateCreated { get; private init; } = DateTime.UtcNow;
  public DateTime? DateUpdated { get; private set; }
  public Guid CreatedBy { get; private init; }
  public Guid? UpdatedBy { get; private set; }

  #region EFCore constructor
  protected Moderator()
  {
    Id = Guid.NewGuid();
  }
  #endregion

  public Moderator(Guid userId, Guid createdBy) : this()
  {
    UserId = userId;
    CreatedBy = createdBy;
    IsActive = true;
  }

  public void DeactivateModerator(Guid updatedBy)
  {
    IsActive = false;
    DateUpdated = DateTime.UtcNow;
    UpdatedBy = updatedBy;
  }

  public void ReactivateModerator(Guid updatedBy)
  {
    IsActive = true;
    DateUpdated = DateTime.UtcNow;
    UpdatedBy = updatedBy;
  }

  public void MarkPostAsMisleading(Guid postId, Guid updatedBy)
  {
    if (!IsActive)
    {
      throw new DomainException("Inactive moderators cannot moderate posts");
    }

    if (!_moderatedPostIds.Contains(postId))
    {
      _moderatedPostIds.Add(postId);
    }
    DateUpdated = DateTime.UtcNow;
    UpdatedBy = updatedBy;
  }

  public void MarkPostAsFalseInformation(Guid postId, Guid updatedBy)
  {
    if (!IsActive)
    {
      throw new DomainException("Inactive moderators cannot moderate posts");
    }

    if (!_moderatedPostIds.Contains(postId))
    {
      _moderatedPostIds.Add(postId);
    }
    DateUpdated = DateTime.UtcNow;
    UpdatedBy = updatedBy;
  }

  // Tracking posts that a moderator has actively managed
  public bool HasModeratedPost(Guid postId)
  {
    return _moderatedPostIds.Contains(postId);
  }
}
