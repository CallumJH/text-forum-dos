namespace Whispers.Chat.Core.BoundedContexts.IdentityAndUsers.Aggregates;

public class User : EntityBase<Guid>, IAggregateRoot, IAuditableEntity
{
  public string Username { get; private set; } = string.Empty;
  public string Email { get; private set; } = string.Empty;
  public string PasswordHash { get; private set; } = string.Empty;
  public List<Guid> LikedPostIds { get; private set; } = new();
  public bool IsAnonymous { get; private set; }

  public DateTime DateCreated { get; private init; } = DateTime.UtcNow;
  public DateTime? DateUpdated { get; private set; }
  public Guid CreatedBy { get; private init; }
  public Guid? UpdatedBy { get; private set; }

  #region EFCore constructor
  protected User()
  {
    Id = Guid.NewGuid();
  }
  #endregion

  public User(string username, string email, string passwordHash, Guid createdBy) : this()
  {
    Username = Guard.Against.NullOrEmpty(username, nameof(username));
    Email = Guard.Against.NullOrEmpty(email, nameof(email));
    PasswordHash = Guard.Against.NullOrEmpty(passwordHash, nameof(passwordHash));
    CreatedBy = createdBy;
    IsAnonymous = false;
  }

  public static User CreateAnonymous()
  {
    return new User
    {
      Username = $"Anonymous_{Guid.NewGuid()}",
      IsAnonymous = true,
      CreatedBy = Guid.Empty // System user
    };
  }

  public bool CanLikePost(Guid postId, Guid postAuthorId)
  {
    return !IsAnonymous && 
           !LikedPostIds.Contains(postId) && 
           Id != postAuthorId;
  }

  public void LikePost(Guid postId, Guid postAuthorId, Guid updatedBy)
  {
    if (!CanLikePost(postId, postAuthorId))
    {
      throw new DomainException("User cannot like this post");
    }

    LikedPostIds.Add(postId);
    DateUpdated = DateTime.UtcNow;
    UpdatedBy = updatedBy;
  }

  public void UpdateProfile(string username, string email, Guid updatedBy)
  {
    if (IsAnonymous)
    {
      throw new DomainException("Anonymous users cannot update their profile");
    }

    Username = Guard.Against.NullOrEmpty(username, nameof(username));
    Email = Guard.Against.NullOrEmpty(email, nameof(email));
    DateUpdated = DateTime.UtcNow;
    UpdatedBy = updatedBy;
  }

  public void UpdatePassword(string newPasswordHash, Guid updatedBy)
  {
    if (IsAnonymous)
    {
      throw new DomainException("Anonymous users cannot update their password");
    }

    PasswordHash = Guard.Against.NullOrEmpty(newPasswordHash, nameof(newPasswordHash));
    DateUpdated = DateTime.UtcNow;
    UpdatedBy = updatedBy;
  }
}
