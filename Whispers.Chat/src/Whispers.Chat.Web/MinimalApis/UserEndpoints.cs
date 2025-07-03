//using Ardalis.SharedKernel;
//using Whispers.Chat.Core.BoundedContexts.IdentityAndUsers.Aggregates;
//using Whispers.Chat.Core.BoundedContexts.IdentityAndUsers.Services;
//using Whispers.Chat.Core.Generated.Services;
//using Whispers.Chat.Core.Interfaces;
//using Whispers.Chat.SharedKernel;

//namespace Whispers.Chat.Web.MinimalApis;

//public static class UserEndpoints
//{
//  public static void MapUserEndpoints(this IEndpointRouteBuilder app)
//  {
//    var group = app.MapGroup("/api/users")
//        .WithTags("Users")
//        .WithOpenApi();

//    // Register new user
//    group.MapPost("/register", async (
//        [FromBody] RegisterUserRequest request,
//        [FromServices] IUserService userService) =>
//    {
//      var result = await userService.CreateUserAsync(
//          request.Username,
//          request.Email,
//          request.Password,
//          request.CreatedBy);

//      return result.IsSuccess
//          ? Results.Ok(new { UserId = result.Value.Id, Username = result.Value.Username })
//          : Results.BadRequest(new { Error = ServiceErrorHandler.FormatResultErrors(result) });
//    });

//    // Login
//    group.MapPost("/login", async (
//        [FromBody] LoginRequest request,
//        [FromServices] IUserService userService) =>
//    {
//      var result = await userService.LoginAsync(
//          request.UsernameOrEmail,
//          request.Password);

//      return result.IsSuccess
//          ? Results.Ok(new
//          {
//            UserId = result.Value.Id,
//            Username = result.Value.Username,
//            Email = result.Value.Email,
//            IsAnonymous = result.Value.IsAnonymous
//          })
//          : Results.BadRequest(new { Error = ServiceErrorHandler.FormatResultErrors(result) });
//    });

//    // Create anonymous user
//    group.MapPost("/anonymous", async (
//        [FromServices] IUserService userService) =>
//    {
//      var result = await userService.CreateAnonymousUserAsync();

//      return result.IsSuccess
//          ? Results.Ok(new
//          {
//            UserId = result.Value.Id,
//            Username = result.Value.Username,
//            IsAnonymous = result.Value.IsAnonymous
//          })
//          : Results.BadRequest(new { Error = ServiceErrorHandler.FormatResultErrors(result) });
//    });

//    // Update profile
//    group.MapPut("/{userId}/profile", async (
//        Guid userId,
//        [FromBody] UpdateProfileRequest request,
//        [FromServices] IUserService userService) =>
//    {
//      var result = await userService.UpdateUserProfileAsync(
//          userId,
//          request.Username,
//          request.Email,
//          request.UpdatedBy);

//      if (result.IsSuccess)
//        return Results.Ok(new { Message = "Profile updated successfully" });

//      return result.Status switch
//      {
//        ResultStatus.NotFound => Results.NotFound(new { Error = "User not found" }),
//        _ => Results.BadRequest(new { Error = ServiceErrorHandler.FormatResultErrors(result) })
//      };
//    });

//    // Update password
//    group.MapPut("/{userId}/password", async (
//        Guid userId,
//        [FromBody] UpdatePasswordRequest request,
//        [FromServices] IUserService userService) =>
//    {
//      var result = await userService.UpdateUserPasswordAsync(
//          userId,
//          request.CurrentPassword,
//          request.NewPassword,
//          request.UpdatedBy);

//      if (result.IsSuccess)
//        return Results.Ok(new { Message = "Password updated successfully" });

//      return result.Status switch
//      {
//        ResultStatus.NotFound => Results.NotFound(new { Error = "User not found" }),
//        _ => Results.BadRequest(new { Error = ServiceErrorHandler.FormatResultErrors(result) })
//      };
//    });

//    // Get user by ID (additional endpoint)
//    group.MapGet("/{userId}", async (
//        Guid userId,
//        [FromServices] IUserService userService,
//        [FromServices] IRepository<User> repository) =>
//    {
//      var user = await repository.GetByIdAsync(userId);

//      return user != null
//          ? Results.Ok(new
//          {
//            UserId = user.Id,
//            Username = user.Username,
//            Email = user.Email,
//            IsAnonymous = user.IsAnonymous,
//            DateCreated = user.DateCreated,
//            DateUpdated = user.DateUpdated
//          })
//          : Results.NotFound(new { Error = "User not found" });
//    });

//    // Like a post (additional endpoint using User aggregate method)
//    group.MapPost("/{userId}/like-post", async (
//        Guid userId,
//        [FromBody] LikePostRequest request,
//        [FromServices] IRepository<User> repository) =>
//    {
//      var user = await repository.GetByIdAsync(userId);
//      if (user == null)
//        return Results.NotFound(new { Error = "User not found" });

//      if (!user.CanLikePost(request.PostId, request.PostAuthorId))
//        return Results.BadRequest(new { Error = "Cannot like this post" });

//      try
//      {
//        user.LikePost(request.PostId, request.PostAuthorId, request.UpdatedBy);
//        await repository.UpdateAsync(user);

//        return Results.Ok(new
//        {
//          Message = "Post liked successfully",
//          LikedPostsCount = user.LikedPostIds.Count
//        });
//      }
//      catch (DomainException ex)
//      {
//        return Results.BadRequest(new { Error = ex.Message });
//      }
//    });
//  }
//}

//// Request DTOs
//public record RegisterUserRequest(string Username, string Email, string Password, Guid CreatedBy);
//public record LoginRequest(string UsernameOrEmail, string Password);
//public record UpdateProfileRequest(string Username, string Email, Guid UpdatedBy);
//public record UpdatePasswordRequest(string CurrentPassword, string NewPassword, Guid UpdatedBy);
//public record LikePostRequest(Guid PostId, Guid PostAuthorId, Guid UpdatedBy);
