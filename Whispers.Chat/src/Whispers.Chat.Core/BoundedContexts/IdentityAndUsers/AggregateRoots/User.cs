using Microsoft.AspNetCore.Identity;

namespace Whispers.Chat.Core.BoundedContexts.IdentityAndUsers.Aggregates;

public class User : IdentityUser<Guid>, IAggregateRoot, IAuditableEntity
{
  public List<Guid> LikedPostIds { get; private set; } = new();
  public bool IsAnonymous { get; private set; }

  public DateTime DateCreated { get; private init; } = DateTime.UtcNow;
  public DateTime? DateUpdated { get; private set; }
  public Guid CreatedBy { get; private init; }
  public Guid? UpdatedBy { get; private set; }

  protected User()
  {
    Id = Guid.NewGuid();
  }

  public User(string username, string email, Guid createdBy) : this()
  {
    UserName = Guard.Against.NullOrEmpty(username, nameof(username));
    Email = Guard.Against.NullOrEmpty(email, nameof(email));
    CreatedBy = createdBy;
    IsAnonymous = false;
  }

  public static User CreateAnonymous()
  {
    return new User
    {
      UserName = $"Anonymous_{Guid.NewGuid()}",
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

    UserName = Guard.Against.NullOrEmpty(username, nameof(username));
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
