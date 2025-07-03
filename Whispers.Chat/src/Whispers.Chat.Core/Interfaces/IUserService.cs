using Whispers.Chat.Core.BoundedContexts.IdentityAndUsers.Aggregates;

namespace Whispers.Chat.Core.Interfaces;
public interface IUserService
{
  Task<Result<User>> CreateUserAsync(string username, string email, string password, Guid createdBy);
  Task<Result<User>> LoginAsync(string usernameOrEmail, string password);
  Task<Result<User>> CreateAnonymousUserAsync();
  Task<Result> UpdateUserProfileAsync(Guid userId, string username, string email, Guid updatedBy);
  Task<Result> UpdateUserPasswordAsync(Guid userId, string currentPassword, string newPassword, Guid updatedBy);
}
