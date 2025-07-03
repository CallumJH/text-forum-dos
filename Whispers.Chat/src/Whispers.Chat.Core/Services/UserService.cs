using Microsoft.AspNetCore.Identity;
using Whispers.Chat.Core.BoundedContexts.IdentityAndUsers.Aggregates;
using Whispers.Chat.Core.BoundedContexts.IdentityAndUsers.Events;
using Whispers.Chat.Core.Generated.Services;
using Whispers.Chat.Core.Interfaces;
using Whispers.Chat.Core.Specifications;

namespace Whispers.Chat.Core.BoundedContexts.IdentityAndUsers.Services;

/// <summary>
/// Domain service for User aggregate operations including creation, authentication, and profile management.
/// Uses ServiceErrorHandler for consistent error handling and logging.
/// </summary>
public class UserService(
    IRepository<User> _repository,
    IMediator _mediator,
    ILogger<UserService> _logger,
    IPasswordHasher<User> _passwordHasher) : IUserService
{
  public async Task<Result<User>> CreateUserAsync(string username, string email, string password, Guid createdBy)
  {
    return await ServiceErrorHandler.ExecuteAsync(
        operation: async () => await CreateUserInternalAsync(username, email, password, createdBy),
        logger: _logger,
        operationName: nameof(CreateUserAsync),
        fallbackErrorMessage: "Failed to create user",
        logParameters: new object[] { username, email, createdBy });
  }

  public async Task<Result<User>> LoginAsync(string usernameOrEmail, string password)
  {
    return await ServiceErrorHandler.ExecuteAsync(
        operation: async () => await LoginInternalAsync(usernameOrEmail, password),
        logger: _logger,
        operationName: nameof(LoginAsync),
        fallbackErrorMessage: "Login failed",
        logParameters: new object[] { usernameOrEmail });
  }

  public async Task<Result<User>> CreateAnonymousUserAsync()
  {
    return await ServiceErrorHandler.ExecuteAsync(
        operation: async () => await CreateAnonymousUserInternalAsync(),
        logger: _logger,
        operationName: nameof(CreateAnonymousUserAsync),
        fallbackErrorMessage: "Failed to create anonymous user",
        logParameters: Array.Empty<object>());
  }

  public async Task<Result> UpdateUserProfileAsync(Guid userId, string username, string email, Guid updatedBy)
  {
    return await ServiceErrorHandler.ExecuteAsync(
        operation: async () => await UpdateUserProfileInternalAsync(userId, username, email, updatedBy),
        logger: _logger,
        operationName: nameof(UpdateUserProfileAsync),
        fallbackErrorMessage: "Failed to update profile",
        logParameters: new object[] { userId, username, email, updatedBy });
  }

  public async Task<Result> UpdateUserPasswordAsync(Guid userId, string currentPassword, string newPassword, Guid updatedBy)
  {
    return await ServiceErrorHandler.ExecuteAsync(
        operation: async () => await UpdateUserPasswordInternalAsync(userId, currentPassword, newPassword, updatedBy),
        logger: _logger,
        operationName: nameof(UpdateUserPasswordAsync),
        fallbackErrorMessage: "Failed to update password",
        logParameters: new object[] { userId, updatedBy });
  }

  #region Internal Implementation Methods

  private async Task<Result<User>> CreateUserInternalAsync(string username, string email, string password, Guid createdBy)
  {
    // Check if username already exists
    var existingUserByUsername = await _repository.FirstOrDefaultAsync(new UserByUsernameSpec(username));
    if (existingUserByUsername != null)
    {
      return Result<User>.Error("Username already exists");
    }

    // Check if email already exists
    var existingUserByEmail = await _repository.FirstOrDefaultAsync(new UserByUsernameSpec(email));
    if (existingUserByEmail != null)
    {
      return Result<User>.Error("Email already exists");
    }

    // Create the user aggregate
    var user = new User(username, email, createdBy);
    if(user is null)
    {
      return Result<User>.Error("User cannot be created");
    }

    // Hash and set the password
    user.PasswordHash = _passwordHasher.HashPassword(user, password);

    // Save to repository
    await _repository.AddAsync(user);

    // Publish domain event
    var domainEvent = new UserRegisteredEvent(user.Id, user.UserName, user.Email, user.IsAnonymous);
    await _mediator.Publish(domainEvent);

    return Result<User>.Success(user);
  }

  private async Task<Result<User>> LoginInternalAsync(string usernameOrEmail, string password)
  {
    // Find user by username or email
    // If time allows I would change this to return all values and throw a domain violation if we have more than one
    var user = await _repository.FirstOrDefaultAsync(new UserByUsernameOrEmailSpec(usernameOrEmail));

    if (user == null)
    {
      return Result<User>.Error("Invalid username/email or password");
    }

    if (user.IsAnonymous)
    {
      return Result<User>.Error("Invalid username/email or password");
    }

    if (string.IsNullOrEmpty(user.PasswordHash))
    {
      throw new DomainException($"User {user.UserName} does not have a password set.");
    }

    // Verify password
    var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
    if (passwordVerificationResult != PasswordVerificationResult.Success)
    {
      return Result<User>.Error("Invalid username/email or password");
    }

    // Publish domain event
    var domainEvent = new UserLoggedInEvent(user.Id, user.UserName);
    await _mediator.Publish(domainEvent);

    return Result<User>.Success(user);
  }

  private async Task<Result<User>> CreateAnonymousUserInternalAsync()
  {
    var user = User.CreateAnonymous();
    await _repository.AddAsync(user);

    var domainEvent = new UserRegisteredEvent(user.Id, user.UserName, user.Email, user.IsAnonymous);
    await _mediator.Publish(domainEvent);

    return Result<User>.Success(user);
  }

  private async Task<Result> UpdateUserProfileInternalAsync(Guid userId, string username, string email, Guid updatedBy)
  {
    var user = await _repository.GetByIdAsync(userId);
    if (user == null)
    {
      return Result.NotFound();
    }

    // Check if new username is taken by another user
    if (user.UserName != username)
    {
      var existingUser = await _repository.FirstOrDefaultAsync(new UserByUsernameSpec(username));
      if (existingUser != null)
      {
        return Result.Error("Username already exists");
      }
    }

    // Check if new email is taken by another user
    if (user.Email != email)
    {
      var existingUser = await _repository.FirstOrDefaultAsync(new UserByUsernameSpec(email));
      if (existingUser != null)
      {
        return Result.Error("Email already exists");
      }
    }

    // Update the profile using domain method
    user.UpdateProfile(username, email, updatedBy);
    await _repository.UpdateAsync(user);

    // Publish domain event
    var domainEvent = new UserProfileUpdatedEvent(user.Id, username, email);
    await _mediator.Publish(domainEvent);

    return Result.Success();
  }

  private async Task<Result> UpdateUserPasswordInternalAsync(Guid userId, string currentPassword, string newPassword, Guid updatedBy)
  {
    var user = await _repository.GetByIdAsync(userId);
    if (user == null)
    {
      return Result.NotFound();
    }

    if (string.IsNullOrEmpty(user.PasswordHash))
    {
      throw new DomainException($"User {user.UserName} does not have a password set.");
    }

    // Verify current password
    var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, currentPassword);
    if (passwordVerificationResult != PasswordVerificationResult.Success)
    {
      return Result.Error("Current password is incorrect");
    }

    // Hash new password
    var newPasswordHash = _passwordHasher.HashPassword(user, newPassword);

    // Update password using domain method
    user.UpdatePassword(newPasswordHash, updatedBy);
    await _repository.UpdateAsync(user);

    // Publish domain event
    var domainEvent = new UserPasswordUpdatedEvent(user.Id);
    await _mediator.Publish(domainEvent);

    return Result.Success();
  }

  #endregion
}
